﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ticket.DAO;
using Ticket.Model;

namespace Ticket.Service
{
    class AbonnementService
    {
        private AbonnementDAO abonnementDao;

        public AbonnementService()
        {
            abonnementDao = new AbonnementDAO();
        }

        public IEnumerable<Abonnement> All()
        {
            return abonnementDao.All();
        }

        public Abonnement Get(int id)
        {
            return abonnementDao.Get(id);
        }

        public void Update(Abonnement entity)
        {
            abonnementDao.Update(entity);
        }

        public void RemoveAbonnement(Abonnement abonnement)
        {
            abonnementDao.removeAbonnement(abonnement);
        }

        public void Add(Abonnement abonnement)
        {
            abonnementDao.Add(abonnement);
        }
    }
}