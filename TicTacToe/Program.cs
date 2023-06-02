using System;
using System.Collections.Generic;
using System.Xml;

namespace TicTacToe
{
    class Program
    {
        public const int Player1 = 1;
        public const int Player2 = 2;

        public static Int32 Winner(Int32[,] board)
        {
            var c1 = board[0, 0] & board[1, 0] & board[2, 0];
            var c2 = board[0, 1] & board[1, 1] & board[2, 1];
            var c3 = board[0, 2] & board[1, 2] & board[2, 2];
            var l1 = board[0, 0] & board[0, 1] & board[0, 2];
            var l2 = board[1, 0] & board[1, 1] & board[1, 2];
            var l3 = board[2, 0] & board[2, 1] & board[2, 2];
            var d1 = board[0, 0] & board[1, 1] & board[2, 2];
            var d2 = board[0, 2] & board[1, 1] & board[2, 0];

            return
                c1 == 1 ||
                c2 == 1 ||
                c3 == 1 ||
                l1 == 1 ||
                l2 == 1 ||
                l3 == 1 ||
                d1 == 1 ||
                d2 == 1 ? Player1 :
                c1 == 2 ||
                c2 == 2 ||
                c3 == 2 ||
                l1 == 2 ||
                l2 == 2 ||
                l3 == 2 ||
                d1 == 2 ||
                d2 == 2 ? Player2 : 0;
        }

        public static void Main()
        {
            var root = new Node();
            GrowTree(root, Player1, Player1);

            while (true)
            {
                var currentPlayer = Player1;
                var currentNode = root;
                while (currentNode.Children.Count > 0)
                {
                    Console.Clear();
                    PrintBoard(currentNode.Board);

                    if (currentPlayer == Player1)
                    {
                        var move = Console.ReadKey().KeyChar - '1';
                        foreach (var child in currentNode.Children)
                        {
                            Node nextMove = null;
                            for (int i = 0; i < currentNode.Board.GetLength(0) && nextMove is null; i++)
                            {
                                for (int j = 0; j < currentNode.Board.GetLength(1) && nextMove is null; j++)
                                {
                                    if (currentNode.Board[i, j] != child.Board[i, j] && i * 3 + j == move)
                                    {
                                        nextMove = child;
                                    }
                                }
                            }

                            if (nextMove!= null)
                            {
                                currentNode = nextMove;
                                break;
                            }
                        }
                    }
                    else
                    {
                        Node minNode = currentNode.Children[0];
                        for (int c = 1; c < currentNode.Children.Count; c++)
                        {
                            if (minNode.Value > currentNode.Children[c].Value)
                            {
                                minNode = currentNode.Children[c];
                            }
                        }

                        currentNode = minNode;
                    }

                    currentPlayer = currentPlayer == Player1 ? Player2 : Player1;
                }
            }
        }

        public static void GrowTree(Node node, int player, int maximizer)
        {
            var winner = Winner(node.Board);
            if (winner != 0)
            {
                node.Value = winner == maximizer ? 1 : -1;

                return;
            }

            for (int i = 0; i < node.Board.GetLength(0); i++)
            {
                for (int j = 0; j < node.Board.GetLength(1); j++)
                {
                    if (node.Board[i, j] == 0)
                    {
                        var newChild = new Node();

                        Array.Copy(node.Board, newChild.Board, node.Board.Length);
                        newChild.Board[i, j] = player;

                        node.Children.Add(newChild);

                        GrowTree(newChild, player == Player1 ? Player2 : Player1, maximizer);

                        if (node.Value is null ||
                            (player == maximizer && node.Value < newChild.Value) ||
                            (player != maximizer && node.Value > newChild.Value))
                        {
                            node.Value = newChild.Value;
                        }
                    }
                }
            }

            if (node.Value is null)
            {
                node.Value = 0;
            }
        }

        public static void PrintBoard(int[,] board)
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    Console.Write(
                        (board[i, j] == Player1 ? "x" :
                            board[i, j] == Player2 ? "o" :
                            (i * 3 + j + 1).ToString()) + " ");
                }
                Console.WriteLine();
            }
        }
    }

    public class Node
    {
        public List<Node> Children { get; set; } = new List<Node>();
        public int[,] Board { get; set; } = new int[3, 3];
        public int? Value { get; set; }
    }
}
