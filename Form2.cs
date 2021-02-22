using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using WindowsFormsAppWithDatabase;
using System.Security.Cryptography;
using System.Text;

namespace WindowsFormsAppFinalTestReject
{

    public partial class Form2 : Form
    {

        public static MySqlConnection connection;

        public static string sql1;
        public static string sql2;
        public static string sql3;
        public static string sql4;

        string path_sql1 = Path.Combine(Directory.GetCurrentDirectory(), "sql1.txt");
        string path_sql2 = Path.Combine(Directory.GetCurrentDirectory(), "sql2.txt");
        string path_sql3 = Path.Combine(Directory.GetCurrentDirectory(), "sql3.txt");
        string path_sql4 = Path.Combine(Directory.GetCurrentDirectory(), "sql4.txt");

        string path_Server = Path.Combine(Directory.GetCurrentDirectory(), "server.txt");
        string path_Port = Path.Combine(Directory.GetCurrentDirectory(), "port.txt");
        string path_Database = Path.Combine(Directory.GetCurrentDirectory(), "database.txt");
        string path_UserID = Path.Combine(Directory.GetCurrentDirectory(), "username.txt");
        string path_Password = Path.Combine(Directory.GetCurrentDirectory(), "password.txt");
        string path_entropy = Path.Combine(Directory.GetCurrentDirectory(), "entropy.txt");


        public static byte[] username;
        public static byte[] password;
        public static byte[] usernameEntropy;
        public static byte[] passwordEntropy;

        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (checkBox1.Checked == true)
            {

                File.WriteAllText(path_Server, textBox1.Text);
                File.WriteAllText(path_Port, textBox2.Text);
                File.WriteAllText(path_Database, textBox3.Text);

                byte[] userID = Encoding.UTF8.GetBytes(textBox4.Text);
                byte[] password = Encoding.UTF8.GetBytes(textBox5.Text);

                byte[] entropy = new byte[20];

                using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(entropy);
                }

                byte[] cipherUserID = ProtectedData.Protect(userID, entropy, DataProtectionScope.CurrentUser);
                byte[] cipherPassword = ProtectedData.Protect(password, entropy, DataProtectionScope.CurrentUser);

                File.WriteAllBytes(path_entropy, entropy);
                File.WriteAllBytes(path_UserID, cipherUserID);
                File.WriteAllBytes(path_Password, cipherPassword);

            }

            connection = TestToConnectMySQLServer.OpenConnection(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text);

            if (connection.State == ConnectionState.Open)
            {
                getSQLCommand();

                Hide();

                Form1 f1 = new Form1();

                f1.Show();
            }

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                textBox1.Text = File.ReadAllText(path_Server);
                textBox2.Text = File.ReadAllText(path_Port);
                textBox3.Text = File.ReadAllText(path_Database);

                byte[] userIDByte = File.ReadAllBytes(path_UserID);
                byte[] passwordByte = File.ReadAllBytes(path_Password);
                byte[] entropyByte = File.ReadAllBytes(path_entropy);

                byte[] decipherUserID = ProtectedData.Unprotect(userIDByte, entropyByte, DataProtectionScope.CurrentUser);
                byte[] decipherPassword = ProtectedData.Unprotect(passwordByte, entropyByte, DataProtectionScope.CurrentUser);

                textBox4.Text = Encoding.UTF8.GetString(decipherUserID);
                textBox5.Text = Encoding.UTF8.GetString(decipherPassword);
            }
            catch
            {
                textBox1.Text = null;
                textBox2.Text = null;
                textBox3.Text = null;
                textBox4.Text = null;
                textBox5.Text = null;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void getSQLCommand()
        {
            StreamReader reader;

            reader = new StreamReader(path_sql1);
            sql1 = reader.ReadToEnd();
            reader.Close();

            reader = new StreamReader(path_sql2);
            sql2 = reader.ReadToEnd();
            reader.Close();
            
            reader = new StreamReader(path_sql3);
            sql3 = reader.ReadToEnd();
            reader.Close();

            reader = new StreamReader(path_sql4);
            sql4 = reader.ReadToEnd();
            reader.Close();

        }
    }
}
