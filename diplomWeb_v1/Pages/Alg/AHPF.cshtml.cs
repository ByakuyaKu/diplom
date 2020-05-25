using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WebApp.AHP.BLL.Interfaces;
using WebApp.AHP.Common;
using WebApp.AHP.Entities;

namespace diplomWeb_v1.Pages.Alg
{
    public class AHPFModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;
        private readonly IFAHPLogic BLLLogic = DependencyResolver.FAHPLogic;

        public int sessionId { get; set; }
        public int criteriaNumber { get; set; }
        public int alternativeNumber { get; set; }
        public List<AlternativeFAHP> Alternatives { get; set; }
        public bool MatrixOfPairedCompRender { get; set; }
        public bool RenderFirstStep { get; set; }
        public bool RenderSecondStep { get; set; }
        public bool RenderThirdStep { get; set; }
        public bool RenderFourthStep { get; set; }
        public bool RenderInput { get; set; }
        public bool RenderResult { get; set; }
        public bool ActivateRun { get; set; }
        public List<CriteriaFAHP> Criterias { get; set; }

        public AHPFModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
            Alternatives = new List<AlternativeFAHP>();
            Criterias = new List<CriteriaFAHP>();
            RenderResult = false;
            MatrixOfPairedCompRender = false;
            RenderFirstStep = true;
            RenderSecondStep = false;
            RenderThirdStep = false;
            RenderFourthStep = false;
            RenderInput = true;
            ActivateRun = false;
        }
        public void OnGet()
        {

        }

        public IActionResult OnPostSubmitCriteriaNumber(int CriteriaNumber, int AlternativeNumber)
        {
            sessionId = BLLLogic.AddSession(CriteriaNumber, AlternativeNumber);

            RenderFirstStep = false;
            RenderSecondStep = true;

            return Page();
        }

        public IActionResult OnPostSubmitCriteria(string CriteriaName, string Matrix)
        {
            sessionId = BLLLogic.GetSessionId();
            criteriaNumber = BLLLogic.GetSessionCriteriaNumber(sessionId);
            alternativeNumber = BLLLogic.GetSessionAlternariveNumber(sessionId);
            BLLLogic.AddCriteria(new CriteriaFAHP(CriteriaName, null, Matrix), Matrix, sessionId);

            //Criterias = BLLLogic.GetAllCriteria(sessionId).ToList();
            Criterias = BLLLogic.ParseMatrixForAlt(BLLLogic.GetAllCriteriaAltMatrOnly(sessionId), alternativeNumber);

            Alternatives = BLLLogic.GetAllAlternative(sessionId).ToList();

            RenderFirstStep = false;
            if (BLLLogic.GetCriteriaName(sessionId).ToList().Count == criteriaNumber)
                RenderFourthStep = true;
            else
                RenderThirdStep = true;

            return Page();
        }

        public IActionResult OnPostSubmitAlternative(string AlternativeName)
        {
            sessionId = BLLLogic.GetSessionId();
            BLLLogic.AddAlternative(new AlternativeFAHP(AlternativeName), sessionId);
            alternativeNumber = BLLLogic.GetSessionAlternariveNumber(sessionId);

            Alternatives = BLLLogic.GetAllAlternative(sessionId).ToList();

            RenderFirstStep = false;
            if (alternativeNumber == Alternatives.Count)
                RenderThirdStep = true;
            else
                RenderSecondStep = true;

            return Page();
        }
        public IActionResult OnPostSubmitCriteriaMatrix(string Matrix)
        {
            sessionId = BLLLogic.GetSessionId();
            alternativeNumber = BLLLogic.GetSessionAlternariveNumber(sessionId);

            BLLLogic.UpdateCriteria(sessionId, Matrix);

            Criterias = BLLLogic.GetAllCriteria(sessionId).ToList();
            Alternatives = BLLLogic.GetAllAlternative(sessionId).ToList();

            Criterias = BLLLogic.ParseMatrixForCr(Criterias);
            Criterias = BLLLogic.ParseMatrixForAlt(Criterias, alternativeNumber);

            MatrixOfPairedCompRender = true;
            RenderFirstStep = false;
            RenderInput = false;
            ActivateRun = true;
            return Page();
        }

        public IActionResult OnPostStartAlg()
        {
            sessionId = BLLLogic.GetSessionId();
            criteriaNumber = BLLLogic.GetSessionCriteriaNumber(sessionId);
            alternativeNumber = BLLLogic.GetSessionAlternariveNumber(sessionId);


            Alternatives = BLLLogic.GetAllAlternative(sessionId).ToList();
            Criterias = BLLLogic.GetAllCriteria(sessionId).ToList();

            Criterias = BLLLogic.ParseMatrixForCr(Criterias);
            Criterias = BLLLogic.ParseMatrixForAlt(Criterias, alternativeNumber);

            Alternatives = BLLLogic.StartFAhp(Criterias, Alternatives);

            Alternatives = BLLLogic.SortFinalScore(Alternatives);

            RenderResult = true;
            MatrixOfPairedCompRender = true;
            RenderInput = false;

            return Page();
        }
    }
}