using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DaxNEATCore;
using ML.Data;
using ML.Data.Basic;
using ML.EA.Train;
using Networks.Training;
using NEAT;
using Util.Simple;

namespace Improved_Enigma.Network
{
    public class NeuralNet
    {
        public double[][] Inputs { get; set; }
        public double[][] Outputs { get; set; }

        private double[][] doubleAllData;

        public void Execute(int inputNeuronsCount, int outputNeuronsCount)
        {
            IMLDataSet trainingSet = new BasicMLDataSet(Inputs, Outputs);
            NEATPopulation pop = new NEATPopulation(inputNeuronsCount, outputNeuronsCount, 10000);
            pop.Reset();
            pop.InitialConnectionDensity = 1.0; // not required, but speeds processing.
            ICalculateScore score = new TrainingSetScore(trainingSet);
            // train the neural network
            TrainEA train = NEATUtil.ConstructNEATTrainer(pop, score);

            MainUtility.TrainToError(train, 0.001);

            NEATNetwork network = (NEATNetwork)train.CODEC.Decode(train.BestGenome);

            // test the neural network
            Console.WriteLine(@"Neural Network Results:");
            MainUtility.Evaluate(network, trainingSet);

            SaveNEATNetwork save = new SaveNEATNetwork();
            //save the current network til an binary file
            save.saveNEATNetwork(network, "data.bin");
            //save the current network til an binary file
            save.saveNEATTextfile(network, "test.txt");

            //Pause the console window
            Console.ReadKey();
        }



        public void FillData(DataTable dataTable, int[] selectedColumns, string outputColumnName)
        {
            string[][] rawData = new string[dataTable.Rows.Count][];

            DataTable dt = new DataTable();
            int k = 0;

            for (int i = 0; i < selectedColumns.Max() + 1; i++)
            {
                if (selectedColumns.Any(x => x == i))
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

            int dtColumnCount = dt.Columns.Count;
            dt.Columns[outputColumnName].SetOrdinal(dtColumnCount-1);

            for (int y = 0; y < dataTable.Rows.Count; y++)
            {
                string[] array = new string[selectedColumns.Length];

                for (int j = 0; j < selectedColumns.Length; j++)
                {
                    array[j] = dt.Rows[y][j].ToString();
                }
                rawData[y] = array;
            }


            string[] categories = new string[dtColumnCount]; 
            // FILLS UP CATEGORY STRING
            for (int i = 0; i < dtColumnCount-1; i++)
            {
                categories[i] = "numeric";
            }

            categories[dtColumnCount-1] = "categorical";

            Standardizer standarizer = new Standardizer(rawData, categories);

            doubleAllData = standarizer.StandardizeAll(rawData);

            int uniqueValuesCount = CountDistinctTypesOfOutput(rawData, dtColumnCount-1);

            Inputs = TransferColumns(doubleAllData, selectedColumns.Length-1, 0);
            Outputs = TransferColumns(doubleAllData, uniqueValuesCount, selectedColumns.Length - 1);
        }

        private double[][] TransferColumns(double[][] x, int numberOfColumns, int startAtPosition)
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

        private int CountDistinctTypesOfOutput(string[][] lines, int positionOfOutput)
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

            return x.Distinct().Count();
        }


    }
}
