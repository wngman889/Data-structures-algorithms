using System;
using System.Collections.Generic;
using System.Text;

namespace HuffmansTree
{
    public class Node
    {
        public char Letter { get; set; }

        public int Frequency { get; set; }

        public Node LeftNode { get; set; }

        public Node RightNode { get; set; }
    }
}
