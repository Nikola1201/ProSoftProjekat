using System.Windows.Forms;

namespace Client.UserControls.UCKandidat
{
    partial class UCUpisiKandidata
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
            this.cmbKandidat = new System.Windows.Forms.ComboBox();
            this.cmbPaketObuke = new System.Windows.Forms.ComboBox();
            this.btnUpisi = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmbKandidat
            // 
            this.cmbKandidat.FormattingEnabled = true;
            this.cmbKandidat.Location = new System.Drawing.Point(113, 105);
            this.cmbKandidat.Name = "cmbKandidat";
            this.cmbKandidat.Size = new System.Drawing.Size(251, 21);
            this.cmbKandidat.TabIndex = 0;
            // 
            // cmbPaketObuke
            // 
            this.cmbPaketObuke.FormattingEnabled = true;
            this.cmbPaketObuke.Location = new System.Drawing.Point(113, 132);
            this.cmbPaketObuke.Name = "cmbPaketObuke";
            this.cmbPaketObuke.Size = new System.Drawing.Size(251, 21);
            this.cmbPaketObuke.TabIndex = 1;
            // 
            // btnUpisi
            // 
            this.btnUpisi.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpisi.Location = new System.Drawing.Point(102, 184);
            this.btnUpisi.Name = "btnUpisi";
            this.btnUpisi.Size = new System.Drawing.Size(229, 58);
            this.btnUpisi.TabIndex = 2;
            this.btnUpisi.Text = "Upisi";
            this.btnUpisi.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(58, 108);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Kandidat";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 135);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Paket Obuke";
            // 
            // UCUpisiKandidata
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnUpisi);
            this.Controls.Add(this.cmbPaketObuke);
            this.Controls.Add(this.cmbKandidat);
            this.Name = "UCUpisiKandidata";
            this.Size = new System.Drawing.Size(440, 367);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbKandidat;
        private System.Windows.Forms.ComboBox cmbPaketObuke;
        private System.Windows.Forms.Button btnUpisi;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;

        public ComboBox CmbKandidat { get => cmbKandidat; set => cmbKandidat = value; }
        public ComboBox CmbPaketObuke { get => cmbPaketObuke; set => cmbPaketObuke = value; }
        public Button BtnUpisi { get => btnUpisi; set => btnUpisi = value; }
        public Label Label1 { get => label1; set => label1 = value; }
        public Label Label2 { get => label2; set => label2 = value; }
    }
}
