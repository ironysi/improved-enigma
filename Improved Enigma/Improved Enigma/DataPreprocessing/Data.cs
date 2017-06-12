using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Improved_Enigma.DataPreprocessing
{
    public class Data
    {
        public DataTable AllDatax { get; set; } = new DataTable();


        public Data(string path, string fileName)
        {
            ReadDataFromCSVToDataTable(path, fileName);
        }


        private List<string> ReadDataFromCSVToList(string path, string fileName)
        {
        List<string> allData = new List<string>();

            using (var fs = File.OpenRead(path + fileName + ".csv"))
            using (var reader = new StreamReader(fs))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();

                    allData.Add(line);
                }
            }
            return allData;
        }

        private void ReadDataFromCSVToDataTable(string path, string fileName)
        {
            List<string> allData = ReadDataFromCSVToList(path, fileName);

            for (int i = 0; i < allData.Count() - 1; i++)
            {
                string[] rowValues = allData[i].Split(';'); //split each row with comma to get individual values  
                {
                    if (i == 0)
                    {
                        for (int j = 0; j < rowValues.Count(); j++)
                        {
                            AllDatax.Columns.Add(rowValues[j]); //add headers  
                        }
                    }
                    else
                    {
                        DataRow dr = AllDatax.NewRow();

                        for (int k = 0; k < rowValues.Count(); k++)
                        {
                            dr[k] = rowValues[k]; // there is 99 columns in excel
                        }
                        AllDatax.Rows.Add(dr); //add other rows  
                    }
                }
            }
        }

        public void PrintTest()
        {
            foreach (DataRow dataRow in AllDatax.Rows)
            {
                foreach (var item in dataRow.ItemArray)
                {
                    Console.Write(item+ ";");
                }
                Console.WriteLine("/n");
            }
            Console.ReadKey();
        }

    }
}
