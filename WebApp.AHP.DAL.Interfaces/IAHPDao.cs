using System;
using System.Collections.Generic;
using WebApp.AHP.Entities;

namespace WebApp.AHP.DAL.Interfaces
{
    public interface IAHPDao
    {
        int AddCriteria(Criteria criteria, int sessionid);

        int AddAlternative(Alternative alternative, string matrix, int sessionId);

        int AddSession(int criterianumber, int alternativenumber);
        int GetSessionCriteriaNumber(int id);
        int GetSessionAlternariveNumber(int id);
        int GetSessionId();
        IEnumerable<Alternative> GetAlternative(int sessionid);
        IEnumerable<Criteria> GetCriteriaName(int sessionid);
        void DeleteById(int id);
        IEnumerable<Criteria> GetAll();
    }
}
