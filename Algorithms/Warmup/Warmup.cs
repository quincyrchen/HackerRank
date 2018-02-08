using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveMeFirst
{
    public class Solution
    {
        public static int solveMeFirst(int a, int b)
        {
            // Hint: Type return a+b; below  
            //We are just adding two numbers here; I hope you don't have any issues!
            return a + b;
        }

        static void Main(String[] args)
        {
            int val1 = Convert.ToInt32(Console.ReadLine());
            int val2 = Convert.ToInt32(Console.ReadLine());
            int sum = solveMeFirst(val1, val2);
            Console.WriteLine(sum);
        }
    }
}

namespace SimpleArraySum
{
    public class Solution
    {
        public static int simpleArraySum(int n, int[] ar)
        {
            // Complete this function
            //we declare the sum variable outside of the for loop to ensure that we don't reset it on each iteration
            var sum = 0;
            //we then iterate through the array and add the numbers up!
            for (var i = 0; i < ar.Length; i++)
            {
                sum += ar[i];
            }
            return sum;
            //Notice that n (the number of items in the array) is not used, since arrays have the Length property we can call on.
        }

        static void Main(String[] args)
        {
            int n = Convert.ToInt32(Console.ReadLine());
            string[] ar_temp = Console.ReadLine().Split(' ');
            int[] ar = Array.ConvertAll(ar_temp, Int32.Parse);
            int result = simpleArraySum(n, ar);
            Console.WriteLine(result);
        }

    }