using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketVerkoopVoetbal.Models
{
    public class AbonnementViewModel
    {
        public int Seizoenid { get; set; }

        public int ClubsId { get; set; }
        public string Club { get; set; }
        public int Persoonid { get; set; }
        
        public int PlaatsId { get; set; }
        public string Plaats { get; set; }
    }
}