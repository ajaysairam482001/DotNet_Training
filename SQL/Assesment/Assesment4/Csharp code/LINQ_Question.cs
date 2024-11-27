using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Code_Assesments.Assesment4
{
    class LINQ_Question
    {
        public static void Main(string[] args)
        {
            List<Employee> emplist = new List<Employee>
            {
                new Employee{ EmployeeID = 1001,First_Name = "Malcolm", Last_Name = "Daruwalla", title = "Manager", DOB = new DateTime(1984, 11, 16), DOJ = new DateTime(2011, 06, 08), city = "Mumbai" },
                new Employee { EmployeeID = 1002, First_Name = "Asdin", Last_Name = "Dhalla", title = "AsstManager", DOB = new DateTime(1994, 08, 20), DOJ = new DateTime(2012, 07, 07), city = "Mumbai" },
                new Employee{ EmployeeID = 1003, First_Name = "Madhavi",Last_Name = "Oza",title = "Consultant", DOB = new DateTime(1987, 11, 14), DOJ = new DateTime(2015, 04, 12), city = "Pune" },
                new Employee { EmployeeID = 1004,First_Name = "Saba", Last_Name = "Shaikh", title = "SE", DOB = new DateTime(1990, 06, 03), DOJ = new DateTime(2016, 02, 02), city = "Pune" },
                new Employee{ EmployeeID = 1005, First_Name = "Nazia", Last_Name = "Shaikh", title = "SE", DOB = new DateTime(1991, 03, 08), DOJ = new DateTime(2016, 02, 02), city = "Mumbai" },
                new Employee { EmployeeID = 1006,First_Name = "Amit", Last_Name = "Pathak", title = "Consultant", DOB = new DateTime(1989, 11, 07), DOJ = new DateTime(2014, 08, 08), city = "Chennai" },
                new Employee{ EmployeeID = 1007, First_Name = "Vijay", Last_Name = "Natrajan", title = "Consultant", DOB = new DateTime(1989, 12, 02), DOJ = new DateTime(2015, 06, 01), city = "Mumbai" },
                new Employee { EmployeeID = 1008,First_Name = "Rahul",Last_Name = "Dubey",title = "Associate", DOB = new DateTime(1993, 11, 11), DOJ = new DateTime(2014, 11, 06), city = "Chennai" },
                new Employee{ EmployeeID = 1009, First_Name = "Suresh", Last_Name = "Mistry", title = "Associate", DOB = new DateTime(1992, 08, 12), DOJ = new DateTime(2014, 12, 03), city = "Chennai" },
                new Employee{ EmployeeID = 1010,First_Name = "Sumit", Last_Name = "Shah",title = "Manager", DOB = new DateTime(1991, 04, 12), DOJ = new DateTime(2016, 01, 02), city = "city" }
            };
            
            //a.
            var AllEmp = from emp in emplist select emp;
            Console.WriteLine("All the Employee Details: ");
            foreach (var emp in AllEmp)
            {
                Console.WriteLine($"ID: {emp.EmployeeID}, Name: {emp.First_Name} {emp.Last_Name}, Title: {emp.title}, DOB: {emp.DOB.ToShortDateString()}, DOJ: {emp.DOJ.ToShortDateString()}, City: {emp.city}");
            }

            //b.
            var notInMumbai = from emp in emplist
                             where emp.city != "Mumbai"
                             select emp;
            Console.WriteLine("\nEmployees Not in Mumbai:");
            foreach (var emp in notInMumbai)
            {
                Console.WriteLine($"ID: {emp.EmployeeID}, Name: {emp.First_Name} {emp.Last_Name}, Title: {emp.title}, DOB: {emp.DOB.ToShortDateString()}, DOJ: {emp.DOJ.ToShortDateString()}, City: {emp.city}");
            }

            //c.
            var assistManagers = from emp in emplist
                               where emp.title == "AsstManager"
                               select emp;
            Console.WriteLine("\nEmployees with Title 'AssistManager':");
            foreach (var emp in assistManagers)
            {
                Console.WriteLine($"ID: {emp.EmployeeID}, Name: {emp.First_Name} {emp.Last_Name}, Title: {emp.title}, DOB: {emp.DOB.ToShortDateString()}, DOJ: {emp.DOJ.ToShortDateString()}, City: {emp.city}");
            }

            //d.
            var lastnameStartsWithS = from emp in emplist
                                      where emp.Last_Name.StartsWith("S")
                                      select emp;
            Console.WriteLine("\nEmployees with Last Name Starting with 'S':");
            foreach (var emp in lastnameStartsWithS)
            {
                Console.WriteLine($"ID: {emp.EmployeeID}, Name: {emp.First_Name} {emp.Last_Name}, Title: {emp.title}, DOB: {emp.DOB.ToShortDateString()}, DOJ: {emp.DOJ.ToShortDateString()}, City: {emp.city}");
            }

            Console.Read();
        }
    }
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string title { get; set; }
        public DateTime DOB { get; set; }
        public DateTime DOJ { get; set; }
        public string city { get; set; }
    }
}
