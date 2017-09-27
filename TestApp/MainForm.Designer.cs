namespace TestApp
{
    partial class MainForm
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
            this.btnRun = new System.Windows.Forms.Button();
            this.rtxtOutput = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(86, 312);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(149, 33);
            this.btnRun.TabIndex = 0;
            this.btnRun.Text = "Get All Prizes Combinations";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // rtxtOutput
            // 
            this.rtxtOutput.Location = new System.Drawing.Point(12, 2);
            this.rtxtOutput.Name = "rtxtOutput";
            this.rtxtOutput.Size = new System.Drawing.Size(299, 304);
            this.rtxtOutput.TabIndex = 5;
            this.rtxtOutput.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(321, 347);
            this.Controls.Add(this.rtxtOutput);
            this.Controls.Add(this.btnRun);
            this.Name = "MainForm";
            this.Text = "Test Application";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.RichTextBox rtxtOutput;
    }
}

