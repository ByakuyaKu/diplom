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
        int GetSessionCriteriaNumber(int sessionid);
        int GetSessionAlternariveNumber(int sessionid);
        int GetSessionId();
        IEnumerable<Criteria> GetCriteriaName(int sessionid);
        List<Criteria> StartAhp(IEnumerable<Criteria> criterias, int alternativenumber);
        IEnumerable<Criteria> GetAllCriteria(int sessionid);
        IEnumerable<Alternative> GetAllAlternative(int sessionid);
    }
}
