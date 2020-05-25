using System;
using System.Collections.Generic;
using WebApp.AHP.Entities;

namespace WebApp.AHP.DAL.Interfaces
{
    public interface IAHPDao
    {
        int AddCriteria(Criteria criteria, string matrix, int sessionid);

        int AddAlternative(Alternative alternative, int sessionId);

        int AddSession(int criterianumber, int alternativenumber);
        int GetSessionCriteriaNumber(int sessionid);
        int GetSessionAlternariveNumber(int sessionid);
        int GetSessionId();
        IEnumerable<Criteria> GetCriteriaName(int sessionid);
        IEnumerable<Criteria> GetAllCriteria(int sessionid);
        IEnumerable<Alternative> GetAllAlternative(int sessionid);

    }
}
