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

           // ExportToExcel.Export(d.AllDatax, "CleanVersion");
           // ExportToExcel.Export(Algorithms.HashValues(d.AllDatax),"Hashed");
            ExportToExcel.Export(Algorithms.ComputePearsonCorrelation(Algorithms.HashValues(d.AllDatax)),"Correlations");



            Console.Read();
        }

    }
}
