using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.AHP.Entities
{
    public class Saati
    {
        public double Index { get; set; }
        public TriangularNumber TrNum { get; set; }

        private Saati(double index, TriangularNumber tn)
        {
            Index = index;
            TrNum = tn;
        }

        public static List<Saati> GetSaatiTable()
        {
            var SaatiTable = new List<Saati>();
            SaatiTable.Add(new Saati(1, new TriangularNumber(Convert.ToDouble(0.5), Convert.ToDouble(1), Convert.ToDouble(1.5))));
            SaatiTable.Add(new Saati(2, new TriangularNumber(Convert.ToDouble(0.5), Convert.ToDouble(1), Convert.ToDouble(1.5))));
            SaatiTable.Add(new Saati(3, new TriangularNumber(1, 1.5, 2)));
            SaatiTable.Add(new Saati(4, new TriangularNumber(1, 1, 1)));
            SaatiTable.Add(new Saati(5, new TriangularNumber(1.5, 2, 2.5)));
            SaatiTable.Add(new Saati(6, new TriangularNumber(1, 1, 1)));
            SaatiTable.Add(new Saati(7, new TriangularNumber(2, 2.5, 3)));
            SaatiTable.Add(new Saati(8, new TriangularNumber(1, 1, 1)));
            SaatiTable.Add(new Saati(9, new TriangularNumber(2.5, 3, 3.5)));

            return SaatiTable;
        }

        public static List<Saati> GetSaatiTableReversed()
        {
            var SaatiTable = new List<Saati>();
            SaatiTable.Add(new Saati(1, new TriangularNumber(Convert.ToDouble(0.5), Convert.ToDouble(1), Convert.ToDouble(1.5))));
            SaatiTable.Add(new Saati(Convert.ToDouble(1) / 3, new TriangularNumber(0.5, Convert.ToDouble(2) / 3, 1)));
            SaatiTable.Add(new Saati(Convert.ToDouble(1) / 5, new TriangularNumber(0.4, 0.5, Convert.ToDouble(2) / 3)));
            SaatiTable.Add(new Saati(Convert.ToDouble(1) / 7, new TriangularNumber(Convert.ToDouble(1) / 3, 0.4, 0.5)));
            SaatiTable.Add(new Saati(Convert.ToDouble(1) / 9, new TriangularNumber(Convert.ToDouble(2) / 7, Convert.ToDouble(1) / 3, 0.4)));

            return SaatiTable;
        }
    }
}
