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
using System.Diagnostics;

namespace Run_Length_Encoding_GUI_2019_
{
    public partial class Form1 : Form
    {
        FileStream fs1;
        StreamWriter sw1;
        StreamReader sr1;
        FileStream fs2;
        StreamWriter sw2;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fs1 = new FileStream("File.txt", FileMode.Create, FileAccess.ReadWrite);

            button2.Enabled = true;

            button4.Enabled = true;

            MessageBox.Show("File is opened", "Opened", MessageBoxButtons.OK);

            button1.Enabled = false;

            textBox1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string str = textBox1.Text.Trim();

            if (str == "")
            {
                MessageBox.Show("Invalid Input, Please Try Again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                sw1 = new StreamWriter(fs1);

                sw1.WriteLine(str);

                sw1.Flush();

                textBox1.Clear();

                button3.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string l;

            int linescount = 0;

            fs1.Seek(0, SeekOrigin.Begin);

            sr1 = new StreamReader(fs1);

            Stopwatch sw = new Stopwatch();

            sw.Start();

            fs2 = new FileStream("Compressed.txt", FileMode.Create, FileAccess.ReadWrite);

            sw2 = new StreamWriter(fs2);

            while ((l = sr1.ReadLine()) != null)
            {
                string compressed = Run_Length_Encoding(l);

                sw2.WriteLine(compressed);

                linescount++;

                sw2.Flush();
            }

            sw.Stop();

            double size1 = fs1.Length - (linescount * 2);

            double size2 = fs2.Length - (linescount * 2);

            sw2.Close();

            fs2.Close();

            linescount = 0;

            if (size2 < size1)
            {
                double ratio = Math.Round((size2 / size1) * 100, 2);

                label2.Text = "Size of original file: " + size1 + " Bytes" + "\r\n";

                label2.Text += "Size of compressed file: " + size2 + " Bytes" + "\r\n";

                label2.Text += "Compression Percentage: " + ratio + "%" + "\r\n";

                label2.Text += "Time elapsed in milliseconds: " + sw.Elapsed.TotalMilliseconds + " ms";
            }

            else if (size2 > size1)
            {
                MessageBox.Show("Compression failed (Compressed file size is larger than original file size)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                MessageBox.Show("Compression failed (Compressed file size is equal to the original file size)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            button3.Enabled = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public static string Run_Length_Encoding(string str)
        {
            long length = str.Length;

            if (str.Length == 1)
            {
                return str.Trim();
            }

            else
            {
                int count = 1;

                string s = "";

                for (int i = 0; i < length; i++)
                {
                    if (i == length - 1)
                    {
                        if (str[i] == str[i - 1])
                        {
                            s += str[i];
                            s += count;
                            break;
                        }

                        else
                        {
                            s += str[i];
                            break;
                        }
                    }

                    else
                    {
                        if (str[i] == str[i + 1])
                        {
                            count++;
                        }

                        else
                        {
                            if (count == 1)
                            {
                                s += str[i];
                            }

                            else
                            {
                                s += str[i];
                                s += count;
                                count = 1;
                            }
                        }
                    }
                }

                return s.Trim();
            }
        }
    }
}
