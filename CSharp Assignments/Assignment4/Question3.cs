using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Ajay_Assignments.Assignment4
{
    class Doctor
    {
        private int _Reg_No;
        private string name;
        private int fee_charged;
        private int age;

        public int Reg_No
        {
            get { return _Reg_No; }
            set { _Reg_No = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public int FeeCharged
        {
            get { return fee_charged; }
            set { fee_charged = value; }
        }
        public int Age
        {
            get { return age; }
            set { age = value; }
        }
    }
    class Books
    {
        string bookName;
        string authorName;
        public string BookName
        {
            get { return bookName; }
            set { bookName = value; }
        }

        public string AuthorName
        {
            get { return authorName; }
            set { authorName = value; }
        }
        public Books(string bookName, string authorName)
        {
            this.bookName = bookName;
            this.authorName = authorName;
        }
        public void display()
        {
            Console.WriteLine("BookName: " + bookName + "\nAuthorName: " + authorName);
        }
    }
    class BookShelf
    {
        public Books book1 { get; set; }
        public Books book2 { get; set; }
        public Books book3 { get; set; }
        public Books book4 { get; set; }
        public Books book5 { get; set; }

        public object this[int index]
        {
            //to get the property values 
            get
            {
                if (index == 1)
                    return book1;
                else if (index == 2)
                    return book2;
                else if (index == 3)
                    return book3;
                else if (index == 4)
                    return book4;
                else if (index == 5)
                    return book5;
                else
                    return null;
            }

            set
            {
                if (index == 1)
                    book1 = (Books)value;
                else if (index == 2)
                    book2 = (Books)value;
                else if (index == 3)
                    book3 = (Books)value;
                else if (index == 4)
                    book4 = (Books)value;
                else if (index == 5)
                    book5 = (Books)value;
            }
        }

        public void dislay()
        {
            Console.WriteLine(book1.BookName + "," + book1.AuthorName);
            Console.WriteLine(book2.BookName + "," + book2.AuthorName);
            Console.WriteLine(book3.BookName + "," + book3.AuthorName);
            Console.WriteLine(book4.BookName + "," + book4.AuthorName);
            Console.WriteLine(book5.BookName + "," + book5.AuthorName);
        }
    }
    class Question3
    {
        public static void Main(string[] args)
        {
            Question3 q = new Question3();
            //q.setDoctor();
            //q.setBooks();
            q.setBookShelf();
            Console.Read();
        }
        public void setDoctor()
        {
            Doctor doctor = new Doctor();
            Console.Write("Enter Name: ");
            doctor.Name = Console.ReadLine();
            Console.Write("Enter Age: ");
            doctor.Age = int.Parse(Console.ReadLine());
            Console.Write("Enter Register Number: ");
            doctor.Reg_No = int.Parse(Console.ReadLine());
            Console.Write("Enter Fees_Charged: ");
            doctor.FeeCharged = int.Parse(Console.ReadLine()); 
            
            Console.WriteLine();

            Console.WriteLine("Name: "+doctor.Name+ "\nAge: " + doctor.Age + "\nRegisterNo: " + doctor.Reg_No + "\nFeesCharged: " + doctor.FeeCharged+ "\n");
            //Console.Read();
            Console.WriteLine();
        }
        public void setBooks()
        {
            Console.Write("Enter BookName: ");
            string bookName = Console.ReadLine();
            Console.Write("Enter AuthorName: ");
            string authorName = Console.ReadLine();
            Books b = new Books(bookName,authorName);
            b.display();
            Console.WriteLine();
            Console.Read();
        }
        public void setBookShelf()
        {
            BookShelf bookShelf = new BookShelf();
            bookShelf[1] = getbook();
            bookShelf[2] = getbook();
            bookShelf[3] = getbook();
            bookShelf[4] = getbook();
            bookShelf[5] = getbook();
            bookShelf.dislay();
        }
        private Books getbook()
        {
            Console.Write("Enter BookName: ");
            string bookName = Console.ReadLine();
            Console.Write("Enter AuthorName: ");
            string Author = Console.ReadLine();
            Books b = new Books(bookName, Author);
            Console.WriteLine();    
            return b;
        }
    }
}
