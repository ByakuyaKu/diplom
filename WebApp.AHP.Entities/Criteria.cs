using System;
using System.Collections.Generic;

namespace WebApp.AHP.Entities
{
    public class Criteria
    {
        public int CriteriaID { get; set; }
        public string CriteriaName { get; set; }

        public double CriteriaWeight { get; set; }
        public int[,] MatrixOfPairedComparisons { get; set; }

        public string Matrix { get; set; }

        public double[] VectorP { get; set; }

        public Criteria(string name)
        {
            CriteriaName = name;
        }

        public Criteria(string name, double criteriaWeight)
        {
            CriteriaName = name;
            CriteriaWeight = criteriaWeight;
            //Matrix = matrix;
        }

        public Criteria()
        {
        }
    }
}
