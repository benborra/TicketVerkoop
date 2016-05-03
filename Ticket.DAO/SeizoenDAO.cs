using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ticket.Model;

namespace Ticket.DAO
{
    public class SeizoenDAO
    {
        public IEnumerable<Seizoen> All()
        {
            using (var db = new TicketEntities())
            {
                return db.Seizoen.ToList();
            }
        }

        public Seizoen Get(int id)
        {
            using (var db = new TicketEntities())
            {
                return db.Seizoen.Where(s => s.id == id).First();
            }
        }

        public void Add(Seizoen seizoen)
        {
            using (var db = new TicketEntities())
            {
                db.Entry(seizoen).State = EntityState.Added;
                db.SaveChanges();
            }

            throw new NotImplementedException();
        }

        public IEnumerable<Seizoen> Update(Seizoen entity)
        {
            using (var db = new TicketEntities())
            {
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }

            return null;
        }

        public void removeSeizoen(Seizoen Seizoen)
        {
            using (var db = new TicketEntities())
            {
                db.Entry(Seizoen).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }
    }
}
