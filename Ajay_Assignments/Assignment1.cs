using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ajay_Assignments
{
    class Assignment1
    {
        public static void Main(string[] args)
        {
            Assignment1 obj = new Assignment1();
            ////obj.question1();
            ////obj.question2();
            ////obj.question3();
            ////obj.question4();
            //obj.question5();
            int i = 0;
            while(i == 0)
            {
                Console.WriteLine("Enter the Question number from 1 to 5 ?: ");
                int a = Convert.ToInt32(Console.ReadLine());
                switch (a) {
                    case 1:
                        obj.question1(); break;
                    case 2:
                        obj.question2(); break;
                    case 3:
                        obj.question3(); break;
                    case 4:
                        obj.question4(); break;
                    case 5:
                        obj.question5(); break;
                    default:
                        Console.WriteLine("Give a valid question number"); break;
                }
                if(a == 0)
                {
                    i = 1;
                }
                Console.WriteLine();
            }
            Console.Read();
        }

        public void question1()
        {
            System.Console.WriteLine("Write a C# Sharp program to accept two integers and check whether they are equal or not.");
            int a = Convert.ToInt32(Console.Read());
            int b = Convert.ToInt32(Console.Read());
            if (a == b)
            {
                Console.WriteLine("{0} and {1} are equal", a, b);
            }
            else
            {
                Console.WriteLine("{0} and {1} are NOT equal", a, b);
            }
        }

        public void question2()
        {
            System.Console.WriteLine("Write a C# Sharp program to check whether a given number is positive or negative.");
            Console.Write("Enter a number: ");
            int a = Convert.ToInt32(Console.ReadLine());

            if (a > 0)
            {
                Console.WriteLine("{0} is positive.", a);
            }
            else if (a < 0)
            {
                Console.WriteLine("{0} is negative.", a);
            }
            else
            {
                Console.WriteLine("Its Zero");
            }
        }

        public void question3()
        {
            System.Console.WriteLine("Write a C# Sharp program that takes two numbers as input and performs all operations (+,-,*,/) on them and displays the result of that operation.");
            Console.Write("Enter the first number: ");
            int a = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter the operator:  ");
            char c = Convert.ToChar(Console.ReadLine());
            Console.Write("Enter the second number: ");
            int b = Convert.ToInt32(Console.ReadLine());

            switch (c)
            {
                case '+':
                    Console.WriteLine(a + b); break;
                case '-':
                    Console.WriteLine(a - b); break;
                case '*':
                    Console.WriteLine(a * b); break;
                case '/':
                    Console.WriteLine(a / b); break;
                default: Console.WriteLine("Invaild Operator."); break;
            }
        }

        public void question4()
        {
            System.Console.WriteLine("Write a C# Sharp program that prints the multiplication table of a number as input.");
            Console.Write("Enter a number to multiply: ");
            int a = Convert.ToInt32(Console.ReadLine());

            for (int i = 0; i <= 10; i++)
            {
                Console.WriteLine("{0} * {1} = {2}", a, i, a * i);
            }
        }

        public void question5()
        {
            System.Console.WriteLine("Write a C# program to compute the sum of two given integers. If two values are the same, return the triple of their sum.");
            Console.Write("Enter the first number: ");
            int a = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter the first number: ");
            int b = Convert.ToInt32(Console.ReadLine());

            if (a == b)
            {
                Console.WriteLine((a + b) * 3);
            }
            else
            {
                Console.WriteLine(a + b);
            }
        }
    }
}
