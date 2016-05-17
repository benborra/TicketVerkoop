using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ticket.DAO;
using Ticket.Model;

namespace Ticket.Service
{
    public class UserService
    {
        private UserDAO userDao;

        public UserService()
        {
            userDao = new UserDAO();
        }

        public IEnumerable<AspNetUsers> All()
        {
            return userDao.All();
        }
        
        public AspNetUsers Get(string email)
        {
            return userDao.Get(email);
        }

        public void Update(AspNetUsers entity)
        {
            userDao.Update(entity);
        }

        public void RemoveUser(AspNetUsers user)
        {
            userDao.RemoveUser(user);
        }

        public void Add(AspNetUsers user)
        {
            userDao.Add(user);
        }

        public AspNetUsers GetUser(string id)
        {
            return userDao.GetUser(id);
        }
    }
}
