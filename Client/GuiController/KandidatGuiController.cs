using Client.UserControls.UCKandidat;
using Client.Utils;
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
        private UCUpisiKandidata _ucUpisiKandidata;
        private UCObrisiKandidata _ucObrisiKandidata;

        private BindingList<Kandidat> neupisaniKandidati;
        private BindingList<PaketObuke> paketiObuke;
        private BindingList<PaketObuke> prikazaniPaketiObuke;
        private BindingList<Kandidat> upisaniKandidati;

        internal Control CreateKandidat()
        {
            _ucKreirajKandidata = new UCKreirajKandidata();
            _ucKreirajKandidata.BtnKreirajKandidata.Click += KreirajKandidata;

            return _ucKreirajKandidata;
        }

        internal Control CreateUpisKandidata()
        {
            _ucUpisiKandidata = new UCUpisiKandidata();
            PrepareUCUpisiKandidata();
            _ucUpisiKandidata.CmbKandidat.SelectedIndexChanged += CmbKandidat_SelectedIndexChanged;
            _ucUpisiKandidata.BtnUpisi.Click += UpisiKandidata;

            return _ucUpisiKandidata;
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
                    ShowMessage.Error(GetCreateKandidatErrorMessage(response.Exception));
                    return;
                }

                if (response.Result == null)
                {
                    ShowMessage.Error("Sistem ne moze da kreira kandidata.");
                    return;
                }

                ShowMessage.Success("Sistem je uspesno kreirao kandidata.");
                ClearForm();
            }
            catch (Exception)
            {
                ShowMessage.ServerDown();
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

            if (string.IsNullOrWhiteSpace(ime))
            {
                ShowMessage.Error("Unesite ime kandidata.", _ucKreirajKandidata.TxtIme);
                return false;
            }

            if (string.IsNullOrWhiteSpace(prezime))
            {
                ShowMessage.Error("Unesite prezime kandidata.", _ucKreirajKandidata.TxtPrezime);
                return false;
            }

            if (string.IsNullOrWhiteSpace(jmbg))
            {
                ShowMessage.Error("Unesite JMBG kandidata.", _ucKreirajKandidata.TxtJMBG);
                return false;
            }

            if (jmbg.Length != 13 || !jmbg.All(char.IsDigit))
            {
                ShowMessage.Error("JMBG mora da sadrzi tacno 13 cifara.", _ucKreirajKandidata.TxtJMBG);
                return false;
            }

            if (string.IsNullOrWhiteSpace(telefon))
            {
                ShowMessage.Error("Unesite broj telefona kandidata.", _ucKreirajKandidata.TxtTelefon);
                return false;
            }

            if (!telefon.All(c => char.IsDigit(c) || c == '+' || c == '/' || c == '-' || c == ' '))
            {
                ShowMessage.Error("Telefon moze da sadrzi samo cifre i znakove +, -, /.", _ucKreirajKandidata.TxtTelefon);
                return false;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                ShowMessage.Error("Unesite email kandidata.", _ucKreirajKandidata.TxtEmail);
                return false;
            }

            if (!Validate.Email(email))
            {
                ShowMessage.Error("Email adresa nije u ispravnom formatu.", _ucKreirajKandidata.TxtEmail);
                return false;
            }

            if (string.IsNullOrWhiteSpace(adresa))
            {
                ShowMessage.Error("Unesite adresu kandidata.", _ucKreirajKandidata.TxtAdresa);
                return false;
            }

            if (datumUpisa > DateTime.Now.Date)
            {
                ShowMessage.Warning("Datum upisa ne moze biti u buducnosti.", "Greska pri unosu");
                _ucKreirajKandidata.DateTimePicker1.Focus();
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
                Aktivan = true
            };

            return true;
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
            _ucKreirajKandidata.TxtIme.Focus();
        }

        private void PrepareUCUpisiKandidata()
        {
            List<Kandidat> sviKandidati = Communication.Instance.GetAllKandidati(false) ?? new List<Kandidat>();
            List<PaketObuke> sviPaketi = Communication.Instance.GetAllPaketiObuke() ?? new List<PaketObuke>();

            neupisaniKandidati = new BindingList<Kandidat>(sviKandidati);
            _ucUpisiKandidata.CmbKandidat.DataSource = neupisaniKandidati;

            paketiObuke = new BindingList<PaketObuke>(sviPaketi);
            prikazaniPaketiObuke = new BindingList<PaketObuke>(new List<PaketObuke>());
            _ucUpisiKandidata.CmbPaketObuke.DataSource = prikazaniPaketiObuke;

            bool imaKandidata = neupisaniKandidati.Count > 0;
            _ucUpisiKandidata.CmbKandidat.Enabled = imaKandidata;
            _ucUpisiKandidata.CmbPaketObuke.Enabled = imaKandidata;
            _ucUpisiKandidata.BtnUpisi.Enabled = imaKandidata;

            if (!imaKandidata)
            {
                ShowMessage.Info("Nema kandidata dostupnih za upisivanje.");
                return;
            }

            RefreshPaketiObuke();
        }

        private void CmbKandidat_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshPaketiObuke();
        }

        private void RefreshPaketiObuke()
        {
            if (_ucUpisiKandidata == null)
            {
                return;
            }

            Kandidat kandidat = _ucUpisiKandidata.CmbKandidat.SelectedItem as Kandidat;

            if (kandidat == null)
            {
                prikazaniPaketiObuke = new BindingList<PaketObuke>(new List<PaketObuke>());
                _ucUpisiKandidata.CmbPaketObuke.DataSource = prikazaniPaketiObuke;
                _ucUpisiKandidata.CmbPaketObuke.Enabled = false;
                _ucUpisiKandidata.BtnUpisi.Enabled = false;
                return;
            }

            List<PaketObuke> filtriraniPaketi = paketiObuke.ToList();

            prikazaniPaketiObuke = new BindingList<PaketObuke>(filtriraniPaketi);
            _ucUpisiKandidata.CmbPaketObuke.DataSource = prikazaniPaketiObuke;

            bool imaPaketa = filtriraniPaketi.Count > 0;
            _ucUpisiKandidata.CmbPaketObuke.Enabled = imaPaketa;
            _ucUpisiKandidata.BtnUpisi.Enabled = imaPaketa;
        }

        private void UpisiKandidata(object sender, EventArgs e)
        {
            Kandidat kandidat = _ucUpisiKandidata.CmbKandidat.SelectedItem as Kandidat;
            PaketObuke paketObuke = _ucUpisiKandidata.CmbPaketObuke.SelectedItem as PaketObuke;

            if (kandidat == null)
            {
                ShowMessage.Warning("Nema kandidata dostupnih za upis.", "Greska");
                _ucUpisiKandidata.CmbKandidat.Focus();
                return;
            }

            if (paketObuke == null)
            {
                ShowMessage.Warning("Izabrani kandidat nema dostupan paket obuke za svoju kategoriju.", "Greska");
                _ucUpisiKandidata.CmbPaketObuke.Focus();
                return;
            }

            Upis upis = new Upis
            {
                KandidatId = kandidat.KandidatId,
                PaketId = paketObuke.PaketId,
                DatumUpisa = DateTime.Now.Date,
                Status = "aktivan",
                Kandidat = kandidat,
                Paket = paketObuke
            };

            try
            {
                Response response = Communication.Instance.UpisiKandidata(upis);
                if (response.Exception != null)
                {
                    ShowMessage.Error(response.Exception.Message);
                    return;
                }

                ShowMessage.Success("Sistem je uspesno upisao kandidata.");
                PrepareUCUpisiKandidata();
            }
            catch (Exception)
            {
                ShowMessage.ServerDown();
            }
        }

        internal Control CreateObrisiKandidata()
        {
            _ucObrisiKandidata = new UCObrisiKandidata();
            PrepareUCObrisiKandidata();
            _ucObrisiKandidata.BtnObrisi.Click += ObrisiKandidata;

            return _ucObrisiKandidata;
        }

        private void ObrisiKandidata(object sender, EventArgs e)
        {
            Kandidat kandidat = _ucObrisiKandidata.CmbKandidat.SelectedItem as Kandidat;
            if (kandidat == null)
            {
                ShowMessage.Warning("Nema kandidata dostupnih za brisanje.", "Greska");
                _ucObrisiKandidata.CmbKandidat.Focus();
                return;
            }
            try
            {
                Response response = Communication.Instance.ObrisiKandidata(kandidat);
                if (response.Exception != null)
                {
                    ShowMessage.Error(response.Exception.Message);
                    return;
                }

                ShowMessage.Success("Sistem je uspesno obrisao kandidata.");
                PrepareUCObrisiKandidata();
            }
            catch (Exception)
            {
                ShowMessage.ServerDown();
            }
        }

        private void PrepareUCObrisiKandidata()
        {
            List<Kandidat> upisani = Communication.Instance.GetAllKandidati(upisani: true) ?? new List<Kandidat>();
            upisaniKandidati = new BindingList<Kandidat>(upisani);
            _ucObrisiKandidata.CmbKandidat.DataSource = upisaniKandidati;

            bool imaUpisanih = upisaniKandidati.Count > 0;
            _ucObrisiKandidata.CmbKandidat.Enabled = imaUpisanih;
            _ucObrisiKandidata.BtnObrisi.Enabled = imaUpisanih;

            if (!imaUpisanih)
            {
                ShowMessage.Info("Nema upisanih kandidata dostupnih za ispisivanje.");
            }
        }
    }
}
