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
            this.btnPresetMesec = new System.Windows.Forms.Button();
            this.btnPresetTridesetDana = new System.Windows.Forms.Button();
            this.btnPresetGodina = new System.Windows.Forms.Button();
            this.lblOdDatuma = new System.Windows.Forms.Label();
            this.lblDoDatuma = new System.Windows.Forms.Label();
            this.lblTipIspitaText = new System.Windows.Forms.Label();
            this.lblKategorijaText = new System.Windows.Forms.Label();
            this.cmbTipIspita = new System.Windows.Forms.ComboBox();
            this.cmbKategorija = new System.Windows.Forms.ComboBox();
            this.chbUkljuciBezRezultata = new System.Windows.Forms.CheckBox();
            this.chbSamoAktivniUpisi = new System.Windows.Forms.CheckBox();
            this.dgvIzvestajIspita = new System.Windows.Forms.DataGridView();
            this.lblNemaRezultata = new System.Windows.Forms.Label();
            this.lblUkupnoKandidata = new System.Windows.Forms.Label();
            this.btnKreirajIzvestaj = new System.Windows.Forms.Button();
            this.btnIzveziCsv = new System.Windows.Forms.Button();
            this.lblPoloziloText = new System.Windows.Forms.Label();
            this.lblPaloText = new System.Windows.Forms.Label();
            this.lblUTokuText = new System.Windows.Forms.Label();
            this.lblProcenatText = new System.Windows.Forms.Label();
            this.lblVrednostUToku = new System.Windows.Forms.Label();
            this.lblVrednostPolozilo = new System.Windows.Forms.Label();
            this.lblVrednostPalo = new System.Windows.Forms.Label();
            this.lblVrednostProcenat = new System.Windows.Forms.Label();
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
            // btnPresetMesec
            //
            this.btnPresetMesec.Location = new System.Drawing.Point(581, 19);
            this.btnPresetMesec.Name = "btnPresetMesec";
            this.btnPresetMesec.Size = new System.Drawing.Size(130, 22);
            this.btnPresetMesec.TabIndex = 24;
            this.btnPresetMesec.Text = "Ovaj mesec";
            this.btnPresetMesec.UseVisualStyleBackColor = true;
            //
            // btnPresetTridesetDana
            //
            this.btnPresetTridesetDana.Location = new System.Drawing.Point(581, 45);
            this.btnPresetTridesetDana.Name = "btnPresetTridesetDana";
            this.btnPresetTridesetDana.Size = new System.Drawing.Size(130, 22);
            this.btnPresetTridesetDana.TabIndex = 25;
            this.btnPresetTridesetDana.Text = "Poslednjih 30 dana";
            this.btnPresetTridesetDana.UseVisualStyleBackColor = true;
            //
            // btnPresetGodina
            //
            this.btnPresetGodina.Location = new System.Drawing.Point(581, 71);
            this.btnPresetGodina.Name = "btnPresetGodina";
            this.btnPresetGodina.Size = new System.Drawing.Size(130, 22);
            this.btnPresetGodina.TabIndex = 26;
            this.btnPresetGodina.Text = "Ova godina";
            this.btnPresetGodina.UseVisualStyleBackColor = true;
            // 
            // lblOdDatuma
            // 
            this.lblOdDatuma.AutoSize = true;
            this.lblOdDatuma.Location = new System.Drawing.Point(18, 19);
            this.lblOdDatuma.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOdDatuma.Name = "lblOdDatuma";
            this.lblOdDatuma.Size = new System.Drawing.Size(76, 16);
            this.lblOdDatuma.TabIndex = 2;
            this.lblOdDatuma.Text = "Od datuma:";
            // 
            // lblDoDatuma
            // 
            this.lblDoDatuma.AutoSize = true;
            this.lblDoDatuma.Location = new System.Drawing.Point(308, 19);
            this.lblDoDatuma.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDoDatuma.Name = "lblDoDatuma";
            this.lblDoDatuma.Size = new System.Drawing.Size(76, 16);
            this.lblDoDatuma.TabIndex = 3;
            this.lblDoDatuma.Text = "Do datuma:";
            // 
            // lblTipIspitaText
            // 
            this.lblTipIspitaText.AutoSize = true;
            this.lblTipIspitaText.Location = new System.Drawing.Point(18, 70);
            this.lblTipIspitaText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTipIspitaText.Name = "lblTipIspitaText";
            this.lblTipIspitaText.Size = new System.Drawing.Size(65, 16);
            this.lblTipIspitaText.TabIndex = 4;
            this.lblTipIspitaText.Text = "Tip ispita:";
            // 
            // lblKategorijaText
            // 
            this.lblKategorijaText.AutoSize = true;
            this.lblKategorijaText.Location = new System.Drawing.Point(159, 70);
            this.lblKategorijaText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblKategorijaText.Name = "lblKategorijaText";
            this.lblKategorijaText.Size = new System.Drawing.Size(71, 16);
            this.lblKategorijaText.TabIndex = 5;
            this.lblKategorijaText.Text = "Kategorija:";
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
            // chbUkljuciBezRezultata
            //
            this.chbUkljuciBezRezultata.AutoSize = true;
            this.chbUkljuciBezRezultata.Location = new System.Drawing.Point(311, 91);
            this.chbUkljuciBezRezultata.Name = "chbUkljuciBezRezultata";
            this.chbUkljuciBezRezultata.Size = new System.Drawing.Size(257, 20);
            this.chbUkljuciBezRezultata.TabIndex = 9;
            this.chbUkljuciBezRezultata.Text = "Uključi i kandidate bez rezultata";
            this.chbUkljuciBezRezultata.UseVisualStyleBackColor = true;
            //
            // chbSamoAktivniUpisi
            //
            this.chbSamoAktivniUpisi.AutoSize = true;
            this.chbSamoAktivniUpisi.Location = new System.Drawing.Point(581, 91);
            this.chbSamoAktivniUpisi.Name = "chbSamoAktivniUpisi";
            this.chbSamoAktivniUpisi.Size = new System.Drawing.Size(160, 20);
            this.chbSamoAktivniUpisi.TabIndex = 22;
            this.chbSamoAktivniUpisi.Text = "Samo aktivni upisi";
            this.chbSamoAktivniUpisi.UseVisualStyleBackColor = true;
            //
            // dgvIzvestajIspita
            //
            this.dgvIzvestajIspita.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvIzvestajIspita.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIzvestajIspita.Location = new System.Drawing.Point(3, 138);
            this.dgvIzvestajIspita.Name = "dgvIzvestajIspita";
            this.dgvIzvestajIspita.Size = new System.Drawing.Size(843, 206);
            this.dgvIzvestajIspita.TabIndex = 10;
            //
            // lblNemaRezultata
            //
            this.lblNemaRezultata.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblNemaRezultata.AutoSize = true;
            this.lblNemaRezultata.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNemaRezultata.ForeColor = System.Drawing.Color.Gray;
            this.lblNemaRezultata.Location = new System.Drawing.Point(330, 230);
            this.lblNemaRezultata.Name = "lblNemaRezultata";
            this.lblNemaRezultata.Size = new System.Drawing.Size(200, 18);
            this.lblNemaRezultata.TabIndex = 20;
            this.lblNemaRezultata.Text = "Nema rezultata za zadate kriterijume.";
            this.lblNemaRezultata.Visible = false;
            //
            // lblUkupnoKandidata
            //
            this.lblUkupnoKandidata.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblUkupnoKandidata.AutoSize = true;
            this.lblUkupnoKandidata.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUkupnoKandidata.Location = new System.Drawing.Point(19, 346);
            this.lblUkupnoKandidata.Name = "lblUkupnoKandidata";
            this.lblUkupnoKandidata.Size = new System.Drawing.Size(150, 15);
            this.lblUkupnoKandidata.TabIndex = 23;
            this.lblUkupnoKandidata.Text = "Prikazano kandidata: 0";
            //
            // btnKreirajIzvestaj
            //
            this.btnKreirajIzvestaj.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKreirajIzvestaj.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKreirajIzvestaj.Location = new System.Drawing.Point(673, 350);
            this.btnKreirajIzvestaj.Name = "btnKreirajIzvestaj";
            this.btnKreirajIzvestaj.Size = new System.Drawing.Size(173, 63);
            this.btnKreirajIzvestaj.TabIndex = 11;
            this.btnKreirajIzvestaj.Text = "Kreiraj izveštaj";
            this.btnKreirajIzvestaj.UseVisualStyleBackColor = true;
            //
            // btnIzveziCsv
            //
            this.btnIzveziCsv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIzveziCsv.Enabled = false;
            this.btnIzveziCsv.Location = new System.Drawing.Point(534, 365);
            this.btnIzveziCsv.Name = "btnIzveziCsv";
            this.btnIzveziCsv.Size = new System.Drawing.Size(133, 33);
            this.btnIzveziCsv.TabIndex = 21;
            this.btnIzveziCsv.Text = "Izvezi u CSV";
            this.btnIzveziCsv.UseVisualStyleBackColor = true;
            //
            // lblPoloziloText
            //
            this.lblPoloziloText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblPoloziloText.AutoSize = true;
            this.lblPoloziloText.Location = new System.Drawing.Point(19, 364);
            this.lblPoloziloText.Name = "lblPoloziloText";
            this.lblPoloziloText.Size = new System.Drawing.Size(107, 16);
            this.lblPoloziloText.TabIndex = 12;
            this.lblPoloziloText.Text = "Ukupno položilo:";
            //
            // lblPaloText
            //
            this.lblPaloText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblPaloText.AutoSize = true;
            this.lblPaloText.Location = new System.Drawing.Point(39, 394);
            this.lblPaloText.Name = "lblPaloText";
            this.lblPaloText.Size = new System.Drawing.Size(87, 16);
            this.lblPaloText.TabIndex = 13;
            this.lblPaloText.Text = "Ukupno palo:";
            //
            // lblUTokuText
            //
            this.lblUTokuText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblUTokuText.AutoSize = true;
            this.lblUTokuText.Location = new System.Drawing.Point(270, 364);
            this.lblUTokuText.Name = "lblUTokuText";
            this.lblUTokuText.Size = new System.Drawing.Size(95, 16);
            this.lblUTokuText.TabIndex = 14;
            this.lblUTokuText.Text = "Ukupno u toku:";
            //
            // lblProcenatText
            //
            this.lblProcenatText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblProcenatText.AutoSize = true;
            this.lblProcenatText.Location = new System.Drawing.Point(233, 394);
            this.lblProcenatText.Name = "lblProcenatText";
            this.lblProcenatText.Size = new System.Drawing.Size(132, 16);
            this.lblProcenatText.TabIndex = 15;
            this.lblProcenatText.Text = "Procenat prolaznosti:";
            //
            // lblVrednostUToku
            //
            this.lblVrednostUToku.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblVrednostUToku.AutoSize = true;
            this.lblVrednostUToku.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVrednostUToku.Location = new System.Drawing.Point(371, 364);
            this.lblVrednostUToku.MinimumSize = new System.Drawing.Size(40, 0);
            this.lblVrednostUToku.Name = "lblVrednostUToku";
            this.lblVrednostUToku.TabIndex = 16;
            this.lblVrednostUToku.Text = "0";
            //
            // lblVrednostPolozilo
            //
            this.lblVrednostPolozilo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblVrednostPolozilo.AutoSize = true;
            this.lblVrednostPolozilo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVrednostPolozilo.ForeColor = System.Drawing.Color.FromArgb(46, 125, 50);
            this.lblVrednostPolozilo.Location = new System.Drawing.Point(131, 364);
            this.lblVrednostPolozilo.MinimumSize = new System.Drawing.Size(40, 0);
            this.lblVrednostPolozilo.Name = "lblVrednostPolozilo";
            this.lblVrednostPolozilo.TabIndex = 17;
            this.lblVrednostPolozilo.Text = "0";
            //
            // lblVrednostPalo
            //
            this.lblVrednostPalo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblVrednostPalo.AutoSize = true;
            this.lblVrednostPalo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVrednostPalo.ForeColor = System.Drawing.Color.FromArgb(198, 40, 40);
            this.lblVrednostPalo.Location = new System.Drawing.Point(131, 394);
            this.lblVrednostPalo.MinimumSize = new System.Drawing.Size(40, 0);
            this.lblVrednostPalo.Name = "lblVrednostPalo";
            this.lblVrednostPalo.TabIndex = 18;
            this.lblVrednostPalo.Text = "0";
            //
            // lblVrednostProcenat
            //
            this.lblVrednostProcenat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblVrednostProcenat.AutoSize = true;
            this.lblVrednostProcenat.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVrednostProcenat.ForeColor = System.Drawing.Color.FromArgb(21, 101, 192);
            this.lblVrednostProcenat.Location = new System.Drawing.Point(371, 380);
            this.lblVrednostProcenat.MinimumSize = new System.Drawing.Size(110, 0);
            this.lblVrednostProcenat.Name = "lblVrednostProcenat";
            this.lblVrednostProcenat.TabIndex = 19;
            this.lblVrednostProcenat.Text = "0,00%";
            // 
            // UCKreirajIzvestajOIspitima
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblVrednostProcenat);
            this.Controls.Add(this.lblVrednostPalo);
            this.Controls.Add(this.lblVrednostPolozilo);
            this.Controls.Add(this.lblVrednostUToku);
            this.Controls.Add(this.lblProcenatText);
            this.Controls.Add(this.lblUTokuText);
            this.Controls.Add(this.lblPaloText);
            this.Controls.Add(this.lblPoloziloText);
            this.Controls.Add(this.btnIzveziCsv);
            this.Controls.Add(this.btnKreirajIzvestaj);
            this.Controls.Add(this.lblUkupnoKandidata);
            this.Controls.Add(this.lblNemaRezultata);
            this.Controls.Add(this.dgvIzvestajIspita);
            this.Controls.Add(this.chbSamoAktivniUpisi);
            this.Controls.Add(this.chbUkljuciBezRezultata);
            this.Controls.Add(this.cmbKategorija);
            this.Controls.Add(this.cmbTipIspita);
            this.Controls.Add(this.lblKategorijaText);
            this.Controls.Add(this.lblTipIspitaText);
            this.Controls.Add(this.lblDoDatuma);
            this.Controls.Add(this.lblOdDatuma);
            this.Controls.Add(this.btnPresetGodina);
            this.Controls.Add(this.btnPresetTridesetDana);
            this.Controls.Add(this.btnPresetMesec);
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
        private System.Windows.Forms.Button btnPresetMesec;
        private System.Windows.Forms.Button btnPresetTridesetDana;
        private System.Windows.Forms.Button btnPresetGodina;
        private System.Windows.Forms.Label lblOdDatuma;
        private System.Windows.Forms.Label lblDoDatuma;
        private System.Windows.Forms.Label lblTipIspitaText;
        private System.Windows.Forms.Label lblKategorijaText;
        private System.Windows.Forms.ComboBox cmbTipIspita;
        private System.Windows.Forms.ComboBox cmbKategorija;
        private System.Windows.Forms.CheckBox chbUkljuciBezRezultata;
        private System.Windows.Forms.CheckBox chbSamoAktivniUpisi;
        private System.Windows.Forms.DataGridView dgvIzvestajIspita;
        private System.Windows.Forms.Label lblNemaRezultata;
        private System.Windows.Forms.Label lblUkupnoKandidata;
        private System.Windows.Forms.Button btnKreirajIzvestaj;
        private System.Windows.Forms.Button btnIzveziCsv;
        private System.Windows.Forms.Label lblPoloziloText;
        private System.Windows.Forms.Label lblPaloText;
        private System.Windows.Forms.Label lblUTokuText;
        private System.Windows.Forms.Label lblProcenatText;
        private System.Windows.Forms.Label lblVrednostUToku;
        private System.Windows.Forms.Label lblVrednostPolozilo;
        private System.Windows.Forms.Label lblVrednostPalo;
        private System.Windows.Forms.Label lblVrednostProcenat;

        public DateTimePicker DtpDatumOd { get => dtpDatumOd; set => dtpDatumOd = value; }
        public DateTimePicker DtpDatumDo { get => dtpDatumDo; set => dtpDatumDo = value; }
        public Button BtnPresetMesec { get => btnPresetMesec; set => btnPresetMesec = value; }
        public Button BtnPresetTridesetDana { get => btnPresetTridesetDana; set => btnPresetTridesetDana = value; }
        public Button BtnPresetGodina { get => btnPresetGodina; set => btnPresetGodina = value; }
        public Label LblOdDatuma { get => lblOdDatuma; set => lblOdDatuma = value; }
        public Label LblDoDatuma { get => lblDoDatuma; set => lblDoDatuma = value; }
        public Label LblTipIspitaText { get => lblTipIspitaText; set => lblTipIspitaText = value; }
        public Label LblKategorijaText { get => lblKategorijaText; set => lblKategorijaText = value; }
        public ComboBox CmbTipIspita { get => cmbTipIspita; set => cmbTipIspita = value; }
        public ComboBox CmbKategorija { get => cmbKategorija; set => cmbKategorija = value; }
        public CheckBox ChbUkljuciBezRezultata { get => chbUkljuciBezRezultata; set => chbUkljuciBezRezultata = value; }
        public CheckBox ChbSamoAktivniUpisi { get => chbSamoAktivniUpisi; set => chbSamoAktivniUpisi = value; }
        public DataGridView DgvIzvestajIspita { get => dgvIzvestajIspita; set => dgvIzvestajIspita = value; }
        public Label LblNemaRezultata { get => lblNemaRezultata; set => lblNemaRezultata = value; }
        public Label LblUkupnoKandidata { get => lblUkupnoKandidata; set => lblUkupnoKandidata = value; }
        public Button BtnKreirajIzvestaj { get => btnKreirajIzvestaj; set => btnKreirajIzvestaj = value; }
        public Button BtnIzveziCsv { get => btnIzveziCsv; set => btnIzveziCsv = value; }
        public Label LblPoloziloText { get => lblPoloziloText; set => lblPoloziloText = value; }
        public Label LblPaloText { get => lblPaloText; set => lblPaloText = value; }
        public Label LblUTokuText { get => lblUTokuText; set => lblUTokuText = value; }
        public Label LblProcenatText { get => lblProcenatText; set => lblProcenatText = value; }
        public Label LblVrednostUToku { get => lblVrednostUToku; set => lblVrednostUToku = value; }
        public Label LblVrednostPolozilo { get => lblVrednostPolozilo; set => lblVrednostPolozilo = value; }
        public Label LblVrednostPalo { get => lblVrednostPalo; set => lblVrednostPalo = value; }
        public Label LblVrednostProcenat { get => lblVrednostProcenat; set => lblVrednostProcenat = value; }
    }
}
