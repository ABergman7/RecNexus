using System.Net;
using System.Net.Mail;
using PickUpSports.Interface;

namespace PickUpSports.Services
{
    public class GMailService : IGMailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly NetworkCredential _networkCredential;
        private bool _hasSent;

        public GMailService(SmtpClient smtpClient, NetworkCredential networkCredential)
        {
            _smtpClient = smtpClient;
            _networkCredential = networkCredential;
            _hasSent = false;
        }

        public string GetEmailAddress()
        {
            return System.Web.Configuration.WebConfigurationManager.AppSettings["GMailUsername"] + "@gmail.com";
        }

        /**
         * This Method will send the message with the message content(body) and the email address passed in
         */
        public bool Send(MailMessage message)
        {
            //Enabling credentials and ssl
            _smtpClient.Credentials = _networkCredential;
            _smtpClient.EnableSsl = true;

            //easier formatting messsages
            message.IsBodyHtml = true;

            //Send the message with the credentials 
            _smtpClient.Send(message);

            //Method reached here, so assuming message has been sent
            _hasSent = true;

            return _hasSent;
        }
    }
}