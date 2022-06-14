using Autofac.Extras.Moq;
using MimeKit;
using Moq;
using NUnit.Framework;

namespace ECommerce.Utility.Tests
{
    public class EmailSenderTests
    {
        private AutoMock _mock;
        private Mock<SmtpConfiguration> _smtpConfigurationMock;
        private Mock<MimeMessage> _mimeMessageMock;
        private Mock<MailboxAddress> _mailBoxAddressMock;
        private IEmailSender _emailSender;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mock = AutoMock.GetLoose(); // Initializing the mock object 
        }

        [SetUp]
        public void Setup()
        {
            _smtpConfigurationMock = _mock.Mock<SmtpConfiguration>();
            _mimeMessageMock = _mock.Mock<MimeMessage>();
            _mailBoxAddressMock = _mock.Mock<MailboxAddress>();

            _emailSender = _mock.Create<EmailSender>();//for test
        }

        [TearDown]
        public void Teardown()
        {
            _smtpConfigurationMock.Reset();
            _mimeMessageMock.Reset();
            _mailBoxAddressMock.Reset();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _mock?.Dispose();
        }

        //[Test]
        //public async Task SendAync_MailSentSuccess_SendAsync()
        //{
        //    //Arrange

        //}
    }
}