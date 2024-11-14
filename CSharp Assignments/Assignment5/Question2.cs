using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ajay_Assignments.Assignment5
{
    class Question2
    {
        public static void Main(string[] args)
        {
            Console.Write("Enter the number of lines you want to write in the file: ");
            int numberOfLines = int.Parse(Console.ReadLine());

            string[] l = new string[numberOfLines];
            for (int i = 0; i < numberOfLines; i++)
            {
                Console.Write($"Enter line {i + 1}: ");
                l[i] = Console.ReadLine();
            }

            string filePath = "OutputFile_A5.txt";

            try
            {
                File.WriteAllLines(filePath, l);
                Console.WriteLine("Lines appended to the file successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            Console.Read();
        }
    }
}
