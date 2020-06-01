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
        //public string Matrix { get; set; }
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
        static public double[,] FAHP(List<CriteriaFAHP> criterias, List<AlternativeFAHP> alternatives)
        {
            double[,] result = new double[criterias.Count, alternatives.Count];

            for (int i = 0; i < criterias.Count; i++)
            {
                MatrixToTriangular(criterias, alternatives);
                alternatives = SumOfRows(criterias[i].MatrixTriangularAlt, alternatives);
                alternatives = InverseMatrix(alternatives);
                alternatives = GetS(alternatives);
                alternatives = GetVForS(alternatives);
                alternatives = GetMinV(alternatives);
                alternatives = GetW(alternatives);
                for (int j = 0; j < alternatives.Count; j++)
                    result[i, j] = alternatives[j].AlternativeWeight;

                for (int j = 0; j < alternatives.Count; j++)
                {
                    alternatives[j].AlternativeWeight = 0;
                    alternatives[j].S = new TriangularNumber(0, 0, 0);
                    alternatives[j].SumVectorRows = new TriangularNumber(0, 0, 0);
                    alternatives[j].MatrixSumTriangularInversedAlt = new TriangularNumber(0, 0, 0);
                    alternatives[j].V = new List<double>();
                }
            }
            return result;
        }

        private static List<AlternativeFAHP> GetW(List<AlternativeFAHP> alternatives)
        {
            double SumV = 0;
            for (int i = 0; i < alternatives.Count; i++)
                SumV += alternatives[i].MinV;

            for (int i = 0; i < alternatives.Count; i++)
                alternatives[i].AlternativeWeight = alternatives[i].MinV / SumV;

            return alternatives;
        }

        private static List<AlternativeFAHP> GetMinV(List<AlternativeFAHP> alternatives)
        {
            for (int i = 0; i < alternatives.Count; i++)
                alternatives[i].MinV = alternatives[i].V.Min();

            return alternatives;
        }

        private static List<AlternativeFAHP> GetVForS(List<AlternativeFAHP> alternatives)
        {
            for (int i = 0; i < alternatives.Count; i++)
            {
                alternatives[i].V = new List<double>();
                for (int j = 0; j < alternatives.Count; j++)
                {
                    if (i == j)
                        continue;

                    if (alternatives[i].S.TriangulatVector[2] <= alternatives[j].S.TriangulatVector[0])
                        alternatives[i].V.Add(0);
                    else if (alternatives[i].S.TriangulatVector[1] >= alternatives[j].S.TriangulatVector[1])
                        alternatives[i].V.Add(1);
                    else
                        alternatives[i].V.Add((alternatives[j].S.TriangulatVector[0] - alternatives[i].S.TriangulatVector[2]) / ((alternatives[i].S.TriangulatVector[1] - alternatives[i].S.TriangulatVector[2]) - (alternatives[j].S.TriangulatVector[1] - alternatives[j].S.TriangulatVector[0])));
                }
            }
            return alternatives;
        }

        private static List<AlternativeFAHP> GetS(List<AlternativeFAHP> alternatives)
        {
            for (int i = 0; i < alternatives.Count; i++)
            {
                alternatives[i].S = new TriangularNumber(0, 0, 0);
                for (int j = 0; j < 3; j++)
                    alternatives[i].S.TriangulatVector[j] = alternatives[i].MatrixSumTriangularInversedAlt.TriangulatVector[j] * alternatives[i].SumVectorRows.TriangulatVector[j];
            }

            return alternatives;
        }

        private static List<AlternativeFAHP> InverseMatrix(List<AlternativeFAHP> alternatives)
        {
            double tmp = 0;
            alternatives[0].MatrixSumTriangularInversedAlt = new TriangularNumber(0, 0, 0);
            for (int j = 0; j < 3; j++)
                for (int ii = 0; ii < alternatives.Count; ii++)
                    alternatives[0].MatrixSumTriangularInversedAlt.TriangulatVector[j] += alternatives[ii].SumVectorRows.TriangulatVector[j];

            tmp = alternatives[0].MatrixSumTriangularInversedAlt.TriangulatVector[0];
            alternatives[0].MatrixSumTriangularInversedAlt.TriangulatVector[0] = alternatives[0].MatrixSumTriangularInversedAlt.TriangulatVector[2];
            alternatives[0].MatrixSumTriangularInversedAlt.TriangulatVector[2] = tmp;

            for (int j = 0; j < 3; j++)
                alternatives[0].MatrixSumTriangularInversedAlt.TriangulatVector[j] = Convert.ToDouble(1) / alternatives[0].MatrixSumTriangularInversedAlt.TriangulatVector[j];

            for (int i = 1; i < alternatives.Count; i++)
            {
                alternatives[i].MatrixSumTriangularInversedAlt = new TriangularNumber(0, 0, 0);
                alternatives[i].MatrixSumTriangularInversedAlt.TriangulatVector = alternatives[0].MatrixSumTriangularInversedAlt.TriangulatVector;
            }

            return alternatives;
        }

        private static List<AlternativeFAHP> SumOfRows(TriangularNumber[,] MatrixTriangularAlt, List<AlternativeFAHP> alternatives)
        {
            for (int i = 0; i < alternatives.Count; i++)
            {
                alternatives[i].SumVectorRows = new TriangularNumber(0, 0, 0);
                for (int j = 0; j < alternatives.Count; j++)
                    for (int c = 0; c < 3; c++)
                        alternatives[i].SumVectorRows.TriangulatVector[c] += MatrixTriangularAlt[i, j].TriangulatVector[c];
            }

            return alternatives;
        }

        private static List<AlternativeFAHP> MatrixToTriangular(List<CriteriaFAHP> criterias, List<AlternativeFAHP> alternatives)
        {
            var SaatiTable = Saati.GetSaatiTable();
            var SaatiTableRev = Saati.GetSaatiTableReversed();

            for (int i = 0; i < criterias.Count; i++)
            {
                criterias[i].MatrixTriangularAlt = new TriangularNumber[alternatives.Count, alternatives.Count];
                for (int j = 0; j < alternatives.Count; j++)
                    for (int c = 0; c < alternatives.Count; c++)
                    {
                        if (criterias[i].MatrixAltPairedComp[j, c] >= 1)
                        {
                            criterias[i].MatrixTriangularAlt[j, c] = SaatiTable[(Convert.ToInt32(criterias[i].MatrixAltPairedComp[j, c]) - 1)].TrNum;
                        }
                        else
                        {
                            for (int k = 0; k < SaatiTableRev.Count; k++)
                                if (SaatiTableRev[k].Index == criterias[i].MatrixAltPairedComp[j, c])
                                    criterias[i].MatrixTriangularAlt[j, c] = SaatiTableRev[k].TrNum;
                        }
                    }
            }

            return alternatives;
        }
    }
}

