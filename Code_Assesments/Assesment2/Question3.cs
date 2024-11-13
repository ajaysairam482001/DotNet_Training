using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Assesments.Assesment2
{
    class NegativeNumberException : ApplicationException
    {
        public NegativeNumberException() : base("Its a Negative Number.") { }
    }
    class Question3
    {
            static void Number(int n)
            {
                if (n < 0)
                {
                    throw new NegativeNumberException();
                }
                else
                {
                    Console.WriteLine("The Positive Number:" + n);
                }
            }
        public static void Main(string[] args)
        {
            int i = 1;
            while (i == 1)
            {
                try
                {
                    Console.WriteLine("\nEnter a Number: ");
                    int num = int.Parse(Console.ReadLine());
                    Number(num);
                }
                catch (NegativeNumberException ex)
                {
                    Console.WriteLine(ex.Message);
                    i = 0;
                }
            }
            Console.Read();
        }
    }
}
