using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Code_Assesments.Assesment2
{
    class Products : IComparable<Products>
    {
        public string name { get; set; }
        public int ID { get; set; }
        public double price { get; set; }

        public Products(int id, string name, double price)
        {
            this.ID = id;
            this.name = name;
            this.price = price;
        }

        public int CompareTo(Products other)
        {
            if(other == null) return 1;
            return price.CompareTo(other.price); //asc
        }

        public override string ToString()
        {
            return $"Product ID: {ID}, Name: {name}, Price: ${price}";
        }

    }
    class Question2
    {
        public static void Main(string[] args)
        {
            Console.Write("Enter the number of Products: ");
            int num = int.Parse(Console.ReadLine());
            List<Products> list = new List<Products>();
            for(int i = 0; i < num; i++)
            {
                Products prodi = menu();
                list.Add(prodi);
            }
            list.Sort();
            Console.WriteLine("\nSorted Products by Price:");
            foreach (Products product in list)
            {
                Console.WriteLine(product.ToString());
            }
            Console.Read();

        }

        public static Products menu()
        {
            Console.Write("Enter Id: ");
            int ID = int.Parse(Console.ReadLine());
            Console.Write("Enter Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter the Price: ");
            double f = Double.Parse(Console.ReadLine());

            Products ug = new Products(ID, name, f);
            return ug;
        }
    }
}
