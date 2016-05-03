using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ticket.DAO;
using Ticket.Model;

namespace Ticket.Service
{
    public class ClubService
    {
        private ClubDAO clubDao;

        public ClubService()
        {
            clubDao = new ClubDAO();
        }

        public IEnumerable<Clubs> All()
        {
            return clubDao.All();
        }

        public Clubs Get(int id)
        {
            return clubDao.Get(id);
        }

        public void Update(Clubs entity)
        {
            clubDao.Update(entity);
        }

        public void RemoveClub(Clubs club)
        {
            clubDao.RemoveClub(club);
        }

        public void Add(Clubs club)
        {
            clubDao.Add(club);
        }
    }
}
