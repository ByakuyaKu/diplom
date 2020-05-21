using System;
using System.Collections.Generic;
using WebApp.AHP.Entities;

namespace WebApp.AHP.BLL.Interfaces
{
    public interface IAHPLogic
    {
        int AddCriteria(Criteria criteria, int sessionid);

        int AddAlternative(Alternative alternative, string matrix, int sessionId);

        int GetInputCriteriaNumber(IEnumerable<Criteria> criterias);

        int AddSession(int criterianumber, int alternativenumber);

        int GetSessionCriteriaNumber(int id);
        int GetSessionAlternariveNumber(int id);
        int GetSessionId();
        IEnumerable<Alternative> GetAlternative(int sessionid);
        IEnumerable<Criteria> GetCriteriaName(int sessionid);

        List<Alternative> StartAhp(IEnumerable<Alternative> alternatives, int criterianumber);

        void DeleteById(int id);

        IEnumerable<Criteria> GetAll();
    }
}
