using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsAppWithDatabase
{

    class TestToConnectMySQLServer
    {

        public static MySqlConnection OpenConnection(string server, string port, string database, string userID, string password)
        {

            string conn011 = server;
            string conn021 = port;
            string conn031 = database;
            string conn041 = userID;
            string conn051 = password;


            string connectionString = string.Format("server={0};Port={1};database={2};uid={3};pwd={4}", conn011, conn021, conn031, conn041, conn051);

            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection ! " + ex.Message);
            }

            return connection;
        }
        
        public static DataTable FillData(string sql, MySqlConnection connection)
        {
            DataTable table = new DataTable();
                
            MySqlCommand command = new MySqlCommand(sql, connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.SelectCommand = command;
            adapter.Fill(table);    

            return table;
        }
    }
}
