using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using WindowsFormsAppWithDatabase;

namespace WindowsFormsAppFinalTestReject
{

    public partial class Form2 : Form
    {

        public static MySqlConnection connection;

        public static string sql1;
        public static string sql2;
        public static string sql3;

        string path = Path.Combine(Directory.GetCurrentDirectory(), "SetUpConnection.csv");
        string path_sql1 = Path.Combine(Directory.GetCurrentDirectory(), "sql1.txt");
        string path_sql2 = Path.Combine(Directory.GetCurrentDirectory(), "sql2.txt");
        string path_sql3 = Path.Combine(Directory.GetCurrentDirectory(), "sql3.txt");

        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (checkBox1.Checked == true)
            {
                StreamWriter writer = new StreamWriter(path);

                writer.WriteLine(textBox1.Text + ',' + textBox2.Text + ',' + textBox3.Text + ',' + textBox4.Text + ',' + textBox5.Text);
                writer.Close();
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

            StreamReader reader = new StreamReader(path);

            var line = reader.ReadLine();
            var values = line.Split(',');

            textBox1.Text = values[0];
            textBox2.Text = values[1];
            textBox3.Text = values[2];
            textBox4.Text = values[3];
            textBox5.Text = values[4];

            reader.Close();

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
        }
    }
}
