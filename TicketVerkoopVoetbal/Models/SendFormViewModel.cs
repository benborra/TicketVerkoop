using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace TicketVerkoopVoetbal.Models
{
    public class SendFormViewModel
    {
        [Required, Display(Name = "Voetbal Ticket")]
        public string FromName { get; set; }
        [Required, Display(Name = "voetbalticket@gmail.com"), EmailAddress]
        public string FromEmail { get; set; }
        [Required]
        public string Message { get; set; }

        public SendFormViewModel()
        {
            var smtpSection = (SmtpSection)ConfigurationManager.GetSection("System.net/mailSettings/smtp");
            FromName = smtpSection.From;
            FromEmail = smtpSection.Network.UserName;
        }
    }
}
