﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaxNEATCore;
using System.Data;

namespace Improved_Enigma
{
    class NeuralNet
    {
        public double[][] Inputs { get; set; }
        public double[][] Outputs { get; set; }

        private double[][] doubleAllData;

        public DataTable FillData(DataTable dataTable)
        {
            string[][] rawData = new string[dataTable.Rows.Count][];

            DataTable dt = new DataTable();
            int k = 0;

            for (int i = 0; i < 24; i++)
            {
                if (i == 6 || i == 9 || i == 10 || i == 13 || i == 20 || i == 21 || i == 22)
                {
                    dt.Columns.Add(new DataColumn(dataTable.Columns[i].ColumnName));

                    for (int j = 0; j < dataTable.Rows.Count; j++)
                    {
                        if (k == 0)
                        {
                            dt.Rows.Add(dt.NewRow());
                        }

                        dt.Rows[j][k] = dataTable.Rows[j][i];
                    }
                    k++;
                }
            }

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                string[] array = new string[7];

                for (int j = 0; j < 7; j++)
                {
                    array[j] = dt.Rows[i][j].ToString();
                }
                rawData[i] = array;
            }


            Standardizer standarizer = new Standardizer(rawData, new string[]{"numeric","numeric", "numeric",
                "numeric", "numeric","numeric","categorical" });

            doubleAllData = standarizer.StandardizeAll(rawData);

            Inputs = TransferColumns(doubleAllData, 6, 0 );
            Outputs = TransferColumns(doubleAllData, 4, 6);


            return dt;
        }

        private double[][] TransferColumns( double[][] x, int numberOfColumns, int startAtPosition)
        {
            double[][] z = new double[x.Length][];



            for (int i = 0; i < x.Length; i++)
            {
                double[] array = new double[numberOfColumns];

                for (int j = 0; j < numberOfColumns; j++)
                {
                    array[j] = x[i][j + startAtPosition];
                }
                z[i] = array;
            }

            return z;
        }

        private List<string> CountDistinctTypesOfOutput(string[][] lines, int positionOfOutput)
        {
            List<string> x = new List<string>();

            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[0].Length; j++)
                {
                    if (j == positionOfOutput)
                    {
                        x.Add(lines[i][j]);
                    }
                }
            }

            Console.WriteLine(x.Distinct().Count());

            return x.Distinct() as List<string>;
        }

    }
}