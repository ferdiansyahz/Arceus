using System;
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
            string x = serial1.ReadLine();
            //string y = serial1.ReadLine();
            hex += 1;

            /*if(hex>51)
            {
                hex = 1;
            }*/

            this.BeginInvoke(new LineReceivedEvent(LineReceived), x, hex);
            this.BeginInvoke(new LineReceivedEvent1(recsv), x);
        }

        private delegate void LineReceivedEvent(string x, int hex);
        private delegate void LineReceivedEvent1(string x);

        private void recsv(string x)
        {
            StringBuilder csvcontent = new StringBuilder();
            string quer = "D:\\CSVFile\\";
            string loc = ".csv";
            string csvpath = quer + textBox2.Text + loc;

            //var newline = String.Format("{0},{1},{2},{3}", "Force", DateTime.Now.ToString("HH:mm:ss tt"), x, System.Environment.NewLine);
            var newline = String.Format("{0},{1},{2}", "Force", DateTime.Now.ToString("HH:mm:ss tt"), x);
            csvcontent.Append(newline);

            System.IO.File.AppendAllText(csvpath, csvcontent.ToString());
        }

        private void LineReceived(string x, int hex)
        {
            //float x1 = float.Parse(x);
            //float y1 = float.Parse(y);
            //x3 = x1 / 10;
            
            int x2 = Convert.ToInt32(x) / 10;
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
                circularProgressBar1.Value = x2;
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
        }

        private void pictureBox3_Click(object sender, EventArgs e)
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
                        pictureBox2.Enabled = true;
                        pictureBox1.Enabled = false;
                        pictureBox1.Visible = false;
                        label3.Visible = false;
                        pictureBox2.Visible = true;
                        label4.Visible = true;

                        System.Timers.Timer timer = new System.Timers.Timer(Convert.ToDouble(textBox3.Text)*1000);
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
            else if (serial1.IsOpen == false)
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
                    else
                    {
                        serial1.PortName = comboBox1.Text;
                        serial1.BaudRate = 9600;
                        serial1.Open();
                        serial1.DataReceived += new SerialDataReceivedEventHandler(serial1_DataReceived);
                        textBox1.Text = "Connected";
                        pictureBox2.Enabled = true;
                        pictureBox1.Enabled = false;
                        pictureBox1.Visible = false;
                        label3.Visible = false;
                        pictureBox2.Visible = true;
                        label4.Visible = true;

                        System.Timers.Timer timer = new System.Timers.Timer(Convert.ToDouble(textBox3.Text)*1000);
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
        }
    }

    
}
