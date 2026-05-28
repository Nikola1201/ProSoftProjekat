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
        private UCPretraziKandidata _ucPretraziKandidata;

        private BindingList<Kandidat> neupisaniKandidati;
        private BindingList<PaketObuke> paketiObuke;
        private BindingList<PaketObuke> prikazaniPaketiObuke;
        private BindingList<Kandidat> upisaniKandidati;
        private BindingList<Kandidat> pretrazeniKandidati;

        private string _sortKolona = nameof(Kandidat.Prezime);
        private bool _sortRastuce = true;

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
                if (!string.IsNullOrEmpty(response.ErrorMessage))
                {
                    ShowMessage.Error(GetCreateKandidatErrorMessage(response.ErrorMessage));
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

        private string GetCreateKandidatErrorMessage(string? errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                return "Sistem ne moze da kreira kandidata. Pokusajte ponovo.";
            }

            string originalMessage = errorMessage.Trim();
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
                ShowMessage.Warning("Nema dostupnih paketa obuke.", "Greska");
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
                if (!string.IsNullOrEmpty(response.ErrorMessage))
                {
                    ShowMessage.Error(response.ErrorMessage);
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
                if (!string.IsNullOrEmpty(response.ErrorMessage))
                {
                    ShowMessage.Error(response.ErrorMessage);
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

        internal Control CreatePretraziKandidata()
        {
            _ucPretraziKandidata = new UCPretraziKandidata();
            ConfigurePretragaTabela();
            BindPretragaRezultati(new List<Kandidat>());

            _ucPretraziKandidata.BtnPretrazi.Click += BtnPretrazi_Click;
            _ucPretraziKandidata.DgvKandidati.ColumnHeaderMouseClick += DgvKandidati_ColumnHeaderMouseClick;

            return _ucPretraziKandidata;
        }

        private void BtnPretrazi_Click(object sender, EventArgs e)
        {
            if (!TryCreatePretragaFilter(out KandidatSearchFilter filter))
            {
                return;
            }

            try
            {
                List<Kandidat> rezultat = Communication.Instance.PretraziKandidate(filter) ?? new List<Kandidat>();
                BindPretragaRezultati(rezultat);

                if (rezultat.Count == 0)
                {
                    ShowMessage.Info("Nema kandidata za zadate kriterijume pretrage.");
                }
            }
            catch (Exception ex)
            {
                ShowMessage.Error(ex.Message);
            }
        }

        private bool TryCreatePretragaFilter(out KandidatSearchFilter filter)
        {
            filter = null;

            string ime = _ucPretraziKandidata.TxtIme.Text.Trim();
            string prezime = _ucPretraziKandidata.TxtPrezime.Text.Trim();
            string jmbg = _ucPretraziKandidata.TxtJMBG.Text.Trim();
            string email = _ucPretraziKandidata.TxtEmail.Text.Trim();

            if (!string.IsNullOrWhiteSpace(jmbg) && (jmbg.Length != 13 || !jmbg.All(char.IsDigit)))
            {
                ShowMessage.Error("JMBG mora da sadrzi tacno 13 cifara.", _ucPretraziKandidata.TxtJMBG);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(email) && !Validate.Email(email))
            {
                ShowMessage.Error("Email adresa nije u ispravnom formatu.", _ucPretraziKandidata.TxtEmail);
                return false;
            }

            filter = new KandidatSearchFilter
            {
                Ime = ime,
                Prezime = prezime,
                JMBG = jmbg,
                Email = email,
                SamoAktivni = _ucPretraziKandidata.ChkSamoAktivni.Checked
            };

            return true;
        }

        private void ConfigurePretragaTabela()
        {
            DataGridView dgv = _ucPretraziKandidata.DgvKandidati;
            dgv.AutoGenerateColumns = false;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.MultiSelect = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.Columns.Clear();

            AddTabelaKolona(nameof(Kandidat.Ime), "Ime");
            AddTabelaKolona(nameof(Kandidat.Prezime), "Prezime");
            AddTabelaKolona(nameof(Kandidat.JMBG), "JMBG");
            AddTabelaKolona(nameof(Kandidat.Telefon), "Telefon");
            AddTabelaKolona(nameof(Kandidat.Email), "Email");
            AddTabelaKolona(nameof(Kandidat.Adresa), "Adresa");

            DataGridViewTextBoxColumn datumUpisa = AddTabelaKolona(nameof(Kandidat.DatumUpisa), "DatumUpisa");
            datumUpisa.DefaultCellStyle.Format = "dd.MM.yyyy";

            AddTabelaKolona(nameof(Kandidat.Aktivan), "Aktivan");
        }

        private DataGridViewTextBoxColumn AddTabelaKolona(string dataPropertyName, string headerText)
        {
            DataGridViewTextBoxColumn kolona = new DataGridViewTextBoxColumn
            {
                DataPropertyName = dataPropertyName,
                HeaderText = headerText,
                SortMode = DataGridViewColumnSortMode.Programmatic,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };

            _ucPretraziKandidata.DgvKandidati.Columns.Add(kolona);
            return kolona;
        }

        private void BindPretragaRezultati(List<Kandidat> rezultat)
        {
            List<Kandidat> source = rezultat ?? new List<Kandidat>();
            pretrazeniKandidati = new BindingList<Kandidat>(source);
            _ucPretraziKandidata.DgvKandidati.DataSource = pretrazeniKandidati;
        }

        private void DgvKandidati_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0)
            {
                return;
            }

            DataGridViewColumn kolona = _ucPretraziKandidata.DgvKandidati.Columns[e.ColumnIndex];
            string dataPropertyName = kolona.DataPropertyName;

            if (string.IsNullOrWhiteSpace(dataPropertyName))
            {
                return;
            }

            if (_sortKolona == dataPropertyName)
            {
                _sortRastuce = !_sortRastuce;
            }
            else
            {
                _sortKolona = dataPropertyName;
                _sortRastuce = true;
            }

            PrimeniSortiranje();
            PostaviSortGlyph(kolona);
        }

        private void PrimeniSortiranje()
        {
            if (pretrazeniKandidati == null)
            {
                return;
            }

            IEnumerable<Kandidat> query = pretrazeniKandidati.ToList();
            bool rastuce = _sortRastuce;

            switch (_sortKolona)
            {
                case nameof(Kandidat.Ime):
                    query = rastuce ? query.OrderBy(k => k.Ime).ThenBy(k => k.Prezime) : query.OrderByDescending(k => k.Ime).ThenByDescending(k => k.Prezime);
                    break;
                case nameof(Kandidat.JMBG):
                    query = rastuce ? query.OrderBy(k => k.JMBG) : query.OrderByDescending(k => k.JMBG);
                    break;
                case nameof(Kandidat.Telefon):
                    query = rastuce ? query.OrderBy(k => k.Telefon) : query.OrderByDescending(k => k.Telefon);
                    break;
                case nameof(Kandidat.Email):
                    query = rastuce ? query.OrderBy(k => k.Email) : query.OrderByDescending(k => k.Email);
                    break;
                case nameof(Kandidat.Adresa):
                    query = rastuce ? query.OrderBy(k => k.Adresa) : query.OrderByDescending(k => k.Adresa);
                    break;
                case nameof(Kandidat.DatumUpisa):
                    query = rastuce ? query.OrderBy(k => k.DatumUpisa) : query.OrderByDescending(k => k.DatumUpisa);
                    break;
                case nameof(Kandidat.Aktivan):
                    query = rastuce ? query.OrderBy(k => k.Aktivan) : query.OrderByDescending(k => k.Aktivan);
                    break;
                case nameof(Kandidat.Prezime):
                default:
                    query = rastuce ? query.OrderBy(k => k.Prezime).ThenBy(k => k.Ime) : query.OrderByDescending(k => k.Prezime).ThenByDescending(k => k.Ime);
                    break;
            }

            BindPretragaRezultati(query.ToList());
        }

        private void PostaviSortGlyph(DataGridViewColumn aktivnaKolona)
        {
            foreach (DataGridViewColumn kolona in _ucPretraziKandidata.DgvKandidati.Columns)
            {
                kolona.HeaderCell.SortGlyphDirection = SortOrder.None;
            }

            aktivnaKolona.HeaderCell.SortGlyphDirection = _sortRastuce ? SortOrder.Ascending : SortOrder.Descending;
        }
    }
}
