using IANTLibrary;
using IANTServer.OperationHandlers;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace IANTServer
{
    class MySQLDatabase : IDatabase
    {
        private MySqlConnection connection;

        private void QueryStartTask()
        {
            if (connection != null && connection.State != ConnectionState.Open)
                connection.Open();
        }
        private void QueryEndTask()
        {
            if (connection != null && connection.State == ConnectionState.Open)
                connection.Close();
        }

        public bool Connect(string hostName, string userName, string password, string database)
        {
            string connectString = string.Format("server={0};uid={1};pwd={2};database={3}", hostName, userName, password, database);
            connection = new MySqlConnection(connectString);
            return connection != null;
        }

        public void Dispose()
        {
            if (connection != null && connection.State != ConnectionState.Closed)
                connection.Close();
        }

        public bool DeleteDataByUniqueID(int uniqueID, string table)
        {
            throw new NotImplementedException();
        }

        public bool InsertData(string[] columns, object[] values, string table)
        {
            try
            {
                QueryStartTask();

                StringBuilder sqlText = new StringBuilder();
                sqlText.Append("INSERT INTO " + table + " (" + columns[0]);
                int insertNumber = columns.Length;
                for (int index1 = 1; index1 < insertNumber; index1++)
                {
                    sqlText.Append("," + columns[index1]);
                }
                sqlText.Append(") values (@insertValue0");
                for (int index1 = 1; index1 < insertNumber; index1++)
                {
                    sqlText.Append(",@insertValue" + index1.ToString());
                }
                sqlText.Append(")");
                using (MySqlCommand cmd = new MySqlCommand(sqlText.ToString(), connection))
                {
                    for (int index1 = 0; index1 < insertNumber; index1++)
                    {
                        cmd.Parameters.AddWithValue("@insertValue" + index1.ToString(), values[index1]);
                    }
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                QueryEndTask();
            }
        }

        public string[] ReadDataByUniqueID(int uniqueID, string[] columns, string table, string joinCondition = "")
        {
            try
            {
                QueryStartTask();

                StringBuilder sqlText = new StringBuilder();
                sqlText.Append("SELECT " + columns[0]);
                int requestNumber = columns.Length;
                for (int index = 1; index < requestNumber; index++)
                {
                    sqlText.Append("," + columns[index]);
                }
                sqlText.Append(" FROM " + table + " WHERE UniqueID=@uniqueID");
                if(joinCondition != "")
                {
                    sqlText.Append(" and " + joinCondition);
                }
                using (MySqlCommand command = new MySqlCommand(sqlText.ToString(), connection))
                {
                    command.Parameters.AddWithValue("@uniqueID", uniqueID);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            List<string> returnValue = new List<string>();
                            for (int index = 0; index < requestNumber; index++)
                            {
                                if (reader.IsDBNull(index))
                                    returnValue.Add("");
                                else
                                    returnValue.Add(reader.GetString(index));
                            }
                            return returnValue.ToArray();
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                QueryEndTask();
            }
        }

        public bool UpdataDataByUniqueID(int uniqueID, List<string> columns, List<string> values, string table)
        {
            try
            {
                QueryStartTask();

                StringBuilder sqlText = new StringBuilder();
                sqlText.Append("UPDATE " + table + " SET " + columns[0] + "=@updateValue0");
                int updateNumber = columns.Count;
                for (int index1 = 1; index1 < updateNumber; index1++)
                {
                    sqlText.Append("," + columns[index1] + "=@updateValue" + index1.ToString());
                }
                sqlText.Append(" where UniqueID=@uniqueID");
                using (MySqlCommand cmd = new MySqlCommand(sqlText.ToString(), connection))
                {
                    for (int index1 = 0; index1 < updateNumber; index1++)
                    {
                        cmd.Parameters.AddWithValue("@updateValue" + index1.ToString(), values[index1]);
                    }
                    cmd.Parameters.AddWithValue("@uniqueID", uniqueID);
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                QueryEndTask();
            }
        }

        public bool UpdataDataByUniqueID(int uniqueID, string[] columns, object[] values, string table)
        {
            try
            {
                QueryStartTask();

                StringBuilder sqlText = new StringBuilder();
                sqlText.Append("UPDATE " + table + " SET " + columns[0] + "=@updateValue0");
                int updateNumber = columns.Length;
                for (int index1 = 1; index1 < updateNumber; index1++)
                {
                    sqlText.Append("," + columns[index1] + "=@updateValue" + index1.ToString());
                }
                sqlText.Append(" where UniqueID=@uniqueID");
                using (MySqlCommand cmd = new MySqlCommand(sqlText.ToString(), connection))
                {
                    for (int index1 = 0; index1 < updateNumber; index1++)
                    {
                        cmd.Parameters.AddWithValue("@updateValue" + index1.ToString(), values[index1]);
                    }
                    cmd.Parameters.AddWithValue("@uniqueID", uniqueID);
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                QueryEndTask();
            }
        }

        public bool UpdataDataByID(int uniqueID, string[] columns, object[] values, string table, string IDName)
        {
            try
            {
                QueryStartTask();

                StringBuilder sqlText = new StringBuilder();
                sqlText.Append("UPDATE " + table + " SET " + columns[0] + "=@updateValue0");
                int updateNumber = columns.Length;
                for (int index1 = 1; index1 < updateNumber; index1++)
                {
                    sqlText.Append("," + columns[index1] + "=@updateValue" + index1.ToString());
                }
                sqlText.Append(" where "+ IDName + "=@IDName");
                using (MySqlCommand cmd = new MySqlCommand(sqlText.ToString(), connection))
                {
                    for (int index1 = 0; index1 < updateNumber; index1++)
                    {
                        cmd.Parameters.AddWithValue("@updateValue" + index1.ToString(), values[index1]);
                    }
                    cmd.Parameters.AddWithValue("@IDName", uniqueID);
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                QueryEndTask();
            }
        }

        public object[] GetDataByUniqueID(int uniqueID, string[] requestItems, TypeCode[] requestTypes, string table, string joinCondition = "")
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();
                StringBuilder sqlText = new StringBuilder();
                sqlText.Append("SELECT " + requestItems[0]);
                int requestNumber = requestItems.Length;
                for (int index1 = 1; index1 < requestNumber; index1++)
                {
                    sqlText.Append("," + requestItems[index1]);
                }
                sqlText.Append(" FROM " + table + " WHERE UniqueID=@uniqueID");
                if (joinCondition != "")
                {
                    sqlText.Append(" and " + joinCondition);
                }
                using (MySqlCommand cmd = new MySqlCommand(sqlText.ToString(), connection))
                {
                    cmd.Parameters.AddWithValue("@uniqueID", uniqueID);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            object[] returnValue = new object[requestNumber];
                            for (int index1 = 0; index1 < requestNumber; index1++)
                            {
                                if (reader.IsDBNull(index1))
                                    returnValue[index1] = null;
                                else
                                {
                                    switch (requestTypes[index1])
                                    {
                                        case TypeCode.Boolean:
                                            returnValue[index1] = reader.GetBoolean(index1);
                                            break;
                                        case TypeCode.String:
                                            returnValue[index1] = reader.GetString(index1);
                                            break;
                                        case TypeCode.Int32:
                                            returnValue[index1] = reader.GetInt32(index1);
                                            break;
                                        case TypeCode.Single:
                                            returnValue[index1] = reader.GetFloat(index1);
                                            break;
                                        case TypeCode.Int64:
                                            returnValue[index1] = reader.GetInt64(index1);
                                            break;
                                        case TypeCode.DateTime:
                                            returnValue[index1] = reader.GetDateTime(index1);
                                            break;
                                    }
                                }
                            }
                            return returnValue;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
        }

        public bool ContainsPlayer(long facebookID, out int uniqueID)
        {
            try
            {
                QueryStartTask();
                using (MySqlCommand command = new MySqlCommand("SELECT UniqueID FROM player WHERE FacebookID = @facebookID LIMIT 1;", connection))
                {
                    command.Parameters.AddWithValue("@facebookID", facebookID);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            uniqueID = reader.GetInt32("UniqueID");
                            return true;
                        }
                        else
                        {
                            uniqueID = -1;
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                QueryEndTask();
            }
        }

        public bool Register(long facebookID)
        {
            try
            {
                QueryStartTask();
                StringBuilder sqlText = new StringBuilder();
                sqlText.Append("INSERT INTO player (FacebookID,Level,EXP) values (@facebookID,1,0);");
                sqlText.Append("INSERT INTO nest (PlayerID) SELECT LAST_INSERT_ID();");
                using (MySqlCommand cmd = new MySqlCommand(sqlText.ToString(), connection))
                {
                    cmd.Parameters.AddWithValue("@facebookID", facebookID);
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                QueryEndTask();
            }
        }

        public Dictionary<long,ChallengePlayerInfo> FetchRandomNPlayerInfo(int n)
        {
            //the sql is slow but that's ok
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT FacebookID, Level, (Duration+Speed+Resistant+Population+Sensitivity+1)as NestLevel FROM player,nest WHERE player.UniqueID = nest.PlayerID ORDER BY RAND() LIMIT @N", connection))
                {
                    cmd.Parameters.AddWithValue("@N", n);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        Dictionary<long, ChallengePlayerInfo> infos = new Dictionary<long, ChallengePlayerInfo>();
                        for (int i = 0; i < n; i++)
                        {
                            if (reader.Read())
                            {
                                infos.Add(reader.GetInt64(0), new ChallengePlayerInfo
                                {
                                    facebookID = reader.GetInt64(0),
                                    level = reader.GetInt32(1),
                                    nestLevel = reader.GetInt32(2)
                                });
                            }
                            else
                            {
                                break;
                            }
                        }
                        return infos;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
        }
        public ChallengePlayerInfo FetchPlayerInfoWithFacebookID(long facebookID)
        {
            try
            {
                ChallengePlayerInfo info = new ChallengePlayerInfo();
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT FacebookID, Level, (Duration+Speed+Resistant+Population+Sensitivity+1)as NestLevel FROM player,nest WHERE FacebookID = @fbid AND player.UniqueID = nest.PlayerID LIMIT 1", connection))
                {
                    cmd.Parameters.AddWithValue("@fbid", facebookID);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            info.facebookID = reader.GetInt64(0);
                            info.level = reader.GetInt32(1);
                            info.nestLevel = reader.GetInt32(2);
                        }
                        return info;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
        }
        public Nest GetNest(long facebookID)
        {
            try
            {
                Nest nest = null;
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT Duration,Speed,Resistant,Population,Sensitivity,DistributionMap1,DistributionMap2,DistributionMap3 FROM player,nest WHERE FacebookID = @fbid AND player.UniqueID = nest.PlayerID LIMIT 1", connection))
                {
                    cmd.Parameters.AddWithValue("@fbid", facebookID);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            nest = new Nest(new AntGrowthProperties
                            {
                                duration = reader.GetInt32(0),
                                speed = reader.GetInt32(1),
                                resistant = reader.GetInt32(2),
                                population = reader.GetInt32(3),
                                sensitivity = reader.GetInt32(4)
                            });
                            nest.Load3DistributionMap(reader.GetString(5), reader.GetString(6), reader.GetString(7));
                        }
                        return nest;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
        }
        public string GetDefenceDataString(long facebookID)
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT DefenceTowersDataString FROM player WHERE FacebookID = @fbid LIMIT 1", connection))
                {
                    cmd.Parameters.AddWithValue("@fbid", facebookID);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read() && !reader.IsDBNull(0))
                        {
                            string dataString = reader.GetString(0);
                            if (dataString[0] != '<')
                                dataString = dataString.Substring(1);
                            return dataString;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
        }
        public Dictionary<long, HarvestPlayerInfo> FetchRandomNHarvestPlayerInfo(int n)
        {
            //the sql is slow but that's ok
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT FacebookID, Level, CakeCount FROM player ORDER BY RAND() LIMIT @N", connection))
                {
                    cmd.Parameters.AddWithValue("@N", n);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        Dictionary<long, HarvestPlayerInfo> infos = new Dictionary<long, HarvestPlayerInfo>();
                        for (int i = 0; i < n; i++)
                        {
                            if (reader.Read())
                            {
                                infos.Add(reader.GetInt64(0), new HarvestPlayerInfo
                                {
                                    facebookID = reader.GetInt64(0),
                                    level = reader.GetInt32(1),
                                    harvestableCakeNumber = reader.GetInt32(2)/2
                                });
                            }
                            else
                            {
                                break;
                            }
                        }
                        return infos;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
        }
        public HarvestPlayerInfo FetchHarvestPlayerInfoWithFacebookID(long facebookID)
        {
            try
            {
                HarvestPlayerInfo info = new HarvestPlayerInfo();
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT FacebookID, Level, CakeCount FROM player WHERE FacebookID = @fbid LIMIT 1", connection))
                {
                    cmd.Parameters.AddWithValue("@fbid", facebookID);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            info.facebookID = reader.GetInt64(0);
                            info.level = reader.GetInt32(1);
                            info.harvestableCakeNumber = reader.GetInt32(2)/2;
                        }
                        return info;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
        }
    }
}
