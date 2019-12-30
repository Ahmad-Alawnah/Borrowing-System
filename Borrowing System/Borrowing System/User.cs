using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borrowing_System
{
    class User
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
        public string Name { set; get; }
        public string Address { set; get; }
        private int age;
        public int Age
        {
            set
            {
                if (value > 0)
                    age = value;
            }
            get
            {
                return age;
            }
        }

        public User()
        {

        }

        public User(string name, string address, int age)
        {
            this.ID = number;
            number++;
            this.Name = name;
            this.Address = address;
            this.Age = age;
        }



    }
}
