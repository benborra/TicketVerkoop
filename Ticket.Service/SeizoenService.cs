using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ticket.DAO;
using Ticket.Model;

namespace Ticket.Service
{
   public class SeizoenService
    {
        private SeizoenDAO seizoenDao;

        public SeizoenService()
        {
            seizoenDao = new SeizoenDAO();
        }

        public IEnumerable<Seizoen> All()
        {
            return seizoenDao.All();
        }

        public Seizoen Get(int id)
        {
            return seizoenDao.Get(id);
        }

        public void Update(Seizoen entity)
        {
            seizoenDao.Update(entity);
        }

        public void RemoveSeizoen(Seizoen seizoen)
        {
            seizoenDao.removeSeizoen(seizoen);
        }

        public void Add(Seizoen seizoen)
        {
            seizoenDao.Add(seizoen);
        }
    }
}
