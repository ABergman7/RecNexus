

using System.Net.Mail;

namespace PickUpSports.Interface
{
    public interface IGMailService
    {
        bool Send(MailMessage message);

        string GetEmailAddress();

    }
}