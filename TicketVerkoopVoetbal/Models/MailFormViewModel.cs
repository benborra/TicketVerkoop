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
    public class ConfirmationEmail
    {
        [Required]
        public string FromName { get; set; }
        [Required, EmailAddress]
        public string FromEmail { get; set; }
    }

    public class ResetPasswordEmail
    {
        [Required]
        public string FromName { get; set; }
        [Required, EmailAddress]
        public string FromEmail { get; set; }
    }
}
