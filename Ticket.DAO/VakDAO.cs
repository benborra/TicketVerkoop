using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Model;

namespace Ticket.DAO
{
    public class VakDAO
    {
        public IEnumerable<Vak> All()
        {
            using (var db = new TicketEntities())
            {
                // include steekt ook deze resultaten in de lijst
                // Gecontroleerd, werkt

                return db.Vak.ToList();
            }
        }

        public int getAantalZitplaatsenPerStadion()
        {
            return 0;
        }
    }
}
