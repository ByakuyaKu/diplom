using System;
using System.Collections.Generic;
using System.Text;
using WebApp.AHP.Entities;

namespace WebApp.AHP.DAL.Interfaces
{
    public interface IFAHPDAO
    {
        int AddSession(int criterianumber, int alternativenumber);
        int GetSessionId();
        int GetSessionAlternariveNumber(int sessionid);
        int GetSessionCriteriaNumber(int sessionid);
        IEnumerable<CriteriaFAHP> GetCriteriaName(int sessionid);
        IEnumerable<AlternativeFAHP> GetAllAlternative(int sessionid);
        IEnumerable<CriteriaFAHP> GetAllCriteria(int sessionid);
        int AddCriteria(CriteriaFAHP criteria, string matrix, int sessionid);
        int AddAlternative(AlternativeFAHP alternative, int sessionId);
        IEnumerable<CriteriaFAHP> GetAllCriteriaAltMatrOnly(int sessionid);
        void UpdateCriteria(int sessionid, string matrix);
    }
}
