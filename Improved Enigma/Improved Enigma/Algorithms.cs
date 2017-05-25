using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                if((100 - result) <= magicNumber)
                    data.Columns.Remove(column.ColumnName);
            }
        }
    }
}


