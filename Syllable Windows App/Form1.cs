using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Tesseract;
using System.Text;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using IronOcr;

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
                button3.Text = "Stop Program";
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
                Bitmap snapshot = new Bitmap(pictureBox1.Image);
                snapshot.Save(string.Format(installLocation + "\\Png.png", System.Drawing.Imaging.ImageFormat.Png, Guid.NewGuid()));
            }
            else
            {
                Console.WriteLine("Cannot take picture if the camera isn't capturing image!");
            }
        }
            private void button1_Click(object sender, EventArgs e)
        {
            string iniPhrase = textBox1.Text;
            string iniPhraseLower = iniPhrase.ToLower();
            string[] iniWords = iniPhraseLower.Split(' ');
            StringBuilder finalResult = new StringBuilder("");
            foreach (string word in iniWords)
            {
                int counter = 0;
                string line;
                StreamReader words = new StreamReader(installLocation + "words.txt");
                while ((line = words.ReadLine()) != null)
                {
                    if (line.Equals(word))
                    {
                        int foundWord = counter;
                        string lines = File.ReadLines(installLocation + "finishedwords.txt").Skip(foundWord).First();
                        finalResult.Append(lines + " ");
                        label1.Text = finalResult.ToString();

                    }
                    counter++;
                }
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            //ignore puncuation
            string iniPhrase = new IronTesseract().Read(installLocation + "Png.png").Text;
            string iniPhraseLower = iniPhrase.ToLower();
            string[] iniWords = iniPhraseLower.Split(' ');
            StringBuilder finalResult = new StringBuilder("");
            foreach (string word in iniWords)
            {
                int counter = 0;
                string line;
                StreamReader words = new StreamReader(installLocation + "words.txt");
                while ((line = words.ReadLine()) != null)
                {
                    if (line.Equals(word))
                    {
                        int foundWord = counter;
                        string lines = File.ReadLines(installLocation + "finishedwords.txt").Skip(foundWord).First();
                        finalResult.Append(lines + " ");
                        label5.Text = finalResult.ToString();
                        counter = 0;

                    } else if (!line.Equals(word) && counter >= 370098)
                    {
                        finalResult.Append(word + " ");
                        Debug.Write(finalResult.ToString());
                        label5.Text = finalResult.ToString();
                    }
                    counter++;
                }
            }

        }
    }
}
            