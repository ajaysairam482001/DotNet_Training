using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Assesments
{
    class Assesment1
    {
        public static void Main(string[] args)
        {
            Assesment1 obj = new Assesment1();
            int i = 1;
            while (i == 1)
            {
                Console.Write("Enter a Question number from 1 to 4 to execute it: ");
                int a = int.Parse(Console.ReadLine());
                switch (a)
                {
                    case 1: obj.question1(); break;
                    case 2: obj.question2(); break;
                    case 3: obj.question3(); break;
                    case 4: obj.question4(); break;
                    case 0: i = 0; break;
                    default: Console.WriteLine("Invalid Input, Try Again."); break;
                }
                Console.WriteLine();
            }
            Console.Read();
        }
        public void question1()
        {
            Console.Write("Enter the String: ");
            string str = Console.ReadLine();
            Console.Write("Enter the position in which character to Remove from 0 to "+ (str.Length-1) +": ");
            int p = int.Parse(Console.ReadLine());

            if(p<0 || p > str.Length - 1)
            {
                Console.WriteLine("Invalid Position.");
                return;
            }
            Console.WriteLine(str.Remove(p,1));
        }
        public void question2()
        {
            Console.Write("Enter the String: ");
            string str = Console.ReadLine();

            if (str.Length <= 1)
            {
                Console.WriteLine(str);
                return;
            }
            char[] ch = str.ToCharArray();
            //swap
            char temp = ch[0];
            ch[0] = ch[ch.Length - 1];
            ch[ch.Length - 1] = temp;
            Console.Write("Result: ");
            Console.WriteLine(new string(ch));
        }
        public void question3()
        {
            Console.Write("Enter the number of elements to be entered: ");
            int size = int.Parse(Console.ReadLine());
            int[] arr = new int[size];
            int max = int.MinValue;
            for (int i = 0; i < size; i++)
            {
                Console.Write("Enter the element " + (i + 1) +": ");
                arr[i] = int.Parse(Console.ReadLine());
                if (max < arr[i])
                {
                    max = arr[i];
                }
            }
            Console.WriteLine("Maximum Value: " + max);
        }
        public void question4()
        {
            Console.Write("Enter the String: ");
            string str = Console.ReadLine().Trim().ToLower();
            Console.WriteLine();
            Console.Write("Enter the letter to be counted in the string: ");
            char c = char.Parse(Console.ReadLine());
            Console.WriteLine();

            char[] chars = str.ToCharArray();
            int count = 0;
            foreach(char cc in chars)
            {
                if(cc == c)
                {
                    count++;
                }
            }
            Console.WriteLine("Occurrence: " + count);
        }
    }
}
