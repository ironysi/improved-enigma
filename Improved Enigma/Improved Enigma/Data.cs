using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Improved_Enigma
{
    class Data
    {
        private List<string> allData = new List<string>();

        public void ReadDataFromCSV(string fileName)
        {
            using (var fs = File.OpenRead(".../Excel/" + fileName + ".csv"))
            using (var reader = new StreamReader(fs))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    allData.Add(line);
                }
            }
        }
    }
}
