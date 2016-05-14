using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.DAO;
using Ticket.Model;

namespace Ticket.Service
{
   public class WedstrijdService
    {
        WedstrijdDAO wedstrijdDAO;

        public WedstrijdService()
        {

            wedstrijdDAO = new WedstrijdDAO();
        }

        public IEnumerable<Wedstrijd> NogTeSpelenWedstrijden()
        {
            return wedstrijdDAO.NogTeSpelenWedstrijden();
        }
        

        public Wedstrijd Get(int id)
        {
            return wedstrijdDAO.Get(id);
        }

        // get wedstrijdPerPloeg

        public IEnumerable<Wedstrijd> GetWedStrijdPerPloeg(int id)
        {
            return wedstrijdDAO.GetWedStrijdPerPloeg(id);
        }
        // get wedstrijd per datum

        public IEnumerable<Wedstrijd> WedstrijdPerDatum(DateTime time)
        {
            return wedstrijdDAO.WedstrijdPerDatum(time);
        }

        public IEnumerable<Wedstrijd> GespeeldeWedstrijden()
        {
            return wedstrijdDAO.GespeeldeWedstrijden();
        }
        public void AddWedstrijd(Wedstrijd w)
        {
            wedstrijdDAO.AddWedstrijd(w);
        }
       

        public IEnumerable<Wedstrijd> All()
        {
            return wedstrijdDAO.All();
        }

        // get wedstrijdPerPloeg die nog gespeeld moeten worden
        public IEnumerable<Wedstrijd> GetWedStrijdPerPloegToekomst(int id)
        {
            return wedstrijdDAO.GetWedStrijdPerPloegToekomst(id);
        }
    }
}
