using System;
using System.Collections.Generic;
using System.Linq;
using WebApp.AHP.BLL.Interfaces;
using WebApp.AHP.DAL.Interfaces;
using WebApp.AHP.Entities;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Complex;

namespace WebApp.AHP.BLL
{
    public class AHPLogic : IAHPLogic
    {
        private readonly IAHPDao _ahpDao;

        public AHPLogic(IAHPDao ahpDao)
        {
            _ahpDao = ahpDao;
        }

        public int GetSessionCriteriaNumber(int id)
        {
            return _ahpDao.GetSessionCriteriaNumber(id);
        }

        public int GetSessionAlternariveNumber(int id)
        {
            return _ahpDao.GetSessionAlternariveNumber(id);
        }

        public int GetSessionId()
        {
            return _ahpDao.GetSessionId();
        }

        public int AddCriteria(Criteria criteria, int sessionid)
        {
            return _ahpDao.AddCriteria(criteria, sessionid);
        }

        public int AddAlternative(Alternative alternative, string matrix, int sessionId)
        {
            return _ahpDao.AddAlternative(alternative, matrix, sessionId);
        }

        public int AddSession(int critriaNumber, int alternativenumber)
        {
            return _ahpDao.AddSession(critriaNumber, alternativenumber);
        }
        public IEnumerable<Alternative> GetAlternative(int sessionid)
        {
            return _ahpDao.GetAlternative(sessionid);
        }

        public IEnumerable<Criteria> GetCriteriaName(int sessionid)
        {
            return _ahpDao.GetCriteriaName(sessionid);
        }

        public void DeleteById(int id)
        {
            _ahpDao.DeleteById(id);
        }

        public IEnumerable<Criteria> GetAll()
        {
            return _ahpDao.GetAll();
        }

        public List<Alternative> StartAhp(IEnumerable<Alternative> alternatives, int criterianumber) => AhpAlgorytm(ParseMatrix(alternatives, criterianumber), criterianumber);

        public int GetInputCriteriaNumber(IEnumerable<Criteria> criterias) => criterias.ToList().Count;

        private string SplitMatrix(string matrix)
        {
            var result = "";
            for (int i = 0; i < matrix.Length; i++)
            {
                if (!(matrix[i] == ';' || matrix[i] == ' ' || matrix[i] =='\r' || matrix[i] == '\n' || matrix[i] == ','))
                    result += matrix[i];
            }
            return result;
        }

        private List<Alternative> ParseMatrix(IEnumerable<Alternative> alternatives, int criterianumber)
        {
            List<Alternative> ParsedAlternatives = new List<Alternative>();
            ParsedAlternatives = alternatives.ToList<Alternative>();

            for (int i = 0; i < ParsedAlternatives.Count; i++)
            {
                ParsedAlternatives[i].MatrixOfPairedComparisons = new int[criterianumber, criterianumber];
                ParsedAlternatives[i].Matrix = SplitMatrix(ParsedAlternatives[i].Matrix);

                for (int j = 0; j < ParsedAlternatives[i].Matrix.Length;)
                {
                    for (int c = 0; c < criterianumber; c++)
                        for (int r = 0; r < criterianumber; r++, j++)
                            ParsedAlternatives[i].MatrixOfPairedComparisons[c, r] = Int32.Parse((ParsedAlternatives[i].Matrix[j]).ToString());
                }
            }

            return ParsedAlternatives;
        }

        private List<Alternative> AhpAlgorytm(List<Alternative> alternatives, int criterianumber)
        {
            alternatives = VectorPInit(alternatives, criterianumber);

            var pnext = new double[criterianumber];

            for (int i = 0; i < alternatives.Count; i++)
            {
                for (int counter = 0; counter < 101; counter++)
                {
                    pnext = MultiplyMatrixByVecor(alternatives[i].MatrixOfPairedComparisons, alternatives[i].VectorP);

                    pnext = Normalize(pnext, VectorSum(alternatives[i].VectorP));

                    if (!Accuracy(alternatives[i].VectorP, pnext))
                        alternatives[i].VectorP = pnext;
                    else if (counter == 100)
                        Console.WriteLine("Error");
                    else
                        break;
                }
            }

            return alternatives;
        }

        private static List<Alternative> VectorPInit(List<Alternative> alternatives, int criterianumber)
        {
            for (int i = 0; i < alternatives.Count; i++)
            {
                alternatives[i].VectorP = new double[criterianumber];
                for (int j = 0; j < criterianumber; j++)
                    alternatives[i].VectorP[j] = 1;
            }

            return alternatives;
        }

        private static double[] Normalize(double[] vector, double normilizecriteria)
        {
            for (int i = 0; i < vector.Length; i++)
                vector[i] = vector[i] / normilizecriteria;
            return vector;
        }

        private static double VectorSum(double[] vector)
        {
            double result = 0;
            foreach (var item in vector)
                result += item;

            return result;
        }

        private static bool Accuracy(double[] p, double[] pnext)
        {
            var Eps = 0.0000001;
            for (int i = 0; i < p.Length; i++)
                if (!(Math.Abs(pnext[i] - p[i]) < Eps))
                    return false;

            return true;
        }

        private static double[] MultiplyMatrixByVecor(int[,] matrix, double[] vector)
        {
            var result = new double[vector.Length];
            for (int i = 0; i < vector.Length; i++)
                for (int j = 0; j < vector.Length; j++)
                    result[i] += matrix[i, j] * vector[j];

            return result;
        }

    }
}
