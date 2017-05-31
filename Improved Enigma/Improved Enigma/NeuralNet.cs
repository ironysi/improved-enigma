using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using ML.Data;
using ML.Data.Basic;
using Networks.Training;
using Util.Simple;
using ML.EA.Train;
using NEAT;
using DaxNEATCore;




namespace Improved_Enigma
{
    class NeuralNet
    {
        public double[][] Inputs { get; set; }
        public double[][] Outputs { get; set; }

        private double[][] doubleAllData;

        public void Execute()
        {
            IMLDataSet trainingSet = new BasicMLDataSet(Inputs, Outputs);
            NEATPopulation pop = new NEATPopulation(2, 1, 10000);
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



        public void FillData(DataTable dataTable)
        {
            string[][] rawData = new string[dataTable.Rows.Count][];

            int[] selected = new int[] { 6, 9, 10, 13, 20, 21, 22 };

            DataTable dt = new DataTable();
            int k = 0;

            for (int i = 0; i < selected.Max() + 1; i++)
            {
                if (selected.Any(x => x == i))
                {
                    dt.Columns.Add(new DataColumn(dataTable.Columns[i].ColumnName));

                    for (int j = 0; j < dataTable.Rows.Count; j++)
                    {
                        if (k == 0)
                        {
                            dt.Rows.Add(dt.NewRow());
                        }
                        else
                        {
                            dt.Rows[j][k] = dataTable.Rows[j][k];

                        }
                        k++;
                    }
                }
            }
            for (int y = 0; y < dataTable.Rows.Count; y++)
            {
                string[] array = new string[selected.Length];

                for (int j = 0; j < selected.Length; j++)
                {
                    array[j] = dt.Rows[y][j].ToString();
                }
                rawData[y] = array;
            }


            Standardizer standarizer = new Standardizer(rawData, new string[]{"numeric","numeric", "numeric",
                "numeric", "numeric","numeric","categorical" });

            doubleAllData = standarizer.StandardizeAll(rawData);

            Inputs = TransferColumns(doubleAllData, 6, 0);
            Outputs = TransferColumns(doubleAllData, 4, 6);

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
