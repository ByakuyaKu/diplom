using System;

namespace WebApp.AHP.Entities
{
    public class Criteria
    {
        public int CriteriaID { get; set; }
        public string CriteriaName { get; set; }

        public double CriteriaWeight { get; set; }

        //public Criteria(int id, string name, double weight)
        //{
        //    CriteriaID = id;
        //    CriteriaName = name;
        //    CriteriaWeight = weight;
        //}
    }
}
