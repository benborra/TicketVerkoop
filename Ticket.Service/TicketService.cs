using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Model;
using Ticket.DAO;

namespace Ticket.Service
{
    public class TicketService
    {
        TicketDAO ticketDao;

        public TicketService()
        {
            ticketDao = new TicketDAO();
        }
        public IEnumerable<Tickets> All()
        {
            return ticketDao.All();
        }


        public Tickets GetById(int id)
        {
            return ticketDao.GetById(id);
        }
        // TE TESTEN
        public IEnumerable<Tickets> getTicketsperPersoon(int persoonId)
        {
            return ticketDao.getTicketsperPersoon(persoonId);
        }
        // TE TESTEN
        public IEnumerable<Tickets> GetTicketsPerPersoonPerWedstrijd(int persId, Wedstrijd wedstrijd)
        {
            return ticketDao.GetTicketsPerPersoonPerWedstrijd(persId, wedstrijd);
        }

        public IEnumerable<Tickets> getTicketsPerWedstrijd(int id)
        {
            return ticketDao.getTicketsPerWedstrijd(id);
        }
        public int getTicketsPerWedstrijdPerVak(int w, int PlaatsID)
        {
            return ticketDao.getTicketsPerWedstrijdPerVak(w, PlaatsID);
        } 
        public void Add(Tickets ticket)
        {
             ticketDao.Add(ticket);
        }
    }
}
