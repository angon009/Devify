using Autofac;
using ECommerce.Membership.Repositories;
using ECommerce.Web.Areas.Vendor.Models.ForMessageNotification;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Web.Enums;
using Microsoft.AspNetCore.Authorization;

namespace ECommerce.Web.Areas.Vendor.Controllers
{

    [Authorize]
    [Area("Vendor")]
    public class MessageController : Controller
    {
        private IAccountRepository _accountRepo;
        private readonly ILifetimeScope _scope;
        private ILogger<MessageController> _logger;
        public MessageController(IAccountRepository accountRepo, ILifetimeScope scope,
            ILogger<MessageController> logger)
        {
            _accountRepo = accountRepo;
            _scope = scope;
            _logger = logger;
        }
        [Route("Messages")]
        public async Task<IActionResult> Index(string ReceiverEmail)
        {
            var model = _scope.Resolve<MessageListModel>();
            model.Resolve(_scope);
            //Default Receiver set
            try
            {
                var role = (await _accountRepo.GetCurrentUserRolesAsync(User.Identity.Name))[0];
                if (role == "Vendor")
                {
                    model.ReceiverEmail = "admin@ecommerce.com";
                }else if (role == "Admin")
                {
                    model.ReceiverEmail = "admin@ecommerce.com";
                }
                else
                {
                    var user = (await model.GetStoreBySubDomainAsync()).User.Email;
                    model.ReceiverEmail = user;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            if (ReceiverEmail != null)
            {
                model.ReceiverEmail = ReceiverEmail;
            }
            model.SenderEmail = User.Identity!.Name!;
            ViewBag.Senders = await model.GetMessageSendersAsync();
            ViewBag.Sender = await _accountRepo.GetUserByEmailAsync(model.SenderEmail);
            ViewBag.Receiver = await _accountRepo.GetUserByEmailAsync(model.ReceiverEmail);
            var messages = await model.GetMessages();
            return View(messages);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MessageCreateModel message)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    message.Resolve(_scope);
                    await message.CreateMessage();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return BadRequest();
        }
    }
}