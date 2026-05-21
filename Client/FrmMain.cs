using Client.GuiController;
using Common.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class FrmMain : Form
    {
        private Admin admin;
        private bool _disconnectShown;

        public FrmMain()
        {
            InitializeComponent();
            ConfigureMainPanel();
        }

        public FrmMain(Admin admin)
        {
            this.admin = admin;
            InitializeComponent();
            ConfigureMainPanel();

            odjaviSeToolStripMenuItem.Click += (s, a)
                => MainCoordinator.Instance.LogoutDialog(admin);

            kreirajKandidataToolStripMenuItem.Click += (s, a)
                => MainCoordinator.Instance.ShowKreirajKandidataPanel();
            upisiKandidataToolStripMenuItem.Click += (s, a)
                => MainCoordinator.Instance.ShowUpisiKandidataPanel();
            ispisiKandidataToolStripMenuItem.Click += (s, a)
                => MainCoordinator.Instance.ShowIspisiKandidataPanel();
            pretraživanjeKandidataToolStripMenuItem.Click += (s, a)
                => MainCoordinator.Instance.ShowPretraziKandidataPanel();


            kreirajInstruktoraToolStripMenuItem.Click += (s, a)
                => MainCoordinator.Instance.ShowKreirajInstruktoraPanel();
            obrisiInstruktoraToolStripMenuItem.Click += (s, a)
                => MainCoordinator.Instance.ShowObrisiInstruktoraPanel();

            zakaziČasToolStripMenuItem.Click += (s, a)
                => MainCoordinator.Instance.ShowZakaziCasPanel();

            otkažiČasToolStripMenuItem.Click += (s, a)
                => MainCoordinator.Instance.ShowOtkaziCasVoznjePanel();

            izvestajProlaznostiToolStripMenuItem.Click += (s, a)
                => MainCoordinator.Instance.ShowKreirajIzvestajProlaznostiPanel();

            evidentiranjePolaganjaToolStripMenuItem.Click += (s, a)
                => MainCoordinator.Instance.ShowEvidentirajIspitPanel();

            pregledDugovanjaToolStripMenuItem.Click += (s, a)
                => MainCoordinator.Instance.ShowPregledDugovanjaPanel();

            Communication.Instance.ConnectionLost += OnConnectionLost;

            this.FormClosed += (s, a) =>
            {
                try { MainCoordinator.Instance.Logout(admin); }
                catch (Common.Communication.ConnectionLostException) { /* already disconnected */ }
            };

        }

        private void ConfigureMainPanel()
        {
            pnlMain.AutoSize = false;
            pnlMain.AutoSizeMode = AutoSizeMode.GrowOnly;
        }

        internal void ChangePanel(Control control)
        {
            if (control == null) return;

            pnlMain.SuspendLayout();
            pnlMain.Controls.Clear();
            control.Dock = DockStyle.Fill;
            pnlMain.Controls.Add(control);
            pnlMain.ResumeLayout(true);
        }

        private void OnConnectionLost(object sender, EventArgs e)
        {
            if (InvokeRequired) { BeginInvoke(new Action(() => OnConnectionLost(sender, e))); return; }
            if (_disconnectShown) return;
            _disconnectShown = true;

            MessageBox.Show(this,
                "Veza sa serverom je izgubljena. Pritisnite 'Poveži se' da pokušate ponovo.",
                "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            SetDisconnectedUi();
        }

        private void SetDisconnectedUi()
        {
            pnlMain.Controls.Clear();
            menuStrip1.Enabled = false;
            lblStatus.Text = "Nije povezano";
            btnPoveziSe.Visible = true;
        }

        internal void SetConnectedUi()
        {
            menuStrip1.Enabled = true;
            lblStatus.Text = "Povezano";
            btnPoveziSe.Visible = false;
            _disconnectShown = false;
        }

        private void btnPoveziSe_Click(object sender, EventArgs e)
        {
            MainCoordinator.Instance.ReconnectAndReLogin(this);
        }
    }
}
