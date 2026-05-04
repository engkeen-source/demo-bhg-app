namespace WinUI
{
    partial class frmInputDialog
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
            this.lblLabel = new System.Windows.Forms.Label();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.btn1 = new System.Windows.Forms.Button();
            this.btn2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblLabel
            // 
            this.lblLabel.AutoSize = true;
            this.lblLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLabel.Location = new System.Drawing.Point(17, 25);
            this.lblLabel.Name = "lblLabel";
            this.lblLabel.Size = new System.Drawing.Size(66, 15);
            this.lblLabel.TabIndex = 3;
            this.lblLabel.Text = "Your Label";
            // 
            // txtInput
            // 
            this.txtInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtInput.Location = new System.Drawing.Point(20, 64);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(263, 21);
            this.txtInput.TabIndex = 0;
            // 
            // btn1
            // 
            this.btn1.Location = new System.Drawing.Point(299, 63);
            this.btn1.Name = "btn1";
            this.btn1.Size = new System.Drawing.Size(116, 32);
            this.btn1.TabIndex = 1;
            this.btn1.Text = "OK";
            this.btn1.UseVisualStyleBackColor = true;
            this.btn1.Click += new System.EventHandler(this.btn1_Click);
            // 
            // btn2
            // 
            this.btn2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn2.Location = new System.Drawing.Point(299, 100);
            this.btn2.Name = "btn2";
            this.btn2.Size = new System.Drawing.Size(116, 32);
            this.btn2.TabIndex = 2;
            this.btn2.Text = "Cancel";
            this.btn2.UseVisualStyleBackColor = true;
            this.btn2.Click += new System.EventHandler(this.btn2_Click);
            // 
            // frmInputDialog
            // 
            this.AcceptButton = this.btn1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn2;
            this.ClientSize = new System.Drawing.Size(435, 153);
            this.Controls.Add(this.btn2);
            this.Controls.Add(this.btn1);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.lblLabel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmInputDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Title";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLabel;
        public System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Button btn1;
        private System.Windows.Forms.Button btn2;
    }
}