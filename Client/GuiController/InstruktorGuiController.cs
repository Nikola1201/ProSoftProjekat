using Client.UserControls.UCInstruktor;
using Client.UserControls.UCKandidat;
using Client.Utils;
using Common.Communication;
using Common.Domain;
using Common.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Client.GuiController
{
    internal class InstruktorGuiController
    {
        UCKreirajInstruktora _ucKreirajInstruktora;
        UCObrisiInstruktora _ucObrisiInstruktora;
        BindingList<Instruktor> instruktori;
        BindingList<Kategorija> kategorije;

        internal Control CreateInstruktor()
        {
            _ucKreirajInstruktora = new UCKreirajInstruktora();
            PopulateKategorijaCombo();
            _ucKreirajInstruktora.BtnKreiraj.Click += KreirajInstruktora;
            return _ucKreirajInstruktora;
        }

        private void PopulateKategorijaCombo()
        {
            try
            {
                List<Kategorija> sve = Communication.Instance.GetAllKategorije() ?? new List<Kategorija>();
                kategorije = new BindingList<Kategorija>(sve);
                _ucKreirajInstruktora.CmbKategorija.DataSource = kategorije;
                _ucKreirajInstruktora.CmbKategorija.SelectedIndex = -1;
                _ucKreirajInstruktora.BtnKreiraj.Enabled = kategorije.Count > 0;
            }
            catch (Exception ex) when (ex is System.Net.Sockets.SocketException || ex is System.IO.IOException)
            {
                ShowMessage.ServerDown();
                _ucKreirajInstruktora.BtnKreiraj.Enabled = false;
            }
            catch (Exception ex)
            {
                ShowMessage.Error(ex.Message);
                _ucKreirajInstruktora.BtnKreiraj.Enabled = false;
            }
        }

        private void KreirajInstruktora(object sender, EventArgs e)
        {
            if (!TryKrerirajInstruktoraFromInput(out Instruktor instruktor, out int kategorijaId))
            {
                return;
            }

            KreirajInstruktoraRequest req = new KreirajInstruktoraRequest
            {
                Instruktor = instruktor,
                KategorijaID = kategorijaId
            };

            try
            {
                Response response = Communication.Instance.CreateInstruktor(req);
                if (!string.IsNullOrEmpty(response.ErrorMessage))
                {
                    ShowMessage.Error(GetCreateInstruktorErrorMessage(response.ErrorMessage));
                    return;
                }

                if (response.Result == null)
                {
                    ShowMessage.Error("Sistem ne moze da kreira instruktora.");
                    return;
                }

                ShowMessage.Success("Sistem je uspesno kreirao instruktora.");
                ClearForm();
            }
            catch (Exception)
            {
                ShowMessage.ServerDown();
            }

        }

        private void ClearForm()
        {
            _ucKreirajInstruktora.TxtIme.Clear();
            _ucKreirajInstruktora.TxtPrezime.Clear();
            _ucKreirajInstruktora.TxtJMBG.Clear();
            _ucKreirajInstruktora.TxtTelefon.Clear();
            _ucKreirajInstruktora.TxtEmail.Clear();
            _ucKreirajInstruktora.DateTP.Value = DateTime.Now;
            _ucKreirajInstruktora.CmbKategorija.SelectedIndex = -1;
            _ucKreirajInstruktora.TxtIme.Focus();
        }

        private string GetCreateInstruktorErrorMessage(string? errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                return "Sistem ne moze da kreira instruktora. Pokusajte ponovo.";
            }

            string originalMessage = errorMessage.Trim();
            string message = originalMessage.ToLower();

            if (message.Contains("duplicate") || message.Contains("unique"))
            {
                if (message.Contains("jmbg"))
                {
                    return "Instruktor sa unetim JMBG vec postoji u sistemu.";
                }

                if (message.Contains("email"))
                {
                    return "Instruktor sa unetom email adresom vec postoji u sistemu.";
                }

                return "Instruktor sa unetim podacima vec postoji u sistemu.";
            }

            return originalMessage;
        }

        private bool TryKrerirajInstruktoraFromInput(out Instruktor instruktor, out int kategorijaId)
        {
            instruktor = null;
            kategorijaId = 0;

            string ime = _ucKreirajInstruktora.TxtIme.Text.Trim();
            string prezime = _ucKreirajInstruktora.TxtPrezime.Text.Trim();
            string jmbg = _ucKreirajInstruktora.TxtJMBG.Text.Trim();
            string telefon = _ucKreirajInstruktora.TxtTelefon.Text.Trim();
            string email = _ucKreirajInstruktora.TxtEmail.Text.Trim();
            DateTime datumZaposlenja = _ucKreirajInstruktora.DateTP.Value.Date;

            if (string.IsNullOrWhiteSpace(ime))
            {
                ShowMessage.Error("Unesite ime instruktora.", _ucKreirajInstruktora.TxtIme);
                return false;
            }

            if (string.IsNullOrWhiteSpace(prezime))
            {
                ShowMessage.Error("Unesite prezime instruktora.", _ucKreirajInstruktora.TxtPrezime);
                return false;
            }

            if (string.IsNullOrWhiteSpace(jmbg))
            {
                ShowMessage.Error("Unesite JMBG instruktora.", _ucKreirajInstruktora.TxtJMBG);
                return false;
            }

            if (jmbg.Length != 13 || !jmbg.All(char.IsDigit))
            {
                ShowMessage.Error("JMBG mora da sadrzi tacno 13 cifara.", _ucKreirajInstruktora.TxtJMBG);
                return false;
            }

            if (string.IsNullOrWhiteSpace(telefon))
            {
                ShowMessage.Error("Unesite broj telefona instruktora.", _ucKreirajInstruktora.TxtTelefon);
                return false;
            }

            if (!telefon.All(c => char.IsDigit(c) || c == '+' || c == '/' || c == '-' || c == ' '))
            {
                ShowMessage.Error("Telefon moze da sadrzi samo cifre i znakove +, -, /.", _ucKreirajInstruktora.TxtTelefon);
                return false;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                ShowMessage.Error("Unesite email instruktora.", _ucKreirajInstruktora.TxtEmail);
                return false;
            }

            if (!Validate.Email(email))
            {
                ShowMessage.Error("Email adresa nije u ispravnom formatu.", _ucKreirajInstruktora.TxtEmail);
                return false;
            }

            if (datumZaposlenja > DateTime.Now.Date)
            {
                ShowMessage.Error("Datum zaposlenja ne moze biti u buducnosti.", _ucKreirajInstruktora.DateTP);
                _ucKreirajInstruktora.DateTP.Focus();
                return false;
            }

            Kategorija kategorija = _ucKreirajInstruktora.CmbKategorija.SelectedItem as Kategorija;
            if (kategorija == null || kategorija.KategorijaID <= 0)
            {
                ShowMessage.Error("Izaberite kategoriju.", _ucKreirajInstruktora.CmbKategorija);
                return false;
            }

            instruktor = new Instruktor()
            {
                Ime = ime,
                Prezime = prezime,
                JMBG = jmbg,
                Telefon = telefon,
                Email = email,
                DatumZaposlenja = datumZaposlenja,
                Aktivan = true
            };

            kategorijaId = kategorija.KategorijaID;
            return true;
        }

        internal Control CreateObrisiInstruktor()
        {

            _ucObrisiInstruktora = new UCObrisiInstruktora();
            PrepareUCObrisiInstruktora();
            _ucObrisiInstruktora.BtnObrisi.Click += ObrisiInstruktora;

            return _ucObrisiInstruktora;
        }

        private void PrepareUCObrisiInstruktora()
        {
            List<Instruktor> i = Communication.Instance.GetAllInstruktori();
            instruktori = new BindingList<Instruktor>(i);
            _ucObrisiInstruktora.CmbInstruktori.DataSource = instruktori;


        }

        private void ObrisiInstruktora(object sender, EventArgs e)
        {
            Instruktor instruktor = _ucObrisiInstruktora.CmbInstruktori.SelectedItem as Instruktor;
            if (instruktor == null)
            {
                ShowMessage.Warning("Nema instruktora dostupnih za brisanje.", "Greska");
                _ucObrisiInstruktora.CmbInstruktori.Focus();
                return;
            }
            try
            {
                Response response = Communication.Instance.ObrisiInstruktora(instruktor);
                if (!string.IsNullOrEmpty(response.ErrorMessage))
                {
                    ShowMessage.Error(response.ErrorMessage);
                    return;
                }

                ShowMessage.Success("Sistem je uspesno obrisao instruktora.");
                PrepareUCObrisiInstruktora();
            }
            catch (Exception)
            {
                ShowMessage.ServerDown();
            }

        }
    }
}
