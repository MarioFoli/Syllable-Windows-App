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
using Tesseract;
using System.Text;

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
            string iniPhrase = textBox1.Text;
            string[] iniWords = iniPhrase.Split(' ');
            StringBuilder finalResult = new StringBuilder("");
            foreach (string word in iniWords)
            {
                int sbLength = iniWords.Length;
                string arg = string.Format(@"C:\Users\greni\Desktop\Projects\Syllable\splitter.py {0}", word);
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo(@"C:\Python\python.exe", arg)
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                p.Start();
                finalResult.Append(p.StandardOutput.ReadToEnd());
                Debug.WriteLine(finalResult);
                label3.Text = finalResult.ToString();
                p.WaitForExit();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {

            var testImagePath = "C:/Users/greni/Desktop/Projects/Syllable/testimage.png";
            try
            {
                using (var engine = new TesseractEngine(@"C:\Users\greni\Desktop\Projects\Syllable", "eng", EngineMode.Default))
                {
                    using (var img = Pix.LoadFromFile(testImagePath))
                    {
                        using (var page = engine.Process(img))
                        {
                            string iniPhrase = page.GetText();
                            string[] iniWords = iniPhrase.Split(' ');
                            foreach (string word in iniWords)
                            {
                                Console.WriteLine(word);
                                string arg = string.Format(@"C:\Users\greni\Desktop\Projects\Syllable\splitter.py {0}", word);
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(e);
            }
        }
    }
}
            