using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Model;

namespace Ticket.DAO
{
    class WedstrijdDAO
    {
        public IEnumerable<Wedstrijd> All()
        {
            using (var db = new TicketEntities())
            {
                return db.Wedstrijd.ToList();
            }
        }


        public Wedstrijd Get(int id)
        {
            using (var db = new TicketEntities())
            {
                return db.Wedstrijd.Where(c => c.id == id).First();
            }
        }

        // get wedstrijdPerPloeg

        public IEnumerable<Wedstrijd> GetWedStrijdPerPloeg(int id)
        {
            using (var db = new TicketEntities())
            {
                return db.Wedstrijd.Where(r => r.thuisPloeg == id ||  r.bezoekersPloeg == id).ToList();
            }

            
        }
        // get wedstrijd per datum

        public IEnumerable<Wedstrijd> WedstrijdPerDatum(DateTime time)
        {
            using (var db = new TicketEntities())
            {
                // geen datetime om op te filteren!
                // return db.Wedstrijd.Where(r => r.).ToList();
            }
            return null;
        }
         
    }
}
