using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Improved_Enigma
{
    class Program
    {
        public static string TimeStamp { get; private set; }

        static void Main(string[] args)
        {
            Data d = new Data("Data sample2");

            Algorithms.RemoveEmptyColumns(d.AllDatax);
            Console.WriteLine("Remove empty columns: " + d.AllDatax.Columns.Count);

            Algorithms.RemoveSameValueColumns(d.AllDatax);
            Console.WriteLine("Remove same value columns: " + d.AllDatax.Columns.Count);

            Algorithms.RemoveLowVarianceColumns(d.AllDatax, 5.0);
            Console.WriteLine("Remove columns based on magicNumber: " + d.AllDatax.Columns.Count);

            SaveFile(d.AllDatax);

            string hello = "hello";

            string hello2 = "hello";


            byte[] byteArray = Encoding.ASCII.GetBytes(hello);
            byte[] byteArray2 = Encoding.ASCII.GetBytes(hello2);

            System.Security.Cryptography.MD5CryptoServiceProvider crypto = new System.Security.Cryptography.MD5CryptoServiceProvider();
            System.Security.Cryptography.MD5CryptoServiceProvider crypto2 = new System.Security.Cryptography.MD5CryptoServiceProvider();



            Console.WriteLine(ByteArrayToString(byteArray));
            Console.WriteLine(ByteArrayToString(byteArray2));



            Console.Read();
        }

        private static string ByteArrayToString( byte[] byteArray)
        {
            StringBuilder hex = new StringBuilder(byteArray.Length * 2);
            foreach (byte b in byteArray)
            {
                hex.AppendFormat("{0:x2}", b);
            }

            return hex.ToString();
        }

        private static void SaveFile(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dt.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                sb.AppendLine(string.Join(",", fields));
            }

            File.WriteAllText("file " + DateTime.Now.ToString("h-m") + ".csv", sb.ToString());
        }
    }
}
