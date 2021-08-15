namespace SoundCenSe.GUI
{
    partial class SoundPanelEntry
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
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SoundPanelEntry));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelFile = new System.Windows.Forms.Label();
            this.labelLength = new System.Windows.Forms.Label();
            this.labelChannel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.btnMute = new System.Windows.Forms.CheckBox();
            this.btnFastForward = new System.Windows.Forms.Button();
            this.btnDwnUp = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panelControls = new System.Windows.Forms.Panel();
            this.VolumeBar = new System.Windows.Forms.TrackBar();
            this.panelControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VolumeBar)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "File:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Remaining:";
            // 
            // labelFile
            // 
            this.labelFile.AutoSize = true;
            this.labelFile.Location = new System.Drawing.Point(63, 7);
            this.labelFile.Name = "labelFile";
            this.labelFile.Size = new System.Drawing.Size(0, 13);
            this.labelFile.TabIndex = 2;
            // 
            // labelLength
            // 
            this.labelLength.AutoSize = true;
            this.labelLength.Location = new System.Drawing.Point(63, 23);
            this.labelLength.Name = "labelLength";
            this.labelLength.Size = new System.Drawing.Size(0, 13);
            this.labelLength.TabIndex = 3;
            // 
            // labelChannel
            // 
            this.labelChannel.AutoSize = true;
            this.labelChannel.Location = new System.Drawing.Point(63, 44);
            this.labelChannel.Name = "labelChannel";
            this.labelChannel.Size = new System.Drawing.Size(0, 13);
            this.labelChannel.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Channel:";
            // 
            // tablePanel
            // 
            this.tablePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tablePanel.ColumnCount = 1;
            this.tablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tablePanel.Location = new System.Drawing.Point(1, 97);
            this.tablePanel.Name = "tablePanel";
            this.tablePanel.RowCount = 1;
            this.tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tablePanel.Size = new System.Drawing.Size(395, 23);
            this.tablePanel.TabIndex = 10;
            // 
            // btnMute
            // 
            this.btnMute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMute.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnMute.AutoSize = true;
            this.btnMute.Image = global::SoundCenSe.Properties.Resources.Mute;
            this.btnMute.Location = new System.Drawing.Point(51, 46);
            this.btnMute.Name = "btnMute";
            this.btnMute.Size = new System.Drawing.Size(38, 38);
            this.btnMute.TabIndex = 8;
            this.toolTip1.SetToolTip(this.btnMute, "Mute channel");
            this.btnMute.UseVisualStyleBackColor = true;
            this.btnMute.CheckedChanged += new System.EventHandler(this.MuteClick);
            // 
            // btnFastForward
            // 
            this.btnFastForward.Image = global::SoundCenSe.Properties.Resources.FastForward;
            this.btnFastForward.Location = new System.Drawing.Point(51, -1);
            this.btnFastForward.Name = "btnFastForward";
            this.btnFastForward.Size = new System.Drawing.Size(38, 38);
            this.btnFastForward.TabIndex = 6;
            this.toolTip1.SetToolTip(this.btnFastForward, "Next sound");
            this.btnFastForward.UseVisualStyleBackColor = true;
            this.btnFastForward.Visible = false;
            this.btnFastForward.Click += new System.EventHandler(this.FastForwardClick);
            // 
            // btnDwnUp
            // 
            this.btnDwnUp.FlatAppearance.BorderSize = 0;
            this.btnDwnUp.ImageIndex = 0;
            this.btnDwnUp.ImageList = this.imageList1;
            this.btnDwnUp.Location = new System.Drawing.Point(1, 69);
            this.btnDwnUp.Name = "btnDwnUp";
            this.btnDwnUp.Size = new System.Drawing.Size(23, 23);
            this.btnDwnUp.TabIndex = 11;
            this.btnDwnUp.TabStop = false;
            this.toolTip1.SetToolTip(this.btnDwnUp, "Open/close");
            this.btnDwnUp.UseVisualStyleBackColor = true;
            this.btnDwnUp.Click += new System.EventHandler(this.button1_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "ArrowDown.png");
            this.imageList1.Images.SetKeyName(1, "ArrowUp.png");
            // 
            // panelControls
            // 
            this.panelControls.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panelControls.Controls.Add(this.VolumeBar);
            this.panelControls.Controls.Add(this.btnFastForward);
            this.panelControls.Controls.Add(this.btnMute);
            this.panelControls.Location = new System.Drawing.Point(177, 3);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(94, 87);
            this.panelControls.TabIndex = 12;
            // 
            // VolumeBar
            // 
            this.VolumeBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.VolumeBar.Location = new System.Drawing.Point(0, 0);
            this.VolumeBar.Maximum = 100;
            this.VolumeBar.Name = "VolumeBar";
            this.VolumeBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.VolumeBar.Size = new System.Drawing.Size(45, 84);
            this.VolumeBar.TabIndex = 10;
            this.VolumeBar.TickFrequency = 10;
            this.toolTip1.SetToolTip(this.VolumeBar, "Volume");
            // 
            // SoundPanelEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControls);
            this.Controls.Add(this.btnDwnUp);
            this.Controls.Add(this.tablePanel);
            this.Controls.Add(this.labelChannel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.labelLength);
            this.Controls.Add(this.labelFile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "SoundPanelEntry";
            this.Size = new System.Drawing.Size(397, 121);
            this.panelControls.ResumeLayout(false);
            this.panelControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VolumeBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelFile;
        private System.Windows.Forms.Label labelLength;
        private System.Windows.Forms.Label labelChannel;
        private System.Windows.Forms.Label label4;
        internal System.Windows.Forms.CheckBox btnMute;
        internal System.Windows.Forms.Button btnFastForward;
        private System.Windows.Forms.TableLayoutPanel tablePanel;
        private System.Windows.Forms.Button btnDwnUp;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel panelControls;
        internal System.Windows.Forms.TrackBar VolumeBar;
    }
}
