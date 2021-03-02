using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.SQLConnection
{
    public class ManageSQLConnection
    {
        private MySqlConnection connection;
        string connectionString = GlobalVariables.ConnectionString;
        MySqlDataAdapter dataAdapter;

        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }


        public DataSet GetDataSetValues(string procedureName)
        {
            connection = new MySqlConnection(connectionString);
            DataSet ds = new DataSet();
            MySqlCommand sqlCommand = new MySqlCommand();
            try
            {

                if (connection.State == 0)
                {
                    connection.Open();
                }
                sqlCommand.Connection = connection;
                sqlCommand.CommandText = procedureName;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                dataAdapter = new MySqlDataAdapter(sqlCommand);
                dataAdapter.Fill(ds);


                return ds;

            }
            finally
            {
                connection.Close();
                ds.Dispose();
                dataAdapter = null;
            }

        }


        public DataSet GetDataSetValues(string procedureName, List<KeyValuePair<string, string>> parameterList)
        {
            connection = new MySqlConnection(connectionString);
            DataSet ds = new DataSet();
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                if (connection.State == 0)
                {
                    connection.Open();
                }
                cmd.Connection = connection;
                cmd.CommandText = procedureName;
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (KeyValuePair<string, string> keyValuePair in parameterList)
                {
                    cmd.Parameters.AddWithValue(keyValuePair.Key, keyValuePair.Value);
                }
                cmd.CommandTimeout = 180;
                dataAdapter = new MySqlDataAdapter(cmd);
                dataAdapter.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
            finally
            {
                connection.Close();
                cmd.Dispose();
                ds.Dispose();
                dataAdapter = null;
            }
        }

        /// <summary>
        /// Query Execution for particlar query
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameterList"></param>
        /// <returns></returns>
        public DataSet GetDataSetValuesFromQuery(string QueryStringName)
        {
            connection = new MySqlConnection(connectionString);
            DataSet ds = new DataSet();
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                if (connection.State == 0)
                {
                    connection.Open();
                }
                cmd.Connection = connection;
                string query = QueryStringName;
                cmd = new MySqlCommand(query, connection);
                cmd.CommandTimeout = 180;
                dataAdapter = new MySqlDataAdapter(cmd);
                dataAdapter.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
            finally
            {
                connection.Close();
                cmd.Dispose();
                ds.Dispose();
                dataAdapter = null;
            }
        }

        public bool UpdateValues(string procedureName, List<KeyValuePair<string, string>> parameterList)
        {
            connection = new MySqlConnection(connectionString);
            DataSet ds = new DataSet();
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                if (connection.State == 0)
                {
                    connection.Open();
                }
                cmd.Connection = connection;
                cmd.CommandText = procedureName;
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (KeyValuePair<string, string> keyValuePair in parameterList)
                {
                    cmd.Parameters.AddWithValue(keyValuePair.Key, keyValuePair.Value);
                }
                int affected = cmd.ExecuteNonQuery();
                //  AuditLog.WriteError(affected.ToString());
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;

            }
            finally
            {
                connection.Close();
                cmd.Dispose();
                ds.Dispose();
                dataAdapter = null;
            }
        }


        public bool UpdateValuesByQuery(string Query)
        {
            connection = new MySqlConnection(connectionString);
            DataSet ds = new DataSet();
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                if (connection.State == 0)
                {
                    connection.Open();
                }
                string query = Query;
                cmd = new MySqlCommand(query, connection);
                cmd.CommandTimeout = 180;
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;

            }
            finally
            {
                connection.Close();
                cmd.Dispose();
                ds.Dispose();
                dataAdapter = null;
            }
        }

        public bool InsertData(string procedureName, List<KeyValuePair<string, string>> parameterList)
        {
            connection = new MySqlConnection(connectionString);
            DataSet ds = new DataSet();
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                if (connection.State == 0)
                {
                    connection.Open();
                }
                cmd.Connection = connection;
                cmd.CommandText = procedureName;
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (KeyValuePair<string, string> keyValuePair in parameterList)
                {
                    cmd.Parameters.AddWithValue(keyValuePair.Key, keyValuePair.Value);
                }
                cmd.ExecuteNonQuery();
                //  AuditLog.WriteError(affected.ToString());
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;

            }
            finally
            {
                connection.Close();
                cmd.Dispose();
                ds.Dispose();
                dataAdapter = null;
            }
        }


    }
}
