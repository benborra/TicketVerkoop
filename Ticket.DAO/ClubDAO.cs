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
                return db.Clubs.Include(s => s.Stadion).ToList();
            }
        }

        public Clubs Get(int id)
        {
            using (var db = new TicketEntities())
            {
                return db.Clubs.Where(c => c.id == id).Include(s => s.Stadion).Include(w => w.Wedstrijd).First();
            }
        }
        public Clubs GetFromStadion(int id)
        {
            using (var db = new TicketEntities())
            {
                return db.Clubs.Where(c => c.Stadionid == id).Include(s => s.Stadion).First();
            }
        }

        public void Add(Clubs club)
        {
            using (var db = new TicketEntities())
            {
                db.Entry(club).State = EntityState.Added;
                db.SaveChanges();
            }

            throw new NotImplementedException();
        }

        public string GetClubLogo(int id)
        {
            using (var db = new TicketEntities())
            {
                return db.Clubs.Where(c => c.id == id).Select(c => c.logo).First();
            }
        }

        public IEnumerable<Clubs> Update(Clubs entity)
        {
            using (var db = new TicketEntities())
            {
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }

            return null;
        }

        public void RemoveClub(Clubs club)
        {
            using (var db = new TicketEntities())
            {
                db.Entry(club).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }
    }
}
