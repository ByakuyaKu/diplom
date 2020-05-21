using System;
using System.Collections.Generic;
using WebApp.AHP.Entities;

namespace WebApp.AHP.BLL.Interfaces
{
    public interface IAHPLogic
    {
        int AddCriteria(Criteria criteria, string matrix, int sessionid);

        int AddAlternative(Alternative alternative, int sessionId);

        int GetInputCriteriaNumber(IEnumerable<Criteria> criterias);

        int AddSession(int criterianumber, int alternativenumber);

        int GetSessionCriteriaNumber(int id);
        int GetSessionAlternariveNumber(int id);
        int GetSessionId();
        //IEnumerable<Alternative> GetAlternative(int sessionid);
        IEnumerable<Criteria> GetCriteriaName(int sessionid);

        List<Criteria> StartAhp(IEnumerable<Criteria> criterias, int alternativenumber);
        IEnumerable<Criteria> GetAllCriteria(int sessionid);
        IEnumerable<Alternative> GetAllAlternative(int sessionid);
    }
}
