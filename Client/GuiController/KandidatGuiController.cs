using Client.UserControls.UCKandidat;
using Common.Communication;
using Common.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Windows.Forms;

namespace Client.GuiController
{
    internal class KandidatGuiController
    {
        private UCKreirajKandidata _ucKreirajKandidata;
        BindingList<Kategorija> kategorije;

        internal Control CreateKandidat()
        {
            _ucKreirajKandidata = new UCKreirajKandidata();
            PrepareUCKreirajKandidata();
            _ucKreirajKandidata.BtnKreirajKandidata.Click += KreirajKandidata;

            return _ucKreirajKandidata;
        }

        private void PrepareUCKreirajKandidata()
        {
            List<Kategorija> k = Communication.Instance.GetAllKategorije();
            kategorije = new BindingList<Kategorija>(k);
            _ucKreirajKandidata.CmbKategorije.DataSource = kategorije;
            _ucKreirajKandidata.CmbKategorije.DisplayMember = nameof(Kategorija.NazivKategorije);
            _ucKreirajKandidata.CmbKategorije.ValueMember = nameof(Kategorija.KategorijaID);

        }
        private void KreirajKandidata(object senter, EventArgs e)
        {
            if (!TryCreateKandidatFromInput(out Kandidat kandidat))
            {
                return;
            }

            try
            {
                Response response = Communication.Instance.CreateKandidat(kandidat);
                if (response.Exception != null)
                {
                    MessageBox.Show(GetCreateKandidatErrorMessage(response.Exception), "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (response.Result == null)
                {
                    MessageBox.Show("Sistem ne moze da kreira kandidata.", "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Sistem je uspesno kreirao kandidata.", "Uspeh", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
            }
            catch (Exception)
            {
                MessageBox.Show("Server je ugasen!", "Greska", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private bool TryCreateKandidatFromInput(out Kandidat kandidat)
        {
            kandidat = null;

            string ime = _ucKreirajKandidata.TxtIme.Text.Trim();
            string prezime = _ucKreirajKandidata.TxtPrezime.Text.Trim();
            string jmbg = _ucKreirajKandidata.TxtJMBG.Text.Trim();
            string telefon = _ucKreirajKandidata.TxtTelefon.Text.Trim();
            string email = _ucKreirajKandidata.TxtEmail.Text.Trim();
            string adresa = _ucKreirajKandidata.TxtAdresa.Text.Trim();
            DateTime datumUpisa = _ucKreirajKandidata.DateTimePicker1.Value.Date;
            Kategorija kategorija = _ucKreirajKandidata.CmbKategorije.SelectedItem as Kategorija;

            if (string.IsNullOrWhiteSpace(ime))
            {
                ShowValidationError("Unesite ime kandidata.", _ucKreirajKandidata.TxtIme);
                return false;
            }

            if (string.IsNullOrWhiteSpace(prezime))
            {
                ShowValidationError("Unesite prezime kandidata.", _ucKreirajKandidata.TxtPrezime);
                return false;
            }

            if (string.IsNullOrWhiteSpace(jmbg))
            {
                ShowValidationError("Unesite JMBG kandidata.", _ucKreirajKandidata.TxtJMBG);
                return false;
            }

            if (jmbg.Length != 13 || !jmbg.All(char.IsDigit))
            {
                ShowValidationError("JMBG mora da sadrzi tacno 13 cifara.", _ucKreirajKandidata.TxtJMBG);
                return false;
            }

            if (string.IsNullOrWhiteSpace(telefon))
            {
                ShowValidationError("Unesite broj telefona kandidata.", _ucKreirajKandidata.TxtTelefon);
                return false;
            }

            if (!telefon.All(c => char.IsDigit(c) || c == '+' || c == '/' || c == '-' || c == ' '))
            {
                ShowValidationError("Telefon moze da sadrzi samo cifre i znakove +, -, /.", _ucKreirajKandidata.TxtTelefon);
                return false;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                ShowValidationError("Unesite email kandidata.", _ucKreirajKandidata.TxtEmail);
                return false;
            }

            if (!IsValidEmail(email))
            {
                ShowValidationError("Email adresa nije u ispravnom formatu.", _ucKreirajKandidata.TxtEmail);
                return false;
            }

            if (string.IsNullOrWhiteSpace(adresa))
            {
                ShowValidationError("Unesite adresu kandidata.", _ucKreirajKandidata.TxtAdresa);
                return false;
            }

            if (datumUpisa > DateTime.Now.Date)
            {
                MessageBox.Show("Datum upisa ne moze biti u buducnosti.", "Greska pri unosu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _ucKreirajKandidata.DateTimePicker1.Focus();
                return false;
            }

            if (kategorija == null)
            {
                MessageBox.Show("Odaberite kategoriju.", "Greska pri unosu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _ucKreirajKandidata.CmbKategorije.Focus();
                return false;
            }

            kandidat = new Kandidat
            {
                Ime = ime,
                Prezime = prezime,
                JMBG = jmbg,
                Telefon = telefon,
                Email = email,
                Adresa = adresa,
                DatumUpisa = datumUpisa,
                Kategorija = kategorija,
                Aktivan = true
            };

            return true;
        }

        private void ShowValidationError(string message, Control control)
        {
            MessageBox.Show(message, "Greska pri unosu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            control.Focus();
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                MailAddress mailAddress = new MailAddress(email);
                return mailAddress.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private string GetCreateKandidatErrorMessage(Exception exception)
        {
            if (exception == null || string.IsNullOrWhiteSpace(exception.Message))
            {
                return "Sistem ne moze da kreira kandidata. Pokusajte ponovo.";
            }

            string originalMessage = exception.Message.Trim();
            string message = originalMessage.ToLower();

            if (message.Contains("duplicate") || message.Contains("unique"))
            {
                if (message.Contains("jmbg"))
                {
                    return "Kandidat sa unetim JMBG vec postoji u sistemu.";
                }

                if (message.Contains("email"))
                {
                    return "Kandidat sa unetom email adresom vec postoji u sistemu.";
                }

                return "Kandidat sa unetim podacima vec postoji u sistemu.";
            }

            return originalMessage;
        }

        private void ClearForm()
        {
            _ucKreirajKandidata.TxtIme.Clear();
            _ucKreirajKandidata.TxtPrezime.Clear();
            _ucKreirajKandidata.TxtJMBG.Clear();
            _ucKreirajKandidata.TxtTelefon.Clear();
            _ucKreirajKandidata.TxtEmail.Clear();
            _ucKreirajKandidata.TxtAdresa.Clear();
            _ucKreirajKandidata.DateTimePicker1.Value = DateTime.Now;

            if (_ucKreirajKandidata.CmbKategorije.Items.Count > 0)
            {
                _ucKreirajKandidata.CmbKategorije.SelectedIndex = 0;
            }

            _ucKreirajKandidata.TxtIme.Focus();

        }
    }
}
