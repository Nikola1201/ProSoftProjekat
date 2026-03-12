namespace Client
{
    partial class FrmMain
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
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.kandidatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kreirajKandidataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.upisiKandidataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pnlMain = new System.Windows.Forms.Panel();
            this.ispisiKandidataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.kandidatToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // kandidatToolStripMenuItem
            // 
            this.kandidatToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.kreirajKandidataToolStripMenuItem,
            this.upisiKandidataToolStripMenuItem,
            this.ispisiKandidataToolStripMenuItem});
            this.kandidatToolStripMenuItem.Name = "kandidatToolStripMenuItem";
            this.kandidatToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
            this.kandidatToolStripMenuItem.Text = "Kandidat";
            // 
            // kreirajKandidataToolStripMenuItem
            // 
            this.kreirajKandidataToolStripMenuItem.Name = "kreirajKandidataToolStripMenuItem";
            this.kreirajKandidataToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.kreirajKandidataToolStripMenuItem.Text = "Kreiraj kandidata";
            // 
            // upisiKandidataToolStripMenuItem
            // 
            this.upisiKandidataToolStripMenuItem.Name = "upisiKandidataToolStripMenuItem";
            this.upisiKandidataToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.upisiKandidataToolStripMenuItem.Text = "Upisi kandidata";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // pnlMain
            // 
            this.pnlMain.Location = new System.Drawing.Point(12, 27);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(776, 411);
            this.pnlMain.TabIndex = 2;
            // 
            // ispisiKandidataToolStripMenuItem
            // 
            this.ispisiKandidataToolStripMenuItem.Name = "ispisiKandidataToolStripMenuItem";
            this.ispisiKandidataToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ispisiKandidataToolStripMenuItem.Text = "Ispisi kandidata";
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.pnlMain);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmMain";
            this.Text = "FrmMain";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem kandidatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kreirajKandidataToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.ToolStripMenuItem upisiKandidataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ispisiKandidataToolStripMenuItem;
    }
}