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
    public class AHPModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;


        private readonly IAHPLogic BLLLogic = DependencyResolver.AHPLogic;

        public int sessionId { get; set; }
        public int criteriaNumber { get; set; }
        public int alternativeNumber { get; set; }
        public List<Alternative> Alternatives { get; set; }
        public bool MatrixOfPairedCompRender { get; set; }
        public bool RenderFirstStep { get; set; }
        public bool RenderSecondStep { get; set; }
        public bool RenderThirdStep { get; set; }
        public bool RenderInput { get; set; }
        public bool RenderResult { get; set; }
        public bool ActivateRun { get; set; }
        public bool Error { get; set; }
        public List<Criteria> Criterias { get; set; }

        public AHPModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
            Alternatives = new List<Alternative>();
            Criterias = new List<Criteria>();
            RenderResult = false;
            MatrixOfPairedCompRender = false;
            RenderFirstStep = true;
            RenderSecondStep = false;
            RenderThirdStep = false;
            RenderInput = true;
            ActivateRun = false;
            Error = false;
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

        public IActionResult OnPostSubmitCriteria(string CriteriaName, double CriteriaWeight, string Matrix)
        {
            sessionId = BLLLogic.GetSessionId();
            criteriaNumber = BLLLogic.GetSessionCriteriaNumber(sessionId);
            alternativeNumber = BLLLogic.GetSessionAlternariveNumber(sessionId);

            if (!BLLLogic.ValidationMatrix(Matrix, alternativeNumber))
            {
                RenderFirstStep = false;
                Error = true;
                RenderThirdStep = true;
                //MatrixOfPairedCompRender = true;
                return Page();
            }

            BLLLogic.AddCriteria(new Criteria(CriteriaName, CriteriaWeight), Matrix,  sessionId);

            Criterias = BLLLogic.GetAllCriteria(sessionId).ToList();

            Alternatives = BLLLogic.GetAllAlternative(sessionId).ToList();

            Criterias = BLLLogic.StartAhp(Criterias, Alternatives);

            //MatrixOfPairedCompRender = true;
            RenderFirstStep = false;
            if (BLLLogic.GetCriteriaName(sessionId).ToList().Count == criteriaNumber)
            {
                ActivateRun = true;
                RenderInput = false;
            }
            else
                RenderThirdStep = true;

            return Page();
        }

        public IActionResult OnPostSubmitAlternative(string AlternativeName)
        {
            sessionId = BLLLogic.GetSessionId();
            BLLLogic.AddAlternative(new Alternative(AlternativeName), sessionId);
            criteriaNumber = BLLLogic.GetSessionCriteriaNumber(sessionId);
            alternativeNumber = BLLLogic.GetSessionAlternariveNumber(sessionId);

            Alternatives = BLLLogic.GetAllAlternative(sessionId).ToList();

            RenderFirstStep = false;
            if (alternativeNumber == Alternatives.Count)
                RenderThirdStep = true;
            else
                RenderSecondStep = true;

            return Page();
        }

        public IActionResult OnPostStartAlg()
        {
            sessionId = BLLLogic.GetSessionId();
            criteriaNumber = BLLLogic.GetSessionCriteriaNumber(sessionId);
            alternativeNumber = BLLLogic.GetSessionAlternariveNumber(sessionId);
            Alternatives = BLLLogic.GetAllAlternative(sessionId).ToList();
            Criterias = BLLLogic.StartAhp(BLLLogic.GetAllCriteria(sessionId), Alternatives);
            Alternatives = BLLLogic.SortFinalScore(Alternatives, Criterias);
            RenderFirstStep = false;
            RenderResult = true;
            MatrixOfPairedCompRender = true;
            RenderInput = false;

            return Page();
        }
    }
}