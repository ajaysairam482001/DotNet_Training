using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ajay_Assignments.Assignment6
{
    class Employees
    {
        public int empID { get; set; }
        public string empName { get; set; }
        public double empSalary { get; set; }
        public string empCity { get; set; }

        public Employees(int id, string name, double salary, string city)
        {
            this.empID = id;
            this.empName = name;
            this.empSalary = salary;
            this.empCity = city;
        }
        public void display()
        {
            System.Console.WriteLine($"Employee ID: {empID}, Name: {empName}, Salary: {empSalary}, City: {empCity}\n");
        }
    }
    class Question3
    {
        static List<Employees> list;
        public static void Main(string[] args)
        {
            Console.Write("Enter the number of employees: ");
            int num = int.Parse(Console.ReadLine());
            list = new List<Employees>();
            for (int j = 0; j < num; j++)
            {
                list.Add(menu());
            }
            Console.WriteLine("Welcome to the menu, Choose an option below: ");
            int i = 1;
            while(i == 1)
            {
                Console.WriteLine("1. DisplayAll.\n2. Filter Salary.\n3. Filter Region(City).\n4. Sort By Name.\n0. Exit\n");
                int a = int.Parse(Console.ReadLine());
                switch (a)
                {
                    case 0: i=0; break;
                    case 1: displayALL(); break;
                    case 2: salaryDisplay(); break;
                    case 3: cityDisplay(); break;
                    case 4: nameDisplay(); break;
                    default: Console.WriteLine("Enter Valid Option."); break;
                }
            }
            Console.Read();
        }
        public static Employees menu()
        {
            Console.Write("Enter Employee Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Employee ID: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Enter Employee Salary: ");
            double sal = double.Parse(Console.ReadLine());
            Console.Write("Enter Employee City: ");
            string city = Console.ReadLine();

            Employees emp = new Employees(id, name, sal, city);
            Console.WriteLine();
            return emp;
        }
        public static void displayALL()
        {
            foreach(Employees emp in list)
            {
                emp.display();
            }
        }
        public static void salaryDisplay()
        {
            IEnumerable<Employees> empList = from e in list where e.empSalary > 45000 select e;

            foreach (Employees emp in empList)
                emp.display();
        }
        public static void cityDisplay()
        {
            IEnumerable<Employees> empList = from e in list where e.empCity.Equals("Bangalore") select e;

            foreach (Employees emp in empList)
                emp.display();
        }
        public static void nameDisplay()
        {
            IEnumerable<Employees> empList = from e in list orderby e.empName ascending select e; //ascending ya

            foreach (Employees emp in empList)
                emp.display();
        }
    }
}
