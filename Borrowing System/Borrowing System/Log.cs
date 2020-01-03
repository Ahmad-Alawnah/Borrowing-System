using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace Borrowing_System
{
    public partial class Log : Form
    {
        StreamReader reader;
        public Log()
        {
            InitializeComponent();
        }

        private void Log_Load(object sender, EventArgs e)
        {
            reader = new StreamReader("Log.txt");
            while (!reader.EndOfStream)
            {
                string x = reader.ReadLine();
                if (x == "") continue;
                textBox1.Text += x + Environment.NewLine;
            }
            reader.Close();
        }
    }
}
