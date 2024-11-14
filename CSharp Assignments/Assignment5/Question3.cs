using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ajay_Assignments.Assignment5
{
    class Question3
    {
        public static void Main(string[] args)
        {
            string filePath = "OutputFile_A5.txt";

            try
            {
                int count = File.ReadAllLines(filePath).Length;
                Console.WriteLine($"Number of lines in the file {filePath} : {count}");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found. Please enter a valid file path.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            Console.Read();
        }
    }
}
