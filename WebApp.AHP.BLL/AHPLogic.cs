﻿using System;
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

        public int AddCriteria(Criteria criteria, string matrix, int sessionid)
        {
            return _ahpDao.AddCriteria(criteria, matrix, sessionid);
        }

        public int AddAlternative(Alternative alternative, int sessionId)
        {
            return _ahpDao.AddAlternative(alternative, sessionId);
        }

        public int AddSession(int critriaNumber, int alternativenumber)
        {
            return _ahpDao.AddSession(critriaNumber, alternativenumber);
        }
        //public IEnumerable<Alternative> GetAlternative(int sessionid)
        //{
        //    return _ahpDao.GetAlternative(sessionid);
        //}

        public IEnumerable<Criteria> GetCriteriaName(int sessionid)
        {
            return _ahpDao.GetCriteriaName(sessionid);
        }
        public IEnumerable<Criteria> GetAllCriteria(int sessionid)
        {
            return _ahpDao.GetAllCriteria(sessionid);
        }
        public IEnumerable<Alternative> GetAllAlternative(int sessionid)
        {
            return _ahpDao.GetAllAlternative(sessionid);
        }

        public List<Criteria> StartAhp(IEnumerable<Criteria> criterias, int alternativenumber) => AhpAlgorytm(ParseMatrix(criterias, alternativenumber), alternativenumber);

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

        private List<Criteria> ParseMatrix(IEnumerable<Criteria> criterias, int alternativenumber)
        {
            List<Criteria> ParsedCriterias = new List<Criteria>();
            ParsedCriterias = criterias.ToList<Criteria>();

            for (int i = 0; i < ParsedCriterias.Count; i++)
            {
                ParsedCriterias[i].MatrixOfPairedComparisons = new int[alternativenumber, alternativenumber];
                ParsedCriterias[i].Matrix = SplitMatrix(ParsedCriterias[i].Matrix);

                for (int j = 0; j < ParsedCriterias[i].Matrix.Length;)
                {
                    for (int c = 0; c < alternativenumber; c++)
                        for (int r = 0; r < alternativenumber; r++, j++)
                            ParsedCriterias[i].MatrixOfPairedComparisons[c, r] = Int32.Parse((ParsedCriterias[i].Matrix[j]).ToString());
                }
            }

            return ParsedCriterias;
        }

        private List<Criteria> AhpAlgorytm(List<Criteria> criterias, int alternativenumber)
        {
            criterias = VectorPInit(criterias, alternativenumber);

            var pnext = new double[alternativenumber];

            for (int i = 0; i < criterias.Count; i++)
            {
                for (int counter = 0; counter < 101; counter++)
                {
                    pnext = MultiplyMatrixByVecor(criterias[i].MatrixOfPairedComparisons, criterias[i].VectorP);

                    pnext = Normalize(pnext, VectorSum(criterias[i].VectorP));

                    if (!Accuracy(criterias[i].VectorP, pnext))
                        criterias[i].VectorP = pnext;
                    else if (counter == 100)
                        Console.WriteLine("Error");
                    else
                        break;
                }
            }

            return criterias;
        }

        private static List<Criteria> VectorPInit(List<Criteria> criterias, int alternativenumber)
        {
            for (int i = 0; i < criterias.Count; i++)
            {
                criterias[i].VectorP = new double[alternativenumber];
                for (int j = 0; j < alternativenumber; j++)
                    criterias[i].VectorP[j] = 1;
            }

            return criterias;
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
