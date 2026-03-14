using System.Windows.Forms;

namespace Client.UserControls.UCIspit
{
    partial class UCKreirajIzvestajOIspitima
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
            this.dtpDatumOd = new System.Windows.Forms.DateTimePicker();
            this.dtpDatumDo = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbTipIspita = new System.Windows.Forms.ComboBox();
            this.cmbKategorija = new System.Windows.Forms.ComboBox();
            this.cbUtoku = new System.Windows.Forms.CheckBox();
            this.dgvIzvestajIspita = new System.Windows.Forms.DataGridView();
            this.btnKreirajIzvestaj = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtUkupnoUToku = new System.Windows.Forms.TextBox();
            this.txtUkupnoPolozilo = new System.Windows.Forms.TextBox();
            this.txtUkupnoPalo = new System.Windows.Forms.TextBox();
            this.txtProcenatProlaznosti = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIzvestajIspita)).BeginInit();
            this.SuspendLayout();
            // 
            // dtpDatumOd
            // 
            this.dtpDatumOd.Location = new System.Drawing.Point(21, 39);
            this.dtpDatumOd.Margin = new System.Windows.Forms.Padding(4);
            this.dtpDatumOd.Name = "dtpDatumOd";
            this.dtpDatumOd.Size = new System.Drawing.Size(262, 22);
            this.dtpDatumOd.TabIndex = 0;
            // 
            // dtpDatumDo
            // 
            this.dtpDatumDo.Location = new System.Drawing.Point(311, 39);
            this.dtpDatumDo.Margin = new System.Windows.Forms.Padding(4);
            this.dtpDatumDo.Name = "dtpDatumDo";
            this.dtpDatumDo.Size = new System.Drawing.Size(262, 22);
            this.dtpDatumDo.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 19);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Od datuma:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(308, 19);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Do datuma:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 70);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Tip ispita:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(159, 70);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 16);
            this.label4.TabIndex = 5;
            this.label4.Text = "Kategorija:";
            // 
            // cmbTipIspita
            // 
            this.cmbTipIspita.FormattingEnabled = true;
            this.cmbTipIspita.Location = new System.Drawing.Point(21, 89);
            this.cmbTipIspita.Name = "cmbTipIspita";
            this.cmbTipIspita.Size = new System.Drawing.Size(121, 24);
            this.cmbTipIspita.TabIndex = 7;
            // 
            // cmbKategorija
            // 
            this.cmbKategorija.FormattingEnabled = true;
            this.cmbKategorija.Location = new System.Drawing.Point(162, 89);
            this.cmbKategorija.Name = "cmbKategorija";
            this.cmbKategorija.Size = new System.Drawing.Size(121, 24);
            this.cmbKategorija.TabIndex = 8;
            // 
            // cbUtoku
            // 
            this.cbUtoku.AutoSize = true;
            this.cbUtoku.Location = new System.Drawing.Point(311, 91);
            this.cbUtoku.Name = "cbUtoku";
            this.cbUtoku.Size = new System.Drawing.Size(64, 20);
            this.cbUtoku.TabIndex = 9;
            this.cbUtoku.Text = "U toku";
            this.cbUtoku.UseVisualStyleBackColor = true;
            // 
            // dgvIzvestajIspita
            // 
            this.dgvIzvestajIspita.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIzvestajIspita.Location = new System.Drawing.Point(3, 138);
            this.dgvIzvestajIspita.Name = "dgvIzvestajIspita";
            this.dgvIzvestajIspita.Size = new System.Drawing.Size(843, 206);
            this.dgvIzvestajIspita.TabIndex = 10;
            // 
            // btnKreirajIzvestaj
            // 
            this.btnKreirajIzvestaj.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKreirajIzvestaj.Location = new System.Drawing.Point(673, 350);
            this.btnKreirajIzvestaj.Name = "btnKreirajIzvestaj";
            this.btnKreirajIzvestaj.Size = new System.Drawing.Size(173, 63);
            this.btnKreirajIzvestaj.TabIndex = 11;
            this.btnKreirajIzvestaj.Text = "Kreiraj izveštaj";
            this.btnKreirajIzvestaj.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 364);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 16);
            this.label5.TabIndex = 12;
            this.label5.Text = "Ukupno položilo:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(39, 394);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 16);
            this.label6.TabIndex = 13;
            this.label6.Text = "Ukupno palo:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(270, 364);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 16);
            this.label7.TabIndex = 14;
            this.label7.Text = "Ukupno u toku:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(233, 394);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(132, 16);
            this.label8.TabIndex = 15;
            this.label8.Text = "Procenat prolaznosti:";
            // 
            // txtUkupnoUToku
            // 
            this.txtUkupnoUToku.Location = new System.Drawing.Point(371, 358);
            this.txtUkupnoUToku.Name = "txtUkupnoUToku";
            this.txtUkupnoUToku.Size = new System.Drawing.Size(70, 22);
            this.txtUkupnoUToku.TabIndex = 16;
            // 
            // txtUkupnoPolozilo
            // 
            this.txtUkupnoPolozilo.Location = new System.Drawing.Point(131, 358);
            this.txtUkupnoPolozilo.Name = "txtUkupnoPolozilo";
            this.txtUkupnoPolozilo.Size = new System.Drawing.Size(70, 22);
            this.txtUkupnoPolozilo.TabIndex = 17;
            // 
            // txtUkupnoPalo
            // 
            this.txtUkupnoPalo.Location = new System.Drawing.Point(131, 391);
            this.txtUkupnoPalo.Name = "txtUkupnoPalo";
            this.txtUkupnoPalo.Size = new System.Drawing.Size(70, 22);
            this.txtUkupnoPalo.TabIndex = 18;
            // 
            // txtProcenatProlaznosti
            // 
            this.txtProcenatProlaznosti.Location = new System.Drawing.Point(371, 386);
            this.txtProcenatProlaznosti.Name = "txtProcenatProlaznosti";
            this.txtProcenatProlaznosti.Size = new System.Drawing.Size(70, 22);
            this.txtProcenatProlaznosti.TabIndex = 19;
            // 
            // UCKreirajIzvestajOIspitima
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtProcenatProlaznosti);
            this.Controls.Add(this.txtUkupnoPalo);
            this.Controls.Add(this.txtUkupnoPolozilo);
            this.Controls.Add(this.txtUkupnoUToku);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnKreirajIzvestaj);
            this.Controls.Add(this.dgvIzvestajIspita);
            this.Controls.Add(this.cbUtoku);
            this.Controls.Add(this.cmbKategorija);
            this.Controls.Add(this.cmbTipIspita);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtpDatumDo);
            this.Controls.Add(this.dtpDatumOd);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UCKreirajIzvestajOIspitima";
            this.Size = new System.Drawing.Size(849, 446);
            ((System.ComponentModel.ISupportInitialize)(this.dgvIzvestajIspita)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpDatumOd;
        private System.Windows.Forms.DateTimePicker dtpDatumDo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbTipIspita;
        private System.Windows.Forms.ComboBox cmbKategorija;
        private System.Windows.Forms.CheckBox cbUtoku;
        private System.Windows.Forms.DataGridView dgvIzvestajIspita;
        private System.Windows.Forms.Button btnKreirajIzvestaj;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtUkupnoUToku;
        private System.Windows.Forms.TextBox txtUkupnoPolozilo;
        private System.Windows.Forms.TextBox txtUkupnoPalo;
        private System.Windows.Forms.TextBox txtProcenatProlaznosti;

        public DateTimePicker DtpDatumOd { get => dtpDatumOd; set => dtpDatumOd = value; }
        public DateTimePicker DtpDatumDo { get => dtpDatumDo; set => dtpDatumDo = value; }
        public Label Label1 { get => label1; set => label1 = value; }
        public Label Label2 { get => label2; set => label2 = value; }
        public Label Label3 { get => label3; set => label3 = value; }
        public Label Label4 { get => label4; set => label4 = value; }
        public ComboBox CmbTipIspita { get => cmbTipIspita; set => cmbTipIspita = value; }
        public ComboBox CmbKategorija { get => cmbKategorija; set => cmbKategorija = value; }
        public CheckBox CbUtoku { get => cbUtoku; set => cbUtoku = value; }
        public DataGridView DgvIzvestajIspita { get => dgvIzvestajIspita; set => dgvIzvestajIspita = value; }
        public Button BtnKreirajIzvestaj { get => btnKreirajIzvestaj; set => btnKreirajIzvestaj = value; }
        public Label Label5 { get => label5; set => label5 = value; }
        public Label Label6 { get => label6; set => label6 = value; }
        public Label Label7 { get => label7; set => label7 = value; }
        public Label Label8 { get => label8; set => label8 = value; }
        public TextBox TxtUkupnoUToku { get => txtUkupnoUToku; set => txtUkupnoUToku = value; }
        public TextBox TxtUkupnoPolozilo { get => txtUkupnoPolozilo; set => txtUkupnoPolozilo = value; }
        public TextBox TxtUkupnoPalo { get => txtUkupnoPalo; set => txtUkupnoPalo = value; }
        public TextBox TxtProcenatProlaznosti { get => txtProcenatProlaznosti; set => txtProcenatProlaznosti = value; }
    }
}
