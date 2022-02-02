using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
    class Node
    {
        //Attributindex
        public int AttributIndex { get; set; }

        //children
        public List<Node> Children { get; set; }
        //label
        public string Label { get; set; }

        /// <summary>
        /// outputs the finished tree to the console, recursively
        /// </summary>
        /// <param name="depth">is used for recursion, start with 0</param>
        public void DisplayTree(int depth)
        {
            for(int i = 0; i < depth; i++)
                Console.Write("-");
            Console.WriteLine(Label);
            if (Children == null) return;
            foreach (var c in Children)
                c.DisplayTree(depth + 1);
        }

        /// <summary>
        /// evauluation of attributes in the built ID3Tree, this method works recursively
        /// </summary>
        /// <param name="inputTree">the attributes it checks for, Evaluationinput</param>
        /// <returns>The result as Result Enum</returns>
        //public Data.Play Evaluate(Enum[] inputTree) 
        public Enum Evaluate<T>(Enum[] inputTree)
        {
            if (inputTree.Length == 0)  //quits when every input is already assigned
            {
                Console.WriteLine("exiting: " + Label);
                foreach (var val in Enum.GetValues(typeof(Data.Play)))
                    if (val.ToString() == Children[0].Label) return (Data.Play)val; //should return yes or no with this dataset
            }
            foreach (Node c in Children)  //checking wether any of the children nodes are in the input tree
                foreach(var i in inputTree)
                    if (c.Label == i.ToString())
                        return c.Evaluate<T>(inputTree.Where(x => x.ToString() != c.Label).ToArray());  //starts recursively again, also returns the result from the lower nodes back

            throw new ArgumentException("Wrong Evaluationinput or error in tree");
        }
    }
}
