﻿using System;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;

namespace Arceus
{
    public partial class Form1 : Form
    {
        static SerialPort serial1 = new SerialPort();
        public int hex = -50;
        public int fex;
        public double timer_1 = 0;
        public double timer_2 = 0;
        public bool hexbool = false;
        public Form1()
        {
            InitializeComponent();
            getAvailablePorts();
            
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x84:
                    base.WndProc(ref m);
                    if ((int)m.Result == 0x1)
                        m.Result = (IntPtr)0x2;
                    return;
            }

            base.WndProc(ref m);
        }

        private void getAvailablePorts()
        {
            String[] ports = SerialPort.GetPortNames();
            comboBox1.Items.AddRange(ports);
        }

        private void serial1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string val = serial1.ReadLine();
                string[] vale = val.Split(',');
                string x = vale[0];
                string y = vale[1];
                //string y = serial1.ReadLine();
                hex += 1;
                if (hexbool == true)
                {
                    if (hex == int.Parse(textBox4.Text))
                    {
                        serial1.Close();
                        MessageBox.Show("Done! Data has been saved", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Application.Restart();
                        hexbool = false;
                    }
                }
                /*if(hex>51)
                {
                    hex = 1;
                }*/
                timer_2 = DateTime.Now.Millisecond;

                this.BeginInvoke(new LineReceivedEvent(LineReceived), x, y, hex);
                this.BeginInvoke(new LineReceivedEvent1(recsv), x, y);
            }
            catch (TimeoutException) { }
        }

        private delegate void LineReceivedEvent(string x, string y, int hex);
        private delegate void LineReceivedEvent1(string x, string y);

        private void recsv(string x, string y)
        {
            StringBuilder csvcontent = new StringBuilder();
            string quer = "D:\\CSVFile\\";
            string loc = ".csv";
            string csvpath = quer + textBox2.Text + loc;
            timer_2 = DateTime.Now.Millisecond;
            //timer_2 = timer_2 - timer_1;
            //var newline = String.Format("{0},{1},{2},{3}", "Force", DateTime.Now.ToString("HH:mm:ss tt"), x, System.Environment.NewLine);
            var newline = String.Format("{0},{1},{2},{3},{4}", "Force", DateTime.Now.ToString("HH:mm:ss tt"), DateTime.Now.ToString("fff"),x,y);
            //var newline = String.Format("{0},{1},{2}", "Force", timer_2, x);
            csvcontent.Append(newline);
            
            
            System.IO.File.AppendAllText(csvpath, csvcontent.ToString());
            
        }

        private void LineReceived(string x, string y, int hex)
        {
            //float x1 = float.Parse(x);
            //float y1 = float.Parse(y);
            //x3 = x1 / 10;
            
            //int x2 = Convert.ToInt32(y) / 10;
            float x2 = float.Parse(y) / 10;
            fex += 1;
            if (x2 < 20)

            {
                textBox1.Text = x;
                circularProgressBar1.Value = 0;
                circularProgressBar1.Text = "0" + "%";
                circularProgressBar1.SubscriptText = x;
                chart1.Series["Force"].Points.AddY(x2);
                chart1.ChartAreas[0].AxisX.Minimum = hex;
                chart1.ChartAreas[0].AxisX.Maximum = hex + 50;
            }


            else

            {
                textBox1.Text = x;
                //circularProgressBar1.Value = x2;
                circularProgressBar1.Text = x2.ToString() + "%";
                circularProgressBar1.SubscriptText = x;
                chart1.Series["Force"].Points.AddY(x2);
                chart1.ChartAreas[0].AxisX.Minimum = hex;
                chart1.ChartAreas[0].AxisX.Maximum = hex + 50;

            }
            /*if (hex > 50)
            {
                

                foreach (var series in chart1.Series)
                {
                    series.Points.Clear();
                }
                
            }*/
            fex++;
        }


        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
            if (serial1.IsOpen == true)
            {
                try
                {
                    if (comboBox1.Text.Equals(""))
                    {
                        MessageBox.Show("Select your port, please", "Port: Empty",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    else
                    {
                        serial1.PortName = comboBox1.Text;
                        serial1.BaudRate = 9600;
                        serial1.DataReceived += new SerialDataReceivedEventHandler(serial1_DataReceived);
                        textBox1.Text = "Connected";
                      
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("COM already used. Disconnect your previous communication", "Dual Port Issue",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
            else if (serial1.IsOpen == false)
            {
                try
                {
                    if (comboBox1.Text.Equals(""))
                    {
                        MessageBox.Show("Select your port", "Port: Empty",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                    else if(textBox2.Text.Equals(""))
                    {
                        MessageBox.Show("Name your csv file", "File: CSV",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        serial1.PortName = comboBox1.Text;
                        serial1.BaudRate = 9600;
                        serial1.Open();
                        serial1.DataReceived += new SerialDataReceivedEventHandler(serial1_DataReceived);
                        textBox1.Text = "Connected";
                        
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("COM already used. Disconnect your previous communication", "Dual Port Issue",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
            
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            pictureBox1.Enabled = true;
            serial1.Close();
            pictureBox2.Enabled = false;
            textBox1.Text = "Disconnected";
            pictureBox2.Visible = false;
            label4.Visible = false;
            pictureBox1.Visible = true;
            label3.Visible = true;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            serial1.Close();
            this.Close();
            System.Windows.Forms.Application.Exit();
            System.Environment.Exit(1);
        }

        private void chart1_Click(object sender, EventArgs e)
        {
            
        }

        private static void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            serial1.Close();
            MessageBox.Show("Done! Data has been saved", "Success",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Restart();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            timer_1 = DateTime.Now.Millisecond;
            System.Timers.Timer timer = new System.Timers.Timer(Convert.ToDouble(textBox3.Text) * 1000);
            try
            {
                if (comboBox1.Text.Equals(""))
                {
                    MessageBox.Show("Select your port", "Port: Empty",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else if (textBox2.Text.Equals(""))
                {
                    MessageBox.Show("Name your csv file", "File: CSV",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if(textBox3.Text.Equals(""))
                {
                    MessageBox.Show("Set your timer", "Time Elapsed",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    
                    serial1.PortName = comboBox1.Text;
                    serial1.BaudRate = 9600;
                    serial1.Open();
                    serial1.DataReceived += new SerialDataReceivedEventHandler(serial1_DataReceived);
                    textBox1.Text = "Connected";
                    timer.Elapsed += Timer_Elapsed;
                    timer.Start();
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("COM already used. Disconnect your previous communication", "Dual Port Issue",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.Text.Equals(""))
                {
                    MessageBox.Show("Select your port", "Port: Empty",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else if (textBox2.Text.Equals(""))
                {
                    MessageBox.Show("Name your csv file", "File: CSV",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if(textBox4.Text.Equals(""))
                {
                    MessageBox.Show("Set Data Elapsed", "Data Control",
                   MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    serial1.PortName = comboBox1.Text;
                    serial1.BaudRate = 9600;
                    serial1.Open();
                    serial1.DataReceived += new SerialDataReceivedEventHandler(serial1_DataReceived);
                    textBox1.Text = "Connected";
                    hexbool = true;
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("COM already used. Disconnect your previous communication", "Dual Port Issue",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }
    }

    
}
