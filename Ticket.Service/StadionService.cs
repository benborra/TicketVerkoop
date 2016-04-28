using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.DAO;
using Ticket.Model;

namespace Ticket.Service
{
   public class StadionService
    {
        private  StadionDAO stadionDao;

        public StadionService()
        {
            stadionDao = new StadionDAO();
        }

        public IEnumerable<Stadion> All()
        {
            return stadionDao.All();
        }

        public Stadion Get(int id)
        {
            return stadionDao.Get(id);
        }
    }
}
