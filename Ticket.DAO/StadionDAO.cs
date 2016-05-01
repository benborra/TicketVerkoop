using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ticket.Model;
using System.Data.Entity;

namespace Ticket.DAO
{
    public class StadionDAO
    {
        /*

            Additional information: A specified Include path is not valid.
            The EntityType 'DB_9FC047_ASPVoeModel.Stadion' does not declare a navigation property with the name 'naam'.

    */


        // vraagt alle stadions op. 
        public IEnumerable<Stadion> All()
        {
            using (var db = new TicketEntities())
            {
                // include steekt ook deze resultaten in de lijst
                // Gecontroleerd, werkt

                return db.Stadion.ToList();
            }
        }

        public Stadion Get(int id)
        {
            using (var db = new TicketEntities())
            {
                // returned eerste gevonden stadion met dit id, 
                // to test
                return db.Stadion.Where(s => s.id == id).First();
            }
        }
    }
}
