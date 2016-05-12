﻿using System;
using System.Collections.Generic;
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
        public IEnumerable<Tickets> getTicketsperPersoon(int persoonId)
        {
            using (var db = new TicketEntities())
            {
                return db.Tickets.Where(t => Convert.ToInt32(t.Persoonid) == persoonId).ToList();
            }
        }
        // TE TESTEN
        public IEnumerable<Tickets> GetTicketsPerPersoonPerWedstrijd(int persId , Wedstrijd wedstrijd )
        {
            var wedstrijdId = wedstrijd.id;
            using (var db = new TicketEntities())
            {
                return db.Tickets.Where(t => Convert.ToInt32(t.Persoonid) == persId && t.Wedstrijdid == wedstrijdId).ToList();
            }
        }

        public IEnumerable<Tickets> getTicketsPerWedstrijd(Wedstrijd wedstrijd)
        {
            var wedstrijdId = wedstrijd.id;
            using (var db = new TicketEntities())
            {
                return db.Tickets.Where(t => t.Wedstrijdid == wedstrijdId).ToList();
            }
        }

        public int getTicketsPerWedstrijdPerVak(Wedstrijd w, int PlaatsID)
        {

            int wedId = w.id;
            using (var db = new TicketEntities())
            {
               
                var list = db.Tickets.Where(q => q.Wedstrijdid == wedId).Where(p => p.plaatsId == PlaatsID).ToList();
                return list.Count() ;
            }
        }

        public void Add(Tickets ticket)
        {
            using (var db = new TicketEntities())
            {
                db.Tickets.Add(ticket);
                db.SaveChanges();
            }
        }
    }
}
