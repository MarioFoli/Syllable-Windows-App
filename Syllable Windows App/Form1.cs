using System;
using Microsoft.Scripting;
using System.Diagnostics;
using System.IO;
using System.Net;
using IronPython.Hosting;
using System.Net.Sockets;
using Microsoft.Scripting.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting.Runtime;
using IronOcr;

namespace Syllable_Windows_App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string iniWord = textBox1.Text;
            string arg = string.Format(@"C:\Users\greni\Desktop\Projects\Syllable\splitter.py {0}", iniWord);
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(@"C:\Python\python.exe", arg)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            p.Start();
            string output = p.StandardOutput.ReadToEnd(); //returns python script results 
            p.WaitForExit();
            label3.Text = output;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var Ocr = new IronTesseract();
            using (var Input = new OcrInput("testimage.png"))
            {
                // Input.Deskew();  // use if image not straight
                // Input.DeNoise(); // use if image contains digital noise
                var Result = Ocr.Read(Input);
                string iniWord = Result.Text;
                string arg = string.Format(@"C:\Users\greni\Desktop\Projects\Syllable\splitter.py {0}", iniWord);
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo(@"C:\Python\python.exe", arg)
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                p.Start();
                string output = p.StandardOutput.ReadToEnd(); //returns python script results 
                p.WaitForExit();
                label5.Text = output;

            }
        }
    }
}