using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ticket.Model;
using System.Data.Entity;

namespace Ticket.DAO
{
    public class UserDAO
    {
        public IEnumerable<AspNetUsers> All()
        {
            using (var db = new TicketEntities())
            {
                return db.AspNetUsers.ToList();
            }
        }

        public AspNetUsers Get(string email)
        {
            try
            {
                using (var db = new TicketEntities())
                {
                    return db.AspNetUsers.Where(u => u.Email.Equals(email)).First();
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public void Add(AspNetUsers user)
        {
            using (var db = new TicketEntities())
            {
                db.Entry(user).State = EntityState.Added;
                db.SaveChanges();
            }

            throw new NotImplementedException();
        }

        public IEnumerable<AspNetUsers> Update(AspNetUsers entity)
        {
            using (var db = new TicketEntities())
            {
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }

            return null;
        }

        public void RemoveUser(AspNetUsers user)
        {
            using (var db = new TicketEntities())
            {
                db.Entry(user).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }

    }
}
