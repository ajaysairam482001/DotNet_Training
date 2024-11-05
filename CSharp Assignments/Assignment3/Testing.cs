using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ajay_Assignments.Assignment3;

namespace Ajay_Assignments.Assignment3
{
    class Testing
    {
        public static void Main()
        {
            //BankAccounts acc = new BankAccounts(123, "Ajay", "Savings");
            //acc.Transaction();
            //acc.display();

            //Student s = new Student(2735, "Ajay", "third", "6", "ECE");
            //s.calculateResult();
            //Console.Read();


            //setting the base 
            //can choose a contructor like method therein class but it didnt make sense so....
            Console.WriteLine("enter the details of the class: ");
            Console.Write("Sales Number: "); SaleDetails.salesNo = int.Parse(Console.ReadLine());
            Console.Write("Product Number: "); SaleDetails.productNo = int.Parse(Console.ReadLine());
            Console.Write("Price: "); SaleDetails.price = double.Parse(Console.ReadLine());
            Console.Write("Quantity: "); SaleDetails.quantity = int.Parse(Console.ReadLine());
            SaleDetails.time = DateTime.Now;
            Console.WriteLine();
            SaleDetails.Sales();
            SaleDetails.display();
            Console.Read();

        }
    }
}
