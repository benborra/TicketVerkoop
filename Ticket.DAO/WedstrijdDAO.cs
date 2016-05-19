using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using Ticket.Model;

namespace Ticket.DAO
{
    public class WedstrijdDAO
    {
        private DateTime now;

        public WedstrijdDAO()
        {
            now = DateTime.Now; 
        }

        public IEnumerable<Wedstrijd> All()
        {
            using (var db = new TicketEntities())
            {
                var list = db.Wedstrijd.ToList();
                var sorted = list.OrderByDescending(d => d.Date).Reverse().ToList();
                return sorted;
            }
        }

       

        public Wedstrijd Get(int id)
        {
            using (var db = new TicketEntities())
            {
                return db.Wedstrijd.Include(a => a.Clubs).Include(ar => ar.Clubs1).Include(ae => ae.Stadion).Where(c => c.id == id).First();
            }
        }

        public void AddWedstrijd(Wedstrijd w)
        {
            using (var db = new TicketEntities())
            {
                db.Wedstrijd.Add(w);
                db.SaveChanges();
            }
        }
        public void AddWedstrijd(int stadionID, int thuisPLoeg, int bezoekersPloeg, DateTime date)
        {
            //  maken nieuwe ploeg
            Wedstrijd w = new Wedstrijd();
            w.stadionId = stadionID;
            w.thuisPloeg = thuisPLoeg;
            w.bezoekersPloeg = bezoekersPloeg;
            w.Date = date;

            AddWedstrijd(w);
        }
        // get wedstrijdPerPloeg

        public IEnumerable<Wedstrijd> GetWedStrijdPerPloeg(int id)
        {
            using (var db = new TicketEntities())
            {
                return db.Wedstrijd.Where(r => r.thuisPloeg == id || r.bezoekersPloeg == id).Include(c => c.Clubs).ToList();
            }
        }

        public void Remove(Wedstrijd wedstrijd)
        {
            using (var db = new TicketEntities())
            {
                db.Entry(wedstrijd).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }

        // get wedstrijd per datum

        public IEnumerable<Wedstrijd> WedstrijdPerDatum(DateTime time)
        {
            using (var db = new TicketEntities())
            {
                // geen datetime om op te filteren!
                return db.Wedstrijd.Where(r => r.Date == time).ToList();
            }
           
        }


        public IEnumerable<Wedstrijd> NogTeSpelenWedstrijden()
        {
           
            using (var db = new TicketEntities())
            {
                // geen datetime om op te filteren!
                var list =  db.Wedstrijd.Include(s => s.Stadion).Include(q => q.Clubs).Include(p => p.Clubs1).Where(r => r.Date > now).ToList();
                var sorted = list.OrderByDescending(d => d.Date).Reverse().ToList();
                return sorted;
            }
        }
        public IEnumerable<Wedstrijd> GespeeldeWedstrijden()
        {
            using (var db = new TicketEntities())
            {
                // geen datetime om op te filteren!
                var list = db.Wedstrijd.Include(s => s.Stadion).Include(c => c.Clubs).Include(p => p.Clubs1).Where(r => r.Date < now).ToList();
                var sorted = list.OrderByDescending(d => d.Date).Reverse().ToList();
                return sorted;
            }
        }

        public void UpdateWedsrijd(Wedstrijd w)
        {
            Wedstrijd we = Get(w.id);
            we.Date = w.Date;

            using (var db = new TicketEntities())
            {
                db.Wedstrijd.Attach(we);
                db.Entry(we).State = EntityState.Modified;
                db.SaveChanges();
            }
        }


        // get wedstrijdPerPloeg die nog gespeeld moeten worden
        public IEnumerable<Wedstrijd> GetWedStrijdPerPloegToekomst(int id)
        {
            using (var db = new TicketEntities())
            {
                var list = db.Wedstrijd.Where(r => r.thuisPloeg == id || r.bezoekersPloeg == id).Where(d => d.Date > DateTime.Now).Include(c => c.Clubs).ToList();
                var sorted = list.OrderByDescending(d => d.Date).Reverse().ToList();
                return sorted;
            }
        }
    }
}
