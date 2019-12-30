using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
namespace Borrowing_System
{
    class Book
    {
        private static int number = 1;
        public static void setNumber(int num)
        {
            if (num > 0)
            {
                number = num;
            }
        }

        public int ID { private set; get; }

        
        public string Title { set; get; }
        public string Author { set; get; }

        public Book()
        {

        }

        public Book(string title, string author)
        {
            this.ID = number;
            number++;
            this.Title = title;
            this.Author = author;
        }

    }
}
