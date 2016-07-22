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
                if (PM != null)
                {
                    PM.Dispose();
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
            this.btnUpdate = new System.Windows.Forms.Button();
            this.Tabs = new System.Windows.Forms.TabControl();
            this.tabPageAudioControl = new System.Windows.Forms.TabPage();
            this.labelThreshold = new System.Windows.Forms.Label();
            this.comboBoxThreshold = new System.Windows.Forms.ComboBox();
            this.tabPageUpdate = new System.Windows.Forms.TabPage();
            this.cbDeleteFiles = new System.Windows.Forms.CheckBox();
            this.cbOverwriteFiles = new System.Windows.Forms.CheckBox();
            this.listBoxUpdateMessages = new System.Windows.Forms.ListBox();
            this.tabPageDisabledSounds = new System.Windows.Forms.TabPage();
            this.lbDisabledSounds = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.reenableSoundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPageConfiguration = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.tbSoundPackPath = new System.Windows.Forms.TextBox();
            this.btnChooseFolder = new System.Windows.Forms.Button();
            this.browseSoundpackPath = new System.Windows.Forms.FolderBrowserDialog();
            this.soundPanel = new SoundCenSe.GUI.SoundPanel();
            this.toolStripSignal1 = new SoundCenSe.GUI.ToolStripSignal();
            this.statusStrip.SuspendLayout();
            this.Tabs.SuspendLayout();
            this.tabPageAudioControl.SuspendLayout();
            this.tabPageUpdate.SuspendLayout();
            this.tabPageDisabledSounds.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.tabPageConfiguration.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1Tick);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1,
            this.toolStripSignal1});
            this.statusStrip.Location = new System.Drawing.Point(0, 474);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(803, 22);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(772, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar1.Visible = false;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(8, 6);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(123, 23);
            this.btnUpdate.TabIndex = 1;
            this.btnUpdate.Text = "Update Soundpack";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdateClick);
            // 
            // Tabs
            // 
            this.Tabs.Controls.Add(this.tabPageAudioControl);
            this.Tabs.Controls.Add(this.tabPageUpdate);
            this.Tabs.Controls.Add(this.tabPageDisabledSounds);
            this.Tabs.Controls.Add(this.tabPageConfiguration);
            this.Tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tabs.Location = new System.Drawing.Point(0, 0);
            this.Tabs.Name = "Tabs";
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new System.Drawing.Size(803, 474);
            this.Tabs.TabIndex = 3;
            // 
            // tabPageAudioControl
            // 
            this.tabPageAudioControl.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageAudioControl.Controls.Add(this.labelThreshold);
            this.tabPageAudioControl.Controls.Add(this.comboBoxThreshold);
            this.tabPageAudioControl.Controls.Add(this.soundPanel);
            this.tabPageAudioControl.Location = new System.Drawing.Point(4, 22);
            this.tabPageAudioControl.Name = "tabPageAudioControl";
            this.tabPageAudioControl.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAudioControl.Size = new System.Drawing.Size(795, 448);
            this.tabPageAudioControl.TabIndex = 0;
            this.tabPageAudioControl.Text = "Audio";
            // 
            // labelThreshold
            // 
            this.labelThreshold.AutoSize = true;
            this.labelThreshold.Location = new System.Drawing.Point(6, 9);
            this.labelThreshold.Name = "labelThreshold";
            this.labelThreshold.Size = new System.Drawing.Size(57, 13);
            this.labelThreshold.TabIndex = 4;
            this.labelThreshold.Text = "Threshold:";
            // 
            // comboBoxThreshold
            // 
            this.comboBoxThreshold.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxThreshold.FormattingEnabled = true;
            this.comboBoxThreshold.Location = new System.Drawing.Point(71, 6);
            this.comboBoxThreshold.Name = "comboBoxThreshold";
            this.comboBoxThreshold.Size = new System.Drawing.Size(176, 21);
            this.comboBoxThreshold.TabIndex = 3;
            this.comboBoxThreshold.SelectedIndexChanged += new System.EventHandler(this.comboBoxThresholdSelectedIndexChanged);
            // 
            // tabPageUpdate
            // 
            this.tabPageUpdate.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageUpdate.Controls.Add(this.cbDeleteFiles);
            this.tabPageUpdate.Controls.Add(this.cbOverwriteFiles);
            this.tabPageUpdate.Controls.Add(this.listBoxUpdateMessages);
            this.tabPageUpdate.Controls.Add(this.btnUpdate);
            this.tabPageUpdate.Location = new System.Drawing.Point(4, 22);
            this.tabPageUpdate.Name = "tabPageUpdate";
            this.tabPageUpdate.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUpdate.Size = new System.Drawing.Size(795, 448);
            this.tabPageUpdate.TabIndex = 1;
            this.tabPageUpdate.Text = "Update";
            // 
            // cbDeleteFiles
            // 
            this.cbDeleteFiles.AutoSize = true;
            this.cbDeleteFiles.Location = new System.Drawing.Point(371, 10);
            this.cbDeleteFiles.Name = "cbDeleteFiles";
            this.cbDeleteFiles.Size = new System.Drawing.Size(78, 17);
            this.cbDeleteFiles.TabIndex = 4;
            this.cbDeleteFiles.Text = "Delete files";
            this.cbDeleteFiles.UseVisualStyleBackColor = true;
            this.cbDeleteFiles.CheckedChanged += new System.EventHandler(this.cbDeleteFilesCheckedChanged);
            // 
            // cbOverwriteFiles
            // 
            this.cbOverwriteFiles.AutoSize = true;
            this.cbOverwriteFiles.Location = new System.Drawing.Point(187, 10);
            this.cbOverwriteFiles.Name = "cbOverwriteFiles";
            this.cbOverwriteFiles.Size = new System.Drawing.Size(130, 17);
            this.cbOverwriteFiles.TabIndex = 3;
            this.cbOverwriteFiles.Text = "Overwrite existing files";
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
            this.listBoxUpdateMessages.Location = new System.Drawing.Point(3, 35);
            this.listBoxUpdateMessages.Name = "listBoxUpdateMessages";
            this.listBoxUpdateMessages.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.listBoxUpdateMessages.Size = new System.Drawing.Size(789, 407);
            this.listBoxUpdateMessages.TabIndex = 2;
            // 
            // tabPageDisabledSounds
            // 
            this.tabPageDisabledSounds.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageDisabledSounds.Controls.Add(this.lbDisabledSounds);
            this.tabPageDisabledSounds.Location = new System.Drawing.Point(4, 22);
            this.tabPageDisabledSounds.Name = "tabPageDisabledSounds";
            this.tabPageDisabledSounds.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDisabledSounds.Size = new System.Drawing.Size(795, 448);
            this.tabPageDisabledSounds.TabIndex = 2;
            this.tabPageDisabledSounds.Text = "Disabled sounds";
            // 
            // lbDisabledSounds
            // 
            this.lbDisabledSounds.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbDisabledSounds.ContextMenuStrip = this.contextMenuStrip1;
            this.lbDisabledSounds.FormattingEnabled = true;
            this.lbDisabledSounds.Location = new System.Drawing.Point(0, 0);
            this.lbDisabledSounds.Name = "lbDisabledSounds";
            this.lbDisabledSounds.Size = new System.Drawing.Size(792, 446);
            this.lbDisabledSounds.TabIndex = 0;
            this.lbDisabledSounds.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBox2_MouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reenableSoundToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(159, 26);
            // 
            // reenableSoundToolStripMenuItem
            // 
            this.reenableSoundToolStripMenuItem.Name = "reenableSoundToolStripMenuItem";
            this.reenableSoundToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.reenableSoundToolStripMenuItem.Text = "Reenable sound";
            this.reenableSoundToolStripMenuItem.Click += new System.EventHandler(this.reenableSoundToolStripMenuItem_Click);
            // 
            // tabPageConfiguration
            // 
            this.tabPageConfiguration.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageConfiguration.Controls.Add(this.btnChooseFolder);
            this.tabPageConfiguration.Controls.Add(this.tbSoundPackPath);
            this.tabPageConfiguration.Controls.Add(this.label1);
            this.tabPageConfiguration.Location = new System.Drawing.Point(4, 22);
            this.tabPageConfiguration.Name = "tabPageConfiguration";
            this.tabPageConfiguration.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageConfiguration.Size = new System.Drawing.Size(795, 448);
            this.tabPageConfiguration.TabIndex = 3;
            this.tabPageConfiguration.Text = "Configuration";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Soundpack path:";
            // 
            // tbSoundPackPath
            // 
            this.tbSoundPackPath.Enabled = false;
            this.tbSoundPackPath.Location = new System.Drawing.Point(103, 12);
            this.tbSoundPackPath.Name = "tbSoundPackPath";
            this.tbSoundPackPath.Size = new System.Drawing.Size(509, 20);
            this.tbSoundPackPath.TabIndex = 1;
            // 
            // btnChooseFolder
            // 
            this.btnChooseFolder.AutoSize = true;
            this.btnChooseFolder.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnChooseFolder.Location = new System.Drawing.Point(618, 10);
            this.btnChooseFolder.Name = "btnChooseFolder";
            this.btnChooseFolder.Size = new System.Drawing.Size(26, 23);
            this.btnChooseFolder.TabIndex = 2;
            this.btnChooseFolder.Text = "...";
            this.btnChooseFolder.UseVisualStyleBackColor = true;
            this.btnChooseFolder.Click += new System.EventHandler(this.button1_Click);
            // 
            // soundPanel
            // 
            this.soundPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.soundPanel.AutoScroll = true;
            this.soundPanel.Location = new System.Drawing.Point(0, 33);
            this.soundPanel.Name = "soundPanel";
            this.soundPanel.Size = new System.Drawing.Size(792, 415);
            this.soundPanel.TabIndex = 2;
            // 
            // toolStripSignal1
            // 
            this.toolStripSignal1.Name = "toolStripSignal1";
            this.toolStripSignal1.Padding = new System.Windows.Forms.Padding(8);
            this.toolStripSignal1.Signal = false;
            this.toolStripSignal1.Size = new System.Drawing.Size(16, 16);
            // 
            // SoundCenSeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(803, 496);
            this.Controls.Add(this.Tabs);
            this.Controls.Add(this.statusStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
    }
}

