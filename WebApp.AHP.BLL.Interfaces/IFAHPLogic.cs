using System;
using System.Collections.Generic;
using System.Text;
using WebApp.AHP.Entities;

namespace WebApp.AHP.BLL.Interfaces
{
    public interface IFAHPLogic
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
        //List<CriteriaFAHP> Parse(IEnumerable<CriteriaFAHP> criterias, int alternativenumber);
        List<CriteriaFAHP> ParseMatrixForAlt(IEnumerable<CriteriaFAHP> criterias, int alternativenumber);
        IEnumerable<CriteriaFAHP> GetAllCriteriaAltMatrOnly(int sessionid);
        void UpdateCriteria(int sessionid, string matrix);
        List<CriteriaFAHP> ParseMatrixForCr(IEnumerable<CriteriaFAHP> criterias);
        List<AlternativeFAHP> StartFAhp(IEnumerable<CriteriaFAHP> criterias, List<AlternativeFAHP> alternatives);
        List<AlternativeFAHP> SortFinalScore(List<AlternativeFAHP> alternatives);
    }
}
