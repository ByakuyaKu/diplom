using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diplomWeb_v1.Pages.Alg
{
    public class Criteria
    {

        public string CriteriaName { get; set; }

        public double CriteriaWeight { get; set; }

        public Criteria(string name, double weight)
        {
            CriteriaName = name;
            CriteriaWeight = weight;
        }

    }
}
