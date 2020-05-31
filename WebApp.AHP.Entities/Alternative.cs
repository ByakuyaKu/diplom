using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.AHP.Entities
{
    public class Alternative
    {
        public int AlternativeID { get; set; }
        public string AlternativeName { get; set; }

        public double FinalScore { get; set; }


        public Alternative(int id, string name)
        {
            AlternativeID = id;
            AlternativeName = name;
        }

        public Alternative(string name, int criterialnum)
        {
            AlternativeName = name;
        }
        public Alternative(string name)
        {
            AlternativeName = name;
            FinalScore = 0;
        }

        public Alternative()
        {
        }
    }
}
