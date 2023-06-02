using System;
using System.Collections.Generic;
using System.Text;

namespace HuffmansTree
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter text: ");
            var text = Console.ReadLine();

            Console.WriteLine(HuffmanCode(text));
        }

        private static string HuffmanCode(string text)
        {
            var letterFrequency = new int[256];

            foreach (var t in text)
                letterFrequency[t]++;

            var nodeList = new List<Node>(letterFrequency.Length);
            for (var i = 0; i < letterFrequency.Length; i++)
            {
                if (letterFrequency[i] <= 0)
                    continue;

                var nodeToInsert = new Node
                {
                    Letter = (char)i,
                    Frequency = letterFrequency[i]
                };
                Insert(nodeList, nodeToInsert);
            }

            while (nodeList.Count > 1)
            {
                var node = new Node
                {
                    LeftNode = nodeList[0],
                    RightNode = nodeList[1],
                    Frequency = nodeList[0].Frequency + nodeList[1].Frequency
                };

                nodeList.RemoveAt(0);
                nodeList.RemoveAt(0);

                Insert(nodeList, node);
            }

            var result = new StringBuilder();
            foreach (var c in text)
            {
                result.AppendLine($"{c}: {Encode(nodeList[0], c)}");
            }

            return result.ToString();
        }

        private static void Insert(IList<Node> nodeList, Node nodeToInsert)
        {
            for (int i = 0; i < nodeList.Count; i++)
            {
                if (nodeList[i].Frequency < nodeToInsert.Frequency)
                    continue;

                nodeList.Insert(i, nodeToInsert);
                return;
            }
            nodeList.Add(nodeToInsert);
        }

        private static string Encode(Node root, char letter)
        {
            if (root is null)
            {
                return null;
            }

            if (root.Letter == letter)
            {
                return "";
            }

            var result = Encode(root.LeftNode, letter);
            if (result != null)
            {
                return result + "0";
            }

            result = Encode(root.RightNode, letter);
            if (result != null)
            {
                return result + "1";
            }

            return null;
        }
    }
}
