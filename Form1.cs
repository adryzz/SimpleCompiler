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
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Diagnostics;

namespace SimpleCompiler
{
    public partial class Form1 : Form
    {
        CSharpCodeProvider provider = new CSharpCodeProvider();
        CompilerParameters parameters;
        bool saved = true;
        string path = "New  C# Class";
        Form2 f2;
        public Form1(string[] Path)
        {
            InitializeComponent();
            foreach(string s in Path)
            {
                if (Path == null && s == "")
                {
                    //No file opened
                    path = "New  C# Class";
                    saved = true;
                }
                else if (!(File.Exists(s)))
                {
                    //File doesn't exists
                    MessageBox.Show("The file " + s + " does not exists", "SimpleCompiler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    path = "New  C# Class";
                }
                else
                {
                    textBox1.Text = File.ReadAllText(s); //read code
                    path = s;
                    saved = true;
                }
            }
            this.Text = "SimpleCompiler - " + path;
        }

        private void ContextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Application.StartupPath;
            saveFileDialog1.InitialDirectory = Application.StartupPath;
            saveFileDialog1.DefaultExt = ".cs";
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult res = openFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                textBox1.Text = File.ReadAllText(openFileDialog1.FileName);
                saved = true;
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult res = saveFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                File.WriteAllText(openFileDialog1.FileName, textBox1.Text);
                saved = true;
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!saved)
            {
                DialogResult res = MessageBox.Show("Save?", "SimpleCompiler", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    SaveToolStripMenuItem_Click(sender, e);
                }
            }
            textBox1.Clear();
            saved = true;
        }
        

        private void dllCompile(bool debug, string path)
        {
            if (!saved)
            {
                SaveToolStripMenuItem_Click(null, null);
            }
            GenerateParameters(debug, path);
            parameters.GenerateExecutable = false;
            CompilerResults results = provider.CompileAssemblyFromSource(parameters, textBox1.Text);
            results.Errors.Cast<CompilerError>().ToList().ForEach(error => listBox1.Items.Add(error.ErrorText));
            listBox1.Items.Add("_______________________________________________________________\n");
        }

        private void exeCompile(bool debug, bool run, string path)
        {
            if (!saved)
            {
            SaveToolStripMenuItem_Click(null, null);
            }
            GenerateParameters(debug, path);
            parameters.GenerateExecutable = true;
            CompilerResults results = provider.CompileAssemblyFromSource(parameters, textBox1.Text);
            results.Errors.Cast<CompilerError>().ToList().ForEach(error => listBox1.Items.Add(error.ErrorText));
            listBox1.Items.Add("_______________________________________________________________\n");
            if (run)
            {
                Process.Start(path);
            }
        }

        public void Compile(bool exe, bool run, bool debug, string path)
        {
            if (!exe)
            {
                dllCompile(debug, path);
            }
            else
            {
                exeCompile(debug, run, path);
            }
        }

        #region params
        private void GenerateParameters(bool debug, string path)
        {
            string s = textBox1.Text;
            List<string> dlls = new List<string>();
            if (s.Contains("using System"))
            {
                dlls.Add("System.Core.dll");
            }
            parameters = new CompilerParameters(dlls.ToArray(), path, debug);
        }
        #endregion

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (!(this.Text.Contains("*")))
            {
                this.Text = "*" + this.Text;
            }
            saved = false;
        }

        private void RunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f2 = new Form2(this, true);
            DialogResult res = f2.ShowDialog();

        }

        private void CompileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f2 = new Form2(this, false);
            DialogResult res = f2.ShowDialog();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (saved)
            {
                e.Cancel = false;
            }
            else
            {
            DialogResult res = MessageBox.Show("Save?", "SimpleCompiler", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    SaveToolStripMenuItem_Click(null, null);
                    e.Cancel = false;
                }
                else if (res == DialogResult.No)
                {
                    e.Cancel = false;
                }
                else if (res == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
