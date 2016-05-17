using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Model;

namespace Ticket.DAO
{
    public class TicketDAO
    {


        public IEnumerable<Tickets> All()
        {
            using (var db = new TicketEntities())
            {
                return db.Tickets.ToList();
            }
        }

        // TE TESTEN
        public Tickets GetById(int id)
        {
            using (var db = new TicketEntities())
            {
                return db.Tickets.Where(t => t.id == id).First();
            }
        }
        // TE TESTEN
        public IEnumerable<Tickets> getTicketsperPersoon(string persoonId)
        {
            using (var db = new TicketEntities())
            {
                return db.Tickets.Where(t => t.Persoonid.Equals(persoonId)).ToList();
            }
        }
        // TE TESTEN
        public IEnumerable<Tickets> GetTicketsPerPersoonPerWedstrijd(string persId , int id)
        {
            using (var db = new TicketEntities())
            {
                return db.Tickets.Where(t => t.Persoonid.Equals(persId) && t.Wedstrijdid == id).ToList();
            }
        }

        public IEnumerable<Tickets> getTicketsPerWedstrijd(int id)
        {
            using (var db = new TicketEntities())
            {
                return db.Tickets.Where(t => t.Wedstrijdid == id).ToList();
            }
        }

        public int getTicketsPerWedstrijdPerVak(int w, int PlaatsID)
        {
            using (var db = new TicketEntities())
            {
               
                var list = db.Tickets.Where(q => q.Wedstrijdid == w).Where(p => p.plaatsId == PlaatsID).ToList();
                return list.Count() ;
            }
        }

        public void Add(Tickets ticket)
        {
            using (var db = new TicketEntities())
            {
                // TODO : verwijder of verander Unique Constraint key in DB
                db.Tickets.Add(ticket);
                db.SaveChanges();
            }
        }

        public int ZoekTicketsBarcode(long barcode)
        {
            using (var db = new TicketEntities())
            {
                return db.Tickets.Where(t => t.barcode == barcode).Count();
            }
        }

        public Tickets GetTicketAll(int id)
        {
            using (var db = new TicketEntities())
            {
                return db.Tickets.Where(t => t.id.Equals(id)).Include(u => u.AspNetUsers).Include(w => w.Wedstrijd).Include(p => p.Plaats).First();
            }
        }
    }
}
