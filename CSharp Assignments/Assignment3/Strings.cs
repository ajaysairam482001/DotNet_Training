using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Ajay_Assignments.Assignment3
{
    class Strings
    {
        public static void Main(string[] args)
        {
            Strings obj = new Strings();
            int i = 0;
            int a;
            while (i == 0)
            {
                Console.WriteLine("Enter the Question number from 1 to 3 ?: ");
                a = Convert.ToInt32(Console.ReadLine());
                switch (a)
                {
                    case 1:
                        obj.question1(); break;
                    case 2:
                        obj.question2(); break;
                    case 3:
                        obj.question3(); break;
                    default:
                        Console.WriteLine("Give a valid question number"); break;
                }
                if (a == 0)
                {
                    i = 1;
                }
                Console.WriteLine("------------------------------------------------------------------------------------------------------");
                Console.WriteLine();
            }
            Console.Read();
        }

        
        public void question1()
        {
            Console.WriteLine("Write a program in C# to accept a word from the user and display the length of it.");
            Console.WriteLine("Enter a String: ");
            string str = Console.ReadLine();
            Console.WriteLine("Length : " + str.Length);
            //Console.Read();
            return;
        }

        public void question2()
        {
            Console.WriteLine("Write a program in C# to accept a word from the user and display the reverse of it.");
            Console.WriteLine("Enter a String: ");
            string str = Console.ReadLine().Trim();
            char[] chars = str.ToCharArray();
            Array.Reverse(chars);
            Console.Write("Reversed_String: ");
            Console.WriteLine(new string(chars));
            //Console.Read();
        }

        public void question3() {

            Console.WriteLine("Write a program in C# to accept two words from user and find out if they are same.");
            Console.Write("Enter String 1: ");
            string str1 = Console.ReadLine().Trim();
            Console.Write("Enter String 2: ");
            string str2 = Console.ReadLine().Trim();

            if (str1.Equals(str2))
            {
                Console.WriteLine("Equal");
            }
            else
            {
                Console.WriteLine("Not Equal");
            }
           // Console.Read();

            //alternate
            // return str1.Equals(str2) ? "Equal":"Not Equal";
        }
    }
}
