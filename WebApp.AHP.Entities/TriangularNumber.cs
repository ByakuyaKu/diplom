using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.AHP.Entities
{
    public class TriangularNumber
    {
        public double[] TriangulatVector { get; set; }

        public TriangularNumber(double l, double m, double u)
        {
            TriangulatVector = new double[3] { l, m, u };
        }
    }
}
