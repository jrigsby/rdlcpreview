namespace ReportsApplication1
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
         this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
         this.menuStrip1 = new System.Windows.Forms.MenuStrip();
         this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.assignDataFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.assignReportFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.reloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
         this.menuStrip1.SuspendLayout();
         this.SuspendLayout();
         // 
         // reportViewer1
         // 
         this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.reportViewer1.Location = new System.Drawing.Point(0, 24);
         this.reportViewer1.Name = "reportViewer1";
         this.reportViewer1.ServerReport.BearerToken = null;
         this.reportViewer1.Size = new System.Drawing.Size(682, 362);
         this.reportViewer1.TabIndex = 0;
         // 
         // menuStrip1
         // 
         this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.reloadToolStripMenuItem});
         this.menuStrip1.Location = new System.Drawing.Point(0, 0);
         this.menuStrip1.Name = "menuStrip1";
         this.menuStrip1.Size = new System.Drawing.Size(682, 24);
         this.menuStrip1.TabIndex = 1;
         this.menuStrip1.Text = "menuStrip1";
         // 
         // fileToolStripMenuItem
         // 
         this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.assignDataFileToolStripMenuItem,
            this.assignReportFileToolStripMenuItem});
         this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
         this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
         this.fileToolStripMenuItem.Text = "File";
         // 
         // assignDataFileToolStripMenuItem
         // 
         this.assignDataFileToolStripMenuItem.Name = "assignDataFileToolStripMenuItem";
         this.assignDataFileToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
         this.assignDataFileToolStripMenuItem.Text = "Assign &Data File";
         this.assignDataFileToolStripMenuItem.Click += new System.EventHandler(this.assignDataFileToolStripMenuItem_Click);
         // 
         // assignReportFileToolStripMenuItem
         // 
         this.assignReportFileToolStripMenuItem.Name = "assignReportFileToolStripMenuItem";
         this.assignReportFileToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
         this.assignReportFileToolStripMenuItem.Text = "Assign &Report File";
         this.assignReportFileToolStripMenuItem.Click += new System.EventHandler(this.assignReportFileToolStripMenuItem_Click);
         // 
         // reloadToolStripMenuItem
         // 
         this.reloadToolStripMenuItem.Name = "reloadToolStripMenuItem";
         this.reloadToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
         this.reloadToolStripMenuItem.Text = "Refresh";
         this.reloadToolStripMenuItem.Click += new System.EventHandler(this.reloadToolStripMenuItem_Click);
         // 
         // openFileDialog1
         // 
         this.openFileDialog1.FileName = "openFileDialog1";
         // 
         // Form1
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(682, 386);
         this.Controls.Add(this.reportViewer1);
         this.Controls.Add(this.menuStrip1);
         this.MainMenuStrip = this.menuStrip1;
         this.Name = "Form1";
         this.Text = "RDCL Preview";
         this.Load += new System.EventHandler(this.Form1_Load);
         this.menuStrip1.ResumeLayout(false);
         this.menuStrip1.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
      private System.Windows.Forms.MenuStrip menuStrip1;
      private System.Windows.Forms.ToolStripMenuItem reloadToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem assignDataFileToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem assignReportFileToolStripMenuItem;
      private System.Windows.Forms.OpenFileDialog openFileDialog1;
   }
}

