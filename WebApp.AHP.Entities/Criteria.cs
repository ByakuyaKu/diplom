using System;
using System.Collections.Generic;

namespace WebApp.AHP.Entities
{
    public class Criteria
    {
        public int CriteriaID { get; set; }
        public string CriteriaName { get; set; }

        public string CriteriaWeight { get; set; }

        public Criteria(string name)
        {
            CriteriaName = name;
        }

        public Criteria()
        {
        }
    }
}
