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
        public int ThuisPloeg { get; set; }
        public string ThuisPloegNaam{ get; set; }
        public int BezoekersPloeg { get; set; }
        public string BezoekersPloegNaam { get; set; }
        public int Stadion { get; set; }
        public string StadionNaam { get; set; }
        public System.DateTime Datum { get; set; }
        public int Aantal { get; set; }
        public float Prijs { get; set; }
        public int Plaats { get; set; }
        public string PlaatsNaam { get; set; }
    }
}
