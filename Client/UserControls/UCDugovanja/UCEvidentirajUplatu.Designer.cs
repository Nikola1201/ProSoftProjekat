namespace Client.UserControls.UCDugovanja
{
    partial class UCEvidentirajUplatu
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblNaslov = new System.Windows.Forms.Label();
            this.lblKandidat = new System.Windows.Forms.Label();
            this.lblUpis = new System.Windows.Forms.Label();
            this.lblPreostalo = new System.Windows.Forms.Label();
            this.lblIznos = new System.Windows.Forms.Label();
            this.txtIznos = new System.Windows.Forms.TextBox();
            this.lblNacin = new System.Windows.Forms.Label();
            this.cmbNacin = new System.Windows.Forms.ComboBox();
            this.lblDatum = new System.Windows.Forms.Label();
            this.dtpDatum = new System.Windows.Forms.DateTimePicker();
            this.lblNapomena = new System.Windows.Forms.Label();
            this.txtNapomena = new System.Windows.Forms.TextBox();
            this.btnSacuvaj = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // lblNaslov
            //
            this.lblNaslov.AutoSize = true;
            this.lblNaslov.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblNaslov.Location = new System.Drawing.Point(15, 12);
            this.lblNaslov.Text = "Evidentiraj uplatu";
            //
            // lblKandidat
            //
            this.lblKandidat.AutoSize = true;
            this.lblKandidat.Location = new System.Drawing.Point(15, 45);
            this.lblKandidat.Text = "Kandidat: -";
            //
            // lblUpis
            //
            this.lblUpis.AutoSize = true;
            this.lblUpis.Location = new System.Drawing.Point(15, 70);
            this.lblUpis.Text = "Upis: -";
            //
            // lblPreostalo
            //
            this.lblPreostalo.AutoSize = true;
            this.lblPreostalo.ForeColor = System.Drawing.Color.DarkRed;
            this.lblPreostalo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblPreostalo.Location = new System.Drawing.Point(15, 95);
            this.lblPreostalo.Text = "Preostalo dugovanje: -";
            //
            // lblIznos
            //
            this.lblIznos.AutoSize = true;
            this.lblIznos.Location = new System.Drawing.Point(15, 140);
            this.lblIznos.Text = "Iznos:";
            //
            // txtIznos
            //
            this.txtIznos.Location = new System.Drawing.Point(140, 137);
            this.txtIznos.Size = new System.Drawing.Size(160, 23);
            //
            // lblNacin
            //
            this.lblNacin.AutoSize = true;
            this.lblNacin.Location = new System.Drawing.Point(15, 175);
            this.lblNacin.Text = "Nacin placanja:";
            //
            // cmbNacin
            //
            this.cmbNacin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNacin.Location = new System.Drawing.Point(140, 172);
            this.cmbNacin.Size = new System.Drawing.Size(160, 23);
            //
            // lblDatum
            //
            this.lblDatum.AutoSize = true;
            this.lblDatum.Location = new System.Drawing.Point(15, 210);
            this.lblDatum.Text = "Datum uplate:";
            //
            // dtpDatum
            //
            this.dtpDatum.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDatum.Location = new System.Drawing.Point(140, 206);
            this.dtpDatum.Size = new System.Drawing.Size(160, 23);
            //
            // lblNapomena
            //
            this.lblNapomena.AutoSize = true;
            this.lblNapomena.Location = new System.Drawing.Point(15, 245);
            this.lblNapomena.Text = "Napomena:";
            //
            // txtNapomena
            //
            this.txtNapomena.Multiline = true;
            this.txtNapomena.Location = new System.Drawing.Point(140, 242);
            this.txtNapomena.Size = new System.Drawing.Size(360, 80);
            this.txtNapomena.MaxLength = 500;
            //
            // btnSacuvaj
            //
            this.btnSacuvaj.Location = new System.Drawing.Point(140, 340);
            this.btnSacuvaj.Size = new System.Drawing.Size(160, 32);
            this.btnSacuvaj.Text = "Sacuvaj uplatu";
            //
            // UCEvidentirajUplatu
            //
            this.Controls.Add(this.lblNaslov);
            this.Controls.Add(this.lblKandidat);
            this.Controls.Add(this.lblUpis);
            this.Controls.Add(this.lblPreostalo);
            this.Controls.Add(this.lblIznos);
            this.Controls.Add(this.txtIznos);
            this.Controls.Add(this.lblNacin);
            this.Controls.Add(this.cmbNacin);
            this.Controls.Add(this.lblDatum);
            this.Controls.Add(this.dtpDatum);
            this.Controls.Add(this.lblNapomena);
            this.Controls.Add(this.txtNapomena);
            this.Controls.Add(this.btnSacuvaj);
            this.Name = "UCEvidentirajUplatu";
            this.Size = new System.Drawing.Size(560, 400);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblNaslov;
        private System.Windows.Forms.Label lblKandidat;
        private System.Windows.Forms.Label lblUpis;
        private System.Windows.Forms.Label lblPreostalo;
        private System.Windows.Forms.Label lblIznos;
        private System.Windows.Forms.TextBox txtIznos;
        private System.Windows.Forms.Label lblNacin;
        private System.Windows.Forms.ComboBox cmbNacin;
        private System.Windows.Forms.Label lblDatum;
        private System.Windows.Forms.DateTimePicker dtpDatum;
        private System.Windows.Forms.Label lblNapomena;
        private System.Windows.Forms.TextBox txtNapomena;
        private System.Windows.Forms.Button btnSacuvaj;

        public System.Windows.Forms.Label LblKandidat { get => lblKandidat; set => lblKandidat = value; }
        public System.Windows.Forms.Label LblUpis { get => lblUpis; set => lblUpis = value; }
        public System.Windows.Forms.Label LblPreostalo { get => lblPreostalo; set => lblPreostalo = value; }
        public System.Windows.Forms.TextBox TxtIznos { get => txtIznos; set => txtIznos = value; }
        public System.Windows.Forms.ComboBox CmbNacin { get => cmbNacin; set => cmbNacin = value; }
        public System.Windows.Forms.DateTimePicker DtpDatum { get => dtpDatum; set => dtpDatum = value; }
        public System.Windows.Forms.TextBox TxtNapomena { get => txtNapomena; set => txtNapomena = value; }
        public System.Windows.Forms.Button BtnSacuvaj { get => btnSacuvaj; set => btnSacuvaj = value; }
    }
}
