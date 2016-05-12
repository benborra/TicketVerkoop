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

        public int getAantalZitplaatsenPerStadionPerVak(Stadion s, int vakId)
        {
           
            int stadId = s.id;

            using (var db = new TicketEntities())
            {
                return Convert.ToInt32(db.Plaats.Where(q => q.Stadionid == stadId).Where(a => a.Vakid == vakId).Select(d => d.aantal).First());
            }           
            
        }

        public int getAantalZitPlaatsenPerStadion(Stadion s)
        {

            int stadId = s.id;

            using (var db = new TicketEntities())
            {
                int aantalPlaatsen = 0;
                var list = db.Plaats.Where(q => q.Stadionid == stadId).ToList();
                foreach (Plaats plaats in list)
                {
                    aantalPlaatsen += plaats.aantal;
                }

                return aantalPlaatsen;
            }
        }

    }
}
