using System;
using System.Configuration;
using System.Data.SqlClient;
using WebApp.AHP.Entities;
using System.Data;
using WebApp.AHP.DAL.Interfaces;
using System.Collections.Generic;

namespace WebApp.AHP.DAL
{
    public class AHPDao : IAHPDao
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["WebAppDb"].ConnectionString;


        public int AddCriteria(Criteria criteria)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "AddCriteria";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@CriteriaName",
                    Value = criteria.CriteriaName,
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input
                });

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@CriteriaWeight",
                    Value = criteria.CriteriaWeight,
                    SqlDbType = SqlDbType.Float,
                    Direction = ParameterDirection.Input
                });

                var id = new SqlParameter
                {
                    ParameterName = "@CriteriaID",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(id);

                connection.Open();

                command.ExecuteNonQuery();

                return (int)id.Value;
            }
        }

        public void DeleteById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "AddCriteria";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@CriteriaID",
                    Value = id,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                });
                connection.Open();

                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<Criteria> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "GetAll";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@CriteriaName",
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Output
                });

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@CriteriaWeight",
                    SqlDbType = SqlDbType.Float,
                    Direction = ParameterDirection.Output
                });

                var id = new SqlParameter
                {
                    ParameterName = "@CriteriaID",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(id);

                connection.Open();

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    yield return new Criteria
                    {
                        CriteriaID = (int)reader["CriteriaID"],
                        CriteriaName = (string)reader["CriteriaName"],
                        CriteriaWeight = (double)reader["CriteriaWeight"],
                    };
                }
            }
        }
    }
}
