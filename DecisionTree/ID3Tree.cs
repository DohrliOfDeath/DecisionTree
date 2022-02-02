using System;
using System.Collections.Generic;
using System.Linq;

namespace DecisionTree
{
    class ID3Tree
    {
        /// <summary>
        /// building a new tree, this method is called recursively and adding nodes to it's root node
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dat">input dataset</param>
        /// <param name="label">current label of the node</param>
        /// <returns>the current node</returns>
        public Node NodeTrain<T>(Enum[][] dat, string label)
        {
            Node next = new();

            double totalEntropy = GetTotalEntropy(dat);
            Console.WriteLine("starting Label: " + label);
            Console.WriteLine("Entropy All: " + totalEntropy);

            (int maxIndex, double maxValue) = GetHighestInfoGain(dat, totalEntropy);
            Console.WriteLine("highest information gain: " + maxValue + " at index " + maxIndex);
            next.AttributIndex = maxIndex;
            next.Label = label;
            next.Children = new List<Node>();

            
            Enum[] labelColumn = new Enum[dat.Length];
            for (int i = 0; i < dat.Length; i++)
                labelColumn[i] = dat[i][Data.labelPosition];
            if (labelColumn.Distinct().Count() == 1) //exiting the recursive loop, if this was binary (it isn't): 
            { //if only one different result is in the dataset, it quits after adding a result node
                Node resultNode = new();
                resultNode.Label = labelColumn[0].ToString();
                next.Children.Add(resultNode);
                return next; 
            }

            Enum[] maxColumn = new Enum[dat.Length];
            for (int i = 0; i < dat.Length; i++)
                maxColumn[i] = dat[i][maxIndex];
            foreach (var line in maxColumn.GroupBy(info => info.ToString()))
            {
                // start recursively again
                next.Children.Add(NodeTrain<T>(CutDataEnum(dat, maxIndex, line.Key.ToString()), line.Key.ToString()));
            }
            return next;
        }

        /// <summary>
        /// Cuts the data enum so the tree building doesn't result in an endless loop
        /// the irreleveant data from the dataset is removed
        /// </summary>
        /// <param name="dat">current dataset</param>
        /// <param name="maxIndex">index where the information gain is highest</param>
        /// <param name="currentOption">das derzeitige Label</param>
        /// <returns>cut data array</returns>
        private static Enum[][] CutDataEnum(Enum[][] dat, int maxIndex, string currentOption) 
        {
            //cut dat so that it isn't an endless loop
            //TODO: use linq for this
            Enum[][] newDat = new Enum[dat.Length][];
            int counter = 0;
            foreach(var line in dat)
            {
                if (line[maxIndex].ToString() != currentOption) continue;  // continue if this column was already executed
                newDat[counter] = line;
                counter++;
            }
            return newDat.Where(c => c != null).ToArray(); 
        }

        /// <summary>
        /// this method is for getting the highest information gain attribute for this specific dataset
        /// </summary>
        /// <param name="dat">the current dataset</param>
        /// <param name="totalEntropy">total entropy of the whole dataset</param>
        /// <returns>tuple of (Index of highest information gain, highest information gain as number)</returns>
        private static (int, double) GetHighestInfoGain(Enum[][] dat, double totalEntropy)
        {
            double[] all_information_gains = new double[dat[0].Length];

            //get information gain
            for (int column = 0; column < dat[0].Length; column++)
            {
                if (column == Data.labelPosition) continue;
                Enum[] currentColumn = new Enum[dat.Length];
                for (int i = 0; i < dat.Length; i++)
                    currentColumn[i] = dat[i][column];

                double information_gain = totalEntropy;
                // calculating all entropies
                foreach (var line in currentColumn.GroupBy(info => info.ToString())
                        .Select(group => new
                        {
                            Metric = group.Key,
                            Count = group.Count()
                        }))
                {
                    //getting yes or no count for each group => only binary
                    //=> getting result count for each group
                    // cut dat to only have this group inside, count the labels accordingly

                    var cutDat = new Enum[dat.Length][];  // TODO: get this into a linq query
                    for (int i = 0; i < dat.Length; i++)
                        if (currentColumn[i].ToString() == line.Metric)
                            cutDat[i] = dat[i];

                    cutDat = cutDat.Where(c => c != null).ToArray();

                    //group the individual labels for this cut array
                    Enum[] label = new Enum[cutDat.Length];
                    for (int i = 0; i < cutDat.Length; i++)
                        label[i] = cutDat[i][Data.labelPosition];

                    //calculate the entropy for this group
                    double currentEntropy = GetEntropyForGroup(cutDat, label);
                    information_gain -= currentEntropy * (Convert.ToDouble(cutDat.Length) / Convert.ToDouble(dat.Length));
                }
                all_information_gains[column] = information_gain;
            }
            return (all_information_gains.ToList().IndexOf(all_information_gains.Max()), all_information_gains.Max());
        }
        /// <summary>
        /// more or less the same as GetTotalEntropy
        /// </summary>
        /// <param name="cutDat">current data</param>
        /// <param name="label">label column as Enum[]</param>
        /// <returns>entropy for this group</returns>
        private static double GetEntropyForGroup(Enum[][] cutDat, Enum[] label)
        {
            double currentEntropy = 0;
            foreach (var lineLabel in label.GroupBy(info => info.ToString())
                        .Select(group => new
                        {
                            Metric = group.Key,
                            Count = group.Count()
                        }))
            {

                currentEntropy -= (Convert.ToDouble(lineLabel.Count) / Convert.ToDouble(cutDat.Length))
                    * Math.Log2(Convert.ToDouble(lineLabel.Count) / Convert.ToDouble(cutDat.Length));
            }

            return currentEntropy;
        }
        /// <summary>
        /// calculates total entropy
        /// </summary>
        /// <param name="dat">the input data array</param>
        /// <returns>total entropy</returns>
        private static double GetTotalEntropy(Enum[][] dat)
        {
            // total entropy
            // íf this would have been binary (it isn't):
            // -(jo/ois)*log(jo/ois)-(na/ois)*log(na/ois)
            Enum[] label = new Enum[dat.Length];
            for (int i = 0; i < dat.Length; i++)
            {
                label[i] = dat[i][Data.labelPosition];
            }

            double total_entropy = 0;
            foreach (var line in label.GroupBy(info => info.ToString())
                        .Select(group => new
                        {
                            Count = group.Count()
                        }))
            {
                //  Console.WriteLine("{0} {1}", line.Metric, line.Count);
                total_entropy -= (Convert.ToDouble(line.Count) / Convert.ToDouble(dat.Length))
                    * Math.Log2(Convert.ToDouble(line.Count) / Convert.ToDouble(dat.Length));
            }
            return total_entropy;
        }
    }
}
