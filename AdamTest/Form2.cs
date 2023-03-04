using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdamTest
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AdvantechUtil adam = new AdvantechUtil(textBox1.Text);
            adam.Connect();
            adam.WriteDio(26, 0);
            adam.Disconnect();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                timer1.Enabled = false;
                AdvantechUtil adam = new AdvantechUtil(textBox1.Text);
                adam.Connect();
                var resp = adam.RefreshDio();
                if (resp != null)
                {
                    checkBox1.Checked = resp[0];
                    checkBox2.Checked = resp[1];
                    checkBox3.Checked = resp[2];
                    checkBox4.Checked = resp[3];

                    checkBox5.Checked = resp[24];
                    checkBox6.Checked = resp[25];
                    checkBox7.Checked = resp[26];
                    checkBox8.Checked = resp[27];
                }
                adam.Disconnect();
            }
            catch (Exception exc)
            {
            }
            finally
            {
                timer1.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (button1.Text == "Durdur")
            {
                timer1.Stop();
                button1.Text = "Başlat";
            }
            else
            {
                timer1.Start();
                button1.Text = "Durdur";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AdvantechUtil adam = new AdvantechUtil(textBox1.Text);
            adam.Connect();
            adam.WriteDio(24, 0);
            adam.Disconnect();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AdvantechUtil adam = new AdvantechUtil(textBox1.Text);
            adam.Connect();
            adam.WriteDio(25, 0);
            adam.Disconnect();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AdvantechUtil adam = new AdvantechUtil(textBox1.Text);
            adam.Connect();
            adam.WriteDio(27, 0);
            adam.Disconnect();
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            AdvantechUtil adam = new AdvantechUtil(textBox1.Text);
            adam.Connect();
            adam.WriteDio(24, checkBox5.Checked ? 1 : 0);
            adam.Disconnect();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            AdvantechUtil adam = new AdvantechUtil(textBox1.Text);
            adam.Connect();
            adam.WriteDio(25, checkBox6.Checked ? 1 : 0);
            adam.Disconnect();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            AdvantechUtil adam = new AdvantechUtil(textBox1.Text);
            adam.Connect();
            adam.WriteDio(26, checkBox7.Checked ? 1 : 0);
            adam.Disconnect();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            AdvantechUtil adam = new AdvantechUtil(textBox1.Text);
            adam.Connect();
            adam.WriteDio(27, checkBox8.Checked ? 1 : 0);
            adam.Disconnect();
        }
    }
}
