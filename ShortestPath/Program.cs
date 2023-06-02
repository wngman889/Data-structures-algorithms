using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
namespace ShortestPath
{
    class Program
    {
        static void Main(string[] args)
        {
            var costs = new int[,]
             {
                {-1, 1, 3, -1, 6},
                {1, -1, 2, -1, 8},
                {3, 2, -1, 8, 2},
                {-1, 1, 8, -1, 1},
                {3, 8, 2, 1, -1}
             };

            var best = new int?[costs.GetLength(0), (1 << costs.GetLength(0)) - 1];

            var routeValue = Route(costs, 0, 0, best);
            Console.WriteLine($"Route: {routeValue}");
        }

        private static int Route(int[,] costs, int from, int visited, int?[,] best)
        {
            if (best[from, visited]!= null)
            {
                return best[from, visited].Value;
            }

            visited |= 1 << from;

            if (visited == ((1 << costs.GetLength(0)) - 1))
            {
                return costs[from, 0];
            }

            for (var to = 0; to < costs.GetLength(0); to++)
            {
                if ((visited & (1 << to)) == 0 && costs[from, to] != -1)
                {
                    var subSolution = Route(costs, to, visited, best);
                    if (subSolution == -1)
                    {
                        continue;
                    }

                    var value = costs[from, to] + subSolution;
                    if (best[from, visited] is null || value < best[from, visited])
                    {
                        best[from, visited] = value;
                    }
                }
            }

            return best[from, visited] ?? -1;
        }
    }
}
