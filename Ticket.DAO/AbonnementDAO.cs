using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ticket.Model;

namespace Ticket.DAO
{
    public class AbonnementDAO
    {
        public IEnumerable<Abonnement> All()
        {
            using (var db = new TicketEntities())
            {
                return db.Abonnement.ToList();
            }
        }

        public Abonnement Get(int id)
        {
            using (var db = new TicketEntities())
            {
                return db.Abonnement.Where(s => s.id == id).First();
            }
        }

        public Abonnement GetFromUserId(string id)
        {
            using (var db = new TicketEntities())
            {
                return db.Abonnement.Where(s => s.Persoonid == id).FirstOrDefault();
            }
        }

        public void Add(Abonnement abonnement)
        {
            using (var db = new TicketEntities())
            {
                db.Entry(abonnement).State = EntityState.Added;
                db.SaveChanges();
            }

            throw new NotImplementedException();
        }

        public IEnumerable<Abonnement> Update(Abonnement entity)
        {
            using (var db = new TicketEntities())
            {
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }

            return null;
        }

        public void removeAbonnement(Abonnement Abonnement)
        {
            using (var db = new TicketEntities())
            {
                db.Entry(Abonnement).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public IEnumerable<Abonnement> getFromClubdId(int id)
        {
            using (var db = new TicketEntities())
            {
                return db.Abonnement.Where(wed => wed.Clubsid == id).ToList();
            }
        }
    }
}
