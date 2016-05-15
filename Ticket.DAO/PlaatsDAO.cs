using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ticket.Model;

namespace Ticket.DAO
{
    public class PlaatsDAO
    {
        public Plaats GetPlaats(int id)
        {
            using (var db = new TicketEntities())
            {
                return db.Plaats.Where(p => p.id == id).First();
            }
        }
    }
}
