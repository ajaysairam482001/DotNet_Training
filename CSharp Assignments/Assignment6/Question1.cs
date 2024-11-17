using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ajay_Assignments.Assignment6
{
    class Question1
    {
        public static void Main(string[] args)
        {
            Console.Write("Enter the number of numbers to be entered? : ");
            int n = int.Parse(Console.ReadLine());
            int[] arr = new int[n];
            for(int i = 0; i < n; i++)
            {
                Console.Write("Enter the element "+(i+1)+": ");
                arr[i] = int.Parse(Console.ReadLine());
            }

            IEnumerable<int> list = from v in arr where (v*v)>20 select v;
            
            Console.Write("Answer: ");

            foreach(int x in list)
            {
                Console.Write(x + " ");
            }
            Console.Read();
        }
    }
}
