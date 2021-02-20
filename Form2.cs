using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsAppWithDatabase;
using MySql.Data.MySqlClient;

namespace WindowsFormsAppFinalTestReject
{

    public partial class Form2 : Form
    {

        public static MySqlConnection connection;

        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (checkBox1.Checked == true)
            {
                StreamWriter writer = new StreamWriter(@"C:\Users\wayan.eka\source\repos\WindowsFormsAppFinalTestReject\SetUpConnection.csv");

                writer.WriteLine(textBox1.Text + ',' + textBox2.Text + ',' + textBox3.Text + ',' + textBox4.Text + ',' + textBox5.Text);
                writer.Close();
            }

            connection = TestToConnectMySQLServer.OpenConnection(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text);

            if (connection.State == ConnectionState.Open)
            {
                Form1 f1 = new Form1();

                f1.Show();

                Hide();

            }

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            StreamReader reader = new StreamReader(@"C:\Users\wayan.eka\source\repos\WindowsFormsAppFinalTestReject\SetUpConnection.csv");

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
    }
}
