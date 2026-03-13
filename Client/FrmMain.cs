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
            kreirajKandidataToolStripMenuItem.Click += (s, a)
                => MainCoordinator.Instance.ShowKreirajKandidataPanel();
            upisiKandidataToolStripMenuItem.Click += (s, a)
                => MainCoordinator.Instance.ShowUpisiKandidataPanel();
            ispisiKandidataToolStripMenuItem.Click += (s, a)
                => MainCoordinator.Instance.ShowIspisiKandidataPanel();


            kreirajInstruktoraToolStripMenuItem.Click += (s, a)
                => MainCoordinator.Instance.ShowKreirajInstruktoraPanel();
            obrisiInstruktoraToolStripMenuItem.Click += (s, a)
                => MainCoordinator.Instance.ShowObrisiInstruktoraPanel();
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
    }
}
