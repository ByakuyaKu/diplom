using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.ProjectModel;
using WebApp.AHP.BLL.Interfaces;
using WebApp.AHP.Common;
using WebApp.AHP.DAL.Interfaces;
using WebApp.AHP.Entities;

namespace diplomWeb_v1.Pages.Alg
{
    public class AHP_FuzzyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;


        private readonly IAHPLogic BLLLogic = DependencyResolver.AHPLogic;

        public int sessionId { get; set; }
        public int criteriaNumber { get; set; }
        public int alternativeNumber { get; set; }
        public List<Alternative> alternatives { get; set; }
        public bool MatrixOfPairedCompRender { get; set; }
        public bool RenderFirstStep { get; set; }
        public bool RenderSecondStep { get; set; }
        public bool RenderThirdStep { get; set; }
        public bool RenderInput { get; set; }
        public bool RenderResult { get; set; }
        public bool ActivateRun { get; set; }

        public AHP_FuzzyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
            alternatives = new List<Alternative>();
            RenderResult = false;
            MatrixOfPairedCompRender = false;
            RenderFirstStep = true;
            RenderSecondStep = false;
            RenderThirdStep = false;
            RenderInput = true;
            ActivateRun = false;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPostSubmitCriteriaNumber(int CriteriaNumber, int AlternativeNumber)
        {
            sessionId = BLLLogic.AddSession(CriteriaNumber, AlternativeNumber);
            criteriaNumber = BLLLogic.GetSessionCriteriaNumber(sessionId);
            alternativeNumber = BLLLogic.GetSessionAlternariveNumber(sessionId);

            RenderFirstStep = false;
            RenderSecondStep = true;

            return Page();
        }

        public IActionResult OnPostSubmitCriteriaName(string CriteriaName)
        {
            sessionId = BLLLogic.GetSessionId();
            criteriaNumber = BLLLogic.GetSessionCriteriaNumber(sessionId);
            BLLLogic.AddCriteria(new Criteria(CriteriaName), sessionId);

            RenderFirstStep = false;
            if (BLLLogic.GetCriteriaName(sessionId).ToList().Count == criteriaNumber)
                RenderThirdStep = true;
            else
                RenderSecondStep = true;

            return Page();
        }

        public IActionResult OnPostSubmitAlternative(string AlternativeName, string Matrix)
        {
            sessionId = BLLLogic.GetSessionId();
            BLLLogic.AddAlternative(new Alternative(AlternativeName), Matrix, sessionId);
            criteriaNumber = BLLLogic.GetSessionCriteriaNumber(sessionId);
            alternativeNumber = BLLLogic.GetSessionAlternariveNumber(sessionId);
            int numbernputAlternatives = BLLLogic.GetAlternative(sessionId).ToList().Count;

            alternatives = BLLLogic.GetAlternative(sessionId).ToList();
            for (int i = 0; i < alternatives.Count; i++)
                for (int j = 0; j < criteriaNumber; j++)
                    alternatives[i].AlternativeCriterias.Add(BLLLogic.GetCriteriaName(sessionId).ToList()[j]);

            alternatives = BLLLogic.StartAhp(alternatives, criteriaNumber);

            MatrixOfPairedCompRender = true;
            RenderFirstStep = false;
            RenderSecondStep = false;
            if (alternativeNumber == numbernputAlternatives)
            {
                ActivateRun = true;
                RenderInput = false;
            }
            else
                RenderThirdStep = true;

            return Page();
        }

        public IActionResult OnPostStartAlg()
        {
            sessionId = BLLLogic.GetSessionId();
            BLLLogic.GetAlternative(sessionId);
            criteriaNumber = BLLLogic.GetSessionCriteriaNumber(sessionId);
            alternativeNumber = BLLLogic.GetSessionAlternariveNumber(sessionId);
            alternatives = BLLLogic.StartAhp(BLLLogic.GetAlternative(sessionId), criteriaNumber);

            for (int i = 0; i < alternatives.Count; i++)
                for (int j = 0; j < criteriaNumber; j++)
                    alternatives[i].AlternativeCriterias.Add(BLLLogic.GetCriteriaName(sessionId).ToList()[j]);

            RenderResult = true;
            MatrixOfPairedCompRender = true;
            RenderInput = false;

            return Page();
        }
    }
}