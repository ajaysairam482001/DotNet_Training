using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibraryA6;

namespace Ajay_Assignments.Assignment6
{
    class Costumers
    {
        public string name { get; set; }
        public const double cost = 500;
        public int age { get; set; }
        public Costumers(string name, int age)
        {
            this.name = name;
            this.age = age;
        }
    }
    class Question4
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the library system. Enter Your details: ");
            Console.Write("Enter your Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter your Age: ");
            int age = int.Parse(Console.ReadLine());
            Costumers c = new Costumers(name,age);


            ConcessionCalc calc = new ConcessionCalc();
            Console.WriteLine(calc.calculateConcession(Costumers.cost,c.age,c.name));
            Console.Read();
        }
        
    }
}
