namespace NppWordCount.Forms
{
	partial class WordCountForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.frmCount = new System.Windows.Forms.Panel();
            this.lblCount = new System.Windows.Forms.Label();
            this.timerDelay = new System.Windows.Forms.Timer(this.components);
            this.frmCount.SuspendLayout();
            this.SuspendLayout();
            // 
            // frmCount
            // 
            this.frmCount.BackColor = System.Drawing.SystemColors.Control;
            this.frmCount.Controls.Add(this.lblCount);
            this.frmCount.Location = new System.Drawing.Point(0, 16);
            this.frmCount.Name = "frmCount";
            this.frmCount.Size = new System.Drawing.Size(55, 20);
            this.frmCount.TabIndex = 1;
            // 
            // lblCount
            // 
            this.lblCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCount.Location = new System.Drawing.Point(0, 0);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(55, 20);
            this.lblCount.TabIndex = 0;
            this.lblCount.Text = "label1";
            // 
            // timerDelay
            // 
            this.timerDelay.Interval = 1;
            // 
            // WordCountForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(67, 48);
            this.Controls.Add(this.frmCount);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "WordCountForm";
            this.Text = "SearchForm";
            this.SizeChanged += new System.EventHandler(this.SearchForm_SizeChanged);
            this.frmCount.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.Panel frmCount;
        private System.Windows.Forms.Timer timerDelay;
        internal System.Windows.Forms.Label lblCount;
	}
}