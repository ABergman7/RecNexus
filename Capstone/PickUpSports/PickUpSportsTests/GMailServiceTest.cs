using System.Net;
using System.Net.Mail;
using Moq;
using NUnit.Framework;
using PickUpSports.Models.Extensions;
using PickUpSports.Services;

namespace PickUpSportsTests
{
    [TestFixture]
    public class GMailServiceTests
    {
        private readonly Mock<NetworkCredential> _networkCredentialsMock;
        private readonly Mock<SmtpClient> _smtpClientMock;
        private readonly GMailService _sut;

        public GMailServiceTests()
        {
            //Before running the test, fill it in with the appsetting secret value
            _networkCredentialsMock = new Mock<NetworkCredential>("username here", "password here");
            _smtpClientMock = new Mock<SmtpClient>("smtp.gmail.com", 587);

            _sut = new GMailService(_smtpClientMock.Object, _networkCredentialsMock.Object);
        }

        [Test]
        [Ignore("Needs creds before running")]
        public void Send_SentMessage_ReturnTrue()
        {
            //Arrange - from value needs to be filled in with the email address 
            var fromEmailAddress = "email here";
            var toEmailAddress = "testingEmail@gmail.com";
            var body = "This is a test message";

            MailMessage testMailMessage = new MailMessage(fromEmailAddress, toEmailAddress)
            {
                Body = body
            };


            //Act 
            var emailHasSent = _sut.Send(testMailMessage);

            //Assert
            Assert.AreEqual(emailHasSent, true);
        }
    }
}
