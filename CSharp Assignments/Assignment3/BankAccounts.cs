using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ajay_Assignments.Assignment3
{
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
            else if (a == 2) { 
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
        }
        private void debit(int amount)
        {
            this.balance -= amount;
        }

        //private void updateBalance(int amount,Transaction_type type) {
        //    if (type == Transaction_type.D)
        //    {
        //        this.balance += amount;
        //    }
        //    else if (type == Transaction_type.W)
        //    {
        //        this.balance -= amount;
        //    }
        //    return;
        //}
        public void display()
        {
            Console.WriteLine("Account Number: " + this.bankAccountNo +"\nHolder's Name: "+this.costumerName+"\nAccount type: "+this.accountType+"\nBalance: "+this.balance);
            Console.Read();
        }

    }
}
