using System;
using System.Collections.Generic;
using System.Text;
using WebApp.AHP.DAL.Interfaces;
using WebApp.AHP.DAL;
using WebApp.AHP.BLL.Interfaces;
using WebApp.AHP.BLL;

namespace WebApp.AHP.Common
{
    public static class DependencyResolver
    {
        private static readonly IAHPLogic _ahpLogic;
        private static readonly IAHPDao _ahpDao;

        private static readonly IFAHPLogic _fahpLogic;
        private static readonly IFAHPDAO _fahpDao;

        public static IAHPLogic AHPLogic => _ahpLogic;
        public static IAHPDao AHPDao => _ahpDao;

        public static IFAHPLogic FAHPLogic => _fahpLogic;
        public static IFAHPDAO FAHPDAO => _fahpDao;

        static DependencyResolver()
        {
            _ahpDao = new AHPDao();
            _ahpLogic = new AHPLogic(_ahpDao);

            _fahpDao = new FAHPDAO();
            _fahpLogic = new FAHPLogic(_fahpDao);
        }
    }
}
