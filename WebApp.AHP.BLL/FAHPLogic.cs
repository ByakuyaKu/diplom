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
            criterias = CriteriaFAHP.GetWforCr(criterias);
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
    }
}
