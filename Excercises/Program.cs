using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Excercise_4
{
    class Program
    {
        const string GENES = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ 1234567890, .-;:_!\"#%&/()=?@${[]}";

        public static List<char[]> RandomPopulation(Random rand, int count)
        {
            var result = new List<char[]>(count);
            for (int i = 0; i < count; i++)
            {
                var newSolution = new char[Env.SolutionSize];

                for (int j = 0; j < newSolution.Length; j++)
                {
                    newSolution[j] = GENES[rand.Next(GENES.Length)];
                }

                result.Add(newSolution);
            }

            return result;
        }

        public static char[] Mutate(Random rand, char[] solution, int mutationPercentage)
        {
            var newSolution = new char[solution.Length];

            for (int i = 0; i < newSolution.Length; i++)
            {
                newSolution[i] = rand.Next(100) < mutationPercentage
                                ? GENES[rand.Next(GENES.Length)]
                                : solution[i];
            }

            return newSolution;
        }

        public static char[] CrossOver(Random rand, char[] solution1, char[] solution2)
        {
            var newSolution = new char[Env.SolutionSize];

            var crosses = rand.Next(newSolution.Length);
            var firstParent = true;

            for (int i = 0; i < newSolution.Length; i++)
            {
                newSolution[i] = firstParent ? solution1[i] : solution2[i];

                if (crosses-- <= 0 && firstParent)
                {
                    firstParent = !firstParent;
                }
            }

            return newSolution;
        }

        public static char[] GeneticListSearch(Random rand,
            DateTime deadLine,
            int parentsCount,
            int populationSize)
        {
            char[] bestSolution = null;

            var population = RandomPopulation(rand, populationSize);

            while (DateTime.Now < deadLine)
            {
                var selectedPopulation = population
                    .OrderByDescending(s => Env.Evaluate(s))
                    .Take(parentsCount)
                    .ToList();

                for (int i = parentsCount; i < populationSize; i++)
                {
                    var parent1 = selectedPopulation[rand.Next(selectedPopulation.Count)];
                    var parent2 = selectedPopulation[rand.Next(selectedPopulation.Count)];

                    var child = CrossOver(rand, parent1, parent2);
                    child = Mutate(rand, child, 30);

                    selectedPopulation.Add(child);
                }

                population = selectedPopulation;

                Console.WriteLine();
                Console.WriteLine(population.Select(s => new string(s)).Aggregate((f, s) => $"{f}{Environment.NewLine}{s}"));

                bestSolution = population
                    .OrderByDescending(s => Env.Evaluate(s))
                    .First();

                if (Env.Evaluate(bestSolution) == Env.SolutionSize)
                {
                    break;
                }
            }

            return bestSolution;
        }

        #region BF

        public static bool NextVariation(int n, int[] vals)
        {
            var k = vals.Length;

            vals[k - 1]++;
            for (int i = k - 1; i > 0; i--)
                if (vals[i] >= n)
                {
                    vals[i] = 0;
                    vals[i - 1]++;
                }

            return vals[0] < n;
        }

        #endregion

        static void Main()
        {
            #region BF

            //var variations = new int[Env.SolutionSize];
            //var solution = new char[Env.SolutionSize];
            //do
            //{
            //    for (int c = 0; c < solution.Length; c++)
            //        solution[c] = GENES[variations[c]];

            //    if (Env.Evaluate(solution) == Env.SolutionSize)
            //        break;

            //    Console.WriteLine(new string(solution));
            //}
            //while (NextVariation(GENES.Length, variations));

            #endregion

            var best = GeneticListSearch(new Random(), DateTime.Now.AddMinutes(10), 2, 4);

            Console.WriteLine();
            Console.WriteLine($"Solution found: { new string(best) }");
            Console.ReadLine();
        }
    }
}
