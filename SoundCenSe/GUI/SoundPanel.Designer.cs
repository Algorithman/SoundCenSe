namespace SoundCenSe.GUI
{
    partial class SoundPanel
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
            this.tablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // tablePanel
            // 
            this.tablePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tablePanel.AutoSize = true;
            this.tablePanel.ColumnCount = 1;
            this.tablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tablePanel.Location = new System.Drawing.Point(0, 0);
            this.tablePanel.Name = "tablePanel";
            this.tablePanel.RowCount = 1;
            this.tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tablePanel.Size = new System.Drawing.Size(284, 116);
            this.tablePanel.TabIndex = 0;
            // 
            // SoundPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.tablePanel);
            this.Name = "SoundPanel";
            this.Size = new System.Drawing.Size(284, 362);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tablePanel;
    }
}
