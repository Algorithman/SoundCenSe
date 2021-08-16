using SoundCenSe.GUI;

namespace SoundCenSe
{
    partial class SoundCenSeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                if (LL != null)
                {
                    LL.Dispose();
                }
                if (allSounds != null)
                {
                    allSounds.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SoundCenSeForm));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripSignal1 = new SoundCenSe.GUI.ToolStripSignal();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.Tabs = new System.Windows.Forms.TabControl();
            this.tabPageAudioControl = new System.Windows.Forms.TabPage();
            this.labelThreshold = new System.Windows.Forms.Label();
            this.comboBoxThreshold = new System.Windows.Forms.ComboBox();
            this.soundPanel = new SoundCenSe.GUI.SoundPanel();
            this.tabPageUpdate = new System.Windows.Forms.TabPage();
            this.cbDeleteFiles = new System.Windows.Forms.CheckBox();
            this.cbOverwriteFiles = new System.Windows.Forms.CheckBox();
            this.listBoxUpdateMessages = new System.Windows.Forms.ListBox();
            this.tabPageDisabledSounds = new System.Windows.Forms.TabPage();
            this.lbDisabledSounds = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.reenableSoundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPageConfiguration = new System.Windows.Forms.TabPage();
            this.btnSelectGamelogPath = new System.Windows.Forms.Button();
            this.tbGamelogPath = new System.Windows.Forms.TextBox();
            this.lbGamelogpath = new System.Windows.Forms.Label();
            this.btnChooseFolder = new System.Windows.Forms.Button();
            this.tbSoundPackPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabDebug = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.btnDebug1 = new System.Windows.Forms.Button();
            this.tabPageCredits = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.browseSoundpackPath = new System.Windows.Forms.FolderBrowserDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.statusStrip.SuspendLayout();
            this.Tabs.SuspendLayout();
            this.tabPageAudioControl.SuspendLayout();
            this.tabPageUpdate.SuspendLayout();
            this.tabPageDisabledSounds.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.tabPageConfiguration.SuspendLayout();
            this.tabDebug.SuspendLayout();
            this.tabPageCredits.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1Tick);
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1,
            this.toolStripSignal1});
            this.statusStrip.Location = new System.Drawing.Point(0, 1100);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(2, 0, 21, 0);
            this.statusStrip.Size = new System.Drawing.Size(668, 23);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(629, 16);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(150, 22);
            this.toolStripProgressBar1.Visible = false;
            // 
            // toolStripSignal1
            // 
            this.toolStripSignal1.Name = "toolStripSignal1";
            this.toolStripSignal1.Padding = new System.Windows.Forms.Padding(8);
            this.toolStripSignal1.Signal = false;
            this.toolStripSignal1.Size = new System.Drawing.Size(16, 16);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(12, 9);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(184, 35);
            this.btnUpdate.TabIndex = 1;
            this.btnUpdate.Text = "Update Soundpack";
            this.toolTip1.SetToolTip(this.btnUpdate, "Update the sound pack");
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdateClick);
            // 
            // Tabs
            // 
            this.Tabs.Controls.Add(this.tabPageAudioControl);
            this.Tabs.Controls.Add(this.tabPageUpdate);
            this.Tabs.Controls.Add(this.tabPageDisabledSounds);
            this.Tabs.Controls.Add(this.tabPageConfiguration);
            this.Tabs.Controls.Add(this.tabDebug);
            this.Tabs.Controls.Add(this.tabPageCredits);
            this.Tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tabs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Tabs.Location = new System.Drawing.Point(0, 0);
            this.Tabs.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Tabs.Name = "Tabs";
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new System.Drawing.Size(668, 1100);
            this.Tabs.TabIndex = 3;
            // 
            // tabPageAudioControl
            // 
            this.tabPageAudioControl.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageAudioControl.Controls.Add(this.labelThreshold);
            this.tabPageAudioControl.Controls.Add(this.comboBoxThreshold);
            this.tabPageAudioControl.Controls.Add(this.soundPanel);
            this.tabPageAudioControl.Location = new System.Drawing.Point(4, 29);
            this.tabPageAudioControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageAudioControl.Name = "tabPageAudioControl";
            this.tabPageAudioControl.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageAudioControl.Size = new System.Drawing.Size(660, 1067);
            this.tabPageAudioControl.TabIndex = 0;
            this.tabPageAudioControl.Text = "Audio";
            // 
            // labelThreshold
            // 
            this.labelThreshold.AutoSize = true;
            this.labelThreshold.Location = new System.Drawing.Point(9, 14);
            this.labelThreshold.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelThreshold.Name = "labelThreshold";
            this.labelThreshold.Size = new System.Drawing.Size(88, 20);
            this.labelThreshold.TabIndex = 4;
            this.labelThreshold.Text = "Threshold:";
            // 
            // comboBoxThreshold
            // 
            this.comboBoxThreshold.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxThreshold.FormattingEnabled = true;
            this.comboBoxThreshold.Location = new System.Drawing.Point(106, 9);
            this.comboBoxThreshold.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBoxThreshold.Name = "comboBoxThreshold";
            this.comboBoxThreshold.Size = new System.Drawing.Size(262, 28);
            this.comboBoxThreshold.TabIndex = 3;
            this.comboBoxThreshold.SelectedIndexChanged += new System.EventHandler(this.comboBoxThresholdSelectedIndexChanged);
            // 
            // soundPanel
            // 
            this.soundPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.soundPanel.AutoScroll = true;
            this.soundPanel.Location = new System.Drawing.Point(0, 51);
            this.soundPanel.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.soundPanel.Name = "soundPanel";
            this.soundPanel.Size = new System.Drawing.Size(651, 1009);
            this.soundPanel.TabIndex = 2;
            // 
            // tabPageUpdate
            // 
            this.tabPageUpdate.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageUpdate.Controls.Add(this.cbDeleteFiles);
            this.tabPageUpdate.Controls.Add(this.cbOverwriteFiles);
            this.tabPageUpdate.Controls.Add(this.listBoxUpdateMessages);
            this.tabPageUpdate.Controls.Add(this.btnUpdate);
            this.tabPageUpdate.Location = new System.Drawing.Point(4, 29);
            this.tabPageUpdate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageUpdate.Name = "tabPageUpdate";
            this.tabPageUpdate.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageUpdate.Size = new System.Drawing.Size(660, 1067);
            this.tabPageUpdate.TabIndex = 1;
            this.tabPageUpdate.Text = "Update";
            // 
            // cbDeleteFiles
            // 
            this.cbDeleteFiles.AutoSize = true;
            this.cbDeleteFiles.Location = new System.Drawing.Point(484, 15);
            this.cbDeleteFiles.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbDeleteFiles.Name = "cbDeleteFiles";
            this.cbDeleteFiles.Size = new System.Drawing.Size(120, 24);
            this.cbDeleteFiles.TabIndex = 4;
            this.cbDeleteFiles.Text = "Delete files";
            this.toolTip1.SetToolTip(this.cbDeleteFiles, "Delete files not belonging to the current sound pack");
            this.cbDeleteFiles.UseVisualStyleBackColor = true;
            this.cbDeleteFiles.CheckedChanged += new System.EventHandler(this.cbDeleteFilesCheckedChanged);
            // 
            // cbOverwriteFiles
            // 
            this.cbOverwriteFiles.AutoSize = true;
            this.cbOverwriteFiles.Location = new System.Drawing.Point(280, 15);
            this.cbOverwriteFiles.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbOverwriteFiles.Name = "cbOverwriteFiles";
            this.cbOverwriteFiles.Size = new System.Drawing.Size(205, 24);
            this.cbOverwriteFiles.TabIndex = 3;
            this.cbOverwriteFiles.Text = "Overwrite existing files";
            this.toolTip1.SetToolTip(this.cbOverwriteFiles, "Overwrite existing files if a different \r\nversion is on the update servers");
            this.cbOverwriteFiles.UseVisualStyleBackColor = true;
            this.cbOverwriteFiles.CheckedChanged += new System.EventHandler(this.cbOverwriteFilesCheckedChanged);
            // 
            // listBoxUpdateMessages
            // 
            this.listBoxUpdateMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxUpdateMessages.BackColor = System.Drawing.SystemColors.Control;
            this.listBoxUpdateMessages.FormattingEnabled = true;
            this.listBoxUpdateMessages.ItemHeight = 20;
            this.listBoxUpdateMessages.Location = new System.Drawing.Point(4, 54);
            this.listBoxUpdateMessages.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.listBoxUpdateMessages.Name = "listBoxUpdateMessages";
            this.listBoxUpdateMessages.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.listBoxUpdateMessages.Size = new System.Drawing.Size(644, 984);
            this.listBoxUpdateMessages.TabIndex = 2;
            // 
            // tabPageDisabledSounds
            // 
            this.tabPageDisabledSounds.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageDisabledSounds.Controls.Add(this.lbDisabledSounds);
            this.tabPageDisabledSounds.Location = new System.Drawing.Point(4, 29);
            this.tabPageDisabledSounds.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageDisabledSounds.Name = "tabPageDisabledSounds";
            this.tabPageDisabledSounds.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageDisabledSounds.Size = new System.Drawing.Size(660, 1067);
            this.tabPageDisabledSounds.TabIndex = 2;
            this.tabPageDisabledSounds.Text = "Disabled sounds";
            // 
            // lbDisabledSounds
            // 
            this.lbDisabledSounds.ContextMenuStrip = this.contextMenuStrip1;
            this.lbDisabledSounds.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbDisabledSounds.FormattingEnabled = true;
            this.lbDisabledSounds.ItemHeight = 20;
            this.lbDisabledSounds.Location = new System.Drawing.Point(4, 5);
            this.lbDisabledSounds.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lbDisabledSounds.Name = "lbDisabledSounds";
            this.lbDisabledSounds.Size = new System.Drawing.Size(652, 1057);
            this.lbDisabledSounds.TabIndex = 0;
            this.toolTip1.SetToolTip(this.lbDisabledSounds, "Right click to reenable sound");
            this.lbDisabledSounds.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBox2_MouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reenableSoundToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(211, 36);
            // 
            // reenableSoundToolStripMenuItem
            // 
            this.reenableSoundToolStripMenuItem.Name = "reenableSoundToolStripMenuItem";
            this.reenableSoundToolStripMenuItem.Size = new System.Drawing.Size(210, 32);
            this.reenableSoundToolStripMenuItem.Text = "Reenable sound";
            this.reenableSoundToolStripMenuItem.Click += new System.EventHandler(this.reenableSoundToolStripMenuItem_Click);
            // 
            // tabPageConfiguration
            // 
            this.tabPageConfiguration.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageConfiguration.Controls.Add(this.btnSelectGamelogPath);
            this.tabPageConfiguration.Controls.Add(this.tbGamelogPath);
            this.tabPageConfiguration.Controls.Add(this.lbGamelogpath);
            this.tabPageConfiguration.Controls.Add(this.btnChooseFolder);
            this.tabPageConfiguration.Controls.Add(this.tbSoundPackPath);
            this.tabPageConfiguration.Controls.Add(this.label1);
            this.tabPageConfiguration.Location = new System.Drawing.Point(4, 29);
            this.tabPageConfiguration.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageConfiguration.Name = "tabPageConfiguration";
            this.tabPageConfiguration.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageConfiguration.Size = new System.Drawing.Size(660, 1067);
            this.tabPageConfiguration.TabIndex = 3;
            this.tabPageConfiguration.Text = "Configuration";
            // 
            // btnSelectGamelogPath
            // 
            this.btnSelectGamelogPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectGamelogPath.AutoSize = true;
            this.btnSelectGamelogPath.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSelectGamelogPath.Location = new System.Drawing.Point(612, 62);
            this.btnSelectGamelogPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSelectGamelogPath.Name = "btnSelectGamelogPath";
            this.btnSelectGamelogPath.Size = new System.Drawing.Size(31, 30);
            this.btnSelectGamelogPath.TabIndex = 5;
            this.btnSelectGamelogPath.Text = "...";
            this.btnSelectGamelogPath.UseVisualStyleBackColor = true;
            this.btnSelectGamelogPath.Click += new System.EventHandler(this.button2_Click);
            // 
            // tbGamelogPath
            // 
            this.tbGamelogPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbGamelogPath.Enabled = false;
            this.tbGamelogPath.Location = new System.Drawing.Point(154, 65);
            this.tbGamelogPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbGamelogPath.Name = "tbGamelogPath";
            this.tbGamelogPath.Size = new System.Drawing.Size(442, 26);
            this.tbGamelogPath.TabIndex = 4;
            // 
            // lbGamelogpath
            // 
            this.lbGamelogpath.AutoSize = true;
            this.lbGamelogpath.Location = new System.Drawing.Point(12, 69);
            this.lbGamelogpath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbGamelogpath.Name = "lbGamelogpath";
            this.lbGamelogpath.Size = new System.Drawing.Size(118, 20);
            this.lbGamelogpath.TabIndex = 3;
            this.lbGamelogpath.Text = "Gamelog path:";
            // 
            // btnChooseFolder
            // 
            this.btnChooseFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChooseFolder.AutoSize = true;
            this.btnChooseFolder.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnChooseFolder.Location = new System.Drawing.Point(612, 15);
            this.btnChooseFolder.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnChooseFolder.Name = "btnChooseFolder";
            this.btnChooseFolder.Size = new System.Drawing.Size(31, 30);
            this.btnChooseFolder.TabIndex = 2;
            this.btnChooseFolder.Text = "...";
            this.btnChooseFolder.UseVisualStyleBackColor = true;
            this.btnChooseFolder.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbSoundPackPath
            // 
            this.tbSoundPackPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSoundPackPath.Enabled = false;
            this.tbSoundPackPath.Location = new System.Drawing.Point(154, 18);
            this.tbSoundPackPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbSoundPackPath.Name = "tbSoundPackPath";
            this.tbSoundPackPath.Size = new System.Drawing.Size(442, 26);
            this.tbSoundPackPath.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Soundpack path:";
            // 
            // tabDebug
            // 
            this.tabDebug.BackColor = System.Drawing.SystemColors.Control;
            this.tabDebug.Controls.Add(this.button1);
            this.tabDebug.Controls.Add(this.btnDebug1);
            this.tabDebug.Location = new System.Drawing.Point(4, 29);
            this.tabDebug.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabDebug.Name = "tabDebug";
            this.tabDebug.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabDebug.Size = new System.Drawing.Size(660, 1067);
            this.tabDebug.TabIndex = 4;
            this.tabDebug.Text = "Debug";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(196, 9);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 35);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // btnDebug1
            // 
            this.btnDebug1.Location = new System.Drawing.Point(12, 9);
            this.btnDebug1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnDebug1.Name = "btnDebug1";
            this.btnDebug1.Size = new System.Drawing.Size(176, 35);
            this.btnDebug1.TabIndex = 0;
            this.btnDebug1.Text = "Longtime gamelog";
            this.btnDebug1.UseVisualStyleBackColor = true;
            this.btnDebug1.Click += new System.EventHandler(this.btnDebug1_Click);
            // 
            // tabPageCredits
            // 
            this.tabPageCredits.Controls.Add(this.label4);
            this.tabPageCredits.Controls.Add(this.label3);
            this.tabPageCredits.Controls.Add(this.label2);
            this.tabPageCredits.Location = new System.Drawing.Point(4, 29);
            this.tabPageCredits.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageCredits.Name = "tabPageCredits";
            this.tabPageCredits.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageCredits.Size = new System.Drawing.Size(660, 1067);
            this.tabPageCredits.TabIndex = 5;
            this.tabPageCredits.Text = "Credits";
            this.tabPageCredits.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 149);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(292, 20);
            this.label4.TabIndex = 2;
            this.label4.Text = "ZweiStein for the original SoundSense";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 58);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(346, 20);
            this.label3.TabIndex = 1;
            this.label3.Text = "Toady One and ThreeToe  for Dwarf Fortress";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 240);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(413, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Audio engine : FMOD Studio by Firelight Technologies";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "gamelog.txt";
            this.openFileDialog1.Filter = "Gamelog|gamelog.txt";
            // 
            // SoundCenSeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 1123);
            this.Controls.Add(this.Tabs);
            this.Controls.Add(this.statusStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "SoundCenSeForm";
            this.Text = "SoundCenSe";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SoundCenSeForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SoundCenSeForm_FormClosed);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.Tabs.ResumeLayout(false);
            this.tabPageAudioControl.ResumeLayout(false);
            this.tabPageAudioControl.PerformLayout();
            this.tabPageUpdate.ResumeLayout(false);
            this.tabPageUpdate.PerformLayout();
            this.tabPageDisabledSounds.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabPageConfiguration.ResumeLayout(false);
            this.tabPageConfiguration.PerformLayout();
            this.tabDebug.ResumeLayout(false);
            this.tabPageCredits.ResumeLayout(false);
            this.tabPageCredits.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.Button btnUpdate;
        private GUI.SoundPanel soundPanel;
        private GUI.ToolStripSignal toolStripSignal1;
        private System.Windows.Forms.TabControl Tabs;
        private System.Windows.Forms.TabPage tabPageAudioControl;
        private System.Windows.Forms.TabPage tabPageUpdate;
        private System.Windows.Forms.ListBox listBoxUpdateMessages;
        private System.Windows.Forms.TabPage tabPageDisabledSounds;
        private System.Windows.Forms.ListBox lbDisabledSounds;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem reenableSoundToolStripMenuItem;
        private System.Windows.Forms.CheckBox cbDeleteFiles;
        private System.Windows.Forms.CheckBox cbOverwriteFiles;
        private System.Windows.Forms.Label labelThreshold;
        private System.Windows.Forms.ComboBox comboBoxThreshold;
        private System.Windows.Forms.TabPage tabPageConfiguration;
        private System.Windows.Forms.Button btnChooseFolder;
        private System.Windows.Forms.TextBox tbSoundPackPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog browseSoundpackPath;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TabPage tabDebug;
        private System.Windows.Forms.Button btnDebug1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnSelectGamelogPath;
        private System.Windows.Forms.TextBox tbGamelogPath;
        private System.Windows.Forms.Label lbGamelogpath;
        private System.Windows.Forms.TabPage tabPageCredits;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
    }
}

