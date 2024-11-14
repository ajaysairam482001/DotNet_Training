using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ajay_Assignments.Assignment5
{
    public class Books
    {
        public string BookName { get; set; }
        public string AuthorName { get; set; }

        public Books(string name, string aName)
        {
            BookName = name;
            AuthorName = aName;
        }

        public void display()
        {
            Console.WriteLine($"Book Name: "+BookName+ ", Author Name: "+ AuthorName);
        }
    }
    public class BookShelf
    {
        private Books[] bookArray = new Books[5];

        public Books this[int index]
        {
            get { return bookArray[index]; }
            set { bookArray[index] = value; }
        }

        public void display()
        {
            foreach (Books book in bookArray)
            {
                book.display();
            }
        }
    }
    class Question1
    {
        public static void Main()
        {
            BookShelf s = new BookShelf();

            for (int i = 0; i < 5; i++)
            {
                Console.Write($"Enter the name of Book {i + 1}: ");
                string bookName = Console.ReadLine();
                Console.Write($"Enter the author of Book {i + 1}: ");
                string authorName = Console.ReadLine();

                s[i] = new Books(bookName, authorName);
            }

            Console.WriteLine("\nDisplaying all books:");
            s.display();
            Console.Read();
        }
    }
}
