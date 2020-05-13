using System;
using System.Collections.Generic;
using WebApp.AHP.Entities;

namespace WebApp.AHP.BLL.Interfaces
{
    public interface IAHPLogic
    {
        int AddCriteria(Criteria criteria);

        void DeleteById(int id);

        IEnumerable<Criteria> GetAll();
    }
}
