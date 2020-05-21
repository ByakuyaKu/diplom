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
        private string _connectionString = "Data Source=.;Initial Catalog=WebAppDb;Integrated Security=True";

        public int AddSession(int criterianumber, int alternativenumber)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "AddSession";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@CriteriaNumber",
                    Value = criterianumber,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                });

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@AlternativeNumber",
                    Value = alternativenumber,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                });

                var id = new SqlParameter
                {
                    ParameterName = "@SessionID",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(id);

                connection.Open();

                command.ExecuteNonQuery();

                return (int)id.Value;
            }
        }

        public int AddAlternative(Alternative alternative, int sessionId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "AddAlternative";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@AlternativeName",
                    Value = alternative.AlternativeName,
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input
                });

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@SessionID",
                    Value = sessionId,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                });

                var id = new SqlParameter
                {
                    ParameterName = "@AlternativeID",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(id);

                connection.Open();

                command.ExecuteNonQuery();

                return (int)id.Value;
            }
        }

        public int AddCriteria(Criteria criteria, string matrix, int sessionid)
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

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@Matrix",
                    Value = matrix,
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 500,
                    Direction = ParameterDirection.Input
                });

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@SessionID",
                    Value = sessionid,
                    SqlDbType = SqlDbType.Int,
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

        public int GetSessionCriteriaNumber(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "GetSessionCriteriaNumber";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@SessionId",
                    Value = id,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                });

                var criterianumber = (new SqlParameter
                {
                    ParameterName = "@CriteriaNumber",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                });
                command.Parameters.Add(criterianumber);

                connection.Open();

                command.ExecuteNonQuery();

                return (int)criterianumber.Value;
            }
        }

        public int GetSessionAlternariveNumber(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "GetSessionAlternativeNumber";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@SessionId",
                    Value = id,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                });

                var alternativenumber = (new SqlParameter
                {
                    ParameterName = "@AlternativeNumber",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                });
                command.Parameters.Add(alternativenumber);

                connection.Open();

                command.ExecuteNonQuery();

                return (int)alternativenumber.Value;
            }
        }
        public IEnumerable<Criteria> GetAllCriteria(int sessionid)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "GetCriteria";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@SessionID",
                    Value = sessionid,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                });

                connection.Open();

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    yield return new Criteria
                    {
                        CriteriaID = (int)reader["CriteriaID"],
                        CriteriaName = (string)reader["CriteriaName"],
                        CriteriaWeight = (double)reader["CriteriaWeight"],
                        Matrix = (string)reader["Matrix"],
                    };
                }
            }
        }
        public IEnumerable<Alternative> GetAllAlternative(int sessionid)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "GetAlternative";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@SessionID",
                    Value = sessionid,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                });

                connection.Open();

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    yield return new Alternative
                    {
                        AlternativeID = (int)reader["AlternativeID"],
                        AlternativeName = (string)reader["AlternativeName"],
                    };
                }
            }
        }     

        public IEnumerable<Criteria> GetCriteriaName(int sessionid)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "GetCriteriaName";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@SessionID",
                    Value = sessionid,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                });

                connection.Open();

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    yield return new Criteria
                    {
                        CriteriaID = (int)reader["CriteriaID"],
                        CriteriaName = (string)reader["CriteriaName"]                    
                    };
                }
            }
        }


        public int GetSessionId()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "GetSessionId";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var sessionId = (new SqlParameter
                {
                    ParameterName = "@SessionId",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                });
                command.Parameters.Add(sessionId);

                connection.Open();

                command.ExecuteNonQuery();

                return (int)sessionId.Value;
            }
        }
    }
}
