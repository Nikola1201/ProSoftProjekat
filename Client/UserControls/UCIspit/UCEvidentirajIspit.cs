using System;
using System.Windows.Forms;

namespace Client.UserControls.UCIspit
{
    public class UCEvidentirajIspit : UserControl
    {
        private Label lblNaslov;
        private Label lblKandidat;
        private Label lblDatum;
        private Label lblTip;
        private Label lblRezultat;
        private Label lblNapomena;
        private ComboBox cmbKandidat;
        private DateTimePicker dtpDatumIspita;
        private ComboBox cmbTip;
        private ComboBox cmbRezultat;
        private TextBox txtNapomena;
        private Button btnSacuvaj;

        public UCEvidentirajIspit()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            lblNaslov = new Label();
            lblKandidat = new Label();
            lblDatum = new Label();
            lblTip = new Label();
            lblRezultat = new Label();
            lblNapomena = new Label();
            cmbKandidat = new ComboBox();
            dtpDatumIspita = new DateTimePicker();
            cmbTip = new ComboBox();
            cmbRezultat = new ComboBox();
            txtNapomena = new TextBox();
            btnSacuvaj = new Button();
            SuspendLayout();
            // 
            // lblNaslov
            // 
            lblNaslov.AutoSize = true;
            lblNaslov.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
            lblNaslov.Location = new System.Drawing.Point(31, 28);
            lblNaslov.Name = "lblNaslov";
            lblNaslov.Size = new System.Drawing.Size(250, 24);
            lblNaslov.TabIndex = 0;
            lblNaslov.Text = "Evidentiranje polaganja";
            // 
            // lblKandidat
            // 
            lblKandidat.AutoSize = true;
            lblKandidat.Location = new System.Drawing.Point(31, 92);
            lblKandidat.Name = "lblKandidat";
            lblKandidat.Size = new System.Drawing.Size(49, 13);
            lblKandidat.TabIndex = 1;
            lblKandidat.Text = "Kandidat";
            // 
            // lblDatum
            // 
            lblDatum.AutoSize = true;
            lblDatum.Location = new System.Drawing.Point(31, 126);
            lblDatum.Name = "lblDatum";
            lblDatum.Size = new System.Drawing.Size(67, 13);
            lblDatum.TabIndex = 2;
            lblDatum.Text = "Datum ispita";
            // 
            // lblTip
            // 
            lblTip.AutoSize = true;
            lblTip.Location = new System.Drawing.Point(31, 161);
            lblTip.Name = "lblTip";
            lblTip.Size = new System.Drawing.Size(49, 13);
            lblTip.TabIndex = 3;
            lblTip.Text = "Tip ispita";
            // 
            // lblRezultat
            // 
            lblRezultat.AutoSize = true;
            lblRezultat.Location = new System.Drawing.Point(31, 197);
            lblRezultat.Name = "lblRezultat";
            lblRezultat.Size = new System.Drawing.Size(45, 13);
            lblRezultat.TabIndex = 4;
            lblRezultat.Text = "Rezultat";
            // 
            // lblNapomena
            // 
            lblNapomena.AutoSize = true;
            lblNapomena.Location = new System.Drawing.Point(31, 233);
            lblNapomena.Name = "lblNapomena";
            lblNapomena.Size = new System.Drawing.Size(59, 13);
            lblNapomena.TabIndex = 5;
            lblNapomena.Text = "Napomena";
            // 
            // cmbKandidat
            // 
            cmbKandidat.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbKandidat.FormattingEnabled = true;
            cmbKandidat.Location = new System.Drawing.Point(142, 89);
            cmbKandidat.Name = "cmbKandidat";
            cmbKandidat.Size = new System.Drawing.Size(333, 21);
            cmbKandidat.TabIndex = 6;
            // 
            // dtpDatumIspita
            // 
            dtpDatumIspita.Format = DateTimePickerFormat.Short;
            dtpDatumIspita.Location = new System.Drawing.Point(142, 123);
            dtpDatumIspita.Name = "dtpDatumIspita";
            dtpDatumIspita.Size = new System.Drawing.Size(200, 20);
            dtpDatumIspita.TabIndex = 7;
            // 
            // cmbTip
            // 
            cmbTip.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTip.FormattingEnabled = true;
            cmbTip.Location = new System.Drawing.Point(142, 158);
            cmbTip.Name = "cmbTip";
            cmbTip.Size = new System.Drawing.Size(200, 21);
            cmbTip.TabIndex = 8;
            // 
            // cmbRezultat
            // 
            cmbRezultat.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRezultat.FormattingEnabled = true;
            cmbRezultat.Location = new System.Drawing.Point(142, 194);
            cmbRezultat.Name = "cmbRezultat";
            cmbRezultat.Size = new System.Drawing.Size(200, 21);
            cmbRezultat.TabIndex = 9;
            // 
            // txtNapomena
            // 
            txtNapomena.Location = new System.Drawing.Point(142, 230);
            txtNapomena.MaxLength = 500;
            txtNapomena.Multiline = true;
            txtNapomena.Name = "txtNapomena";
            txtNapomena.Size = new System.Drawing.Size(333, 75);
            txtNapomena.TabIndex = 10;
            // 
            // btnSacuvaj
            // 
            btnSacuvaj.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            btnSacuvaj.Location = new System.Drawing.Point(142, 324);
            btnSacuvaj.Name = "btnSacuvaj";
            btnSacuvaj.Size = new System.Drawing.Size(221, 44);
            btnSacuvaj.TabIndex = 11;
            btnSacuvaj.Text = "Evidentiraj ispit";
            btnSacuvaj.UseVisualStyleBackColor = true;
            // 
            // UCEvidentirajIspit
            // 
            Controls.Add(btnSacuvaj);
            Controls.Add(txtNapomena);
            Controls.Add(cmbRezultat);
            Controls.Add(cmbTip);
            Controls.Add(dtpDatumIspita);
            Controls.Add(cmbKandidat);
            Controls.Add(lblNapomena);
            Controls.Add(lblRezultat);
            Controls.Add(lblTip);
            Controls.Add(lblDatum);
            Controls.Add(lblKandidat);
            Controls.Add(lblNaslov);
            Name = "UCEvidentirajIspit";
            Size = new System.Drawing.Size(620, 410);
            ResumeLayout(false);
            PerformLayout();
        }

        public ComboBox CmbKandidat { get { return cmbKandidat; } }
        public DateTimePicker DtpDatumIspita { get { return dtpDatumIspita; } }
        public ComboBox CmbTip { get { return cmbTip; } }
        public ComboBox CmbRezultat { get { return cmbRezultat; } }
        public TextBox TxtNapomena { get { return txtNapomena; } }
        public Button BtnSacuvaj { get { return btnSacuvaj; } }
    }
}
