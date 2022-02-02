using System;
using System.Linq;

namespace DecisionTree
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ID3Tree decisionTree = new();

            Enum[] labelColumn = new Enum[Data.data.Length];
            for (int i = 0; i < Data.data.Length; i++)
                labelColumn[i] = Data.data[i][Data.labelPosition];

            Data.distinctLabel = labelColumn.Distinct().Count();
            var rootNode = decisionTree.NodeTrain<Data.Play>(Data.data, "root");
            rootNode.DisplayTree(0);
            Data.Play result = (Data.Play)rootNode.Evaluate<Data.Play>(new Enum[] { Data.Outlook.Sunny, Data.Humidity.Normal });
            Console.WriteLine("RESULT: " + result.ToString());
        }
    }
}
