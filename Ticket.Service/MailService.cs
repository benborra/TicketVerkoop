using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Ticket.Service
{
    public class MailService
    {
        public static void SendConfirmationMail(string firstName, string email, string callbackUrl)
        {
            var fromAdress = new MailAddress("voetbalticket@gmail.com", "Voetbal Tickets");
            var toAdress = new MailAddress(email);
            const string fromPassword = "azerty-123";

            MailMessage message = new MailMessage(fromAdress.Address, toAdress.Address);
            SmtpClient server = new SmtpClient("smtp.gmail.com");

            server.Host = "smtp.gmail.com";
            server.Port = 587;
            server.EnableSsl = true;
            server.DeliveryMethod = SmtpDeliveryMethod.Network;
            server.Credentials = new NetworkCredential(fromAdress.Address, fromPassword);
            server.Timeout = 20000;

            message.Subject = "VoetbalTickets - Registratie bevestiging";
            message.Body = "</p>Beste " + firstName + System.Environment.NewLine + "</p><p>Er werd een account voor u aangemaakt op VoetbalTickets.</p>";
            message.Body += "</p>Gelieve uw account te bevestigen door op onderstaande link te drukken <br/><a href =\"" + callbackUrl + "\"><img class='transparent' alt='http://zeldawiki.org/images/thumb/8/8a/LoZ_ST_Link.png/170px-LoZ_ST_Link.png' src='http://zeldawiki.org/images/thumb/8/8a/LoZ_ST_Link.png/170px-LoZ_ST_Link.png'></a>";
            message.Body += "<p>Met vriendelijke groet</p>" + System.Environment.NewLine + "<p>Het VoetbalTickets-team</p>";

            server.Timeout = 30000;

            message.Priority = MailPriority.Normal;
            message.IsBodyHtml = true;
            server.Send(message);

        }

        public static void sendPasswordResetMail(string firstName, string email, string callbackUrl)
        {
            var fromAdress = new MailAddress("voetbalticket@gmail.com", "Voetbal Tickets");
            var toAdress = new MailAddress(email);
            const string fromPassword = "azerty-123";

            MailMessage message = new MailMessage(fromAdress.Address, toAdress.Address);
            SmtpClient server = new SmtpClient("smtp.gmail.com");

            server.Host = "smtp.gmail.com";
            server.Port = 587;
            server.EnableSsl = true;
            server.DeliveryMethod = SmtpDeliveryMethod.Network;
            server.Credentials = new NetworkCredential(fromAdress.Address, fromPassword);
            server.Timeout = 20000;

            message.Subject = "VoetbalTickets - Paswoord herstellen";
            message.Body = "</p>Beste " + firstName + System.Environment.NewLine + "</p><p>Er werd een paswoordherstel gevraagd voor uw account.</p>";
            message.Body += "</p>Gelieve op onderstaande link te drukken <br/><a href =\"" + callbackUrl + "\"><img class='transparent' alt='http://zeldawiki.org/images/thumb/8/8a/LoZ_ST_Link.png/170px-LoZ_ST_Link.png' src='http://zeldawiki.org/images/thumb/8/8a/LoZ_ST_Link.png/170px-LoZ_ST_Link.png'></a>";
            message.Body += "<br /><p>Indien u geen paswoordherstel hebt aangevraagd gelieve deze mail dan te negeren.</p><br /><br />";
            message.Body += "<p>Met vriendelijke groet</p>" + System.Environment.NewLine + "<p>Het VoetbalTickets-team</p>";

            server.Timeout = 30000;

            message.Priority = MailPriority.Normal;
            message.IsBodyHtml = true;
            server.Send(message);
        }
    }
}
