using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Model;

namespace TicketVerkoopVoetbal.Models
{
    public class CartViewModel
    {
        public int WedstrijdId { get; set; }
        public string ThuisPloeg{ get; set; }
        public string BezoekersPloeg { get; set; }
        public string Stadion { get; set; }
        public System.DateTime Datum { get; set; }
        public int Aantal { get; set; }
        public float Prijs{ get; set; }
    }
}
