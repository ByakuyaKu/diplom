using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.AHP.Entities
{
    public class Alternative
    {
        public int AlternativeID { get; set; }
        public string AlternativeName { get; set; }

        public List<Criteria> AlternativeCriterias { get; set; }

        public int[,] MatrixOfPairedComparisons { get; set; }

        //public double[] VectorP { get; set; }

        //public string Matrix { get; set; }

        public Alternative(int id, string name)
        {
            AlternativeID = id;
            AlternativeName = name;
            AlternativeCriterias = new List<Criteria>();
        }

        public Alternative(string name, int criterialnum)
        {
            AlternativeName = name;
            AlternativeCriterias = new List<Criteria>(criterialnum);
        }
        public Alternative(string name)
        {
            AlternativeName = name;
            AlternativeCriterias = new List<Criteria>();
        }

        public Alternative()
        {
            AlternativeCriterias = new List<Criteria>();
        }

        //public Alternative(string name, string matrix)
        //{
        //    Matrix = matrix;
        //    AlternativeName = name;
        //    AlternativeCriterias = new List<Criteria>();
        //}
    }
}
