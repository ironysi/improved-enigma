using System;
using Improved_Enigma.DataPreprocessing;
using Improved_Enigma.Network;
using System.Data;



namespace Improved_Enigma
{
    class Program
    {
        static void Main(string[] args)
        {
            DataPreprocessing.Data d = new DataPreprocessing.Data("Data sample2");

            Algorithms.RemoveEmptyColumns(d.AllDatax);
            Console.WriteLine("Remove empty columns: " + d.AllDatax.Columns.Count);

            Algorithms.RemoveSameValueColumns(d.AllDatax);
            Console.WriteLine("Remove same value columns: " + d.AllDatax.Columns.Count);

            Algorithms.RemoveLowVarianceColumns(d.AllDatax, 8);
            Console.WriteLine("Remove columns based on 'variance': " + d.AllDatax.Columns.Count);



            //ExportToExcel.Export(Algorithms.HashValues(d.AllDatax),"Hashed");
            // ExportToExcel.Export(Algorithms.ComputeCorrelation(Algorithms.HashValues(d.AllDatax), 3),"CorrelationsOnlyNumbers");

            NeuralNet net = new NeuralNet();
            DataTable hashDataTable = Algorithms.HashValues(d.AllDatax);

            // array indexes of selected columns
            int[] selected =
                Algorithms.SelectColumnsBasedOnCorrelation(
                    Algorithms.ComputeCorrelation(hashDataTable, 3), "Dimension", 6, 0.2);

            net.FillData(hashDataTable, selected, "Dimension");
            net.Execute(selected.Length - 1, 6);


            Console.Read();
        }

    }
}
