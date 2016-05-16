using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Model;
using Ticket.DAO;

namespace Ticket.Service
{
    public class VakService
    {
        VakDAO vakDao;

        public VakService()
        {
            vakDao = new VakDAO();
        }

        public IEnumerable<Vak> All()
        {
            return vakDao.All();
        }

        public int getAantalZitplaatsenPerStadionPerVak(Stadion s, int vakId)
        {
            return vakDao.getAantalZitplaatsenPerStadionPerVak(s, vakId);
        }


        public int getAantalZitPlaatsenPerStadion(Stadion s)
        {
            return vakDao.getAantalZitPlaatsenPerStadion(s);
        }

        public Vak getVak(int id)
        {
            return vakDao.GetVak(id);
        }
    }
}