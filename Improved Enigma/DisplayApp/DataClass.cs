using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayApp
{
    class DataClass
    {
        public DataTable AllDatax { get; set; } = new DataTable();


        private List<string> ReadDataFromCSVToList(string fileName)
        {
            List<string> allData = new List<string>();

            using (var fs = File.OpenRead("....../SavedFile.xlsx"))
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

        private void ReadDataFromCSVToDataTable(string fileName)
        {
            List<string> allData = ReadDataFromCSVToList(fileName);

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
                            dr[k] = rowValues[k].ToString(); // there is 99 columns in excel
                        }
                        AllDatax.Rows.Add(dr); //add other rows  
                    }
                }
            }
        }

    }
}
