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

namespace Syllable_Windows_App
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}