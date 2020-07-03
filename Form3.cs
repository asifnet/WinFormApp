using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathLibrary;

namespace WF1
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        Calculate cal = new Calculate();
        private void button1_Click(object sender, EventArgs e)
        {
            int i = cal.Add(int.Parse(textBox1.Text), int.Parse(textBox2.Text));
            textBox3.Text = i.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int i = cal.Sub(int.Parse(textBox1.Text), int.Parse(textBox2.Text));
            textBox3.Text = i.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //int i = cal.Mult(int.Parse(textBox1.Text), int.Parse(textBox2.Text));
            //calling mathLibrary class

            Class1 math = new Class1();
            float i = math.Multiply(5, 2);
            textBox3.Text = i.ToString();

        }

        private void button4_Click(object sender, EventArgs e)
        {

            try
            {
                int i = cal.Div(int.Parse(textBox1.Text), int.Parse(textBox2.Text));
                textBox3.Text = i.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

       
        

}
}
