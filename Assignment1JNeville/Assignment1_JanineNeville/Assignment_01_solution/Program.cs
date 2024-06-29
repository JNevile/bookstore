using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

//Bow Valley College, Instructor: Pedro Ferreira Student: Janine Neville, Student ID: 164657 

namespace Assignment_01
{
    class Book  /*This class is complete.*/
    {
        public string BookName { get; set; } //Book name alone is not unique
        public int Serial { get; set; } //Together, book name and serial is unique
        public bool Status { get; set; } = false; //false means the book is available for renting
        public Book(string name, int serial)
        {
            BookName = name;
            Serial = serial;
        }
        public bool Available()
        {
            //returns true if a book is available
            if (!Status)
                return true;
            return false;
        }
        public bool Rent()
        {
            if (Status == false)
            {
                //A book can only be rented if it's rental status is false
                Status = true;
                return true;
            }
            return false; //otherwise, the book is not available.  
        }
        public bool Return()
        {
            if (Status == true)
            {
                //A book can be only returned if it is already rented.
                Status = false;
                return true;
            }
            else
            {
                // Status false means, the book is available in the store. (already returned)
                return false;
            }
        }
        public void BookInfo()
        {
            //Show name of the book, it's serial and rental status.
            Console.WriteLine("Book Name: {0}, Serial: {1}, Status: {2}", BookName, Serial, Status == true ? "Rented" : "Available");
        }
    }
    class Reader
    {
        //Fields/Properties are complete.
        public string ReaderName { get; set; }
        public List<Book> readersBookList; //Readers book list that will contain all books rented by that reader.
        public Reader(string name)
        {
            //Reader Constructor is complete.
            ReaderName = name;
            readersBookList = new List<Book>();   //initialize empty book list 
        }
        public void RentABook(Book book)
        {
            //Reader is allowed to rent maximum two books at a time.
            //Issue error message, if users want to rent more than two books.
            if (readersBookList.Count >= 2)
            {
                Console.WriteLine("You cannot rent more than two books!");
            }
            //Rent the book if available and display confirmation message.
            //A book can only be rented if it's rental status is false
            else if (book.Status == false)
            {
                readersBookList.Add(book);
                Console.WriteLine("You have successfully rented the book!");
            }
            //If the book is already rented display message indicating it is rented.
            else 
            {
                Console.WriteLine("Sorry! The book is already rented.");
            }
        }
        public void ReturnABook(Book book)
        {
            //First check if the reader rented this book, 
            //if yes, change the book's status = false - meaning available
            //and remove the book for the readers book list.
            if (readersBookList.Contains(book) == true)
            {
                book.Status = false; // Change the book's status
                readersBookList.Remove(book);
            }
        }
        public void ReaderInfo()
        {
            //This method is complete.
            //Shows the reader's name and the list of books rented by the reader.
            //If no books are rented by the reader yet, displays "No books rented yet!".
            Console.WriteLine("Reader {0} rented the following books:", ReaderName);
            if (readersBookList.Count == 0)
            {
                Console.WriteLine("No books rented yet!");
                return;
            }
            foreach (var book in readersBookList)
            {
                book.BookInfo();
            }
        }
    }
    class BookStore
    {
        //Fields are provided.
        public List<Book> BookStoreBooksList;
        public List<Reader> BookStoreReadersList;
        public BookStore()
        {
            //Constructor method is complete.
            BookStoreBooksList = new List<Book>();  //initially bookstore has no books
            BookStoreReadersList = new List<Reader>(); //initially bookstore has no readers
        }
        public void AddAReader(string name)
        {
            //This method is complete
            //Add a new reader to the bookstoreReadersList.
            Reader reader = new Reader(name);
            BookStoreReadersList.Add(reader);
        }
        public void RemoveAReader(string name)
        {
            /*Determine if the reader is registered to the bookstore,
            if not generate error message.*/
            /*For a registered/existing reader, in order to remove a reader,
            first (1) return all books (if any) rented by that reader 
            and then (2) remove the reader from BookStoreReadersList.*/
            Reader readerToRemove = BookStoreReadersList.FirstOrDefault(r => r.ReaderName == name);
            if (readerToRemove == null)
            {
                Console.WriteLine("The reader is not registered to the bookstore");
                return;
            }
            foreach (Book book in readerToRemove.readersBookList)
            {
                book.Return(); // Return the book to the store
            }
            BookStoreReadersList.Remove(readerToRemove);
        }
        public void AddABook(string name, int serial)
        {
            //add a book object to the BookStoreBooksList with BookName and Serial.
            Book book = new Book(name, serial);
            BookStoreBooksList.Add(book);
        }
        public void RemoveABook(string name, int serial)
        {
            //find the book with correct name and serial from BookStoreBooksList.
            //In order to remove a book from book store, only allow if the book's status==false
            //meaning the book is 'available' to the bookstore.   
            Book bookToRemove = BookStoreBooksList.FirstOrDefault(b => b.BookName == name && b.Serial == serial);
            if (bookToRemove == null || bookToRemove.Status == true)
            {
                Console.WriteLine("The book is already rented by another reader.");
                return;
            }
            BookStoreBooksList.Remove(bookToRemove);
        }
        public void RentABook(string readerName, string bookName, int serial)
        {
            //Find the reader from the BookStoreReadersList
            Reader reader = BookStoreReadersList.FirstOrDefault(r => r.ReaderName == readerName);
            if (reader == null)
            {
                //If the reader is not registered to bookstore, display error message.
                Console.WriteLine("You are not a registered reader of this bookstore.");
                return;
            }
            Book book = BookStoreBooksList.FirstOrDefault(b => b.BookName == bookName && b.Serial == serial);
            if (book == null || book.Status == true)
            {
                //Display error message if book is already rented by another reader.
                Console.WriteLine("Sorry the book is not available for renting, as it is currently in use by another reader.");
                return;
            }
            // If the book is available and the reader can rent it:
            reader.RentABook(book);
        }
        public void ReturnABook(string readerName, string bookName, int serial)
        {
            //A book can be returned by a reader, if he/she actually rented the book.
            //Find the reader from BookStoreReadersList
            //Find the book with correct serial from from reader's personal book list
            //Return the book by calling 'ReturnABook' method of the Reader class.
            Reader reader = BookStoreReadersList.FirstOrDefault(r => r.ReaderName == readerName);
            if (reader == null)
            {
                Console.WriteLine("The reader is not registered to the bookstore.");
                return;
            }
            Book book = BookStoreBooksList.FirstOrDefault(b => b.BookName == bookName && b.Serial == serial);
            if (book == null)
            {
                Console.WriteLine("The book is not in the bookstore's inventory.");
                return;
            }
            if (reader.readersBookList.Contains(book))
            {
                reader.ReturnABook(book);
            }
            else
            {
                Console.WriteLine("The reader did not rent this book.");
            }
        }
        public void ShowBookInformation()
        {
            //Show all books that are available to the bookstore (if any).
            if (BookStoreBooksList.Count == 0)
            {
                Console.WriteLine("The bookstore currently has no books available.");
                return;
            }
            Console.WriteLine("This is a list of all the books that are available to the bookstore:");
            foreach (var book in BookStoreBooksList)
            {
                book.BookInfo();
            }
        }
        public void ShowReaderInformation()
        {
            //Show all readers that are added to the bookstore (if any).
            if (BookStoreReadersList.Count == 0)
            {
                Console.WriteLine("The bookstore currently has no readers.");
                return;
            }
            Console.WriteLine("The bookstore has the following readers:");
            foreach (var reader in BookStoreReadersList)
            {
                reader.ReaderInfo();
            }
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            /*You should not change the code in the Main Method. */
            BookStore bs = new BookStore();
            bs.AddAReader("Mahbub Murshed");
            bs.AddAReader("David Alwright");
            bs.AddAReader("Susan Harper");
            bs.AddABook("Object Oriented Programming", 1);
            bs.AddABook("Object Oriented Programming", 2);
            bs.AddABook("Object Oriented Programming", 3);
            bs.AddABook("Programming Fundamentals", 1);
            bs.AddABook("Programming Fundamentals", 2);
            bs.AddABook("Let us C#", 1);
            bs.AddABook("Programming is Fun", 1);
            bs.AddABook("Life is Beautiful", 1);
            bs.AddABook("Let's Talk About the Logic", 1);
            bs.AddABook("How to ace a job interview", 1);
            bs.ShowBookInformation();
            bs.ShowReaderInformation();
            bs.RentABook("Salma Hayek", "Object Oriented Programming", 1);
            bs.RentABook("Mahbub Murshed", "Object Oriented Programming", 1);
            bs.RentABook("Mahbub Murshed", "How to ace a job interview", 1);
            bs.RentABook("Mahbub Murshed", "Life is Beautiful", 1);
            Console.WriteLine();
            bs.RentABook("David Alwright", "Object Oriented Programming", 1);
            bs.RentABook("David Alwright", "Programming Fundamentals", 1);
            Console.WriteLine();
            bs.RentABook("Susan Harper", "Let's Talk About the Logic", 1);
            bs.RentABook("Susan Harper", "How to ace a job interview", 1);
            Console.WriteLine();
            bs.ShowBookInformation();
            bs.ShowReaderInformation();
            bs.ReturnABook("Mahbub Murshed", "Object Oriented Programming", 1);
            bs.RentABook("Mahbub Murshed", "Life is Beautiful", 1);
            Console.WriteLine();
            bs.ReturnABook("Mahbub Murshed", "Programming Fundamentals", 1);
            bs.RemoveABook("Let us C#", 1);
            bs.RemoveABook("Let's Talk About the Logic", 1);
            bs.ShowReaderInformation();
            bs.RemoveAReader("Mahbub Murshed");
            bs.ShowBookInformation();
            bs.ShowReaderInformation();
            Console.Read();
        }
    }
}

/*
Once Executed, Your program will have the following output:

The bookstore has the following books available:
Book Name: Object Oriented Programming, Serial: 1, Status: Available
Book Name: Object Oriented Programming, Serial: 2, Status: Available
Book Name: Object Oriented Programming, Serial: 3, Status: Available
Book Name: Programming Fundamentals, Serial: 1, Status: Available
Book Name: Programming Fundamentals, Serial: 2, Status: Available
Book Name: Let us C#, Serial: 1, Status: Available
Book Name: Programming is Fun, Serial: 1, Status: Available
Book Name: Life is Beautiful, Serial: 1, Status: Available
Book Name: Let's Talk About the Logic, Serial: 1, Status: Available
Book Name: How to ace a job interview, Serial: 1, Status: Available

The bookstore has the following readers:
Reader Mahbub Murshed rented the following books:
No books rented yet!
Reader David Alwright rented the following books:
No books rented yet!
Reader Susan Harper rented the following books:
No books rented yet!

Salma Hayek, you are not a registered reader of this bookstore!

Book: 'Object Oriented Programming' successfully rented by Mahbub Murshed.
Book: 'How to ace a job interview' successfully rented by Mahbub Murshed.
Sorry! Mahbub Murshed, You cannot rent more than two books!

Book: 'Object Oriented Programming' successfully rented by David Alwright.
Book: 'Programming Fundamentals' successfully rented by David Alwright.

Book: 'Let's Talk About the Logic' successfully rented by Susan Harper.
Sorry Susan Harper, The book 'How to ace a job interview' is not Available for renting.

The bookstore has the following books available:
Book Name: Object Oriented Programming, Serial: 3, Status: Available
Book Name: Programming Fundamentals, Serial: 2, Status: Available
Book Name: Let us C#, Serial: 1, Status: Available
Book Name: Programming is Fun, Serial: 1, Status: Available
Book Name: Life is Beautiful, Serial: 1, Status: Available

The bookstore has the following readers:
Reader Mahbub Murshed rented the following books:
Book Name: Object Oriented Programming, Serial: 1, Status: Rented
Book Name: How to ace a job interview, Serial: 1, Status: Rented
Reader David Alwright rented the following books:
Book Name: Object Oriented Programming, Serial: 2, Status: Rented
Book Name: Programming Fundamentals, Serial: 1, Status: Rented
Reader Susan Harper rented the following books:
Book Name: Let's Talk About the Logic, Serial: 1, Status: Rented

Book: 'Object Oriented Programming', Serial: 1 successfully returned by Mahbub Murshed.
Book: 'Life is Beautiful' successfully rented by Mahbub Murshed.

Return Error! Mahbub Murshed, you have not rented Programming Fundamentals, Serial: 1

The book: Let us C#,Serial: 1, is successfully removed from the bookstore.
Sorry! 'Let's Talk About the Logic' is already rented. System cannot remove a rented book!

The bookstore has the following readers:
Reader Mahbub Murshed rented the following books:
Book Name: How to ace a job interview, Serial: 1, Status: Rented
Book Name: Life is Beautiful, Serial: 1, Status: Rented
Reader David Alwright rented the following books:
Book Name: Object Oriented Programming, Serial: 2, Status: Rented
Book Name: Programming Fundamentals, Serial: 1, Status: Rented
Reader Susan Harper rented the following books:
Book Name: Let's Talk About the Logic, Serial: 1, Status: Rented

Returning books rented by Mahbub Murshed:
Book: 'How to ace a job interview', Serial: 1 successfully returned by Mahbub Murshed.
Book: 'Life is Beautiful', Serial: 1 successfully returned by Mahbub Murshed.
Reader Mahbub Murshed successfully removed from bookstore.

The bookstore has the following books available:
Book Name: Object Oriented Programming, Serial: 1, Status: Available
Book Name: Object Oriented Programming, Serial: 3, Status: Available
Book Name: Programming Fundamentals, Serial: 2, Status: Available
Book Name: Programming is Fun, Serial: 1, Status: Available
Book Name: Life is Beautiful, Serial: 1, Status: Available
Book Name: How to ace a job interview, Serial: 1, Status: Available

The bookstore has the following readers:
Reader David Alwright rented the following books:
Book Name: Object Oriented Programming, Serial: 2, Status: Rented
Book Name: Programming Fundamentals, Serial: 1, Status: Rented
Reader Susan Harper rented the following books:
Book Name: Let's Talk About the Logic, Serial: 1, Status: Rented

 */
