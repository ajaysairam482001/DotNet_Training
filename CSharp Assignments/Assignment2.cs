using System;

namespace Ajay_Assignments
{
    class Assignment2
    {
        public static void Main(String[] args)
        {
            Assignment2 obj = new Assignment2();
            int i = 0;
            while (i == 0)
            {
                Console.WriteLine("Enter the Question number from 1 to 6 ?: ");
                int a = Convert.ToInt32(Console.ReadLine());
                switch (a)
                {
                    case 1:
                        obj.question1(); break;
                    case 2:
                        obj.question2(); break;
                    case 3:
                        obj.question3(); break;
                    case 4:
                        obj.arr1(); break;
                    case 5:
                        obj.arr2(); break;
                    case 6:
                        obj.arr3(); break;
                    default:
                        Console.WriteLine("Give a valid question number"); break;
                }
                if (a == 0)
                {
                    i = 1;
                }
                Console.WriteLine();
            }
            Console.Read();
        }

        public void question1()
        {
            Console.WriteLine("Write a C# Sharp program to swap two numbers.");
            int a = Convert.ToInt32(Console.ReadLine());
            int b = Convert.ToInt32(Console.ReadLine());
            int temp = a;
            a = b;
            b = temp;
            Console.WriteLine("The Values of A and B are {0} , {1}", a, b);
            return;
        }

        public void question2()
        {
            Console.WriteLine("Write a C# program that takes a number as input and displays it four times in a row (separated by blank spaces), and then four times in the next row, with no separation. You should do it twice: Use the console. Write and use {0}.\r\n");
            int a = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("{0} {0} {0} {0}",a);
            Console.WriteLine("{0}{0}{0}{0}",a);
            Console.WriteLine("{0} {0} {0} {0}",a); 
            Console.WriteLine("{0}{0}{0}{0}", a);
            return;
        }

        public void question3()
        {
            Console.WriteLine("Write a C# Sharp program to read any day number as an integer and display the name of the day as a word.");
            Console.WriteLine("Enter a valid day number");
            int a = Convert.ToInt32(Console.ReadLine());
            switch (a)
            {
                case 1:
                    Console.WriteLine("Monday"); break;
                case 2:
                    Console.WriteLine("Tuesday"); break;
                case 3:
                    Console.WriteLine("Wednesday"); break;
                case 4:
                    Console.WriteLine("Thursday"); break;
                case 5:
                    Console.WriteLine("Friday"); break;
                case 6:
                    Console.WriteLine("Saturday"); break;
                case 7:
                    Console.WriteLine("Sunday"); break;
                default:
                    Console.WriteLine("Give a valid day number"); break;
            }
            return;
        }

        // Arrays and numbers 
        public void arr1()
        {
            Console.WriteLine("Write a  Program to assign integer values to an array  and then print the following");
            int[] arr11 = new int[5];
            Console.WriteLine("Enter number");

            for (int i = 0; i < arr11.Length; i++)
            {
                Console.WriteLine($"number at position {i}: ");
                arr11[i] = Convert.ToInt32(Console.ReadLine());
            }
            int total = 0;
            for (int i = 0; i < arr11.Length; i++)
            {
                total += arr11[i];
            }

            double average = total / arr11.Length;
            Console.WriteLine($"The average of the array is {average}");


            int min = arr11[0];
            int max = arr11[0];
            for (int i = 0; i < arr11.Length; i++)
            {
                if (arr11[i] > max)
                {
                    max = arr11[i];
                }
                if (arr11[i] < min)
                {
                    min = arr11[i];
                }

            }
            Console.WriteLine("Minimum Value" + min);
            Console.WriteLine("Maximum Value" + max);
            return;
        }

        public void arr2()
        {
            Console.WriteLine("Write a program in C# to accept ten marks and display the following");
            int[] score = new int[10];
            Console.WriteLine("Enter marks:");

            for (int i = 0; i < score.Length; i++)
            {
               // Console.WriteLine($"Mark{i + 1}:");
                score[i] = Convert.ToInt32(Console.ReadLine());
            }
            int total = 0, min = score[0], max = score[0];
            foreach (int i in score)
            {
                total += i;
                if (i < min) min = i;
                if (i > max) max = i;
            }
            double average = total / 10;

            Console.WriteLine($"Average:{average}");

            Console.WriteLine($"Minimum:{min}");

            Console.WriteLine($"Maximum:{max}");

            Console.WriteLine($"Total:{total}");

            Array.Sort(score);
            Console.WriteLine("Ascending Order:");

            foreach(int i in score)
            {
                Console.Write(i + " ");
            }

            Console.WriteLine("Descending Order:");
            Array.Reverse(score);

            foreach (int i in score)
            {
                Console.Write(i + " ");
            }
            return;
        }

        public void arr3()
        {
            Console.WriteLine("Write a C# Sharp program to copy the elements of one array into another array.(do not use any inbuilt functions)");
            Console.WriteLine("Enter number of elements:");
            int n = int.Parse(Console.ReadLine());
            int[] org = new int[n];
            int[] copy = new int[n];
            Console.WriteLine("Enter elements of original array");
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"{i + 1}:");
                org[i] = int.Parse(Console.ReadLine());
            }
            for (int i = 0; i < n; i++)
            {
                copy[i] = org[i];
            }
            Console.WriteLine("Original Array:");
            foreach (int i in org)
            {
                Console.Write(i + " ");
            };
            Console.WriteLine("Copy Array");
            foreach (int i in copy)
            {
                Console.Write(i + " ");
            }
            return;
        }
    }
}
