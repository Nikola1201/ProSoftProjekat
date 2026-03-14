using Client.UserControls;
using Client.UserControls.UCCasVoznje;
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
            _instruktorGuiKontroler = new InstruktorGuiKontroler();
            _casGuiKontroler = new CasGuiKontroler();
        }

        private FrmMain _frmMain;
        private KandidatGuiController _kandidatGuiController;
        private InstruktorGuiKontroler _instruktorGuiKontroler;
        private CasGuiKontroler _casGuiKontroler;

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
            _frmMain.ChangePanel(_instruktorGuiKontroler.CreateInstruktor());

        }

        internal void ShowObrisiInstruktoraPanel()
        {
            _frmMain.ChangePanel(_instruktorGuiKontroler.CreateObrisiInstruktor());

        }

        internal void ShowZakaziCasPanel()
        {
            _frmMain.ChangePanel(_casGuiKontroler.CreateZakaziCas());
        }

        internal void ShowOtkaziCasVoznjePanel()
        {
            _frmMain.ChangePanel(_casGuiKontroler.CreateIzmeniCas());
        }

        internal void ShowPretraziKandidataPanel()
        {
            _frmMain.ChangePanel(_kandidatGuiController.CreatePretraziKandidata());
        }
    }
}