using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Assesments.Assesment3
{

    class Question4
    {
        public delegate int calc(int a, int b);
        public static void Main(string[] args)
        {
            
            Calculator calculator = new Calculator();
            calc fourFunction = new calc(calculator.Add);
            fourFunction += new calc(calculator.Subtract);
            fourFunction += new calc(calculator.Multiply);
            fourFunction += new calc(calculator.Divide);

            Console.WriteLine("Enter two integers");
            Console.Write("Integer 1 : ");
            int a = int.Parse(Console.ReadLine());
            Console.Write("Integer 2 : ");
            int b = int.Parse(Console.ReadLine());

            //going to give same inputs anyway for all four
            foreach (Delegate d in fourFunction.GetInvocationList())
            {
                Console.WriteLine("Method executed: " + d.Method.Name);
                Console.WriteLine("Result: " + d.DynamicInvoke(a, b));
            }
            //Console.WriteLine(fourFunction(a,b)); //this is just invoking the last func
            //I need to execute all one by one
            Console.Read();
        }
    
    }
    

    class Calculator
    {
        public int Add(int a, int b)
        {
            return a + b;
        }
        public int Subtract(int a, int b)
        {
            return a - b;
        }
        public int Multiply(int a, int b)
        {
            return a * b;
        }
        public int Divide(int a, int b)
        {
            try
            {
                return a / b;
            }catch(DivideByZeroException e)
            {
                Console.WriteLine(e.Message);
            }
            return 0;
        }
    }
}
