using Client.UserControls;
using Client.UserControls.UCCasVoznje;
using Client.Utils;
using Common.Domain;
using System;
using System.Windows.Forms;

namespace Client.GuiController
{
    internal class MainCoordinator
    {
        private static MainCoordinator _instance;
        public static MainCoordinator Instance => _instance ?? (_instance = new MainCoordinator());

        private MainCoordinator() {
            _kandidatGuiController = new KandidatGuiController();
            _instruktorGuiController = new InstruktorGuiController();
            _casGuiController = new CasGuiController();
            _ispitGuiController = new IspitGuiController();
        }

        private FrmMain _frmMain;
        private KandidatGuiController _kandidatGuiController;
        private InstruktorGuiController _instruktorGuiController;
        private CasGuiController _casGuiController;
        private IspitGuiController _ispitGuiController;
        internal void ShowFrmMain(Admin admin)
        {
            _frmMain = new FrmMain(admin);
            _frmMain.AutoSize = true;
            _frmMain.ShowDialog();
        }
        internal void Logout(Admin admin)
        {
            Communication.Instance.Logout(admin);

        }
        internal void ShowKreirajKandidataPanel()
        {
            _frmMain.ChangePanel(_kandidatGuiController.CreateKandidat());
        }

        internal void ShowUpisiKandidataPanel()
        {
            _frmMain.ChangePanel(_kandidatGuiController.CreateUpisKandidata());
        }

        internal void ShowDefault()
        {
            UCDefault uCDefault = new UCDefault();
            _frmMain.ChangePanel(uCDefault);
        }

        internal void ShowIspisiKandidataPanel()
        {
            _frmMain.ChangePanel(_kandidatGuiController.CreateObrisiKandidata());
        }

        internal void ShowKreirajInstruktoraPanel()
        {
            _frmMain.ChangePanel(_instruktorGuiController.CreateInstruktor());

        }

        internal void ShowObrisiInstruktoraPanel()
        {
            _frmMain.ChangePanel(_instruktorGuiController.CreateObrisiInstruktor());

        }

        internal void ShowZakaziCasPanel()
        {
            _frmMain.ChangePanel(_casGuiController.CreateZakaziCas());
        }

        internal void ShowOtkaziCasVoznjePanel()
        {
            _frmMain.ChangePanel(_casGuiController.CreateIzmeniCas());
        }

        internal void ShowPretraziKandidataPanel()
        {
            _frmMain.ChangePanel(_kandidatGuiController.CreatePretraziKandidata());
        }

        internal void ShowKreirajIzvestajProlaznostiPanel()
        {
            _frmMain.ChangePanel(_ispitGuiController.CreateIzvestajProlaznosti());
        }

        internal void ShowEvidentirajIspitPanel()
        {
            _frmMain.ChangePanel(_ispitGuiController.CreateEvidentirajIspit());
        }

        internal void LogoutDialog(Admin admin)
        {
            if (ShowMessage.Dialog("Da li ste sigurni da želite da se odjavite?") == DialogResult.OK)
            {
                Logout(admin);
                new FrmLogin().Show();
                _frmMain.Hide();
            }
        }
    }
}