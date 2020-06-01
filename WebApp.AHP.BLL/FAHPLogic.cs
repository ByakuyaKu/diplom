using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApp.AHP.BLL.Interfaces;
using WebApp.AHP.DAL.Interfaces;
using WebApp.AHP.Entities;

namespace WebApp.AHP.BLL
{
    public class FAHPLogic : IFAHPLogic
    {
        private readonly IFAHPDAO _fahpDao;

        public FAHPLogic(IFAHPDAO fahpDao)
        {
            _fahpDao = fahpDao;
        }
        public int AddSession(int criterianumber, int alternativenumber) => _fahpDao.AddSession(criterianumber, alternativenumber);
        public int GetSessionId() => _fahpDao.GetSessionId();
        public int GetSessionAlternariveNumber(int sessionid) => _fahpDao.GetSessionAlternariveNumber(sessionid);
        public int GetSessionCriteriaNumber(int sessionid) => _fahpDao.GetSessionCriteriaNumber(sessionid);
        public IEnumerable<CriteriaFAHP> GetCriteriaName(int sessionid) => _fahpDao.GetCriteriaName(sessionid);
        public IEnumerable<AlternativeFAHP> GetAllAlternative(int sessionid) => _fahpDao.GetAllAlternative(sessionid);
        public IEnumerable<CriteriaFAHP> GetAllCriteria(int sessionid) => _fahpDao.GetAllCriteria(sessionid);
        public int AddCriteria(CriteriaFAHP criteria, string matrix, int sessionid) => _fahpDao.AddCriteria(criteria, matrix, sessionid);
        public int AddAlternative(AlternativeFAHP alternative, int sessionId) => _fahpDao.AddAlternative(alternative, sessionId);
        public IEnumerable<CriteriaFAHP> GetAllCriteriaAltMatrOnly(int sessionid) => _fahpDao.GetAllCriteriaAltMatrOnly(sessionid);
        public List<AlternativeFAHP> StartFAhp(IEnumerable<CriteriaFAHP> criterias, List<AlternativeFAHP> alternatives) => StartFAHP(Parse(criterias, alternatives.Count), alternatives);
        public void UpdateCriteria(int sessionid, string matrix) => _fahpDao.UpdateCriteria(sessionid, matrix);
        public bool ValidationMatrix(string matrix, int size)
        {
            matrix = SplitMatrix(matrix);
            if (matrix.Length != Math.Pow(size, 2) * 3)
                return false;

            for (int i = 0; i < matrix.Length; i += 3 * (size + 1))
                if (matrix.Substring(i, 3) != "EQn" && matrix.Substring(i, 3) != "EQr")
                    return false;

            for (int i = 0; i < matrix.Length; i+=3)
                if (matrix.Substring(i, 3) != "EQn" && matrix[i] != '1' && matrix.Substring(i, 3) != "EQr" && 
                    matrix.Substring(i, 3) != "WKn" && matrix.Substring(i, 3) != "WKr" && matrix.Substring(i, 3) != "FSn" && 
                    matrix.Substring(i, 3) != "FSr" && matrix.Substring(i, 3) != "VSn" && matrix.Substring(i, 3) != "VSr" && 
                    matrix.Substring(i, 3) != "ABn" && matrix.Substring(i, 3) != "ABr")
                    return false;

            return true;
        }
        public List<AlternativeFAHP>SortFinalScore(List<AlternativeFAHP> alternatives)
        {
            for (int i = 0; i < alternatives.Count; i++)
                for (int j = i + 1; j < alternatives.Count; j++)
                    if (alternatives[i].FinalScore < alternatives[j].FinalScore)
                    {
                        var tmp = alternatives[i].FinalScore;
                        alternatives[i].FinalScore = alternatives[j].FinalScore;
                        alternatives[j].FinalScore = tmp;
                    }

            return alternatives;
        }
        private string SplitMatrix(string matrix)
        {
            var result = "";
            for (int i = 0; i < matrix.Length; i++)
            {
                if (!(matrix[i] == ';' || matrix[i] == ' ' || matrix[i] == '\r' || matrix[i] == '\n' || matrix[i] == ','))
                    result += matrix[i];
            }
            return result;
        }

        private List<CriteriaFAHP> Parse(IEnumerable<CriteriaFAHP> criterias, int alternativenumber) => ParseMatrixForAlt(ParseMatrixForCr(criterias), alternativenumber);
        public List<CriteriaFAHP> ParseMatrixForAlt(IEnumerable<CriteriaFAHP> criterias, int alternativenumber)
        {
            List<CriteriaFAHP> ParsedAlternatives = new List<CriteriaFAHP>();
            ParsedAlternatives = criterias.ToList<CriteriaFAHP>();

            for (int i = 0; i < ParsedAlternatives.Count; i++)
            {
                ParsedAlternatives[i].MatrixAltPairedComp = new double[alternativenumber, alternativenumber];
                ParsedAlternatives[i].MatrixAltPairedCompStr = SplitMatrix(ParsedAlternatives[i].MatrixAltPairedCompStr);

                for (int j = 0; j < ParsedAlternatives[i].MatrixAltPairedCompStr.Length;)
                    for (int c = 0; c < alternativenumber; c++)
                        for (int r = 0; r < alternativenumber; r++, j+=3)
                            ParsedAlternatives[i].MatrixAltPairedComp[c, r] = ParseInput(ParsedAlternatives[i].MatrixAltPairedCompStr.Substring(j, 3));
            }

            return ParsedAlternatives;
        }

        private double ParseInput(string s)
        {
            if (s == "EQn" || s == "EQr")
                return 1;
            else if (s == "WKn")
                return 3;
            else if (s == "FSn")
                return 5;
            else if (s == "VSn")
                return 7;
            else if (s == "ABn")
                return 9;
            else if (s == "WKr")
                return Convert.ToDouble(1) / 3;
            else if (s == "FSr")
                return Convert.ToDouble(1) / 5;
            else if (s == "VSr")
                return Convert.ToDouble(1) / 7;
            else
                return Convert.ToDouble(1) / 9;
        }
        public List<CriteriaFAHP> ParseMatrixForCr(IEnumerable<CriteriaFAHP> criterias)
        {
            List<CriteriaFAHP> ParsedCriterias = new List<CriteriaFAHP>();
            ParsedCriterias = criterias.ToList<CriteriaFAHP>();

            for (int i = 0; i < ParsedCriterias.Count; i++)
            {
                ParsedCriterias[i].MatrixCrPairedComp = new double[ParsedCriterias.Count, ParsedCriterias.Count];
                ParsedCriterias[i].MatrixCrPairedCompStr = SplitMatrix(ParsedCriterias[i].MatrixCrPairedCompStr);

                for (int j = 0; j < ParsedCriterias[i].MatrixCrPairedCompStr.Length;)
                    for (int c = 0; c < ParsedCriterias.Count; c++)
                        for (int r = 0; r < ParsedCriterias.Count; r++, j+=3)
                            ParsedCriterias[i].MatrixCrPairedComp[c, r] = ParseInput(ParsedCriterias[i].MatrixCrPairedCompStr.Substring(j, 3));
            }

            return ParsedCriterias;
        }
        private List<AlternativeFAHP> StartFAHP(List<CriteriaFAHP> criterias, List<AlternativeFAHP> alternatives)
        {
            criterias = GetWforCr(criterias);
            double[,] result = new double[criterias.Count, alternatives.Count];
            result = AlternativeFAHP.FAHP(criterias, alternatives);

            double[,] resultT = new double[alternatives.Count, criterias.Count];
            resultT = T(result, alternatives.Count, criterias.Count);

            double[] ResultCrWeights = new double[alternatives.Count];
            for (int i = 0; i < criterias.Count; i++)
                ResultCrWeights[i] = criterias[i].CriteriaWeight;

            alternatives = GetFinalScoreAlternative(resultT, ResultCrWeights, alternatives, criterias);

            return alternatives;
        }
        private List<AlternativeFAHP> GetFinalScoreAlternative(double[,] resultT, double[] ResultCrWeights, List<AlternativeFAHP> alternatives, List<CriteriaFAHP> criterias)
        {
            for (int i = 0; i < alternatives.Count; i++) 
             alternatives[i].FinalScore = MultiplyMatrixByVecor(resultT, ResultCrWeights, alternatives.Count, criterias.Count)[i];

            return alternatives;
        }

        private double[] MultiplyMatrixByVecor(double[,] matrix, double[] vector, int alternativeNumber, int criteriaNumber)
        {
            var result = new double[alternativeNumber];
            for (int i = 0; i < alternativeNumber; i++)
                for (int j = 0; j < criteriaNumber; j++)
                    result[i] += matrix[i, j] * vector[j];

            return result;
        }
        private double[,] T(double[,] matrix, int alternativeNumber, int criteriaNumber)
        {
            double[,] tmp = new double[alternativeNumber, criteriaNumber];
            for (int i = 0; i < criteriaNumber; i++)
                for (int j = 0; j < alternativeNumber; j++)
                    tmp[j, i] = matrix[i, j];
            return tmp;
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
