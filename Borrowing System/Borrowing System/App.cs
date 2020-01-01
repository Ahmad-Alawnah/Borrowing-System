using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;

namespace Borrowing_System
{
    public partial class App : Form
    {
        SQLiteConnection connection;
        StreamReader reader;
        StreamWriter writer;
        public App()
        {
            InitializeComponent();
        }
        public void Select_Star_Book()
        {
            string books = "SELECT * FROM BOOK";
            SQLiteCommand command = new SQLiteCommand(books, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ListViewItem li = new ListViewItem(reader["ID"].ToString());
                li.SubItems.Add(reader["Title"].ToString());
                li.SubItems.Add(reader["Author"].ToString());
                li.SubItems.Add(reader["UserID"].ToString());
                li.Name = li.Text;
                if (!listView2.Items.ContainsKey(li.Text))
                    listView2.Items.Add(li);

            }
            reader.Close();
        }

        public void Select_Star_User()
        {
            string users = "SELECT * FROM USER";
            SQLiteCommand command = new SQLiteCommand(users, connection);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ListViewItem li = new ListViewItem(reader["ID"].ToString());
                li.SubItems.Add(reader["Name"].ToString());
                li.SubItems.Add(reader["Address"].ToString());
                li.SubItems.Add(reader["Age"].ToString());
                li.Name = li.Text;
                if (!listView1.Items.ContainsKey(li.Text))
                    listView1.Items.Add(li);

            }
            reader.Close();
        }
        public void Select_Star_User_Borrow_Tab()
        {
            string users = "SELECT * FROM USER";
            SQLiteCommand command = new SQLiteCommand(users, connection);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ListViewItem li = new ListViewItem(reader["ID"].ToString());
                li.SubItems.Add(reader["Name"].ToString());
                li.SubItems.Add(reader["Address"].ToString());
                li.SubItems.Add(reader["Age"].ToString());
                li.Name = li.Text;
                if (!listView3.Items.ContainsKey(li.Text))
                    listView3.Items.Add(li);

            }
            reader.Close();
        }
        public void Select_Star_Book_Borrow_Tab()
        {
            string books = "SELECT * FROM BOOK WHERE UserID is NULL;";
            SQLiteCommand command = new SQLiteCommand(books, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ListViewItem li = new ListViewItem(reader["ID"].ToString());
                li.SubItems.Add(reader["Title"].ToString());
                li.SubItems.Add(reader["Author"].ToString());
                li.Name = li.Text;
                if (!listView4.Items.ContainsKey(li.Text))
                    listView4.Items.Add(li);

            }
            reader.Close();
        }
        public void Select_Star_Book_Unborrow_Tab()
        {
            string books = "SELECT * FROM BOOK WHERE UserID IS NOT NULL";
            SQLiteCommand command = new SQLiteCommand(books, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ListViewItem li = new ListViewItem(reader["ID"].ToString());
                li.SubItems.Add(reader["Title"].ToString());
                li.SubItems.Add(reader["Author"].ToString());
                li.SubItems.Add(reader["UserID"].ToString());
                li.Name = li.Text;
                if (!listView5.Items.ContainsKey(li.Text))
                    listView5.Items.Add(li);

            }
            reader.Close();
        }
        public void Deselect_User()
        {
            button2.Enabled = false;
            button6.Enabled = false;
            textBox8.Text = "";
            textBox9.Text = "";
            numericUpDown4.Value = 1m;
            numericUpDown3.Value = 1m;
            if (listView1.SelectedItems.Count > 0)
            {               
                listView1.SelectedItems[0].Focused = false;
                listView1.SelectedItems[0].Selected = false;
            }
        }
        public void Deselect_Book()
        {
            button5.Enabled = false;
            textBox5.Text = "";
            textBox7.Text = "";
            numericUpDown2.Value = 1m;
            button4.Enabled = false;
            if (listView2.SelectedItems.Count > 0)
            {                
                listView2.SelectedItems[0].Focused = false;
                listView2.SelectedItems[0].Selected = false;
            }
        }
        public void Add_To_Log(string log)
        {
            reader = new StreamReader("log.txt");
            string tempData = "";
            while (!reader.EndOfStream)
            {
                tempData += reader.ReadLine() + '\n';
            }

            reader.Close();
            writer = new StreamWriter("log.txt");
            writer.WriteLine(tempData);
            writer.WriteLine(log + DateTime.Now);
            writer.Close();
        }
        private void App_Load(object sender, EventArgs e)
        {
            numericUpDown2.Controls[0].Visible = false;
            numericUpDown3.Controls[0].Visible = false;

            connection = new SQLiteConnection("Data Source=DB.sqlite;Version=3;");
            connection.Open();

            Select_Star_User();
            Select_Star_Book();

            Select_Star_User_Borrow_Tab();
            Select_Star_Book_Borrow_Tab();
            Select_Star_Book_Unborrow_Tab();

            string sql = "SELECT COUNT(ID) FROM User;";
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            int num = int.Parse(reader["COUNT(ID)"].ToString());
            if (num > 0) {
                sql = "SELECT MAX(ID) FROM User;";
                command = new SQLiteCommand(sql, connection);
                reader = command.ExecuteReader();
                reader.Read();
                User.setNumber(int.Parse(reader["MAX(ID)"].ToString())+1);
 
            }

            sql = "SELECT COUNT(ID) FROM Book;";
            command = new SQLiteCommand(sql, connection);
            reader = command.ExecuteReader();
            num = int.Parse(reader["COUNT(ID)"].ToString());
            if (num > 0)
            {
                sql = "SELECT MAX(ID) FROM Book;";
                command = new SQLiteCommand(sql, connection);
                reader = command.ExecuteReader();
                reader.Read();
                Book.setNumber(int.Parse(reader["MAX(ID)"].ToString())+1);
            }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Please fill all fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (numericUpDown1.Value > 120m || numericUpDown1.Value < 1m)
            {
                MessageBox.Show("Please enter a valid age (between 1 and 120)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            User u1 = new User(textBox1.Text, textBox2.Text, int.Parse(numericUpDown1.Value.ToString()));

            string add = $"INSERT INTO USER VALUES({u1.ID},'{u1.Name}','{u1.Address}',{u1.Age});";
            SQLiteCommand command = new SQLiteCommand(add, connection);
            command.ExecuteNonQuery();

            Select_Star_User();
            Select_Star_User_Borrow_Tab();

            //storing into the log
            Add_To_Log($"Added a user: '{u1.Name}, {u1.Address}, {u1.Age}', on ");
        }


        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                button2.Enabled = true;
                button6.Enabled = true;
                numericUpDown3.Value = decimal.Parse(listView1.SelectedItems[0].Text);
                textBox8.Text = listView1.SelectedItems[0].SubItems[1].Text;
                textBox9.Text = listView1.SelectedItems[0].SubItems[2].Text;
                numericUpDown4.Value = decimal.Parse(listView1.SelectedItems[0].SubItems[3].Text);

            }
            else
            {
                Deselect_User();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            User u1 = new User();
            u1.Name = listView1.SelectedItems[0].SubItems[1].Text;
            u1.Address = listView1.SelectedItems[0].SubItems[2].Text;
            u1.Age = int.Parse(listView1.SelectedItems[0].SubItems[3].Text);

            string delete = $"DELETE FROM USER WHERE ID ={listView1.SelectedItems[0].Text};";
            SQLiteCommand command = new SQLiteCommand(delete, connection);
            command.ExecuteNonQuery();

            listView1.Items.Clear();
            listView3.Items.Clear();
            listView2.Items.Clear();
            listView4.Items.Clear();
            listView5.Items.Clear();

            Select_Star_User();
            Select_Star_User_Borrow_Tab();
            Select_Star_Book();
            Select_Star_Book_Borrow_Tab();
            Select_Star_Book_Unborrow_Tab();

            Deselect_User();
            Deselect_Book();
            button7.Enabled = false;
            button8.Enabled = false;
            //storing into the log

            Add_To_Log($"Deleted a user: '{u1.Name}, {u1.Address}, {u1.Age}', on ");

        }

        private void button6_Click(object sender, EventArgs e)
        {

            if (textBox8.Text=="" || textBox9.Text == "")
            {
                MessageBox.Show("Please fill all fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (numericUpDown4.Value > 120m || numericUpDown4.Value < 1m)
            {
                MessageBox.Show("Please enter a valid age (between 1 and 120)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string update = $"UPDATE User SET Name='{textBox8.Text}', Address='{textBox9.Text}', Age={numericUpDown4.Value} WHERE ID = {numericUpDown3.Value};";

            User u1 = new User();
            u1.Name = textBox8.Text;
            u1.Address = textBox9.Text;
            u1.Age = int.Parse(numericUpDown4.Value.ToString());

            SQLiteCommand command = new SQLiteCommand(update, connection);
            command.ExecuteNonQuery();

            listView1.Items.Clear();
            listView3.Items.Clear();

            Select_Star_User();
            Select_Star_User_Borrow_Tab();

            Deselect_Book();

            button7.Enabled = false;

            //storing into the log
            Add_To_Log($"Updated a user, new record: '{u1.Name}, {u1.Address}, {u1.Age}', on ");

        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (textBox3.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("Please fill all fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Book b1 = new Book(textBox3.Text, textBox4.Text);

            string add = $"INSERT INTO Book VALUES({b1.ID},'{b1.Title}','{b1.Author}',NULL);";
            SQLiteCommand command = new SQLiteCommand(add, connection);
            command.ExecuteNonQuery();

            listView4.Items.Clear();
            listView5.Items.Clear();

            Select_Star_Book_Borrow_Tab();
            Select_Star_Book();
            Select_Star_Book_Unborrow_Tab();

            //storing into the log
            Add_To_Log($"Added a Book: '{b1.Title}, {b1.Author}', on ");

        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count > 0)
            {
                button5.Enabled = true;
                button4.Enabled = true;
                numericUpDown2.Value = decimal.Parse(listView2.SelectedItems[0].Text);
                textBox5.Text = listView2.SelectedItems[0].SubItems[1].Text;
                textBox7.Text = listView2.SelectedItems[0].SubItems[2].Text;
            }
            else
            {
                Deselect_Book();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Book b1 = new Book();
            b1.Title = listView2.SelectedItems[0].SubItems[1].Text;
            b1.Author = listView2.SelectedItems[0].SubItems[2].Text;

            string delete = $"DELETE FROM Book WHERE ID ={listView2.SelectedItems[0].Text};";
            SQLiteCommand command = new SQLiteCommand(delete, connection);
            command.ExecuteNonQuery();

            listView2.Items.Remove(listView2.SelectedItems[0]);
            listView4.Items.Clear();
            listView5.Items.Clear();
            listView2.Items.Clear();

            Select_Star_Book_Borrow_Tab();
            Select_Star_Book();
            Select_Star_Book_Unborrow_Tab();

            Deselect_Book();

            button7.Enabled = false;
            button8.Enabled = false;

            //storing into the log
             Add_To_Log($"Deleted a book: '{b1.Title}, {b1.Author}', on ");

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox5.Text == "" || textBox7.Text == "")
            {
                MessageBox.Show("Please fill all fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string update = $"UPDATE Book SET Title='{textBox5.Text}', Author='{textBox7.Text}' WHERE ID={numericUpDown2.Value};";
            SQLiteCommand command = new SQLiteCommand(update, connection);
            command.ExecuteNonQuery();

            Book b1 = new Book();
            b1.Author = textBox7.Text;
            b1.Title = textBox5.Text;

            listView4.Items.Clear();
            listView5.Items.Clear();
            listView2.Items.Clear();

            Select_Star_Book_Borrow_Tab();
            Select_Star_Book();
            Select_Star_Book_Unborrow_Tab();

            Deselect_Book();

            button8.Enabled = false;
            button7.Enabled = false;

            //storing into the log
            Add_To_Log($"Updated a book, new record: '{b1.Title}, {b1.Author}', on ");
        }

        private void button7_Click(object sender, EventArgs e)
        {

            string borrow = $"UPDATE BOOK SET UserID ={listView3.SelectedItems[0].Text} WHERE ID = {listView4.SelectedItems[0].Text};";
            SQLiteCommand command = new SQLiteCommand(borrow, connection);
            command.ExecuteNonQuery();

            Book b1 = new Book();
            b1.Title = listView4.SelectedItems[0].SubItems[1].Text;
            b1.Author = listView4.SelectedItems[0].SubItems[2].Text;

            User u1 = new User();
            u1.Name = listView3.SelectedItems[0].SubItems[1].Text;
            u1.Address = listView3.SelectedItems[0].SubItems[2].Text;
            u1.Age = int.Parse(listView3.SelectedItems[0].SubItems[3].Text);

            listView4.Items.Clear();
            listView5.Items.Clear();
            listView2.Items.Clear();

            Select_Star_Book_Borrow_Tab();
            Select_Star_Book();
            Select_Star_Book_Unborrow_Tab();

            Deselect_User();
            Deselect_Book();

            button7.Enabled = false;
            button8.Enabled = false;

            //storing into the log
            Add_To_Log($"Borrowed a book, User: '{u1.Name}, {u1.Address}, {u1.Age}' Borrowed Book: '{b1.Title}, {b1.Author}', on ");
        }

        private void listView3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count >0 && listView4.SelectedItems.Count > 0)
            {
                button7.Enabled = true;
            }
            else
            {
                button7.Enabled = false;
            }
        }

        private void listView4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count > 0 && listView4.SelectedItems.Count > 0)
            {
                button7.Enabled = true;
            }
            else
            {
                button7.Enabled = false;
            }
        }

        private void listView5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView5.SelectedItems.Count > 0)
            {
                button8.Enabled = true;
            }
            else
                button8.Enabled = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string log = $"SELECT Name, Address, Age FROM User WHERE ID ={listView5.SelectedItems[0].SubItems[3].Text};";
            string unborrow = $"UPDATE Book SET UserID = NULL WHERE ID={listView5.SelectedItems[0].Text};";
            SQLiteCommand command = new SQLiteCommand(unborrow, connection);
            command.ExecuteNonQuery();

            command = new SQLiteCommand(log, connection);
            SQLiteDataReader dataReader = command.ExecuteReader();
            //dataReader.Read();

            User u1 = new User();
            u1.Name = dataReader["Name"].ToString();
            u1.Address = dataReader["Address"].ToString();
            u1.Age = int.Parse(dataReader["Age"].ToString());

            Book b1 = new Book();
            b1.Title = listView5.SelectedItems[0].SubItems[1].Text;
            b1.Author = listView5.SelectedItems[0].SubItems[2].Text;

            listView4.Items.Clear();
            listView5.Items.Clear();
            listView2.Items.Clear();

            Select_Star_Book_Borrow_Tab();
            Select_Star_Book();
            Select_Star_Book_Unborrow_Tab();

            Deselect_User();
            Deselect_Book();

            button7.Enabled = false;
            button8.Enabled = false;

            //storing into the log
            Add_To_Log($"Unborrowed a book, User: '{u1.Name}, {u1.Address}, {u1.Age}' Unborrowed Book: '{b1.Title}, {b1.Author}', on ");
        }
    }
}
