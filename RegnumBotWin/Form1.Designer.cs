namespace RegnumBotWin
{
    partial class Form1
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
            this.Consola = new System.Windows.Forms.TextBox();
            this.CoordenadasImg = new System.Windows.Forms.PictureBox();
            this.coordenadasText = new System.Windows.Forms.TextBox();
            this.VidaImg = new System.Windows.Forms.PictureBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.CoordenadasImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VidaImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Consola
            // 
            this.Consola.Enabled = false;
            this.Consola.Location = new System.Drawing.Point(12, 12);
            this.Consola.Multiline = true;
            this.Consola.Name = "Consola";
            this.Consola.Size = new System.Drawing.Size(305, 473);
            this.Consola.TabIndex = 0;
            // 
            // CoordenadasImg
            // 
            this.CoordenadasImg.Location = new System.Drawing.Point(323, 12);
            this.CoordenadasImg.Name = "CoordenadasImg";
            this.CoordenadasImg.Size = new System.Drawing.Size(286, 200);
            this.CoordenadasImg.TabIndex = 1;
            this.CoordenadasImg.TabStop = false;
            // 
            // coordenadasText
            // 
            this.coordenadasText.Enabled = false;
            this.coordenadasText.Location = new System.Drawing.Point(323, 218);
            this.coordenadasText.Multiline = true;
            this.coordenadasText.Name = "coordenadasText";
            this.coordenadasText.Size = new System.Drawing.Size(286, 89);
            this.coordenadasText.TabIndex = 2;
            // 
            // VidaImg
            // 
            this.VidaImg.Location = new System.Drawing.Point(323, 313);
            this.VidaImg.Name = "VidaImg";
            this.VidaImg.Size = new System.Drawing.Size(286, 200);
            this.VidaImg.TabIndex = 3;
            this.VidaImg.TabStop = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 491);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(305, 22);
            this.textBox1.TabIndex = 4;
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(641, 29);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(734, 383);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 577);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.VidaImg);
            this.Controls.Add(this.coordenadasText);
            this.Controls.Add(this.CoordenadasImg);
            this.Controls.Add(this.Consola);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CoordenadasImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VidaImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Consola;
        private System.Windows.Forms.PictureBox CoordenadasImg;
        private System.Windows.Forms.TextBox coordenadasText;
        private System.Windows.Forms.PictureBox VidaImg;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

