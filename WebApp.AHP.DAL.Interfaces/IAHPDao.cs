using System;
using System.Collections.Generic;
using WebApp.AHP.Entities;

namespace WebApp.AHP.DAL.Interfaces
{
    public interface IAHPDao
    {
        int AddCriteria(Criteria criteria);

        void DeleteById(int id);
        IEnumerable<Criteria> GetAll();
    }
}
