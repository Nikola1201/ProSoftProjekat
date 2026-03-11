using Common.Domain;
using System;

namespace Client.GuiController
{
    internal class MainCoordinator
    {
        private static MainCoordinator _instance;
        public static MainCoordinator Instance => _instance ?? (_instance = new MainCoordinator());

        private MainCoordinator() { }

        private FrmMain _frmMain;

        internal void ShowFrmMain(Admin admin)
        {
            _frmMain = new FrmMain(admin);
            _frmMain.AutoSize = true;
            _frmMain.ShowDialog();
        }
    }
}