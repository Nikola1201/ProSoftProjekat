namespace Client.UserControls.UCDugovanja
{
    partial class UCPregledDugovanja
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
            this.btnEvidentirajUplatu = new System.Windows.Forms.Button();
            this.lblBrojRedova = new System.Windows.Forms.Label();
            this.dgvDugovanja = new System.Windows.Forms.DataGridView();
            this.lblNemaPodataka = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDugovanja)).BeginInit();
            this.SuspendLayout();
            //
            // lblNaslov
            //
            this.lblNaslov.AutoSize = true;
            this.lblNaslov.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblNaslov.Location = new System.Drawing.Point(15, 12);
            this.lblNaslov.Name = "lblNaslov";
            this.lblNaslov.Text = "Pregled kandidata sa dugovanjem";
            //
            // btnEvidentirajUplatu
            //
            this.btnEvidentirajUplatu.Location = new System.Drawing.Point(15, 45);
            this.btnEvidentirajUplatu.Size = new System.Drawing.Size(180, 30);
            this.btnEvidentirajUplatu.Name = "btnEvidentirajUplatu";
            this.btnEvidentirajUplatu.Text = "Evidentiraj uplatu";
            this.btnEvidentirajUplatu.Enabled = false;
            //
            // lblBrojRedova
            //
            this.lblBrojRedova.AutoSize = true;
            this.lblBrojRedova.Location = new System.Drawing.Point(210, 52);
            this.lblBrojRedova.Name = "lblBrojRedova";
            this.lblBrojRedova.Text = "";
            //
            // dgvDugovanja
            //
            this.dgvDugovanja.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDugovanja.Location = new System.Drawing.Point(15, 85);
            this.dgvDugovanja.Size = new System.Drawing.Size(820, 460);
            this.dgvDugovanja.Name = "dgvDugovanja";
            this.dgvDugovanja.AllowUserToAddRows = false;
            this.dgvDugovanja.AllowUserToDeleteRows = false;
            this.dgvDugovanja.ReadOnly = true;
            this.dgvDugovanja.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDugovanja.MultiSelect = false;
            //
            // lblNemaPodataka
            //
            this.lblNemaPodataka.AutoSize = true;
            this.lblNemaPodataka.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblNemaPodataka.ForeColor = System.Drawing.Color.Gray;
            this.lblNemaPodataka.Location = new System.Drawing.Point(380, 280);
            this.lblNemaPodataka.Name = "lblNemaPodataka";
            this.lblNemaPodataka.Text = "Nema kandidata sa dugovanjem.";
            this.lblNemaPodataka.Visible = false;
            //
            // UCPregledDugovanja
            //
            this.Controls.Add(this.lblNaslov);
            this.Controls.Add(this.btnEvidentirajUplatu);
            this.Controls.Add(this.lblBrojRedova);
            this.Controls.Add(this.dgvDugovanja);
            this.Controls.Add(this.lblNemaPodataka);
            this.Name = "UCPregledDugovanja";
            this.Size = new System.Drawing.Size(850, 560);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDugovanja)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblNaslov;
        private System.Windows.Forms.Button btnEvidentirajUplatu;
        private System.Windows.Forms.Label lblBrojRedova;
        private System.Windows.Forms.DataGridView dgvDugovanja;
        private System.Windows.Forms.Label lblNemaPodataka;

        public System.Windows.Forms.DataGridView DgvDugovanja { get => dgvDugovanja; set => dgvDugovanja = value; }
        public System.Windows.Forms.Button BtnEvidentirajUplatu { get => btnEvidentirajUplatu; set => btnEvidentirajUplatu = value; }
        public System.Windows.Forms.Label LblBrojRedova { get => lblBrojRedova; set => lblBrojRedova = value; }
        public System.Windows.Forms.Label LblNemaPodataka { get => lblNemaPodataka; set => lblNemaPodataka = value; }
    }
}
