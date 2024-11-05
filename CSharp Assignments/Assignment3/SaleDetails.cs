using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ajay_Assignments.Assignment3
{
    static class SaleDetails
    {
        public static int salesNo;
        public static int productNo;
        public static double price;
        public static DateTime time;
        public static int quantity;
        public static double totalAmount;

        public static void Sales()
        {
            totalAmount = price * quantity;
            return;
        }

        public static void display()
        {
            Console.WriteLine("Details of the Sale below: ");
            Console.WriteLine("Sale Number: " + salesNo + "\nProduct Number: " + productNo + "\nPrice: " + price + "\nDate of sale: " + time + "\nQuantity: " + quantity + "\nTotalAmount: " + totalAmount);
            Console.WriteLine("--------------------------------------------------");
        }
    }
}
