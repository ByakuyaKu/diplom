using System;
using System.Collections.Generic;
using System.Linq;
using WebApp.AHP.BLL.Interfaces;
using WebApp.AHP.DAL.Interfaces;
using WebApp.AHP.Entities;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Complex;

namespace WebApp.AHP.BLL    
{
    public class AHPLogic : IAHPLogic
    {
        private readonly IAHPDao _ahpDao;

        public AHPLogic(IAHPDao ahpDao)
        {
            _ahpDao = ahpDao;
        }

        public int AddCriteria(Criteria criteria)
        {
            return _ahpDao.AddCriteria(criteria);
        }

        public void DeleteById(int id)
        {
            _ahpDao.DeleteById(id);
        }

        public IEnumerable<Criteria> GetAll()
        {
            return _ahpDao.GetAll();
        }

        //public Criteria AhpAlgorytm()
        //{
        //    var result = 0;



        //    return result;
        //}

        private void BuildRelativeWeightMatrix()
        {

        }

        private void BuildMatrixOfPairedComparisons()
        {
            
        }
    }
}
