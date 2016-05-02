using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ticket.Model;
using System.Data.Entity;

namespace Ticket.DAO
{
    public class ClubDAO
    {
        public IEnumerable<Clubs> All()
        {
            using (var db = new TicketEntities())
            {
                return db.Clubs.ToList();
            }
        }

        public Clubs Get(int id)
        {
            using (var db = new TicketEntities())
            {
                return db.Clubs.Where(c => c.id == id).First();
            }
        }

        // methods to write:
        // public 
    }
}
