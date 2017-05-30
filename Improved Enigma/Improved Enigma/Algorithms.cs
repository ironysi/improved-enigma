using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Statistics;

namespace Improved_Enigma
{
    class Algorithms
    {
        public static void RemoveEmptyColumns(DataTable data)
        {
            bool isEmpty = false;
            DataTable copyOfData;
            copyOfData = data.Copy();

            foreach (DataColumn column in copyOfData.Columns)
            {
                foreach (DataRow row in copyOfData.Rows)
                {
                    if (row[column.ColumnName] == null || row[column.ColumnName].Equals(""))
                    {
                        isEmpty = true;
                    }
                    else
                    {
                        isEmpty = false;
                        break;
                    }
                }

                if (isEmpty == true)
                {
                    data.Columns.Remove(column.ColumnName);
                }
            }
        }


        public static void RemoveSameValueColumns(DataTable data)
        {
            DataTable copyOfData;
            copyOfData = data.Copy();

            foreach (DataColumn column in copyOfData.Columns)
            {
                bool delete = true;
                string value = copyOfData.Rows[0][column.ColumnName].ToString();

                foreach (DataRow row in copyOfData.Rows)
                {
                    if (!row[column.ColumnName].Equals(value))
                    {
                        delete = false;
                        break;
                    }
                }

                if (delete == true)
                {
                    data.Columns.Remove(column.ColumnName);
                }

            }
        }

        public static void RemoveLowVarianceColumns(DataTable data, double magicNumber)
        {
            DataTable copyOfData;
            copyOfData = data.Copy();

            foreach (DataColumn column in copyOfData.Columns)
            {
                Dictionary<string, int> uniqueValues = new Dictionary<string, int>();

                foreach (DataRow row in copyOfData.Rows)
                {
                    if (uniqueValues.ContainsKey(row[column.ColumnName].ToString()))
                    {
                        uniqueValues[row[column.ColumnName].ToString()]++;
                    }
                    else
                    {
                        uniqueValues.Add(row[column.ColumnName].ToString(), 0);
                    }
                }

                double max = uniqueValues.Max(x => x.Value);
                double totalRows = copyOfData.Rows.Count;

                double result = (max / totalRows) * 100;

                if ((100 - result) <= magicNumber)
                    data.Columns.Remove(column.ColumnName);
            }
        }

        public static DataTable HashValues(DataTable dataTable)
        {
            DataTable copyOfData;
            copyOfData = dataTable.Copy();

            for (int i = 0; i < copyOfData.Rows.Count; i++)
            {
                for (int k = 0; k < copyOfData.Columns.Count; k++)
                {
                    copyOfData.Rows[i][k] = copyOfData.Rows[i][k].GetHashCode();
                }
            }

            return copyOfData;
        }



        struct aStruct
        {
            public string secondColumnName { get; set; }
            public double correlation { get; set; }

            public aStruct(string desc, double corr)
            {
                secondColumnName = desc;
                correlation = corr;
            }
        }

        private void PrintDictionary()
        {

        }

        public static DataTable ComputePearsonCorrelation(DataTable dt)
        {
            DataTable correlationDataTable = dt.Copy();
            correlationDataTable.Clear();

            //Create empty rows in DataTable
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                correlationDataTable.Rows.Add(correlationDataTable.NewRow());
            }

            // loop thru columns
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                Dictionary<string, List<aStruct>> correlations = new Dictionary<string, List<aStruct>>();

                double[] columnAValues = new double[dt.Rows.Count];

                // save whole column to array for later use
                for (int y = 0; y < dt.Rows.Count; y++)
                {
                    columnAValues[y] = Double.Parse(dt.Rows[y][i].ToString());
                    }

                // loop thru all columns except the one that I'm on currently
                for (int k = 0; k < dt.Columns.Count; k++)
                {
                    if (k != i)
                    {
                        double[] columnBValues = new double[dt.Rows.Count];

                        // save whole column for later use
                        for (int y = 0; y < dt.Rows.Count; y++)
                        {
                            columnBValues[y] = Double.Parse(dt.Rows[y][k].ToString());
                        }

                        // correlation methods
                        double c = Correlation.Pearson(columnAValues, columnBValues);
                        //double c1 = Correlation.Spearman(columnAValues, columnBValues);

                        string name = dt.Columns[k].ColumnName;
                        aStruct a = new aStruct(name, c);

                        // create dictionary member if key does not exist
                        if (!correlations.ContainsKey(dt.Columns[i].ColumnName))
                        {
                            List<aStruct> structList = new List<aStruct>();
                            structList.Add(a);
                            correlations.Add(dt.Columns[i].ColumnName, structList);
                        }
                        // add value to dictionary
                        else
                        {
                            correlations[dt.Columns[i].ColumnName].Add(a);
                        }
                    }
                }

                // save dictionary to datatable that we are returning
                for (int j = 0; j < correlations[dt.Columns[i].ColumnName].Count; j++)
                {
                    correlationDataTable.Rows[j][i] =  correlations[dt.Columns[i].ColumnName][j].secondColumnName +
                          "       " + correlations[dt.Columns[i].ColumnName][j].correlation;
                }

            }

            return correlationDataTable;
        }
    }
}


