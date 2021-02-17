using Org.BouncyCastle.Utilities.Zlib;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsAppFinalTestReject;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.IO;

namespace WindowsFormsAppWithDatabase
{

  
    public partial class Form1 : Form
    {

        List <Panel> listPanel = new List <Panel>();

        string sql1 = "select Time, Station, SA_SN, SA_PN, Line, State " +
                "from sfcs_semi_fgtest " +
                "where time >= now() - interval 4 day and State NOT LIKE \"OK\" " +
                "UNION\n" +
                "select Time, Station, SA_SN, SA_PN, Line, State " +
                "from sfcs_spotsoldering " +
                "where time >= now() - interval 4 day and State NOT LIKE \"OK\" " +
                "UNION\n" +
                "select Time, Station, SA_SN, SA_PN, Line, State " +
                "from sfcs_fgtest " +
                "where time >= now() - interval 4 day and State NOT LIKE \"OK\" " +
                "order by Time desc " +
                "limit 20";

        string sql2 =

                "select Time, DATE_FORMAT(Time, '%H') as Date, Station, SA_SN, SA_PN, Line, State " +
                "from sfcs_semi_fgtest " +
                "where time >= now() - interval 1 day and State NOT LIKE \"OK\" " +
                "UNION\n" +
                "select Time, DATE_FORMAT(Time, '%H') as Date, Station, SA_SN, SA_PN, Line, State " +
                "from sfcs_spotsoldering " +
                "where time >= now() - interval 1 day and State NOT LIKE \"OK\" " +
                "UNION\n" +
                "select Time, DATE_FORMAT(Time, '%H') as Date, Station, SA_SN, SA_PN, Line, State " +
                "from sfcs_fgtest " +
                "where time >= now() - interval 1 day and State NOT LIKE \"OK\" ";

        string sql3 =

        "select Time, DATE_FORMAT(Time, '%H') as Date, Station, SA_SN, SA_PN, Line, State " +
        "from sfcs_fgtest " +
        "where time >= now() - interval 1 day ";


        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {

            listPanel.Add(panel1);
            listPanel.Add(panel2);


                StreamReader reader = new StreamReader(@"C:\Users\wayan.eka\source\repos\WindowsFormsAppFinalTestReject\SetUpConnection.csv");

                var line = reader.ReadLine();
                var values = line.Split(',');

            textBox3.Text = values[0];
            textBox4.Text = values[1];
            textBox5.Text = values[2];
            textBox6.Text = values[3];
            textBox7.Text = values[4];

            reader.Close();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {


            MySqlConnection connection = TestToConnectMySQLServer.OpenConnection(textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox7.Text);

            DataTable dttable1 = TestToConnectMySQLServer.FillData(sql1, connection);
            DataTable dttable2 = TestToConnectMySQLServer.FillData(sql2, connection);
            DataTable dttable3 = TestToConnectMySQLServer.FillData(sql3, connection);
            DataTable dtReturn = Pivot.GetFixedPivotTable(dttable2, "Date", "Line", "SA_SN", "0", "Count");
            DataTable dtReturn2 = Pivot.GetFixedPivotTable(dttable3, "Date", "Line", "SA_SN", "0", "Count");
            //DataTable dtPercentage = Pivot.GetPercentagePivotTable(dtReturn, "Row Total");


            dataGridView1.DataSource = dttable1.DefaultView;
            dataGridView2.DataSource = dtReturn.DefaultView;
            dataGridView3.DataSource = dtReturn2.DefaultView;
            Pivot.GetDivisionCellFormat(dataGridView2, dataGridView3);
            //dataGridView3.DataSource = dtPercentage.DefaultView;

            Pivot.GetDataGridCellColor(dataGridView2);

            textBox1.Text = ("Last Refresh: " + DateTime.Now.ToLongTimeString());


        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(checkBox1.Checked == true)
            {
                StreamWriter writer = new StreamWriter(@"C:\Users\wayan.eka\source\repos\WindowsFormsAppFinalTestReject\SetUpConnection.csv");
                writer.WriteLine(textBox3.Text + ',' + textBox4.Text + ',' + textBox5.Text + ',' + textBox6.Text + ',' + textBox7.Text);
                writer.Close();
            }
           

            listPanel[1].SendToBack();

            MySqlConnection connection = TestToConnectMySQLServer.OpenConnection(textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox7.Text);

            DataTable dttable1 = TestToConnectMySQLServer.FillData(sql1, connection);
            DataTable dttable2 = TestToConnectMySQLServer.FillData(sql2, connection);
            DataTable dttable3 = TestToConnectMySQLServer.FillData(sql3, connection);
            DataTable dtReturn = Pivot.GetFixedPivotTable(dttable2, "Date", "Line", "SA_SN", "0", "Count");
            DataTable dtReturn2 = Pivot.GetFixedPivotTable(dttable3, "Date", "Line", "SA_SN", "0", "Count");
            //DataTable dtPercentage = Pivot.GetPercentagePivotTable(dtReturn, "Row Total");

            dataGridView1.DataSource = dttable1.DefaultView;
            dataGridView2.DataSource = dtReturn.DefaultView;
            dataGridView3.DataSource = dtReturn2.DefaultView;
            Pivot.GetDivisionCellFormat(dataGridView2, dataGridView3);
            //dataGridView3.DataSource = dtPercentage.DefaultView;

            Pivot.GetDataGridCellColor(dataGridView2);

            textBox1.Text = ("Last Refresh: " + DateTime.Now.ToLongTimeString());

            timer1.Enabled = true;

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
