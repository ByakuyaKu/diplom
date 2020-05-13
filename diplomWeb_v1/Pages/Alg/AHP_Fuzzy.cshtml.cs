using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace diplomWeb_v1.Pages.Alg
{
    public class AHP_FuzzyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        //public List<Criteria> Criterias { get; set; }
        public string Message { get; private set; }

        public AHP_FuzzyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
            //Criterias = new List<Criteria>();
        }

        public void OnGet()
        {
            
        }

        public void OnPost(string CriteriaName, double CriteriaWeight)
        {
            //Criterias.Add(new Criteria(CriteriaName, CriteriaWeight));
        }
    }
}