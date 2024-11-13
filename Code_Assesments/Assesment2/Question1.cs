using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Code_Assesments.Assesment2
{
    abstract class Student
    {
        string name;
        int studentId;
        float Grade;

        public abstract bool isPassed(float grade);
    }

    class UnderGrad : Student
    {
        string name;
        int studentId;
        public float Grade {  get; set; }

        public UnderGrad(string name, int id, float grade)
        {
        this.name = name;
        this.studentId = id;
        this.Grade = grade;
        }


        public override bool isPassed(float grade)
        {
        //if(grade >= 70.0)
        //{
        //    return true;
        //}
        //return false;
        return grade >= 70.0 ? true : false;
        }
     }
    

    class Graduate : Student
    {
        string name;
        int studentId;
        public float Grade { get; set; }

        public Graduate(string name, int id, float grade)
        {
            this.name = name;
            this.studentId = id;
            this.Grade = grade;
        }

        public override bool isPassed(float grade)
        {
            return grade >= 80.0 ? true : false;
        }
    }
    class Question1
    {
        public static void Main(string[] args)
        {
            Question1 q = new Question1();
            int i = 0;
            do
            {
            Console.WriteLine();
            Console.WriteLine("Enter which catogory:");
            Console.WriteLine("1: UnderGraduate.");
            Console.WriteLine("2: Graduate.");
            int a = int.Parse(Console.ReadLine());
            switch (a)
            {
                case 1:
                    {
                        UnderGrad ug = q.menuUG();
                        Console.WriteLine("calculating Mark...");
                            if (ug.isPassed(ug.Grade))
                            {
                                Console.WriteLine("Passed");
                            }
                            else
                                Console.WriteLine("Failed");
                        break;
                    }
                    case 2:
                        {
                            Graduate ug = q.menuG();
                            Console.WriteLine("calculating Mark...");
                            if (ug.isPassed(ug.Grade))
                            {
                                Console.WriteLine("Passed");
                            }
                            else
                                Console.WriteLine("Failed");
                            break;
                        }
                    default: Console.WriteLine("Invalid Input"); i = 1; break;
                }
            } while (i == 0);
            Console.Read();
            }

        public UnderGrad menuUG()
        {
            Console.Write("Enter Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Id: ");
            int ID = int.Parse(Console.ReadLine());
            Console.Write("Enter the Grade: ");
            float f = float.Parse(Console.ReadLine());

            UnderGrad ug = new UnderGrad(name, ID, f);
            return ug;
        }
        public Graduate menuG()
        {
            Console.Write("Enter Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Id: ");
            int ID = int.Parse(Console.ReadLine());
            Console.Write("Enter the Grade: ");
            float f = float.Parse(Console.ReadLine());

            Graduate g = new Graduate(name, ID, f);
            return g;
        }
    }

}
