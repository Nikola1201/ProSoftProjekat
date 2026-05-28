using System.Windows.Forms;

namespace Client.UserControls.UCInstruktor
{
    partial class UCObrisiInstruktora
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
            this.cmbInstruktori = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnObrisi = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmbInstruktori
            // 
            this.cmbInstruktori.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbInstruktori.FormattingEnabled = true;
            this.cmbInstruktori.Location = new System.Drawing.Point(48, 67);
            this.cmbInstruktori.Name = "cmbInstruktori";
            this.cmbInstruktori.Size = new System.Drawing.Size(287, 24);
            this.cmbInstruktori.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(45, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(179, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Izaberi instrukora za brisanje:";
            // 
            // btnObrisi
            // 
            this.btnObrisi.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnObrisi.Location = new System.Drawing.Point(119, 114);
            this.btnObrisi.Name = "btnObrisi";
            this.btnObrisi.Size = new System.Drawing.Size(139, 64);
            this.btnObrisi.TabIndex = 2;
            this.btnObrisi.Text = "Obriši Instruktora";
            this.btnObrisi.UseVisualStyleBackColor = true;
            // 
            // UCObrisiInstruktora
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnObrisi);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbInstruktori);
            this.Name = "UCObrisiInstruktora";
            this.Size = new System.Drawing.Size(393, 275);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbInstruktori;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnObrisi;

        public ComboBox CmbInstruktori { get => cmbInstruktori; set => cmbInstruktori = value; }
        public Label Label1 { get => label1; set => label1 = value; }
        public Button BtnObrisi { get => btnObrisi; set => btnObrisi = value; }
    }
}
