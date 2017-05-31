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
            Console.WriteLine("Remove columns based on 'variance': " + d.AllDatax.Columns.Count);



            // ExportToExcel.Export(Algorithms.HashValues(d.AllDatax),"Hashed");
            // ExportToExcel.Export(Algorithms.ComputePearsonCorrelation(Algorithms.HashValues(d.AllDatax)),"CorrelationsOnlyNumbers");

            NeuralNet net = new NeuralNet();
            int[] selected = new int[] { 6, 9, 10, 13, 20, 21, 22 };
            int[] selected1 = new int[] { 0,1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13,
                                          14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
            int[] selected2 = new int[] { 3, 6, 9, 10, 12, 13, 19, 20, 21, 22 };

            int[] selected3 = new int[99];
            for (int i = 0; i < selected3.Length; i++)
            {
                selected3[i] = i;
            }

            net.FillData(Algorithms.HashValues(d.AllDatax), selected2, "Statistics group");
            net.Execute();

            //      STATISTICS GROUP
            // with 6 high correlation inputs
            // Iteration #20 Error:0.641211% Target Error: 0.100000%

            // with 23 inputs
            // Iteration #20 Error:0.640036% Target Error: 0.100000%

            // with 99 inputs
            // Iteration #20 Error:0.644229% Target Error: 0.100000%



            Console.Read();
        }

    }
}
