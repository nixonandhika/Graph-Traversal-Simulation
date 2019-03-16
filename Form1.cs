using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graph_Traversal
{
    public partial class Form1 : Form
    {
        private static string file1 = "";
        private static string file2 = "";

        public void setfile1(string name)
        {
            file1 = name;
        }

        public void setfile2(string name)
        {
            file2 = name;
        }

        public string getfile1()
        {
            return file1;
        }

        public string getfile2()
        {
            return file2;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                textBox1.Text = openFileDialog1.FileName;
                textBox3.Text += "Graph file read from: " + System.Environment.NewLine + openFileDialog1.FileName + System.Environment.NewLine + System.Environment.NewLine;
                this.setfile1(openFileDialog1.FileName);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                textBox2.Text = openFileDialog1.FileName;
                textBox3.Text += "Query file read from: " + System.Environment.NewLine + openFileDialog1.FileName + System.Environment.NewLine + System.Environment.NewLine;
                this.setfile2(openFileDialog1.FileName);
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox3.Text += "Starting Program..." + System.Environment.NewLine + System.Environment.NewLine;
            Program.Main2(this.getfile1(), this.getfile2());
            textBox3.Text += "Result: " + System.Environment.NewLine;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
