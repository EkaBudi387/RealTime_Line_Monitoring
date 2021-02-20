using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsAppFinalTestReject
{
    public class Pivot
    {
        public static DataTable GetPivotTable(DataTable tableDBInput, string columnFieldInput, string rowFieldInput, string valueFieldInput, string nullValueInput, string aggregateMethod)
        {
            DataTable tableInput = tableDBInput.Copy();

            DataTable tableReturner = new DataTable();
            tableReturner.Columns.Add(rowFieldInput);
            List<string> columnHeaderCollector = new List<string>();

            foreach (DataRow dataRow in tableInput.Rows)
            {
                string columnHeader = dataRow[columnFieldInput].ToString();
                if (!columnHeaderCollector.Contains(columnHeader))
                {
                    columnHeaderCollector.Add(columnHeader);
                }
            }
            columnHeaderCollector.Sort((x, y) => x.CompareTo(y));
            foreach (string a in columnHeaderCollector)
            {
                tableReturner.Columns.Add(a);
            }
            if (rowFieldInput != "" && valueFieldInput != "")
            {
                List<string> rowHeaderCollector = new List<string>();
                foreach (DataRow dataRow in tableInput.Rows)
                {
                    string rowHeader = dataRow[rowFieldInput].ToString();
                    if (!rowHeaderCollector.Contains(rowHeader))
                        rowHeaderCollector.Add(rowHeader);
                }
                foreach (string rowHeaderInsideCollector in rowHeaderCollector)
                {
                    DataRow rowToAppend = tableReturner.NewRow();
                    rowToAppend[0] = rowHeaderInsideCollector;
                    DataRow[] filteredTableInput = tableInput.Select(rowFieldInput + "='" + rowHeaderInsideCollector + "'");
                    foreach (DataRow dataRow in filteredTableInput)
                    {
                        string columnHeaderPointer = dataRow[columnFieldInput].ToString();
                        foreach (DataColumn dataColumn in tableReturner.Columns)
                        {
                            if (dataColumn.ColumnName == columnHeaderPointer)
                            {
                                if (aggregateMethod == "Count")
                                {
                                    try
                                    {
                                        rowToAppend[columnHeaderPointer] = Convert.ToUInt16(rowToAppend[columnHeaderPointer]) + 1;
                                    }
                                    catch
                                    {
                                        rowToAppend[columnHeaderPointer] = 1;
                                    }
                                }
                                else if (aggregateMethod == "Sum")
                                {
                                    try
                                    {
                                        rowToAppend[columnHeaderPointer] = Convert.ToDecimal(rowToAppend[columnHeaderPointer]) + Convert.ToDecimal(rowToAppend[valueFieldInput]);
                                    }
                                    catch
                                    {
                                        rowToAppend[columnHeaderPointer] = Convert.ToDecimal(rowToAppend[valueFieldInput]); ;
                                    }
                                }
                                else
                                {
                                    throw new Exception("Not an aggregation method");
                                }
                            }
                        }
                    }
                    tableReturner.Rows.Add(rowToAppend);
                }
            }

            if (nullValueInput != "")
            {
                foreach (DataRow dataRow in tableReturner.Rows)
                {
                    foreach (DataColumn dataColumn in tableReturner.Columns)
                    {
                        if (dataRow[dataColumn.ColumnName].ToString() == "")
                            dataRow[dataColumn.ColumnName] = nullValueInput;
                    }
                }
            }

            tableReturner.Columns.Add("Grand Total");

            foreach (DataRow dataRowX in tableReturner.Rows)
            {
                dataRowX["Grand Total"] = 0;

                foreach (string columnHeader in columnHeaderCollector)
                {
                    if (columnHeader != rowFieldInput && dataRowX[columnHeader].ToString() != "")
                        dataRowX["Grand Total"] = Convert.ToUInt16(dataRowX["Grand Total"]) + Convert.ToUInt16(dataRowX[columnHeader]);
                }
            }

            DataRow newRowForGrandTotal = tableReturner.NewRow();
            newRowForGrandTotal[0] = "Grand Total";
            columnHeaderCollector.Add("Grand Total");
            foreach (string columnHeader in columnHeaderCollector)
            {
                newRowForGrandTotal[columnHeader] = 0;
                foreach (DataRow dataRowX in tableReturner.Rows)
                {
                    if (columnHeader != rowFieldInput && dataRowX[columnHeader].ToString() != "")
                        newRowForGrandTotal[columnHeader] = Convert.ToUInt16(newRowForGrandTotal[columnHeader]) + Convert.ToUInt16(dataRowX[columnHeader]);
                }
            }

            tableReturner.Rows.Add(newRowForGrandTotal);

            return tableReturner;
        }

        public static DataTable GetPercentagePivotTable(DataTable pivotTableInput, string percentageMethod)
        {

            DataTable percentagePivotTable = pivotTableInput.Copy();

            int grandTotalDevider = Convert.ToInt32(percentagePivotTable.Rows[percentagePivotTable.Rows.Count - 1]["Grand Total"]);


            foreach (DataRow dataRowInTableInput in percentagePivotTable.Rows)
            {

                foreach (DataColumn dataColumnInTableReturn in percentagePivotTable.Columns)

                {

                    try
                    {
                        if (percentageMethod == "Grand Total")
                        {
                            dataRowInTableInput[dataColumnInTableReturn.ColumnName] = decimal.Round(Convert.ToDecimal(dataRowInTableInput[dataColumnInTableReturn.ColumnName]) / grandTotalDevider * 100, 2).ToString() + "%";
                        }
                        else
                        {
                            int rowTotalDevider = Convert.ToInt32(percentagePivotTable.Rows[percentagePivotTable.Rows.Count - 1][dataColumnInTableReturn.ColumnName]);

                            dataRowInTableInput[dataColumnInTableReturn.ColumnName] = decimal.Round(Convert.ToDecimal(dataRowInTableInput[dataColumnInTableReturn.ColumnName]) / rowTotalDevider * 100, 2).ToString() + "%";
                        }
                    }
                    catch
                    {

                    }
                }
            }

            return percentagePivotTable;
        }

        public static void GetDataGridCellColor(DataGridView dataGridView, int highlightQty)
        {

            for (int i = 0; i < dataGridView.Rows.Count - 1; i++)
            {
                for (int j = 0; j < dataGridView.Columns.Count - 1; j++)
                {
                    try
                    {
                        int k = Convert.ToInt32(dataGridView.Rows[i].Cells[j].Value);
                        if (k >= highlightQty)
                            dataGridView.Rows[i].Cells[j].Style.BackColor = Color.Red;
                    }
                    catch
                    {

                    }
                }
            }

        }

        public static DataTable GetFixedPivotTable(DataTable tableDBInput, string columnFieldInput, string rowFieldInput, string valueFieldInput, string nullValueInput, string aggregateMethod, List<string> columnHeaderInput)
        {

            DataTable tableInput = tableDBInput.Copy();
            DataTable tableReturner = new DataTable();

            tableReturner.Columns.Add(rowFieldInput);

            List<string> columnHeaderCollector = new List<string>(columnHeaderInput);

            foreach (string a in columnHeaderCollector)
            {
                tableReturner.Columns.Add(a);
            }
            if (rowFieldInput != "" && valueFieldInput != "")
            {
                List<string> rowHeaderCollector = new List<string>();
                rowHeaderCollector.Add("L01");
                rowHeaderCollector.Add("L02");
                rowHeaderCollector.Add("L03");
                rowHeaderCollector.Add("L04");
                rowHeaderCollector.Add("L05");

                foreach (string rowHeaderInsideCollector in rowHeaderCollector)
                {
                    DataRow rowToAppend = tableReturner.NewRow();
                    rowToAppend[0] = rowHeaderInsideCollector;
                    DataRow[] filteredTableInput = tableInput.Select(rowFieldInput + "='" + rowHeaderInsideCollector + "'");
                    foreach (DataRow dataRow in filteredTableInput)
                    {
                        string columnHeaderPointer = dataRow[columnFieldInput].ToString();
                        foreach (DataColumn dataColumn in tableReturner.Columns)
                        {
                            if (dataColumn.ColumnName == columnHeaderPointer)
                            {
                                if (aggregateMethod == "Count")
                                {
                                    try
                                    {
                                        rowToAppend[columnHeaderPointer] = Convert.ToUInt16(rowToAppend[columnHeaderPointer]) + 1;
                                    }
                                    catch
                                    {
                                        rowToAppend[columnHeaderPointer] = 1;
                                    }
                                }
                                else if (aggregateMethod == "Sum")
                                {
                                    try
                                    {
                                        rowToAppend[columnHeaderPointer] = Convert.ToDecimal(rowToAppend[columnHeaderPointer]) + Convert.ToDecimal(rowToAppend[valueFieldInput]);
                                    }
                                    catch
                                    {
                                        rowToAppend[columnHeaderPointer] = Convert.ToDecimal(rowToAppend[valueFieldInput]); ;
                                    }
                                }
                                else
                                {
                                    throw new Exception("Not an aggregation method");
                                }
                            }
                        }
                    }
                    tableReturner.Rows.Add(rowToAppend);
                }
            }

            if (nullValueInput != "")
            {
                foreach (DataRow dataRow in tableReturner.Rows)
                {
                    foreach (DataColumn dataColumn in tableReturner.Columns)
                    {
                        if (dataRow[dataColumn.ColumnName].ToString() == "")
                            dataRow[dataColumn.ColumnName] = nullValueInput;
                    }
                }
            }

            tableReturner.Columns.Add("Grand Total");

            foreach (DataRow dataRowX in tableReturner.Rows)
            {
                dataRowX["Grand Total"] = 0;

                foreach (string columnHeader in columnHeaderCollector)
                {
                    if (columnHeader != rowFieldInput && dataRowX[columnHeader].ToString() != "")
                        dataRowX["Grand Total"] = Convert.ToUInt16(dataRowX["Grand Total"]) + Convert.ToUInt16(dataRowX[columnHeader]);
                }
            }

            DataRow newRowForGrandTotal = tableReturner.NewRow();
            newRowForGrandTotal[0] = "Grand Total";
            columnHeaderCollector.Add("Grand Total");
            foreach (string columnHeader in columnHeaderCollector)
            {
                newRowForGrandTotal[columnHeader] = 0;
                foreach (DataRow dataRowX in tableReturner.Rows)
                {
                    if (columnHeader != rowFieldInput && dataRowX[columnHeader].ToString() != "")
                        newRowForGrandTotal[columnHeader] = Convert.ToUInt16(newRowForGrandTotal[columnHeader]) + Convert.ToUInt16(dataRowX[columnHeader]);
                }
            }

            tableReturner.Rows.Add(newRowForGrandTotal);

            return tableReturner;
        }

        public static void GetDivisionCellFormat(DataGridView dataGridView1, DataGridView dataGridView2, int rejectHighlightQty)
        {
            GetDataGridCellColor(dataGridView1, rejectHighlightQty);

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                for (int j = 1; j < dataGridView1.ColumnCount; j++)
                {

                    dataGridView1.Rows[i].Cells[j].Value = dataGridView1.Rows[i].Cells[j].Value.ToString() + "/" + dataGridView2.Rows[i].Cells[j].Value.ToString();

                }
            }
        }

    }
}
