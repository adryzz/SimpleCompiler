using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleCompiler
{
    public partial class Form2 : Form
    {
        Form1 form1;
        bool compiling;
        bool runExe;
        public Form2(Form1 f1, bool run)
        {
            InitializeComponent();
            form1 = f1;
            runExe = run;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            form1.Compile(radioButton1.Checked, runExe, radioButton3.Checked, textBox1.Text);
            this.Close();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                saveFileDialog1.DefaultExt = ".exe";
            }
            else
            {
                saveFileDialog1.DefaultExt = ".dll";
            }

            DialogResult res = saveFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                textBox1.Text = saveFileDialog1.FileName;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (compiling)
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
        }
    }
}
