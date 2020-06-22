using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApp.AHP.Entities
{
    public class AlternativeFAHP
    {
        public int AlternativeID { get; set; }
        public string AlternativeName { get; set; }

        public TriangularNumber MatrixSumTriangularInversedAlt { get; set; }
        public double AlternativeWeight { get; set; }
        public double[,] MatrixAlt { get; set; }
        public double[,] MatrixAltPairedComp { get; set; }
        public TriangularNumber[,] MatrixTriangularAlt { get; set; }
        public TriangularNumber SumVectorRows { get; set; }
        public TriangularNumber S { get; set; }
        public List<double> V { get; set; }
        public double MinV { get; set; }
        public double FinalScore { get; set; }


        public AlternativeFAHP(int id, string name)
        {
            AlternativeID = id;
            AlternativeName = name;
        }

        public AlternativeFAHP(string name)
        {
            AlternativeName = name;
            MatrixSumTriangularInversedAlt = new TriangularNumber(0, 0, 0);
            AlternativeWeight = 0;
            FinalScore = 0;
            SumVectorRows = new TriangularNumber(0, 0, 0);
            SumVectorRows = new TriangularNumber(0, 0, 0);
            V = new List<double>();
            MatrixSumTriangularInversedAlt = new TriangularNumber(0, 0, 0);
            S = new TriangularNumber(0, 0, 0);
        }

        public AlternativeFAHP()
        {
        }
        
    }
}

