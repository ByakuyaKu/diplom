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

        static public List<CriteriaFAHP> GetWforCr(List<CriteriaFAHP> criterias)
        {
            MatrixToTriangular(criterias);
            criterias = SumOfRows(criterias);
            criterias = InverseMatrix(criterias);
            criterias = GetS(criterias);
            criterias = GetVForS(criterias);
            criterias = GetMinV(criterias);
            criterias = GetW(criterias);


            return criterias;
        }
        static private List<CriteriaFAHP> GetW(List<CriteriaFAHP> criterias)
        {
            double SumV = 0;
            for (int j = 0; j < criterias.Count; j++)
                SumV += criterias[j].MinV;
            for (int i = 0; i < criterias.Count; i++)
                criterias[i].CriteriaWeight = criterias[i].MinV / SumV;

            return criterias;
        }

        static private List<CriteriaFAHP> GetMinV(List<CriteriaFAHP> criterias)
        {
            for (int i = 0; i < criterias.Count; i++)
                criterias[i].MinV = criterias[i].V.Min();

            return criterias;
        }

        static private List<CriteriaFAHP> GetVForS(List<CriteriaFAHP> criterias)
        {
            for (int i = 0; i < criterias.Count; i++)
            {
                criterias[i].V = new List<double>();
                for (int j = 0; j < criterias.Count; j++)
                {
                    if (i == j)
                        continue;
                    else if (criterias[i].S.TriangulatVector[1] >= criterias[j].S.TriangulatVector[1])
                        criterias[i].V.Add(1);
                    else if (criterias[i].S.TriangulatVector[2] <= criterias[j].S.TriangulatVector[0])
                        criterias[i].V.Add(0);
                    else
                        criterias[i].V.Add((criterias[j].S.TriangulatVector[0] - criterias[i].S.TriangulatVector[2]) / (criterias[i].S.TriangulatVector[1] - criterias[i].S.TriangulatVector[2] - criterias[j].S.TriangulatVector[1] + criterias[j].S.TriangulatVector[0]));
                }
            }
            return criterias;
        }

        static private List<CriteriaFAHP> GetS(List<CriteriaFAHP> criterias)
        {
            for (int i = 0; i < criterias.Count; i++)
            {
                criterias[i].S = new TriangularNumber(0, 0, 0);
                for (int j = 0; j < 3; j++)
                {
                    criterias[i].S.TriangulatVector[j] = criterias[i].MatrixSumTriangularInversedCr.TriangulatVector[j] * criterias[i].SumVectorRows.TriangulatVector[j];
                }
            }

            return criterias;
        }

        static private List<CriteriaFAHP> InverseMatrix(List<CriteriaFAHP> criterias)
        {
            double tmp = 0;
            criterias[0].MatrixSumTriangularInversedCr = new TriangularNumber(0, 0, 0);

            for (int j = 0; j < 3; j++)
                for (int ii = 0; ii < criterias.Count; ii++)
                    criterias[0].MatrixSumTriangularInversedCr.TriangulatVector[j] += criterias[ii].SumVectorRows.TriangulatVector[j];

            tmp = criterias[0].MatrixSumTriangularInversedCr.TriangulatVector[0];
            criterias[0].MatrixSumTriangularInversedCr.TriangulatVector[0] = criterias[0].MatrixSumTriangularInversedCr.TriangulatVector[2];
            criterias[0].MatrixSumTriangularInversedCr.TriangulatVector[2] = tmp;

            for (int j = 0; j < 3; j++)
                criterias[0].MatrixSumTriangularInversedCr.TriangulatVector[j] = Convert.ToDouble(1) / criterias[0].MatrixSumTriangularInversedCr.TriangulatVector[j];

            for (int i = 1; i < criterias.Count; i++)
            {
                criterias[i].MatrixSumTriangularInversedCr = new TriangularNumber(0, 0, 0);
                criterias[i].MatrixSumTriangularInversedCr.TriangulatVector = criterias[0].MatrixSumTriangularInversedCr.TriangulatVector;
            }

            return criterias;
        }

        static private List<CriteriaFAHP> SumOfRows(List<CriteriaFAHP> criterias)
        {
            for (int i = 0; i < criterias.Count; i++)
            {
                criterias[i].SumVectorRows = new TriangularNumber(0, 0, 0);
                for (int j = 0; j < criterias.Count; j++)
                    for (int c = 0; c < 3; c++)
                        criterias[i].SumVectorRows.TriangulatVector[c] += criterias[i].MatrixTriangularCr[i, j].TriangulatVector[c];
            }

            return criterias;
        }

        static private List<CriteriaFAHP> MatrixToTriangular(List<CriteriaFAHP> criterias)
        {
            var SaatiTable = Saati.GetSaatiTable();
            var SaatiTableRev = Saati.GetSaatiTableReversed();

            for (int i = 0; i < criterias.Count; i++)
            {
                criterias[i].MatrixTriangularCr = new TriangularNumber[criterias.Count, criterias.Count];
                for (int j = 0; j < criterias.Count; j++)
                {
                    for (int c = 0; c < criterias.Count; c++)
                    {
                        if (criterias[i].MatrixCrPairedComp[j, c] >= 1)
                        {
                            criterias[i].MatrixTriangularCr[j, c] = SaatiTable[(Convert.ToInt32(criterias[i].MatrixCrPairedComp[j, c]) - 1)].TrNum;
                        }
                        else
                        {
                            for (int k = 0; k < SaatiTableRev.Count; k++)
                                if (SaatiTableRev[k].Index == criterias[i].MatrixCrPairedComp[j, c])
                                    criterias[i].MatrixTriangularCr[j, c] = SaatiTableRev[k].TrNum;
                        }
                    }
                }
            }
            return criterias;
        }
    }
}

