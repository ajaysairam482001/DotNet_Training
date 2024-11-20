using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Assesments.Assesment3
{
    class Question3
    {
        void Write()
        {
            try
            {
                FileStream stream = new FileStream("Question3.Txt", FileMode.Append, FileAccess.Write);
                StreamWriter streamwriter = new StreamWriter(stream);
                Console.WriteLine("Enter the Data to be Added (Append)? :");
                string s = Console.ReadLine();
                streamwriter.Write(s);
                streamwriter.Close();
                stream.Close();
            }
            catch(FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        static void Main(string[] args)
        {
            Question3 q3 = new Question3();
            q3.Write();
            Console.Read();
        }
    }
}
