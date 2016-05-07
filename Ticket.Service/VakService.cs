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
    }
}
