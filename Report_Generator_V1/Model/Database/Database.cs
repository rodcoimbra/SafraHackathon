using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report_Generator_V1
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

                                if(reader.FieldCount == row.Account_fields_count)
                                {
                                    row.Description = reader[0].ToString();
                                    
                                    Double.TryParse(reader[1].ToString(), out double value_in);
                                    row.Balance_in = value_in;
                                    
                                    Double.TryParse(reader[2].ToString(), out double value_out);
                                    row.Balance_out = value_out;
                                    
                                    Int16.TryParse(reader[3].ToString(), out Int16 value_cluster);
                                    row.Cluster = value_cluster;

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
                        if(connection.State == System.Data.ConnectionState.Open)
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
        public string Description { get;  set; }
        public double Balance_in { get;  set; }
        public double Balance_out { get;  set; }
        public int Cluster { get;  set; }

        public Account()
        {
            Account_fields_count = 4;
        }

    }

    public class ReturnStructure
    {
        public string Message { get; set; }
        public Boolean Status { get; set; }
        public IEnumerable<object> Data { get; set; }
    }



}
