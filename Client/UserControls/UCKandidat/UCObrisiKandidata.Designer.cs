using System.Windows.Forms;

namespace Client.UserControls.UCKandidat
{
    partial class UCObrisiKandidata
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnObriši = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmbKandidat
            // 
            this.cmbKandidat.FormattingEnabled = true;
            this.cmbKandidat.Location = new System.Drawing.Point(108, 103);
            this.cmbKandidat.Name = "cmbKandidat";
            this.cmbKandidat.Size = new System.Drawing.Size(153, 21);
            this.cmbKandidat.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 106);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Kandidat:";
            // 
            // btnObriši
            // 
            this.btnObriši.Location = new System.Drawing.Point(78, 149);
            this.btnObriši.Name = "btnObriši";
            this.btnObriši.Size = new System.Drawing.Size(134, 43);
            this.btnObriši.TabIndex = 2;
            this.btnObriši.Text = "Obriši kandidata";
            this.btnObriši.UseVisualStyleBackColor = true;
            // 
            // UCObrisiKandidata
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnObriši);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbKandidat);
            this.Name = "UCObrisiKandidata";
            this.Size = new System.Drawing.Size(322, 289);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbKandidat;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnObriši;

        public ComboBox CmbKandidat { get => cmbKandidat; set => cmbKandidat = value; }
        public Label Label1 { get => label1; set => label1 = value; }
        public Button BtnObrisi { get => btnObriši; set => btnObriši = value; }
    }
}
