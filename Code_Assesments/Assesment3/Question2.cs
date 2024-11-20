using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Assesments.Assesment3
{
    class Box
    {
        public double length { get; set; }
        public double breadth { get; set; }

        public Box(double l, double b)
        {
            length = l;
            breadth = b;
        }

        public static Box AddBoxes(Box box1, Box box2)
        {
            double newLength = box1.length + box2.length;
            double newBreadth = box1.breadth + box2.breadth;
            return new Box(newLength, newBreadth);
        }

        public void Display()
        {
            Console.WriteLine("Box Dimensions: Length = " + length + ", Breadth = " + breadth);
        }   
    }
    class Question2 //Test class
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Enter the dimensions for First Box");
            Console.Write("Enter Length for Box 1: ");
            double l1 = double.Parse(Console.ReadLine());
            Console.Write("Enter Breadth for Box 1: ");
            double b1 = double.Parse(Console.ReadLine());
            Console.WriteLine();

            Console.WriteLine("Enter the dimensions for Second Box:");
            Console.Write("Enter Length for Box 2: ");
            double l2 = double.Parse(Console.ReadLine());
            Console.Write("Enter Breadth for Box 2: ");
            double b2 = double.Parse(Console.ReadLine());
            Console.WriteLine();

            Box box1 = new Box(l1, b1);
            Box box2 = new Box(l2, b2);
            Box box3 = Box.AddBoxes(box1, box2);

            Console.WriteLine("\nBox 3 -> Result of Adding Box 1 and Box 2: ");
            box3.Display();

            Console.ReadLine();
        }

    }
}
