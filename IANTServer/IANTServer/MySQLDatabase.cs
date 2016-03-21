using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace IANTServer
{
    class MySQLDatabase : IDatabase
    {
        private MySqlConnection connection;

        public ConnectionState Connect(string hostName, string userName, string password, string database)
        {
            string connectString = string.Format("server={0};uid={1};pwd={2};database={3}", hostName, userName, password, database);
            connection = new MySqlConnection(connectString);
            return connection.State;
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

        public bool InsertData(List<string> columns, List<string> values, string table)
        {
            try
            {
                QueryStartTask();

                StringBuilder sqlText = new StringBuilder();
                sqlText.Append("INSERT INTO " + table + " (" + columns[0]);
                int insertNumber = columns.Count;
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

        public List<string> ReadDataByUniqueID(int uniqueID, List<string> columns, string table)
        {
            try
            {
                QueryStartTask();

                StringBuilder sqlText = new StringBuilder();
                sqlText.Append("SELECT " + columns[0]);
                int requestNumber = columns.Count;
                for (int index = 1; index < requestNumber; index++)
                {
                    sqlText.Append("," + columns[index]);
                }
                sqlText.Append(" FROM " + table + " WHERE UniqueID=@uniqueID");
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
    }
}
