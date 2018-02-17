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
            char[][] froggyMap = new char[m][];
            for (var i = 0; i < m; i++) {
                froggyMap[i] = new char[n];
            }
            int[] currentPosition = new int[2];
            for (int a0 = 0; a0 < n; a0++)
            {
                string row = Console.ReadLine();
                for (var i = 0; i < row.Length; i++)
                {
                    froggyMap[i][n-1-a0] = row[i];
                    if (row[i] == 'A') {
                        currentPosition[0] = i;
                        currentPosition[1] = n-1-a0;
                    }
                }
            }
            //for each pair of tunnels, presumably we will read each tunnel pairing into something actually usable
            int[][] tunnelSet1 = new int[k][];
            int[][] tunnelSet2 = new int[k][];
            for (var i = 0; i < k; i++) {
                tunnelSet1[i] = new int[2];
                tunnelSet2[i] = new int[2];
            }

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
                tunnelSet1[a0][1] = n - i1;
                tunnelSet1[a0][0] = j1 - 1;
                tunnelSet2[a0][1] = n - i2;
                tunnelSet2[a0][0] = j2 - 1;
            }
            //Now we have froggyMap as a map of the land, as well as tunnelSet1 and tunnelSet2 as corresponding pairs of coordinate points for tunnels in the map!
            //Here's where we actually do the work, and will be using our data from above to calculate and return the probability (as a function of Main, I suppose?)
            // Write Your Code Here
            var testReachableCells = getReachableCells(new HashSet<int[]>(), froggyMap, currentPosition, tunnelSet1, tunnelSet2);

            Console.WriteLine(buildMatrixAndSolve(getReachableCells(new HashSet<int[]>(), froggyMap, currentPosition, tunnelSet1, tunnelSet2), froggyMap, currentPosition, tunnelSet1, tunnelSet2));
        }

        //DFS Function to obtain a hashset of all the reachable cells
        public static HashSet<int[]> getReachableCells(HashSet<int[]> visitedCells, char[][] froggyMap, int[] currentPosition, int[][] tunnelSet1, int[][] tunnelSet2) {
            //This function will be recursive, so for each cell we call it on, we will add it to the reachable hashSet, then merge it at the end, so we can capture them all this way.
            //We use a HashSet to automatically remove all duplicates, so we won't double count anything.
            if (visitedCells.Contains<int[]>(currentPosition, new cellComparer())) {
                return visitedCells;
            }
            visitedCells.Add(currentPosition);
            //Check for walls! We won't want to parse through cells we can't actually walk into
            //+0/+1
            if (currentPosition[1] + 1 < froggyMap[0].Length && froggyMap[currentPosition[0]][currentPosition[1] + 1] != '#')
            {
                //Check for tunnels! Remember - escapability will depend on the tunnel you exit out of, not the entrance you hop into!
                for (var i = 0; i < tunnelSet1.Length; i++)
                {
                    //if we fall into a hole...
                    if (tunnelSet1[i][0] == currentPosition[0] && tunnelSet1[i][1] == currentPosition[1] + 1)
                    {
                        //parse the exit! & union!
                        visitedCells.UnionWith(getReachableCells(visitedCells, froggyMap, new int[] { tunnelSet2[i][0], tunnelSet2[i][1] }, tunnelSet1, tunnelSet2));
                    }//if we fall into a hole from the other set...
                    else if (tunnelSet2[i][0] == currentPosition[0] && tunnelSet2[i][1] == currentPosition[1] + 1) 
                    {
                        //parse the (other) exit! & union!
                        visitedCells.UnionWith(getReachableCells(visitedCells, froggyMap, new int[] { tunnelSet1[i][0], tunnelSet1[i][1] }, tunnelSet1, tunnelSet2));
                    }//if we reach the end of the for-loop and we don't fall into either hole
                    else if (i == tunnelSet1.Length - 1) 
                    {
                        //then parse the hopped position!
                        visitedCells.UnionWith(getReachableCells(visitedCells, froggyMap, new int[] { currentPosition[0], currentPosition[1] + 1 }, tunnelSet1, tunnelSet2));
                    }
                }
                //But what if there ARE no tunnels? + what if there is an exit/bomb ON the tunnel?
                //then parse the hopped position!
                if (tunnelSet1.Length == 0)
                {
                    visitedCells.UnionWith(getReachableCells(visitedCells, froggyMap, new int[] { currentPosition[0], currentPosition[1] + 1 }, tunnelSet1, tunnelSet2));
                }
            }
            //+1/+0
            if (currentPosition[0] + 1 < froggyMap.Length && froggyMap[currentPosition[0] + 1][currentPosition[1]] != '#')
            {
                //Check for tunnels! Remember - escapability will depend on the tunnel you exit out of, not the entrance you hop into!
                for (var i = 0; i < tunnelSet1.Length; i++)
                {
                    //if we fall into a hole...
                    if (tunnelSet1[i][0] == currentPosition[0] + 1 && tunnelSet1[i][1] == currentPosition[1])
                    {
                        //parse the exit! & union!
                        visitedCells.UnionWith(getReachableCells(visitedCells, froggyMap, new int[] { tunnelSet2[i][0], tunnelSet2[i][1] }, tunnelSet1, tunnelSet2));
                    }//if we fall into a hole from the other set...
                    else if (tunnelSet2[i][0] == currentPosition[0] + 1 && tunnelSet2[i][1] == currentPosition[1]) 
                    {
                        //parse the (other) exit! & union!
                        visitedCells.UnionWith(getReachableCells(visitedCells, froggyMap, new int[] { tunnelSet1[i][0], tunnelSet1[i][1] }, tunnelSet1, tunnelSet2));
                    }//if we reach the end of the for-loop and we don't fall into either hole
                    else if (i == tunnelSet1.Length - 1) 
                    {
                        //then parse the hopped position!
                        visitedCells.UnionWith(getReachableCells(visitedCells, froggyMap, new int[] { currentPosition[0] + 1, currentPosition[1] }, tunnelSet1, tunnelSet2));
                    }
                }
                //But what if there ARE no tunnels? + what if there is an exit/bomb ON the tunnel?
                //then parse the hopped position!
                if (tunnelSet1.Length == 0)
                {
                    visitedCells.UnionWith(getReachableCells(visitedCells, froggyMap, new int[] { currentPosition[0] + 1, currentPosition[1] }, tunnelSet1, tunnelSet2));
                }
            }
            //-1/+0
            if (currentPosition[0] - 1 >= 0 && froggyMap[currentPosition[0] - 1][currentPosition[1]] != '#')
            {
                //Check for tunnels! Remember - escapability will depend on the tunnel you exit out of, not the entrance you hop into!
                for (var i = 0; i < tunnelSet1.Length; i++)
                {
                    //if we fall into a hole...
                    if (tunnelSet1[i][0] == currentPosition[0] - 1 && tunnelSet1[i][1] == currentPosition[1])
                    {
                        //parse the exit! & union!
                        visitedCells.UnionWith(getReachableCells(visitedCells, froggyMap, new int[] { tunnelSet2[i][0], tunnelSet2[i][1] }, tunnelSet1, tunnelSet2));
                    }//if we fall into a hole from the other set...
                    else if (tunnelSet2[i][0] == currentPosition[0] - 1 && tunnelSet2[i][1] == currentPosition[1]) 
                    {
                        //parse the (other) exit! & union!
                        visitedCells.UnionWith(getReachableCells(visitedCells, froggyMap, new int[] { tunnelSet1[i][0], tunnelSet1[i][1] }, tunnelSet1, tunnelSet2));
                    }//if we reach the end of the for-loop and we don't fall into either hole
                    else if (i == tunnelSet1.Length - 1) 
                    {
                        //then parse the hopped position!
                        visitedCells.UnionWith(getReachableCells(visitedCells, froggyMap, new int[] { currentPosition[0] - 1, currentPosition[1] }, tunnelSet1, tunnelSet2));
                    }
                }
                //But what if there ARE no tunnels? + what if there is an exit/bomb ON the tunnel?
                //then parse the hopped position!
                if (tunnelSet1.Length == 0)
                {
                    visitedCells.UnionWith(getReachableCells(visitedCells, froggyMap, new int[] { currentPosition[0] - 1, currentPosition[1] }, tunnelSet1, tunnelSet2));
                }
            }
            //+0/-1
            if (currentPosition[1] - 1 >= 0 && froggyMap[currentPosition[0]][currentPosition[1] - 1] != '#')
            {
                //Check for tunnels! Remember - escapability will depend on the tunnel you exit out of, not the entrance you hop into!
                for (var i = 0; i < tunnelSet1.Length; i++)
                {
                    //if we fall into a hole...
                    if (tunnelSet1[i][0] == currentPosition[0] && tunnelSet1[i][1] == currentPosition[1] - 1)
                    {
                        //parse the exit! & union!
                        visitedCells.UnionWith(getReachableCells(visitedCells, froggyMap, new int[] { tunnelSet2[i][0], tunnelSet2[i][1] }, tunnelSet1, tunnelSet2));
                    }//if we fall into a hole from the other set...
                    else if (tunnelSet2[i][0] == currentPosition[0] && tunnelSet2[i][1] == currentPosition[1] - 1) 
                    {
                        //parse the (other) exit! & union!
                        visitedCells.UnionWith(getReachableCells(visitedCells, froggyMap, new int[] { tunnelSet1[i][0], tunnelSet1[i][1] }, tunnelSet1, tunnelSet2));
                    }//if we reach the end of the for-loop and we don't fall into either hole
                    else if (i == tunnelSet1.Length - 1) 
                    {
                        //then parse the hopped position!
                        visitedCells.UnionWith(getReachableCells(visitedCells, froggyMap, new int[] { currentPosition[0], currentPosition[1] - 1 }, tunnelSet1, tunnelSet2));
                    }
                }
                //But what if there ARE no tunnels?
                //then parse the hopped position!
                if(tunnelSet1.Length == 0)
                {
                    visitedCells.UnionWith(getReachableCells(visitedCells, froggyMap, new int[] { currentPosition[0], currentPosition[1] - 1 }, tunnelSet1, tunnelSet2));
                }
            }
            //return all reachable cells after the stack is parsed
            return visitedCells;
        }

        //gaussianReduction Function to ultimately find the probability of frog escaping
        public static double buildMatrixAndSolve(HashSet<int[]> reachableCells, char[][] froggyMap, int[] currentPosition, int[][] tunnelSet1, int[][] tunnelSet2) {
            //First, we put our reachable cells into an array, and convert it to a dictionary with enumerated values, so we can access their coefficient (key) at a constant complexity based on the position (row,column)!
            var coefficientArray= reachableCells.ToArray();
            var coefficientDictionary = new Dictionary<int[], int>(new cellComparer());
            for (var i = 0; i < coefficientArray.Length; i++) {
                coefficientDictionary.Add(coefficientArray[i], i);
            }
            //then we can build our matrix very quickly, without wasting time!
            //build constant array
            double[] constants = new double[coefficientArray.Length];
            //build coefficients array
            double[][] coefficients = new double[coefficientArray.Length][];
            //initialize all the arrays inside the coefficient array (with 0's first)
            for (var j = 0; j < coefficientArray.Length; j++) {
                coefficients[j] = new double[coefficientArray.Length];
            }
            //time to parse through the coefficientArray and put in the probabilities!
            for (var k = 0; k < coefficientArray.Length; k++) {
                //first add the cell in question to the matrix
                coefficients[coefficientDictionary[coefficientArray[k]]][coefficientDictionary[coefficientArray[k]]] = 1;
                //check the cell itself
                if (froggyMap[coefficientArray[k][0]][coefficientArray[k][1]] == '%')
                {   //probability is incremented by 1
                    constants[coefficientDictionary[coefficientArray[k]]] += 1;
                }
                else if (froggyMap[coefficientArray[k][0]][coefficientArray[k][1]] == '*')
                {
                    //do nothing; probability is zero
                }
                else
                {
                    //then check all four potential paths so we know our total possible paths
                    double maxPaths = 0;
                    //+1/+0
                    if (coefficientArray[k][0] + 1 < froggyMap.Length && froggyMap[coefficientArray[k][0] + 1][coefficientArray[k][1]] != '#')
                    {
                        maxPaths++;
                    }
                    //+0/+1
                    if (coefficientArray[k][1] + 1 < froggyMap[0].Length && froggyMap[coefficientArray[k][0]][coefficientArray[k][1] + 1] != '#')
                    {
                        maxPaths++;
                    }
                    //-1/+0
                    if (coefficientArray[k][0] - 1 >= 0 && froggyMap[coefficientArray[k][0] - 1][coefficientArray[k][1]] != '#')
                    {
                        maxPaths++;
                    }
                    //+0/-1
                    if (coefficientArray[k][1] - 1 >= 0 && froggyMap[coefficientArray[k][0]][coefficientArray[k][1] - 1] != '#')
                    {
                        maxPaths++;
                    }
                    //Now that we have maxPaths, we can check for walls (sigh, yes, again), tunnels, and generate the respective coefficient in the matrix!
                    //Check for walls! We won't want to parse through cells we can't actually walk into
                    //+1/+0
                    if (coefficientArray[k][0] + 1 < froggyMap.Length && froggyMap[coefficientArray[k][0] + 1][coefficientArray[k][1]] != '#')
                    {
                        //Check for tunnels! Remember - escapability will depend on the tunnel you exit out of, not the entrance you hop into!
                        //First, if we insta-win or insta-die
                        if (froggyMap[coefficientArray[k][0] + 1][coefficientArray[k][1]] == '%')
                        {   //probability is incremented by 1 / max paths
                            constants[coefficientDictionary[coefficientArray[k]]] += 1 / maxPaths;
                        }
                        else if (froggyMap[coefficientArray[k][0] + 1][coefficientArray[k][1]] == '*')
                        {
                            //probability stays zero for a bomb, do nothing
                        }
                        //But what if there ARE no tunnels?
                        else if (tunnelSet1.Length == 0)
                        {
                            //then parse the hopped position!
                            coefficients[coefficientDictionary[coefficientArray[k]]][coefficientDictionary[new int[] { coefficientArray[k][0] + 1, coefficientArray[k][1] }]] = -1 / maxPaths;
                        }else
                        {//if we fall into a hole...
                            bool foundTunnel = false;
                            for (var l = 0; l < tunnelSet1.Length; l++)
                                {
                                if (tunnelSet1[l][0] == coefficientArray[k][0] + 1 && tunnelSet1[l][1] == coefficientArray[k][1])
                                {
                                    foundTunnel = true;
                                    if (froggyMap[tunnelSet2[l][0]][tunnelSet2[l][1]] == '%')
                                    {   //probability is incremented by 1 / max paths
                                        constants[coefficientDictionary[coefficientArray[k]]] += 1 / maxPaths;
                                    }
                                    else if (froggyMap[tunnelSet2[l][0]][tunnelSet2[l][1]] == '*')
                                    {
                                        //probability stays zero for a bomb, do nothing
                                    }
                                    else
                                    {   //probability is 1/maxPaths for the coefficient (so 1/maxPaths * cell probability) for a free space. It is negative due to being on the right side of the matrix equation
                                        coefficients[coefficientDictionary[coefficientArray[k]]][coefficientDictionary[new int[] { tunnelSet2[l][0], tunnelSet2[l][1] }]] = -1 / maxPaths;
                                    }
                                }//if we fall into a hole from the other set...
                                else if (tunnelSet2[l][0] == coefficientArray[k][0] + 1 && tunnelSet2[l][1] == coefficientArray[k][1])
                                {
                                    foundTunnel = true;
                                    if (froggyMap[tunnelSet1[l][0]][tunnelSet1[l][1]] == '%')
                                    {   //probability is incremented by 1 / max paths
                                        constants[coefficientDictionary[coefficientArray[k]]] += 1 / maxPaths;
                                    }
                                    else if (froggyMap[tunnelSet1[l][0]][tunnelSet1[l][1]] == '*')
                                    {
                                        //probability stays zero for a bomb, do nothing
                                    }
                                    else
                                    {   //probability is 1/maxPaths for the coefficient (so 1/maxPaths * cell probability) for a free space. It is negative due to being on the right side of the matrix equation
                                        coefficients[coefficientDictionary[coefficientArray[k]]][coefficientDictionary[new int[] { tunnelSet1[l][0], tunnelSet1[l][1] }]] = -1 / maxPaths;
                                    }
                                }//if we reach the end of the for-loop and we have found a tunnel
                                else if (l == tunnelSet1.Length - 1 && !foundTunnel)
                                {
                                    //then parse the hopped position!
                                    //probability is 1/maxPaths for the coefficient (so 1/maxPaths * cell probability) for a free space. It is negative due to being on the right side of the matrix equation
                                    coefficients[coefficientDictionary[coefficientArray[k]]][coefficientDictionary[new int[] { coefficientArray[k][0] + 1, coefficientArray[k][1] }]] = -1 / maxPaths;
                                }
                            }
                        }
                    }
                    //+0/+1
                    if (coefficientArray[k][1] + 1 < froggyMap[0].Length && froggyMap[coefficientArray[k][0]][coefficientArray[k][1] + 1] != '#')
                    {
                        //If we insta-die or insta-win
                        if (froggyMap[coefficientArray[k][0]][coefficientArray[k][1] + 1] == '%')
                        {   //probability is incremented by 1 / max paths
                            constants[coefficientDictionary[coefficientArray[k]]] += 1 / maxPaths;
                        }
                        else if (froggyMap[coefficientArray[k][0]][coefficientArray[k][1] + 1] == '*')
                        {
                            //probability stays zero for a bomb, do nothing
                        }//But what if there ARE no tunnels?
                        else if (tunnelSet1.Length == 0)
                        {
                            //then parse the hopped position!
                            coefficients[coefficientDictionary[coefficientArray[k]]][coefficientDictionary[new int[] { coefficientArray[k][0], coefficientArray[k][1] + 1 }]] = -1 / maxPaths;
                        }
                        else
                        {
                            bool foundTunnel = false;
                            //Check for tunnels! Remember - escapability will depend on the tunnel you exit out of, not the entrance you hop into!
                            for (var l = 0; l < tunnelSet1.Length; l++)
                            {
                                //if we fall into a hole...
                                if (tunnelSet1[l][0] == coefficientArray[k][0] && tunnelSet1[l][1] == coefficientArray[k][1] + 1)
                                {
                                    foundTunnel = true;
                                    if (froggyMap[tunnelSet2[l][0]][tunnelSet2[l][1]] == '%')
                                    {   //probability is incremented by 1 / max paths
                                        constants[coefficientDictionary[coefficientArray[k]]] += 1 / maxPaths;
                                    }
                                    else if (froggyMap[tunnelSet2[l][0]][tunnelSet2[l][1]] == '*')
                                    {
                                        //probability stays zero for a bomb, do nothing
                                    }
                                    else
                                    {   //probability is 1/maxPaths for the coefficient (so 1/maxPaths * cell probability) for a free space. It is negative due to being on the right side of the matrix equation
                                        coefficients[coefficientDictionary[coefficientArray[k]]][coefficientDictionary[new int[] { tunnelSet2[l][0], tunnelSet2[l][1] }]] = -1 / maxPaths;
                                    }
                                }//if we fall into a hole from the other set...
                                else if (tunnelSet2[l][0] == coefficientArray[k][0] && tunnelSet2[l][1] == coefficientArray[k][1] + 1)
                                {
                                    foundTunnel = true;
                                    if (froggyMap[tunnelSet1[l][0]][tunnelSet1[l][1]] == '%')
                                    {   //probability is incremented by 1 / max paths
                                        constants[coefficientDictionary[coefficientArray[k]]] += 1 / maxPaths;
                                    }
                                    else if (froggyMap[tunnelSet1[l][0]][tunnelSet1[l][1]] == '*')
                                    {
                                        //probability stays zero for a bomb, do nothing
                                    }
                                    else
                                    {   //probability is 1/maxPaths for the coefficient (so 1/maxPaths * cell probability) for a free space. It is negative due to being on the right side of the matrix equation
                                        coefficients[coefficientDictionary[coefficientArray[k]]][coefficientDictionary[new int[] { tunnelSet1[l][0], tunnelSet1[l][1] }]] = -1 / maxPaths;
                                    }
                                }//if we reach the end of the for-loop and we havent found a tunnel
                                else if (l == tunnelSet1.Length - 1 && !foundTunnel)
                                {
                                    //then parse the hopped position!
                                    //probability is 1/maxPaths for the coefficient (so 1/maxPaths * cell probability) for a free space. It is negative due to being on the right side of the matrix equation
                                    coefficients[coefficientDictionary[coefficientArray[k]]][coefficientDictionary[new int[] { coefficientArray[k][0], coefficientArray[k][1] + 1 }]] = -1 / maxPaths;
                                }
                            }
                        }
                    }
                    //-1/+0
                    if (coefficientArray[k][0] - 1 >= 0 && froggyMap[coefficientArray[k][0] - 1][coefficientArray[k][1]] != '#')
                    {
                        //if we insta-win or insta-die
                        if (froggyMap[coefficientArray[k][0] - 1][coefficientArray[k][1]] == '%')
                        {   //probability is incremented by 1 / max paths
                            constants[coefficientDictionary[coefficientArray[k]]] += 1 / maxPaths;
                        }
                        else if (froggyMap[coefficientArray[k][0] - 1][coefficientArray[k][1]] == '*')
                        {
                            //probability stays zero for a bomb, do nothing
                        }//But what if there ARE no tunnels?
                        else if (tunnelSet1.Length == 0)
                        {
                            //then parse the hopped position!
                            coefficients[coefficientDictionary[coefficientArray[k]]][coefficientDictionary[new int[] { coefficientArray[k][0] - 1, coefficientArray[k][1] }]] = -1 / maxPaths;
                        }
                        else
                        {
                            bool foundtunnel = false;
                            //Check for tunnels! Remember - escapability will depend on the tunnel you exit out of, not the entrance you hop into!
                            for (var l = 0; l < tunnelSet1.Length; l++)
                            {
                                //if we fall into a hole...
                                if (tunnelSet1[l][0] == coefficientArray[k][0] - 1 && tunnelSet1[l][1] == coefficientArray[k][1])
                                {
                                    foundtunnel = true;
                                    if (froggyMap[tunnelSet2[l][0]][tunnelSet2[l][1]] == '%')
                                    {   //probability is incremented by 1 / max paths
                                        constants[coefficientDictionary[coefficientArray[k]]] += 1 / maxPaths;
                                    }
                                    else if (froggyMap[tunnelSet2[l][0]][tunnelSet2[l][1]] == '*')
                                    {
                                        //probability stays zero for a bomb, do nothing
                                    }
                                    else
                                    {   //probability is 1/maxPaths for the coefficient (so 1/maxPaths * cell probability) for a free space. It is negative due to being on the right side of the matrix equation
                                        coefficients[coefficientDictionary[coefficientArray[k]]][coefficientDictionary[new int[] { tunnelSet2[l][0], tunnelSet2[l][1] }]] = -1 / maxPaths;
                                    }
                                }//if we fall into a hole from the other set...
                                else if (tunnelSet2[l][0] == coefficientArray[k][0] - 1 && tunnelSet2[l][1] == coefficientArray[k][1])
                                {
                                    foundtunnel = true;
                                    if (froggyMap[tunnelSet1[l][0]][tunnelSet1[l][1]] == '%')
                                    {   //probability is incremented by 1 / max paths
                                        constants[coefficientDictionary[coefficientArray[k]]] += 1 / maxPaths;
                                    }
                                    else if (froggyMap[tunnelSet1[l][0]][tunnelSet1[l][1]] == '*')
                                    {
                                        //probability stays zero for a bomb, do nothing
                                    }
                                    else
                                    {   //probability is 1/maxPaths for the coefficient (so 1/maxPaths * cell probability) for a free space. It is negative due to being on the right side of the matrix equation
                                        coefficients[coefficientDictionary[coefficientArray[k]]][coefficientDictionary[new int[] { tunnelSet1[l][0], tunnelSet1[l][1] }]] = -1 / maxPaths;
                                    }
                                }//if we reach the end of the for-loop and we don't fall into either hole
                                else if (l == tunnelSet1.Length - 1 && !foundtunnel)
                                {
                                    //then parse the hopped position!
                                    //probability is 1/maxPaths for the coefficient (so 1/maxPaths * cell probability) for a free space. It is negative due to being on the right side of the matrix equation
                                    coefficients[coefficientDictionary[coefficientArray[k]]][coefficientDictionary[new int[] { coefficientArray[k][0] - 1, coefficientArray[k][1] }]] = -1 / maxPaths;
                                }
                            }
                        }                        
                    }
                    //+0/-1
                    if (coefficientArray[k][1] - 1 >= 0 && froggyMap[coefficientArray[k][0]][coefficientArray[k][1] - 1] != '#')
                    {
                        //if we insta-win or insta-die
                        if (froggyMap[coefficientArray[k][0]][coefficientArray[k][1] - 1] == '%')
                        {   //probability is incremented by 1 / max paths
                            constants[coefficientDictionary[coefficientArray[k]]] += 1 / maxPaths;
                        }
                        else if (froggyMap[coefficientArray[k][0]][coefficientArray[k][1] - 1] == '*')
                        {
                            //probability stays zero for a bomb, do nothing
                        }//But what if there ARE no tunnels?
                        else if (tunnelSet1.Length == 0)
                        {
                            //then parse the hopped position!
                            coefficients[coefficientDictionary[coefficientArray[k]]][coefficientDictionary[new int[] { coefficientArray[k][0], coefficientArray[k][1] - 1 }]] = -1 / maxPaths;
                        }
                        else
                        {
                            bool foundTunnel = false;
                            //Check for tunnels! Remember - escapability will depend on the tunnel you exit out of, not the entrance you hop into!
                            for (var l = 0; l < tunnelSet1.Length; l++)
                            {
                                //if we fall into a hole...
                                if (tunnelSet1[l][0] == coefficientArray[k][0] && tunnelSet1[l][1] == coefficientArray[k][1] - 1)
                                {
                                    foundTunnel = true;
                                    if (froggyMap[tunnelSet2[l][0]][tunnelSet2[l][1]] == '%')
                                    {   //probability is incremented by 1 / max paths
                                        constants[coefficientDictionary[coefficientArray[k]]] += 1 / maxPaths;
                                    }
                                    else if (froggyMap[tunnelSet2[l][0]][tunnelSet2[l][1]] == '*')
                                    {
                                        //probability stays zero for a bomb, do nothing
                                    }
                                    else
                                    {   //probability is 1/maxPaths for the coefficient (so 1/maxPaths * cell probability) for a free space. It is negative due to being on the right side of the matrix equation
                                        coefficients[coefficientDictionary[coefficientArray[k]]][coefficientDictionary[new int[] { tunnelSet2[l][0], tunnelSet2[l][1] }]] = -1 / maxPaths;
                                    }
                                }//if we fall into a hole from the other set...
                                else if (tunnelSet2[l][0] == coefficientArray[k][0] && tunnelSet2[l][1] == coefficientArray[k][1] - 1)
                                {
                                    foundTunnel = true;
                                    if (froggyMap[tunnelSet1[l][0]][tunnelSet1[l][1]] == '%')
                                    {   //probability is incremented by 1 / max paths
                                        constants[coefficientDictionary[coefficientArray[k]]] += 1 / maxPaths;
                                    }
                                    else if (froggyMap[tunnelSet1[l][0]][tunnelSet1[l][1]] == '*')
                                    {
                                        //probability stays zero for a bomb, do nothing
                                    }
                                    else
                                    {   //probability is 1/maxPaths for the coefficient (so 1/maxPaths * cell probability) for a free space. It is negative due to being on the right side of the matrix equation
                                        coefficients[coefficientDictionary[coefficientArray[k]]][coefficientDictionary[new int[] { tunnelSet1[l][0], tunnelSet1[l][1] }]] = -1 / maxPaths;
                                    }
                                }//if we reach the end of the for-loop and we don't fall into either hole
                                else if (l == tunnelSet1.Length - 1 && !foundTunnel)
                                {
                                    //then parse the hopped position!
                                    //probability is 1/maxPaths for the coefficient (so 1/maxPaths * cell probability) for a free space. It is negative due to being on the right side of the matrix equation
                                    coefficients[coefficientDictionary[coefficientArray[k]]][coefficientDictionary[new int[] { coefficientArray[k][0], coefficientArray[k][1] - 1 }]] = -1 / maxPaths;
                                }
                            }
                        }
                    }
                }
            }

            var reducedConstantArray = reducedRowEchelon(constants, coefficients);

            return reducedConstantArray[coefficientDictionary[currentPosition]];
        }

        //reduced row echelon function to solve our matrix. You might want to just copy paste this, but bonus points for understanding this monstrosity.
        public static double[] reducedRowEchelon(double[] constants, double[][] coefficients)
        {
            //invalid matrix
            if (constants.Length != coefficients[0].Length)
            {
                throw new Exception("You must have the same number of constants as coefficients for this function.");
            }
            //really irritating simple algorithm to divide each coefficient as well as constant when reducing rows
            //it's really irritating because it's hard to keep track of in linear loops
            for (var i = 0; i < constants.Length; i++)
            {
                for (var j = 0; j < constants.Length; j++)
                {
                    //coefficients[i][i] != 0 - we do this to prevent division by 0, which can happen in the program logic when you eliminate an entire row
                    if (j != i && coefficients[i][i] != 0)
                    {
                        //we save the damn coefficient because otherwise we would change it while dividing
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
            //ugh. just ugh.
            return constants;
        }

        public class cellComparer : IEqualityComparer<int[]>
        {
            public bool Equals(int[] b1, int[] b2)
            {
                if (b2 == null && b1 == null)
                    return true;
                else if (b1 == null | b2 == null)
                    return false;
                else if (b1.Length != b2.Length)
                    return false;
                else {
                    var equality = true;
                    for (var i = 0; i < b1.Length; i++)
                    {
                        if (b1[i] != b2[i])
                            equality = false;
                    }
                    return equality;
                }
            }

            public int GetHashCode(int[] bx)
            {
                var hCode = 2;
                for (var i = 0; i < bx.Length; i++) {
                    if (bx[i] == 0)
                    {
                        hCode += 1;
                    }
                    else
                    {
                        hCode ^= bx[i];
                    }
                }
                return hCode.GetHashCode();
            }
        }
    }
}
