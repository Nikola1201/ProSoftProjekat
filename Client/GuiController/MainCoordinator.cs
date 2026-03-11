using Client.UserControls;
using Common.Domain;
using System;

namespace Client.GuiController
{
    internal class MainCoordinator
    {
        private static MainCoordinator _instance;
        public static MainCoordinator Instance => _instance ?? (_instance = new MainCoordinator());

        private MainCoordinator() {
            _kandidatGuiController = new KandidatGuiController();
        }

        private FrmMain _frmMain;
        private KandidatGuiController _kandidatGuiController;

        internal void ShowFrmMain(Admin admin)
        {
            _frmMain = new FrmMain(admin);
            _frmMain.AutoSize = true;
            _frmMain.ShowDialog();
        }

        internal void ShowKreirajKandidataPanel()
        {
            _frmMain.ChangePanel(_kandidatGuiController.CreateKandidat());
        }
        internal void ShowDefault()
        {
            UCDefault uCDefault = new UCDefault();
            _frmMain.ChangePanel(uCDefault);
        }
    }
}