using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ajay_Assignments.Assignment4
{
    class InsufficientBalanceException : ApplicationException
    {
        public InsufficientBalanceException() : base("Insufficient Balance.") { }
        public InsufficientBalanceException(string message) : base(message) { }
    }

    public enum Transaction_type
    {
        D = 1, //deposit
        W = 2, //withdrawal
        I = 0 //invalid
    }
    class BankAccounts
    {
        int bankAccountNo;
        string costumerName;
        string accountType;
        int balance;
        //Transaction_type transactionType; 

        public BankAccounts(int accNo, string name, string accType)
        {
            this.bankAccountNo = accNo;
            this.costumerName = name;
            this.accountType = accType;
            this.balance = 10000;
        }

        private Transaction_type getTransactionType()
        {
            Console.WriteLine("Enter Transaction_Type-> \nPress 1 for Deposit. \nPress 2 for Withdrawal ");
            int a = int.Parse(Console.ReadLine());
            if (a == 1)
            {
                return Transaction_type.D;
            }
            else if (a == 2)
            {
                return Transaction_type.W;
            }
            Console.WriteLine("Please Press a VALID button.");
            return Transaction_type.I;
        }
        public void Transaction()
        {
            Transaction_type type = getTransactionType();
            if (type == Transaction_type.I)
                return;
            Console.Write("Enter Amount: ");
            int amount = int.Parse(Console.ReadLine());
            //updateBalance(amount, type);
            if (type == Transaction_type.D)
                credit(amount);
            else
                debit(amount);
            return;
        }
        private void credit(int amount)
        {
            this.balance += amount;
            Console.WriteLine();
        }
        private void debit(int amount)
        {
            try
            {
                if (this.balance < amount)
                    throw new InsufficientBalanceException();
                else
                {
                    this.balance -= amount;
                    Console.WriteLine();
                }
            }
            catch(InsufficientBalanceException e)
            {
                Console.WriteLine("Error : " + e.Message);
                Console.WriteLine("---------------------------");
            }
        }
        public void displayBalance()
        {
            Console.WriteLine("Balance: "+ this.balance);
            Console.WriteLine();
        }
    }
    class Question1
    {
        public static void Main(string[] args)
        {
            BankAccounts acc = new BankAccounts(123, "Ajay", "Savings");
            int i;
            do
            {
                i = menu();
                switch (i)
                {
                    case 1:
                        acc.displayBalance(); break;
                    case 2:
                        acc.Transaction(); break;
                    case 3:
                        i = 0; break;
                    default:
                        Console.WriteLine("Enter Valid Input."); break;
                }
            } while (i!=0);
            Console.Read();
        }

        private static int menu()
        {
            Console.WriteLine("Banking portal --- Please Enter the Actions");
            Console.WriteLine("Press 1 to Check Balance.");
            Console.WriteLine("Press 2 for Transaction.");
            Console.WriteLine("Press 3 to Exit.");
            int n;
            n = int.Parse(Console.ReadLine());
            return n;
        }
    }
}
