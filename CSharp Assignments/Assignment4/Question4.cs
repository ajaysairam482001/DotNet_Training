using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ajay_Assignments.Assignment4
{
    class Employee
    {
        public int empId { get; set; }
        public string empName { get; set; }
        public float salary { get; set; }

        public Employee(int empID, string name, float sal)
        {
            this.empId = empID;
            this.empName = name;
            this.salary = sal;
        }
    }

    class PartTimeEmployee : Employee
    {
        public float wages { get; set; }

        public PartTimeEmployee(int empId, string name, float sal, float wages) : base(empId, name, sal)
        {
            {
                this.wages = wages;
            }
        }
        public void display()
        {
            Console.WriteLine("EmpId: " + empId + "\nEmpName: " + empName + "\nSalary: " + salary + "\nWage: " + wages);
        }
    }
        
    class Question4
    {
        public static void Main(string[] args)
        {
          //  PartTimeEmployee pt = new PartTimeEmployee(123,"ajay",123.23f,125.45f);
          Question4 question4 = new Question4();
            PartTimeEmployee pt = question4.menu();
            pt.display();
            Console.Read();
        }

        PartTimeEmployee menu()
        {
            Console.WriteLine("Enter the Details below");
            Console.Write("Enter ID: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Enter Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Salary: ");
            float salary = float.Parse(Console.ReadLine());
            Console.Write("Enter Wages: ");
            float wage = float.Parse(Console.ReadLine());
            PartTimeEmployee pt = new PartTimeEmployee(id,name,salary,wage);
            return pt;
        }
    }
}
