using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ticket.Model;
using Ticket.DAO;

namespace Ticket.Service
{

    public class PlaatsService
    {
        private PlaatsDAO plaatsDao;

        public PlaatsService()
        {
            plaatsDao = new PlaatsDAO();
        }

        public Plaats GetPlaats(int id)
        {
            return plaatsDao.GetPlaats(id);
        }

    }
}
