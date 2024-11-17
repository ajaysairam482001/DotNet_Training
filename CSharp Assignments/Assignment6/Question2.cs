using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ajay_Assignments.Assignment6
{
    class Question2
    {
        public static void Main(string[] args)
        {
            Console.Write("Enter the number of words to be entered? : ");
            int n = int.Parse(Console.ReadLine());
            string[] arr = new string[n];
            for (int i = 0; i < n; i++)
            {
                Console.Write("Enter the string " + (i + 1) + ": ");
                arr[i] = (Console.ReadLine());
            }

            IEnumerable<string> list = from s in arr where (s.StartsWith("a")) && (s.EndsWith("m")) select s;

            Console.Write("Answer: ");

            foreach (string x in list)
            {
                Console.Write(x + " ");
            }
            Console.Read();
        }
    }
}
