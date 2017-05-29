using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;



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

            Algorithms.RemoveLowVarianceColumns(d.AllDatax, 8);
            Console.WriteLine("Remove columns based on magicNumber: " + d.AllDatax.Columns.Count);

          //  ExportToExcel.Export(d.AllDatax);

            string hello = "";

            string hello2 = "hello";


            byte[] byteArray = Encoding.ASCII.GetBytes(hello);
            byte[] byteArray2 = Encoding.ASCII.GetBytes(hello2);

            System.Security.Cryptography.MD5CryptoServiceProvider crypto = new System.Security.Cryptography.MD5CryptoServiceProvider();
            System.Security.Cryptography.MD5CryptoServiceProvider crypto2 = new System.Security.Cryptography.MD5CryptoServiceProvider();



            Console.WriteLine(ByteArrayToString(byteArray));
            Console.WriteLine(ByteArrayToString(byteArray2));
                
            Console.WriteLine(hello.GetHashCode() );


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



    }
}
