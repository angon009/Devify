using AutoMapper;
using ECommerce.Infrastructure.BusinessObjects.MessageNotification;
using ECommerce.Web.Areas.Vendor.Models.ForMessageNotification;
using ECommerce.Infrastructure.BusinessObjects.Orders;
using ECommerce.Web.Models;

namespace ECommerce.Web.Profiles
{
    public class WebProfile : Profile
    {
        public WebProfile()
        {
            CreateMap<MessageCreateModel, Message>()
                .ReverseMap();
            CreateMap<MessageListModel, Message>()
               .ReverseMap();

            CreateMap<CartItemModel, Cart>()
               .ReverseMap();
        }
    }
}
