using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApp.AHP.Entities
{
    public class CriteriaFAHP
    {
        public int CriteriaID { get; set; }
        public string CriteriaName { get; set; }
        public double CriteriaWeight { get; set; }
        public double[,] MatrixCrPairedComp { get; set; }
        public double[,] MatrixAltPairedComp { get; set; }
        public TriangularNumber[,] MatrixTriangularAlt { get; set; }
        public TriangularNumber[,] MatrixTriangularCr { get; set; }
        public TriangularNumber MatrixSumTriangularInversedCr { get; set; }
        public string MatrixCrPairedCompStr { get; set; }
        public string MatrixAltPairedCompStr { get; set; }
        public TriangularNumber SumVectorRows { get; set; }
        public TriangularNumber S { get; set; }
        public List<double> V { get; set; }
        public double MinV { get; set; }

        public CriteriaFAHP(string name)
        {
            CriteriaName = name;
        }

        public CriteriaFAHP(string name, double[,] matrix, double[,] matrix2)
        {
            CriteriaName = name;
            CriteriaWeight = 0;
            MatrixCrPairedComp = matrix;
            MatrixAltPairedComp = matrix2;
            SumVectorRows = new TriangularNumber(0, 0, 0);
            V = new List<double>();
            MatrixSumTriangularInversedCr = new TriangularNumber(0, 0, 0);
            S = new TriangularNumber(0, 0, 0);
        }

        public CriteriaFAHP(string name, string matrix, string matrix2)
        {
            CriteriaName = name;
            CriteriaWeight = 0;
            MatrixCrPairedCompStr = matrix;
            MatrixAltPairedCompStr = matrix2;
            SumVectorRows = new TriangularNumber(0, 0, 0);
            V = new List<double>();
            MatrixSumTriangularInversedCr = new TriangularNumber(0, 0, 0);
            S = new TriangularNumber(0, 0, 0);
        }

        public CriteriaFAHP()
        {
        }
    }
}

