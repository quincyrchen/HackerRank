using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrogInMaze //In Progress
{
    public class Solution
    {
        //This is actually a pretty annoying problem. We basically want the probability the frog will "make it" to the exit given a certain maze, but there are a ton of pitfalls. How do we do this?
        //The logic can get pretty annoying and un-intuitive, so let's first consider two simple cases to help us think:
        /* Case 1: X = death, F = frog, S = success, O = path
         *    X
         *  X F S
         *    X
         * In this case, it's pretty clear that Froggy has only 4 possible options. 3 which lead to death, 1 which leads to success. Clear answer of 25% success.
         */
        /* Case 2: X = death, F = frog, S = success, O = path
         *   X X
         * X O F S
         *   X X
         * In this case, you might think "Oh crap, what if Froggy just jumps back and forth in the middle? How in blazes do we capture that probability? Remember: The frog can indeed backtrack.
         * Well, in actuality, if we do this the most intuitive way where the frog has a 50% chance of death, 25% chance of success, and a 25% chance of hopping into a position of 75% chance 
         * of death, and 25% chance of hopping into a position of... etc. back to where we started... we will have our time complexity explode! There's actually a mathematical trick to this.
         * 
         * We will be using Gaussian Elimination to solve this problem more efficiently than brute-forcing it.
         * 
         * So let's give that a shot.
         * 
         * Also notice that this setup is wildly inconvenient, and we'll have to tangle with part of it ourselves, instead of just GETTING variables passed to us. How irritating.
         */
        static void Main(String[] args)
        {
            //read n, m, k, which are the number of rows, number of columns, and number of tunnels respectively.
            string[] tokens_n = Console.ReadLine().Split(' ');
            int n = Convert.ToInt32(tokens_n[0]); //# of rows
            int m = Convert.ToInt32(tokens_n[1]); //# of columns (items in a row)
            int k = Convert.ToInt32(tokens_n[2]); //# of tunnels
            //for reach row, presumably we will read each row as an input, and put it in something actually usable
            char[,] froggyMap = new char[n,m];
            int[] currentPosition = new int[2];
            for (int a0 = 0; a0 < n; a0++)
            {
                string row = Console.ReadLine();
                for (var i = 0; i < row.Length; i++)
                {
                    froggyMap[a0, i] = row[i];
                    if (row[i] == 'A') {
                        currentPosition[0] = a0;
                        currentPosition[1] = i;
                    }
                }
            }
            //for each pair of tunnels, presumably we will read each tunnel pairing into something actually usable
            int[,] tunnelSet1 = new int[k, 2];
            int[,] tunnelSet2 = new int[k, 2];
            for (int a0 = 0; a0 < k; a0++)
            {
                string[] tokens_i1 = Console.ReadLine().Split(' ');
                int i1 = Convert.ToInt32(tokens_i1[0]);
                int j1 = Convert.ToInt32(tokens_i1[1]);
                int i2 = Convert.ToInt32(tokens_i1[2]);
                int j2 = Convert.ToInt32(tokens_i1[3]);
                // Write Your Code Here
                //The rows are going from top to bottom 1-index in the prompt, and for us we are going from bottom to top 0-index, therefore, ours must be n-row.
                //The columns are going from left to right 1-index in the prompt, and for us we are going from left to right 0-index, therefore, ours must be column-1.
                tunnelSet1[a0, 0] = n - i1;
                tunnelSet1[a0, 1] = j1 - 1;
                tunnelSet2[a0, 0] = n - i2;
                tunnelSet2[a0, 1] = j2 - 1;
            }
            //Now we have froggyMap as a map of the land, as well as tunnelSet1 and tunnelSet2 as corresponding pairs of coordinate points for tunnels in the map!
            //Here's where we actually do the work, and will be using our data from above to calculate and return the probability (as a function of Main, I suppose?)
            // Write Your Code Here
            
        }

        //DFS Function to obtain a hashset of all the reachable cells
        public HashSet<int[]> DFS(char[,] froggyMap, int[] currentPosition, int[,] tunnelSet1, int[,] tunnelSet2) {



            return new HashSet<int[]>();
        }

        //gaussianReduction Function to ultimately find the probability of frog escaping
        public int buildMatrix(HashSet<int[]> reachableCells, char[,] froggyMap, int[] currentPosition, int[,] tunnelSet1, int[,] tunnelSet2) {



            return 0;
        }

        public static double[] reducedRowEchelon(double[] constants, double[][] coefficients)
        {
            if (constants.Length != coefficients[0].Length)
            {
                throw new Exception("You must have the same number of constants as coefficients for this function.");
            }

            for (var i = 0; i < constants.Length; i++)
            {
                for (var j = 0; j < constants.Length; j++)
                {
                    if (j != i)
                    {
                        var coefficient = coefficients[j][i] / coefficients[i][i];
                        constants[j] -= constants[i] * coefficient;
                        for (var k = 0; k < constants.Length; k++)
                        {
                            coefficients[j][k] -= coefficients[i][k] * coefficient;
                        }
                    }
                }
            }

            for (var l = 0; l < constants.Length; l++)
            {
                constants[l] /= coefficients[l][l];
            }

            return constants;
        }
    }
}
