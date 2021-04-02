using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanEncoding
{
    public class Node
    {
        public string Symbol;
        public Node Left, Right;
        public int Freq;

        public Node(string symbol, int freq)
        {
            Symbol = symbol;
            Left = Right = null;
            Freq = freq;
        }
    }

    static class Compression
    {
        public static string Huffman(string input)
        {
            if (String.IsNullOrEmpty(input)) return String.Empty;

            StringBuilder sb = new StringBuilder();
            Dictionary<string, string> mapping = new Dictionary<string, string>();
      
            List<Node> freqList = GetFreqList(input);

            Sort(ref freqList);

            while (true)
            { 
                //PrintList(freqList);

                if (freqList.Count == 1)
                {
                    break;
                }

                Node lowest1 = freqList[0];
                Node lowest2 = freqList[1];

                int combinedFreq = lowest1.Freq + lowest2.Freq;

                for (int i = 0; i < freqList.Count; ++i)
                {
                    if (freqList[i].Symbol == lowest1.Symbol || freqList[i].Symbol == lowest2.Symbol)
                    {
                        freqList.RemoveAt(i);
                        i--;
                    }
                }

                Node root = new Node(lowest1.Symbol + lowest2.Symbol, combinedFreq);
                freqList.Add(root);

                root.Left = lowest1;
                root.Right = lowest2;

                Sort(ref freqList);
            }

            mapping = Traverse(freqList);

            // Print Mapping:
            Console.WriteLine("Mapping:");
            foreach (KeyValuePair<string, string> kvp in mapping)
            {
                Console.WriteLine(kvp.Key + " : " + kvp.Value);
            }

            Console.WriteLine();

            for (int i = 0; i < input.Length; ++i)
            {
                string curr = input[i].ToString();

                if (mapping.ContainsKey(curr))
                {
                    sb.Append(mapping[curr]);
                }
            }

            return sb.ToString();
        }

        private static string GetPath(Node node, Dictionary<Node, Node> parents)
        {
            if (node == null || parents == null) return String.Empty;

            StringBuilder sb = new StringBuilder();
            Stack<Node> stack = new Stack<Node>();
            
            Node temp = node;

            while (temp != null)
            {
                stack.Push(temp);
                temp = parents[temp];
            }
           
            while (stack.Count != 0)
            {
                Node curr = stack.Pop();

                if (curr.Left != null && stack.Peek() != null)
                {
                    if (curr.Left.Symbol == stack.Peek().Symbol)
                    {
                        sb.Append("0");
                    }
                }

                if (curr.Right != null && stack.Peek() != null)
                {
                    if (curr.Right.Symbol == stack.Peek().Symbol)
                    {
                        sb.Append("1");
                    }
                }
            }           

            return sb.ToString();
        }

        private static Dictionary<string, string> Traverse(List<Node> list)
        {
            Dictionary<string, string> mapping = new Dictionary<string, string>();

            if (list == null) return mapping;

            Dictionary<Node, Node> parents = new Dictionary<Node, Node>();
            Stack<Node> stack = new Stack<Node>();

            stack.Push(list[0]);
            parents.Add(list[0], null);

            while (stack.Count != 0)
            {
                Node curr = stack.Pop();

                if (curr.Left == null && curr.Right == null)
                {
                    string encoding = GetPath(curr, parents);
                    mapping.Add(curr.Symbol, encoding);

                    continue;
                }

                if (curr.Left != null)
                {
                    parents.Add(curr.Left, curr);
                    stack.Push(curr.Left);
                }

                if (curr.Right != null)
                {
                    parents.Add(curr.Right, curr);
                    stack.Push(curr.Right);
                }
            }

            return mapping;
        }

        private static List<Node> GetFreqList(string input)
        {
            List<Node> list = new List<Node>();

            if (String.IsNullOrEmpty(input)) return list;

            Dictionary<string, int> dict = new Dictionary<string, int>();

            for (int i = 0; i < input.Length; ++i)
            {
                string curr = input[i].ToString();
                if (!dict.ContainsKey(curr))
                {
                    dict.Add(curr, 1);
                    continue;
                }

                dict[curr]++;
            }

            foreach (KeyValuePair<string, int> kvp in dict)
            {
                list.Add(new Node(kvp.Key, kvp.Value));
            }

            return list;
        }

        private static void PrintList(List<Node> list)
        {
            if (list == null) return;

            for (int i = 0; i < list.Count; ++i)
            {
                Console.WriteLine(list[i].Symbol + " : " + list[i].Freq);
            }

            Console.WriteLine();
        }

        private static void Sort(ref List<Node> list)
        {
            if (list == null) return;

            for (int i = 0; i < list.Count; ++i)
            {
                for (int j = 0; j < list.Count; ++j)
                {
                    if (list[i].Freq < list[j].Freq)
                    {
                        Node temp = list[i];
                        list[i] = list[j];
                        list[j] = temp;
                    }
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //string input = "A_DEAD_DAD_CEDED_A_BAD_BABE_A_BEADED_ABACA_BED";

            Console.WriteLine("Enter a string you would like to compress:");

            string input = Console.ReadLine();

            while (String.IsNullOrEmpty(input))
            {
                Console.WriteLine("Input not valid. Please try again.");

                input = Console.ReadLine();
            }

            Console.WriteLine("\nEntered: " + input);
            Console.WriteLine("--Compressing string--\n");

            string compressed = Compression.Huffman(input);

            Console.WriteLine("Compressed Result:");
            Console.WriteLine(compressed);

            Console.WriteLine("\n--Finished compressing string--");

            Console.ReadKey();
        }
    }
}
