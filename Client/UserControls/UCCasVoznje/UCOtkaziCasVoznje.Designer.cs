using System.Windows.Forms;

namespace Client.UserControls.UCCasVoznje
{
    partial class UCOtkaziCasVoznje
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
            this.dgvCasovi = new System.Windows.Forms.DataGridView();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOtkazi = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCasovi)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvCasovi
            // 
            this.dgvCasovi.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCasovi.Location = new System.Drawing.Point(3, 82);
            this.dgvCasovi.Name = "dgvCasovi";
            this.dgvCasovi.Size = new System.Drawing.Size(537, 224);
            this.dgvCasovi.TabIndex = 0;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker1.Location = new System.Drawing.Point(117, 34);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(194, 20);
            this.dateTimePicker1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Izaberite datum";
            // 
            // btnOtkazi
            // 
            this.btnOtkazi.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOtkazi.Location = new System.Drawing.Point(358, 312);
            this.btnOtkazi.Name = "btnOtkazi";
            this.btnOtkazi.Size = new System.Drawing.Size(182, 53);
            this.btnOtkazi.TabIndex = 1;
            this.btnOtkazi.Text = "Otkaži čas";
            this.btnOtkazi.UseVisualStyleBackColor = true;
            // 
            // UCOtkaziCasVoznje
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnOtkazi);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.dgvCasovi);
            this.Name = "UCOtkaziCasVoznje";
            this.Size = new System.Drawing.Size(543, 383);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCasovi)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvCasovi;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label1;
        private Button btnOtkazi;

        public DataGridView DgvCasovi { get => dgvCasovi; set => dgvCasovi = value; }
        public Button BtnOtkazi { get => btnOtkazi; set => btnOtkazi = value; }
        public DateTimePicker DateTimePicker1 { get => dateTimePicker1; set => dateTimePicker1 = value; }
        public Label Label1 { get => label1; set => label1 = value; }
    }
}
