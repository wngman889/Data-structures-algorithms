using System;

namespace Knapsack
{
    class Program
    {
        public static double KnapsackRec(double[] weights,
           double[] values,
           int item,
           double maxWeight,
           out string path)
        {
            path = "";
            if (item == 0)
            {
                return 0;
            }

            string pathExcl;
            var excl = KnapsackRec(weights, values, item - 1, maxWeight, out pathExcl);
            if (maxWeight >= weights[item - 1])
            {
                string pathIncl;
                var incl = values[item - 1] +
                           KnapsackRec(weights, values, item - 1, maxWeight - weights[item - 1], out pathIncl);

                if (incl > excl)
                {
                    path = pathIncl + " " + item;
                    return incl;
                }
            }

            return 0;
        }
    }
}
