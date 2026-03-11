using Common.Communication;
using Common.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.GuiController
{
    internal class LoginGuiController
    {
        private static LoginGuiController _instance;
        public static LoginGuiController Instance => _instance ?? (_instance = new LoginGuiController());
        private LoginGuiController()
        {
        }
        private FrmLogin _frmLogin;

        internal void ShowFrmLogin()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            _frmLogin = new FrmLogin();
            _frmLogin.BtnLogin.Click += (s, e) => Login();
            _frmLogin.AutoSize = true;
            Application.Run(_frmLogin);
        }

        private void Login()
        {
            string username = _frmLogin.TxtUsername.Text;
            string password = _frmLogin.TxtPassword.Text;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)
                )
            {
                MessageBox.Show("Unesite korisnicko ime i sifru");
                return;
            }
            try
            {
                Admin admin = new Admin()
                {
                    Username = username,
                    Lozinka = password
                };
                Response response = Communication.Instance.Login(admin);
                if (response.Exception == null)
                {
                    if (response.Result == null)
                    {
                        MessageBox.Show("Admin nije pronadjen");
                    }
                    else
                    {
                        if (((Admin)response.Result).Ime == "Vec ulogovan")
                        {
                            MessageBox.Show("Vec ste prijavljeni");
                            return;
                        }
                        else
                        {

                            MessageBox.Show("Uspesno ste se prijavili na sistem");
                            _frmLogin.Visible = false;
                            MainCoordinator.Instance.ShowFrmMain((Admin)response.Result);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Neuspesna prijava, molimo Vas pokusajte kasnije");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                MessageBox.Show($"Server je ugasen! {ex.Message}");
                return;
            }
        }
    }
}
