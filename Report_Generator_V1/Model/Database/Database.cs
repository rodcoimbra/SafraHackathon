using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report_Generator_V1.Model.Database
{
    class Database
    {

        public String pathDatabase = "";
        private String ConnectionString { get; set; }

        public Database()
        {
            Set_connectionString();
        }


        private String Set_connectionString()
        {
            return ConnectionString = @"Persist Security Info = False; server = localhost; database = safra; uid = root; pwd = safra";
        }

        public ReturnStructure Get_Accounts()
        {
            ReturnStructure returnStructure = new ReturnStructure() { Data = null, Message = "", Status = true };
            List<Account> data = new List<Account>();
            Account row;
            string insertSQL = "SELECT * FROM teste";

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                using (MySqlCommand command = new MySqlCommand(insertSQL))
                {

                    command.Connection = connection;

                    try
                    {
                        connection.Open();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                row = new Account();

                                if (reader.FieldCount == row.Account_fields_count)
                                {
                                    row.Description = reader[0].ToString();

                                    Double.TryParse(reader[1].ToString(), out double value_in);
                                    row.Balance_in = value_in;

                                    Double.TryParse(reader[2].ToString(), out double value_out);
                                    row.Balance_out = value_out;

                                    row.Cluster = reader[3].ToString();

                                    Double.TryParse(reader[4].ToString(), out double value_id_exec);
                                    row.id_exec = value_id_exec;

                                    DateTime.TryParse(reader[5].ToString(), out DateTime value_date_exec);
                                    row.time_exec = value_date_exec;


                                }


                                data.Add(row);
                            }
                        }

                        returnStructure.Data = data;

                    }
                    catch (Exception ex)
                    {
                        returnStructure.Status = false;
                        returnStructure.Message = ex.Message;
                    }
                    finally
                    {
                        if (connection.State == System.Data.ConnectionState.Open)
                            connection.Close();
                    }

                }
            }

            return returnStructure;
        }


        public ReturnStructure Set_Accounts(List<Account> accounts_to_insert)
        {
            ReturnStructure returnStructure = new ReturnStructure() { Data = null, Message = "", Status = true };
            List<Account> data = new List<Account>();
            Account row;
            StringBuilder insertSQL = new StringBuilder("INSERT INTO transferencias(conta, entrada, saida) VALUES ");

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {

                List<string> Rows = new List<string>();
                for (int i = 0; i < accounts_to_insert.Count; i++)
                {
                    Rows.Add(string.Format("('{0}','{1}','{2}')", accounts_to_insert[i].Description, accounts_to_insert[i].Balance_in, accounts_to_insert[i].Balance_out));
                }

                insertSQL.Append(string.Join(",", Rows));
                insertSQL.Append(";");

                connection.Open();
                using (MySqlCommand command = new MySqlCommand(insertSQL.ToString(), connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    command.ExecuteNonQuery();
                }

                using (MySqlCommand command = new MySqlCommand(insertSQL.ToString()))
                {

                    command.Connection = connection;

                    try
                    {
                        connection.Open();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                row = new Account();

                                if (reader.FieldCount == row.Account_fields_count)
                                {
                                    row.Description = reader[0].ToString();

                                    Double.TryParse(reader[1].ToString(), out double value_in);
                                    row.Balance_in = value_in;

                                    Double.TryParse(reader[2].ToString(), out double value_out);
                                    row.Balance_out = value_out;

                                    row.Cluster = reader[3].ToString();

                                    Double.TryParse(reader[4].ToString(), out double value_id_exec);
                                    row.id_exec = value_id_exec;

                                    DateTime.TryParse(reader[5].ToString(), out DateTime value_date_exec);
                                    row.time_exec = value_date_exec;


                                }


                                data.Add(row);
                            }
                        }

                        returnStructure.Data = data;

                    }
                    catch (Exception ex)
                    {
                        returnStructure.Status = false;
                        returnStructure.Message = ex.Message;
                    }
                    finally
                    {
                        if (connection.State == System.Data.ConnectionState.Open)
                            connection.Close();
                    }

                }
            }

            return returnStructure;
        }

    }


    public class Account
    {
        public int Account_fields_count { get; private set; }
        public string Description { get; set; }
        public double Balance_in { get; set; }
        public double Balance_out { get; set; }
        public string Cluster { get; set; }
        public double id_exec { get; set; }
        public DateTime time_exec { get; set; }

        public Account()
        {
            Account_fields_count = 6;
        }

    }

    public class ReturnStructure
    {
        public string Message { get; set; }
        public Boolean Status { get; set; }
        public IEnumerable<object> Data { get; set; }
    }



}
