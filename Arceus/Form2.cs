using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Arceus
{
    public partial class Form2 : Form
    {
        Form1 fm = new Form1();
        public int i = 1;
        public Form2()
        {
            
            InitializeComponent();
            Cursor = Cursors.WaitCursor;
        }

        

        private void timer1_Tick(object sender, EventArgs e)
        {
            Random rnd = new Random();
            int rand = rnd.Next(15, 25);
            if (i<100)
            {
                i = i + rand;
                label2.Text = i.ToString();
                if(i>100)
                {
                    label2.Text = "100";
                }
            }
           else
            {
                label2.Text = "100";
                timer1.Stop();
                timer1.Enabled = false;
                this.Hide();
                fm.Show();
            }


        }

        private void Form2_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Start();
            timer1.Interval = 1000;
            timer1.Tick += new EventHandler(timer1_Tick);
        }
    }
}
