using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ajay_Assignments.Assignment4
{
    class InvalidMarkException : ApplicationException
    {
        public InvalidMarkException() : base("Invalid Mark.") { }
        public InvalidMarkException(string message) : base() { }
    }
    class Scholorship
    {
        public void Merit(int mark, double fee)
        {
            try
            {
                if (mark >= 70 && mark <= 80)
                {
                    fee -= (fee * 20) / 100;
                    Console.WriteLine("Discounted Fee: " + fee);
                }
                else if (mark > 80 && mark <= 90)
                {
                    fee -= (fee * 30) / 100;
                    Console.WriteLine("Discounted Fee: " + fee);
                }
                else if (mark > 90 && mark<=100)
                {
                    fee -= (fee * 50) / 100;
                    Console.WriteLine("Discounted Fee: " + fee);
                }
                else if (mark < 0 || mark > 100)
                {
                    throw new InvalidMarkException();
                    
                }
                else
                {
                    Console.WriteLine("Insufficient marks.");
                }
                Console.WriteLine();
                
            }
            catch (InvalidMarkException e) { 
                Console.WriteLine("Error man!: "+e.Message);
            }
        }
    }
    class Question2
    {
        public static void Main(string[] args)
        {
            Scholorship s = new Scholorship();
            Console.WriteLine("Welcome to the Scholorship Association");
            Console.Write("Enter your Marks: ");
            int mark = int.Parse(Console.ReadLine());
            Console.Write("Enter your Fees: ");
            double fee = int.Parse(Console.ReadLine());

            s.Merit(mark, fee);
            Console.Read();
        }
    }
}
