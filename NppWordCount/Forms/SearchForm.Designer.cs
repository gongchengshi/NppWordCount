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
            this.frmWordCount = new System.Windows.Forms.Panel();
            this.txtWordCount = new System.Windows.Forms.TextBox();
            this.timerDelay = new System.Windows.Forms.Timer(this.components);
            this.frmWordCount.SuspendLayout();
            this.SuspendLayout();
            // 
            // frmWordCount
            // 
            this.frmWordCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.frmWordCount.BackColor = System.Drawing.SystemColors.Control;
            this.frmWordCount.Controls.Add(this.txtWordCount);
            this.frmWordCount.Location = new System.Drawing.Point(0, 14);
            this.frmWordCount.Name = "frmWordCount";
            this.frmWordCount.Size = new System.Drawing.Size(100, 20);
            this.frmWordCount.TabIndex = 1;
            // 
            // txtWordCount
            // 
            this.txtWordCount.CausesValidation = false;
            this.txtWordCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtWordCount.Location = new System.Drawing.Point(0, 0);
            this.txtWordCount.Name = "txtWordCount";
            this.txtWordCount.ReadOnly = true;
            this.txtWordCount.Size = new System.Drawing.Size(100, 20);
            this.txtWordCount.TabIndex = 0;
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
            this.ClientSize = new System.Drawing.Size(100, 48);
            this.Controls.Add(this.frmWordCount);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "WordCountForm";
            this.Text = "SearchForm";
            this.SizeChanged += new System.EventHandler(this.SearchForm_SizeChanged);
            this.frmWordCount.ResumeLayout(false);
            this.frmWordCount.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.Panel frmWordCount;
        private System.Windows.Forms.Timer timerDelay;
        internal System.Windows.Forms.TextBox txtWordCount;
	}
}