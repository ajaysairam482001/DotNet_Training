using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Ajay_Assignments.Assignment4
{
    public interface IStudent
    {
        int StudentId { get; set; }
        string Name { get; set; }
        void ShowDetails();
    }

    class DayScholor : IStudent
    {
        public int StudentId { get; set; }
        public string Name { get; set; }

        public DayScholor(int Id, string name)
        {
            StudentId = Id;
            Name = name;
        }

        public void ShowDetails() {
            Console.WriteLine("DayScholor Name: " + Name);
            Console.WriteLine("DayScholor Id: " + StudentId);
        }
    }
    class Resident : IStudent
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public Resident(int Id, string name)
        {
            StudentId = Id;
            Name = name;
        }

        public void ShowDetails()
        {
            Console.WriteLine("Resident Name: " + Name);
            Console.WriteLine("Resident Id: " + StudentId);
        }

    }
    class Question5
    {
        public static void Main(string[] args)
        {
            Resident r = new Resident(123,"Banu");
            DayScholor d = new DayScholor(456,"Ajay");
            r.ShowDetails();
            Console.WriteLine();
            d.ShowDetails();
            Console.Read();
        }

    }

}
