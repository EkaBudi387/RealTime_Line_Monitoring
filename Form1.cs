using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using WindowsFormsAppFinalTestReject;

namespace WindowsFormsAppWithDatabase
{


    public partial class Form1 : Form
    {

        List<string> columnHeaderInput = new List<string>();

        DataTable dttable1;
        DataTable dttable2;
        DataTable dttable3;
        DataTable dttable4;

        MySqlConnection connection = Form2.connection;

        int rejectHighlightQty;

        string sql1 = Form2.sql1;
        string sql2 = Form2.sql2;
        string sql3 = Form2.sql3;
        string sql4 = Form2.sql4;

        TimeSpan breakShiftMorning = new TimeSpan(07, 00, 00);
        TimeSpan breakShiftAfternoon2 = new TimeSpan(16, 00, 00);
        TimeSpan breakShiftAfternoon3 = new TimeSpan(15, 00, 00);
        TimeSpan breakShiftNight2 = new TimeSpan(01, 00, 00);
        TimeSpan breakShiftNight3 = new TimeSpan(23, 00, 00);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            comboBox2.Text = "5 min";
            comboBox3.Text = "3";

            textBox1.Text = ("Last Refresh: " + DateTime.Now.ToLongTimeString());

            dttable1 = TestToConnectMySQLServer.FillData(sql1, connection);
            dttable2 = TestToConnectMySQLServer.FillData(sql2, connection);
            dttable3 = TestToConnectMySQLServer.FillData(sql3, connection);
            dttable4 = TestToConnectMySQLServer.FillData(sql4, connection);

            timer1.Enabled = true;

            comboBox1.SelectedItem = "2-Shift";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            dttable1 = TestToConnectMySQLServer.FillData(sql1, connection);
            dttable2 = TestToConnectMySQLServer.FillData(sql2, connection);
            dttable3 = TestToConnectMySQLServer.FillData(sql3, connection);
            dttable4 = TestToConnectMySQLServer.FillData(sql4, connection);

            comboBox1.SelectedItem = comboBox1.Text;

            textBox1.Text = ("Last Refresh: " + DateTime.Now.ToLongTimeString());

            
        }

        private void button3_Click(object sender, EventArgs e)
        {

            dttable1 = TestToConnectMySQLServer.FillData(sql1, connection);
            dttable2 = TestToConnectMySQLServer.FillData(sql2, connection);
            dttable3 = TestToConnectMySQLServer.FillData(sql3, connection);
            dttable4 = TestToConnectMySQLServer.FillData(sql4, connection);

            DataTable dtReturn = Pivot.GetFixedPivotTable(dttable2, "Date", "Line", "SA_SN", "0", "Count", columnHeaderInput);
            DataTable dtReturn2 = Pivot.GetFixedPivotTable(dttable3, "Date", "Line", "SA_SN", "0", "Count", columnHeaderInput);

            dataGridView1.DataSource = dttable1.DefaultView;
            dataGridView2.DataSource = dtReturn.DefaultView;
            dataGridView3.DataSource = dtReturn2.DefaultView;
            dataGridView4.DataSource = dttable4.DefaultView;

            Pivot.GetDivisionCellFormat(dataGridView2, dataGridView3, rejectHighlightQty);

            dataGridView3.DataSource = null;

            dataGridView3.DataSource = Pivot.GetPercentagePivotTable(dtReturn2, "Total").DefaultView;

            textBox1.Text = ("Last Refresh: " + DateTime.Now.ToLongTimeString());
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBox1.Text == "2-Shift" && DateTime.Now.Hour >= breakShiftMorning.Hours && DateTime.Now.Hour < breakShiftAfternoon2.Hours)
            {

                columnHeaderInput.Clear();
                columnHeaderInput.Add("07");
                columnHeaderInput.Add("08");
                columnHeaderInput.Add("09");
                columnHeaderInput.Add("10");
                columnHeaderInput.Add("11");
                columnHeaderInput.Add("12");
                columnHeaderInput.Add("13");
                columnHeaderInput.Add("14");
                columnHeaderInput.Add("15");

            }


            else if (comboBox1.Text == "2-Shift" && (DateTime.Now.Hour >= breakShiftAfternoon2.Hours || DateTime.Now.Hour < breakShiftNight2.Hours))
            {
                columnHeaderInput.Clear();
                columnHeaderInput.Add("16");
                columnHeaderInput.Add("17");
                columnHeaderInput.Add("18");
                columnHeaderInput.Add("19");
                columnHeaderInput.Add("20");
                columnHeaderInput.Add("21");
                columnHeaderInput.Add("22");
                columnHeaderInput.Add("23");
                columnHeaderInput.Add("00");

            }
            else if (comboBox1.Text == "3-Shift" && DateTime.Now.Hour >= breakShiftMorning.Hours && DateTime.Now.Hour < breakShiftAfternoon3.Hours)
            {
                columnHeaderInput.Clear();
                columnHeaderInput.Add("07");
                columnHeaderInput.Add("08");
                columnHeaderInput.Add("09");
                columnHeaderInput.Add("10");
                columnHeaderInput.Add("11");
                columnHeaderInput.Add("12");
                columnHeaderInput.Add("13");
                columnHeaderInput.Add("14");

            }
            else if (comboBox1.Text == "3-Shift" && DateTime.Now.Hour >= breakShiftAfternoon3.Hours && DateTime.Now.Hour < breakShiftNight3.Hours)
            {
                columnHeaderInput.Clear();
                columnHeaderInput.Add("15");
                columnHeaderInput.Add("16");
                columnHeaderInput.Add("17");
                columnHeaderInput.Add("18");
                columnHeaderInput.Add("19");
                columnHeaderInput.Add("20");
                columnHeaderInput.Add("21");
                columnHeaderInput.Add("22");

            }
            else
            {
                columnHeaderInput.Clear();
                columnHeaderInput.Add("23");
                columnHeaderInput.Add("00");
                columnHeaderInput.Add("01");
                columnHeaderInput.Add("02");
                columnHeaderInput.Add("03");
                columnHeaderInput.Add("04");
                columnHeaderInput.Add("05");
                columnHeaderInput.Add("06");

            }

            dataGridView1.DataSource = null;
            dataGridView2.DataSource = null;
            dataGridView3.DataSource = null;
            dataGridView4.DataSource = null;

            DataTable dtReturn = Pivot.GetFixedPivotTable(dttable2, "Date", "Line", "SA_SN", "0", "Count", columnHeaderInput);
            DataTable dtReturn2 = Pivot.GetFixedPivotTable(dttable3, "Date", "Line", "SA_SN", "0", "Count", columnHeaderInput);

            dataGridView1.DataSource = dttable1.DefaultView;
            dataGridView2.DataSource = dtReturn.DefaultView;
            dataGridView3.DataSource = dtReturn2.DefaultView;
            dataGridView4.DataSource = dttable4.DefaultView;

            Pivot.GetDivisionCellFormat(dataGridView2, dataGridView3, rejectHighlightQty);

            dataGridView3.DataSource = null;

            dataGridView3.DataSource = Pivot.GetPercentagePivotTable(dtReturn2, "Total").DefaultView;

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text == "3 min")
            {
                timer1.Interval = 180000;
            }
            else if (comboBox2.Text == "5 min")
            {
                timer1.Interval = 300000;
            }
            else if (comboBox2.Text == "10 min")
            {
                timer1.Interval = 600000;
            }
            else if (comboBox2.Text == "20 min")
            {
                timer1.Interval = 1200000;
            }
            else if (comboBox2.Text == "30 min")
            {
                timer1.Interval = 1800000;
            }
            else
            {
                timer1.Interval = 3600000;
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            rejectHighlightQty = Convert.ToInt32(comboBox3.Text);
        }

        private void buttonExitWindow(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            label2.Text = ("Time: " + DateTime.Now.ToLongTimeString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string message =
            "AS = Al Stripping\n" +
            "SS = Spot Soldering\n" +
            "SI = Soldering Inspection\n" +
            "ST = Semi Test\n" +
            "BA = Backshell Assembly\n" +
            "FT = Final Test";
            MessageBox.Show(message);
        }
    }
}
