﻿// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: SoundDisabler.cs
// 
// Last modified: 2016-07-30 19:37

using System;
using System.IO;
using System.Windows.Forms;
using SoundCenSe.Events;

namespace SoundCenSe.GUI
{
    public partial class SoundDisabler : UserControl
    {
        #region Fields and Constants

        private string filename;
        public EventHandler<DisableSoundEventArgs> SoundDisabled;

        #endregion

        #region Properties

        public string Filename
        {
            get { return filename; }
            set
            {
                filename = value;
                label1.Text = Path.GetFileNameWithoutExtension(filename);
            }
        }

        #endregion

        public SoundDisabler(string filename)
        {
            InitializeComponent();
            Filename = filename;
        }

        public SoundDisabler()
        {
            InitializeComponent();
        }

        private void btnDisableClick(object sender, EventArgs e)
        {
            var handler = SoundDisabled;
            if (handler != null)
            {
                handler(this, new DisableSoundEventArgs(filename, true));
            }
        }
    }
}