// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: Program.cs
// 
// Last modified: 2016-07-30 19:37

using System;
using System.Windows.Forms;

namespace SoundCenSe
{
    static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SoundCenSeForm());
        }
    }
}