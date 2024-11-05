using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ajay_Assignments.Assignment3
{
    class Student
    {
        int rollNo;
        string name;
        string class_;
        string semester;
        string branch;
        int[] marks;

        public Student(int roll,string name,string classes,string sem,string branch)
        {
            this.rollNo = roll;
            this.name = name;
            this.class_ = classes;
            this.branch = branch;
            this.semester = sem;    
        }
        public void setMark()
        {
            this.marks = getMarks();
        }
        private int[] getMarks()
        {
            int[] marks = new int[5];
            Console.WriteLine("Enter the 5 Subjects marks below.");
            for (int i = 0; i < marks.Length; i++) {
                Console.Write("Enter the mark for Subject" + (i + 1) + " : ");
                marks[i] = int.Parse(Console.ReadLine());
            }
            return marks;
        }
        public void calculateResult()
        {
            if (marks == null) setMark();
            int total = 0;
            foreach (int i in marks)
            {
                if (i < 35) { Console.WriteLine(); Console.WriteLine("Failed"); return; }
                total += i;
            }
            if((total/5) < 50)
            {
                Console.WriteLine();
                Console.WriteLine("Failed");
                return;
            }
            Console.WriteLine();
            Console.WriteLine("Passed");
            Console.WriteLine();
            display();
            return;
        }
        public void display()
        {
            Console.WriteLine("Roll_Number: " + this.rollNo + "\nName: " + this.name + "\nClass: " + this.class_ + "\nBranch: " + this.branch + "\nSemester: " + this.semester);
            Console.WriteLine("Displaying Marks below.");
            for (int i = 0; i < this.marks.Length; i++)
            {
                Console.WriteLine("Subject" + (i + 1) + " mark: " + this.marks[i]);
            }
            Console.WriteLine("---------------------------------------------------------");
        }
    }
}
