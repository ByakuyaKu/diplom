using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using WebApp.AHP.DAL.Interfaces;
using WebApp.AHP.Entities;

namespace WebApp.AHP.DAL
{
    public class FAHPDAO : IFAHPDAO
    {
        private string _connectionString = "Data Source=.;Initial Catalog=WebAppDb;Integrated Security=True";
        //private string _connectionString = "workstation id=mcdaOnlineDB.mssql.somee.com;packet size=4096;user id=Toga_SQLLogin_1;pwd=om3qbycw6p;data source=mcdaOnlineDB.mssql.somee.com;persist security info=False;initial catalog=mcdaOnlineDB";

        public int AddSession(int criterianumber, int alternativenumber)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "FAHPAddSession";
                command.CommandType = CommandType.StoredProcedure;

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

        public int AddAlternative(AlternativeFAHP alternative, int sessionId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "FAHPAddAlternative";
                command.CommandType = CommandType.StoredProcedure;

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

        public int AddCriteria(CriteriaFAHP criteria, string matrix, int sessionid)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "FAHPAddCriteria";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@CriteriaName",
                    Value = criteria.CriteriaName,
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input
                });

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@MatrixAlt",
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

        public int GetSessionCriteriaNumber(int sessionid)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "FAHPGetSessionCriteriaNumber";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@SessionId",
                    Value = sessionid,
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

        public int GetSessionAlternariveNumber(int sessionid)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "FAHPGetSessionAlternativeNumber";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@SessionId",
                    Value = sessionid,
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

        public IEnumerable<CriteriaFAHP> GetAllCriteria(int sessionid)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "FAHPGetCriteria";
                command.CommandType = CommandType.StoredProcedure;

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
                    yield return new CriteriaFAHP
                    {
                        CriteriaID = (int)reader["CriteriaID"],
                        CriteriaName = (string)reader["CriteriaName"],
                        MatrixCrPairedCompStr = (string)reader["Matrix"],
                        MatrixAltPairedCompStr = (string)reader["MatrixAlt"],
                    };
                }
            }
        }

        public IEnumerable<CriteriaFAHP> GetAllCriteriaAltMatrOnly(int sessionid)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "FAHPGetCriteriaAltMatrOnly";
                command.CommandType = CommandType.StoredProcedure;

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
                    yield return new CriteriaFAHP
                    {
                        CriteriaID = (int)reader["CriteriaID"],
                        CriteriaName = (string)reader["CriteriaName"],
                        MatrixAltPairedCompStr = (string)reader["MatrixAlt"],
                    };
                }
            }
        }

        public IEnumerable<AlternativeFAHP> GetAllAlternative(int sessionid)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "FAHPGetAlternative";
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
                    yield return new AlternativeFAHP
                    {
                        AlternativeID = (int)reader["AlternativeID"],
                        AlternativeName = (string)reader["AlternativeName"],
                    };
                }
            }
        }

        public IEnumerable<CriteriaFAHP> GetCriteriaName(int sessionid)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "FAHPGetCriteriaName";
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
                    yield return new CriteriaFAHP
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
                command.CommandText = "FAHPGetSessionId";
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
        public void UpdateCriteria(int sessionid, string matrix)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "FAHPUpdateCriteria";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@SessionID",
                    Value = sessionid,
                    SqlDbType = SqlDbType.Int,
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

                connection.Open();

                command.ExecuteNonQuery();
            }
        }
    }
}

