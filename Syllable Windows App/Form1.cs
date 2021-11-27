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
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Syllable_Windows_App
{
    public partial class Form1 : Form
    {

        VideoCapture capture;
        Mat frame;
        Bitmap image;
        private Thread camera;
        bool isCameraRunning = false;
        string installLocation = Application.StartupPath;

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private void CaptureCamera()
        {
            camera = new Thread(new ThreadStart(CaptureCameraCallback));
            camera.Start();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.Text.Equals("Start Camera"))
            {
                CaptureCamera();
                button3.Text = "Stop Camera";
                isCameraRunning = true;
            }
            else
            {
                capture.Release();
                button3.Text = "Start";
                isCameraRunning = false;
            }
        }
  
            private void CaptureCameraCallback()
        {
            frame = new Mat();
            capture = new VideoCapture(0);
            capture.Open(0);

            if (capture.IsOpened())
            {
                while (isCameraRunning)
                {

                    capture.Read(frame);
                    image = BitmapConverter.ToBitmap(frame);
                    if (pictureBox1.Image != null)
                    {
                        pictureBox1.Image.Dispose();
                    }
                    pictureBox1.Image = image;
                }
            }
        }


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (isCameraRunning)
            {
                // Take snapshot of the current image generate by OpenCV in the Picture Box
                Bitmap snapshot = new Bitmap(pictureBox1.Image);

                // Save in some directory
                // in this example, we'll generate a random filename e.g 47059681-95ed-4e95-9b50-320092a3d652.png
                // snapshot.Save(@"C:\Users\sdkca\Desktop\mysnapshot.png", ImageFormat.Png);
                snapshot.Save(string.Format(installLocation + "\\Png.png", ImageFormat.Png, Guid.NewGuid()));
            }
            else
            {
                Console.WriteLine("Cannot take picture if the camera isn't capturing image!");
            }
        }
            private void button1_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(installLocation);
            string iniPhrase = textBox1.Text;
            string[] iniWords = iniPhrase.Split(' ');
            StringBuilder finalResult = new StringBuilder("");
            foreach (string word in iniWords)
            {
                int sbLength = iniWords.Length;
                string arg = string.Format(@installLocation + "\\splitter.py {0}", word);
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo(@installLocation + "\\python.exe", arg)
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

            var testImagePath = installLocation + "Png.png";
            try
            {
                using (var engine = new TesseractEngine(@installLocation, "eng", EngineMode.Default))
                {
                    using (var img = Pix.LoadFromFile(testImagePath))
                    {
                        using (var page = engine.Process(img))
                        {
                            string iniPhrase = page.GetText();
                            string[] iniWords = iniPhrase.Split(' ');
                            StringBuilder finalResult = new StringBuilder("");
                            foreach (string word in iniWords)
                            {
                                int sbLength = iniWords.Length;
                                string arg = string.Format(@installLocation + "\\splitter.py {0}", word);
                                Process p = new Process();
                                p.StartInfo = new ProcessStartInfo(@installLocation + "\\Python.exe", arg)
                                {
                                    RedirectStandardOutput = true,
                                    UseShellExecute = false,
                                    CreateNoWindow = true
                                };
                                p.Start();
                                finalResult.Append(p.StandardOutput.ReadToEnd());
                                Debug.WriteLine(finalResult);
                                label5.Text = finalResult.ToString();
                                p.WaitForExit();
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
            