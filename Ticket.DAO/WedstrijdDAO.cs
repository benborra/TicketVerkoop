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
        // get wedstrijd per datum
         
    }
}
