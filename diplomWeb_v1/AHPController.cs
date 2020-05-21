using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.AHP.BLL.Interfaces;
using WebApp.AHP.Common;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace diplomWeb_v1
{
    public class AHPController : Controller
    {
        private readonly IAHPLogic BLLLogic = DependencyResolver.AHPLogic;
        // GET: /<controller>/
        public IActionResult Index(int CriteriaNumber)
        {
            //BLLLogic.AddSession(CriteriaNumberь );
            return View();
        }
    }
}
