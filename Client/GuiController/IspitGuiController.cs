using Client.UserControls.UCIspit;
using Client.Utils;
using Common.Communication;
using Common.Domain;
using Common.Domain.Izvestaji;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.GuiController
{
    internal class IspitGuiController
    {
        private UCEvidentirajIspit _ucEvidentirajIspit;
        private UCKreirajIzvestajOIspitima _ucKreirajIzvestaj;
        private BindingList<Kategorija> _kategorije;
        private BindingList<Kandidat> _kandidati;
        private List<IzvestajProlaznostiStavkaDto> _poslednjeStavke;

        private class TipIspitaOption
        {
            public string Naziv { get; set; }
            public TipIspitaFilter Vrednost { get; set; }
        }

        private class StringOption
        {
            public string Naziv { get; set; }
            public string Vrednost { get; set; }
        }

        internal Control CreateEvidentirajIspit()
        {
            _ucEvidentirajIspit = new UCEvidentirajIspit();
            PrepareEvidentirajIspitControl();
            _ucEvidentirajIspit.BtnSacuvaj.Click += BtnSacuvajIspit_Click;
            return _ucEvidentirajIspit;
        }

        private void PrepareEvidentirajIspitControl()
        {
            _ucEvidentirajIspit.DtpDatumIspita.Value = DateTime.Today;

            _ucEvidentirajIspit.CmbTip.DisplayMember = "Naziv";
            _ucEvidentirajIspit.CmbTip.ValueMember = "Vrednost";
            _ucEvidentirajIspit.CmbTip.DataSource = new List<StringOption>
            {
                new StringOption { Naziv = "Teorijski", Vrednost = "teorijski" },
                new StringOption { Naziv = "Prakticni", Vrednost = "prakticni" }
            };

            _ucEvidentirajIspit.CmbRezultat.DisplayMember = "Naziv";
            _ucEvidentirajIspit.CmbRezultat.ValueMember = "Vrednost";
            _ucEvidentirajIspit.CmbRezultat.DataSource = new List<StringOption>
            {
                new StringOption { Naziv = "Polozio", Vrednost = "polozio" },
                new StringOption { Naziv = "Pao", Vrednost = "pao" },
                new StringOption { Naziv = "Nije pristupio", Vrednost = "nije_pristupio" }
            };

            try
            {
                List<Kandidat> sviKandidati = Communication.Instance.GetAllKandidati(upisani: true) ?? new List<Kandidat>();
                _kandidati = new BindingList<Kandidat>(sviKandidati);
                _ucEvidentirajIspit.CmbKandidat.DataSource = _kandidati;

                bool imaKandidata = _kandidati.Count > 0;
                _ucEvidentirajIspit.CmbKandidat.Enabled = imaKandidata;
                _ucEvidentirajIspit.BtnSacuvaj.Enabled = imaKandidata;

                if (!imaKandidata)
                {
                    ShowMessage.Info("Nema kandidata za evidentiranje ispita.");
                }
            }
            catch (Exception)
            {
                ShowMessage.ServerDown();
                _ucEvidentirajIspit.BtnSacuvaj.Enabled = false;
            }
        }

        private void BtnSacuvajIspit_Click(object sender, EventArgs e)
        {
            if (!TryBuildEvidentirajIspitRequest(out EvidentirajIspitRequest request))
            {
                return;
            }

            _ucEvidentirajIspit.BtnSacuvaj.Enabled = false;
            try
            {
                Response response = Communication.Instance.EvidentirajIspit(request);

                if (!string.IsNullOrEmpty(response.ErrorMessage))
                {
                    ShowMessage.Error(response.ErrorMessage);
                    return;
                }

                if (response.Result == null)
                {
                    ShowMessage.Error("Sistem ne moze da evidentira ispit.");
                    return;
                }
                EvidentirajIspitResponse result = Communication.Instance.ResultAs<EvidentirajIspitResponse>(response);

                ShowMessage.Success(result.Poruka ?? "Ispit je uspesno evidentiran.");
                ResetEvidentirajIspitForm();
            }
            catch (Exception ex) when (ex is SocketException || ex is IOException)
            {
                ShowMessage.ServerDown();
            }
            catch (Exception ex)
            {
                ShowMessage.Error(ex.Message);
            }
            finally
            {
                _ucEvidentirajIspit.BtnSacuvaj.Enabled = true;
            }
        }

        private bool TryBuildEvidentirajIspitRequest(out EvidentirajIspitRequest request)
        {
            request = null;

            Kandidat kandidat = _ucEvidentirajIspit.CmbKandidat.SelectedItem as Kandidat;
            if (kandidat == null)
            {
                ShowMessage.Error("Izaberite kandidata.", _ucEvidentirajIspit.CmbKandidat);
                return false;
            }

            DateTime datumIspita = _ucEvidentirajIspit.DtpDatumIspita.Value.Date;
            if (datumIspita > DateTime.Today)
            {
                ShowMessage.Error("Datum ispita ne moze biti u buducnosti.", _ucEvidentirajIspit.DtpDatumIspita);
                return false;
            }

            StringOption tip = _ucEvidentirajIspit.CmbTip.SelectedItem as StringOption;
            if (tip == null || string.IsNullOrWhiteSpace(tip.Vrednost))
            {
                ShowMessage.Error("Izaberite tip ispita.", _ucEvidentirajIspit.CmbTip);
                return false;
            }

            StringOption rezultat = _ucEvidentirajIspit.CmbRezultat.SelectedItem as StringOption;
            if (rezultat == null || string.IsNullOrWhiteSpace(rezultat.Vrednost))
            {
                ShowMessage.Error("Izaberite rezultat ispita.", _ucEvidentirajIspit.CmbRezultat);
                return false;
            }

            request = new EvidentirajIspitRequest
            {
                KandidatId = kandidat.KandidatId,
                DatumIspita = datumIspita,
                Tip = tip.Vrednost,
                Rezultat = rezultat.Vrednost,
                Napomena = (_ucEvidentirajIspit.TxtNapomena.Text ?? string.Empty).Trim()
            };

            return true;
        }

        private void ResetEvidentirajIspitForm()
        {
            _ucEvidentirajIspit.DtpDatumIspita.Value = DateTime.Today;
            _ucEvidentirajIspit.TxtNapomena.Clear();
            _ucEvidentirajIspit.CmbTip.SelectedIndex = 0;
            _ucEvidentirajIspit.CmbRezultat.SelectedIndex = 0;
        }

        internal Control CreateIzvestajProlaznosti()
        {
            _ucKreirajIzvestaj = new UCKreirajIzvestajOIspitima();
            PrepareControl();

            _ucKreirajIzvestaj.BtnKreirajIzvestaj.Click += BtnKreirajIzvestaj_Click;
            _ucKreirajIzvestaj.BtnIzveziCsv.Click += BtnIzveziCsv_Click;

            _ucKreirajIzvestaj.BtnPresetMesec.Click += (s, e) =>
            {
                DateTime today = DateTime.Today;
                _ucKreirajIzvestaj.DtpDatumOd.Value = new DateTime(today.Year, today.Month, 1);
                _ucKreirajIzvestaj.DtpDatumDo.Value = today;
            };
            _ucKreirajIzvestaj.BtnPresetTridesetDana.Click += (s, e) =>
            {
                DateTime today = DateTime.Today;
                _ucKreirajIzvestaj.DtpDatumOd.Value = today.AddDays(-29);
                _ucKreirajIzvestaj.DtpDatumDo.Value = today;
            };
            _ucKreirajIzvestaj.BtnPresetGodina.Click += (s, e) =>
            {
                DateTime today = DateTime.Today;
                _ucKreirajIzvestaj.DtpDatumOd.Value = new DateTime(today.Year, 1, 1);
                _ucKreirajIzvestaj.DtpDatumDo.Value = today;
            };

            return _ucKreirajIzvestaj;
        }

        private void PrepareControl()
        {
            _ucKreirajIzvestaj.DtpDatumOd.Value = DateTime.Today.AddMonths(-1);
            _ucKreirajIzvestaj.DtpDatumDo.Value = DateTime.Today;

            _ucKreirajIzvestaj.CmbTipIspita.DisplayMember = "Naziv";
            _ucKreirajIzvestaj.CmbTipIspita.ValueMember = "Vrednost";
            _ucKreirajIzvestaj.CmbTipIspita.DataSource = new List<TipIspitaOption>
            {
                new TipIspitaOption { Naziv = "Teorijski", Vrednost = TipIspitaFilter.Teorijski },
                new TipIspitaOption { Naziv = "Prakticni", Vrednost = TipIspitaFilter.Prakticni },
                new TipIspitaOption { Naziv = "Oba", Vrednost = TipIspitaFilter.Oba }
            };

            try
            {
                List<Kategorija> sveKategorije = Communication.Instance.GetAllKategorije() ?? new List<Kategorija>();
                _kategorije = new BindingList<Kategorija>(sveKategorije);
                _ucKreirajIzvestaj.CmbKategorija.DataSource = _kategorije;

                bool imaKategorija = _kategorije.Count > 0;
                _ucKreirajIzvestaj.CmbKategorija.Enabled = imaKategorija;
                _ucKreirajIzvestaj.BtnKreirajIzvestaj.Enabled = imaKategorija;

                if (!imaKategorija)
                {
                    ShowMessage.Warning("Kategorije nisu dostupne. Izvestaj nije moguce kreirati.", "Upozorenje");
                }
            }
            catch (Exception)
            {
                ShowMessage.ServerDown();
                _ucKreirajIzvestaj.BtnKreirajIzvestaj.Enabled = false;
            }

            ConfigureProlaznostGrid();
            ResetSummaryForProlaznost();
        }

        private void BtnKreirajIzvestaj_Click(object sender, EventArgs e)
        {
            if (!TryBuildProlaznostKriterijum(out IzvestajProlaznostiKriterijum kriterijum))
            {
                return;
            }

            _ucKreirajIzvestaj.BtnKreirajIzvestaj.Enabled = false;
            _ucKreirajIzvestaj.Cursor = Cursors.WaitCursor;
            try
            {
                Response response = Communication.Instance.KreirajIzvestajProlaznosti(kriterijum);
                if (!string.IsNullOrEmpty(response.ErrorMessage))
                {
                    ShowMessage.Error(response.ErrorMessage);
                    return;
                }

                if (response.Result == null)
                {
                    ShowMessage.Error("Sistem ne moze da kreira izvestaj prolaznosti.");
                    return;
                }
                IzvestajProlaznostiResponseDto result = Communication.Instance.ResultAs<IzvestajProlaznostiResponseDto>(response);

                BindProlaznostResult(result);
            }
            catch (Exception ex) when (ex is SocketException || ex is IOException)
            {
                ShowMessage.ServerDown();
            }
            catch (Exception ex)
            {
                ShowMessage.Error(ex.Message);
            }
            finally
            {
                _ucKreirajIzvestaj.Cursor = Cursors.Default;
                _ucKreirajIzvestaj.BtnKreirajIzvestaj.Enabled = true;
            }
        }
        private bool TryBuildProlaznostKriterijum(out IzvestajProlaznostiKriterijum kriterijum)
        {
            kriterijum = null;

            DateTime datumOd = DateTime.MinValue;
            DateTime datumDo = DateTime.MinValue;
            string kategorija = null;

            if (_ucKreirajIzvestaj.DtpDatumOd.Value.Date > _ucKreirajIzvestaj.DtpDatumDo.Value.Date)
            {
                ShowMessage.Error("Datum od mora biti manji ili jednak datumu do.", _ucKreirajIzvestaj.DtpDatumOd);
                return false;
            }

            if (!(_ucKreirajIzvestaj.CmbKategorija.SelectedItem is Kategorija selectedKategorija)
                || string.IsNullOrWhiteSpace(selectedKategorija.NazivKategorije))
            {
                ShowMessage.Error("Izaberite kategoriju.", _ucKreirajIzvestaj.CmbKategorija);
                return false;
            }

            datumOd = _ucKreirajIzvestaj.DtpDatumOd.Value.Date;
            datumDo = _ucKreirajIzvestaj.DtpDatumDo.Value.Date;
            kategorija = selectedKategorija.NazivKategorije.Trim();

            if (!(_ucKreirajIzvestaj.CmbTipIspita.SelectedItem is TipIspitaOption tipOption))
            {
                ShowMessage.Error("Izaberite tip ispita.", _ucKreirajIzvestaj.CmbTipIspita);
                return false;
            }

            kriterijum = new IzvestajProlaznostiKriterijum
            {
                DatumOd = datumOd,
                DatumDo = datumDo,
                Kategorija = kategorija,
                TipIspita = tipOption.Vrednost,
                IncludeNoData = _ucKreirajIzvestaj.ChbUkljuciBezRezultata.Checked,
                IncludeOnlyAktivanUpis = _ucKreirajIzvestaj.ChbSamoAktivniUpisi.Checked
            };

            return true;
        }

        private void ToggleButton(bool enabled)
        {
            _ucKreirajIzvestaj.BtnKreirajIzvestaj.Enabled = enabled;
        }

        private void ConfigureProlaznostGrid()
        {
            DataGridView dgv = _ucKreirajIzvestaj.DgvIzvestajIspita;
            dgv.AutoGenerateColumns = false;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.MultiSelect = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.Columns.Clear();

            AddColumn(nameof(IzvestajProlaznostiStavkaDto.Ime), "Ime", fillWeight: 80, minWidth: 80);
            AddColumn(nameof(IzvestajProlaznostiStavkaDto.Prezime), "Prezime", fillWeight: 100, minWidth: 90);
            AddColumn(nameof(IzvestajProlaznostiStavkaDto.Jmbg), "JMBG", fillWeight: 100, minWidth: 120);
            AddColumn(nameof(IzvestajProlaznostiStavkaDto.Kategorija), "Kategorija", fillWeight: 60, minWidth: 80);
            AddColumn(nameof(IzvestajProlaznostiStavkaDto.Status), "Status", fillWeight: 80, minWidth: 90);

            DataGridViewTextBoxColumn datumKolona = AddColumn(nameof(IzvestajProlaznostiStavkaDto.DatumPoslednjegIspita), "Poslednji ispit", fillWeight: 80, minWidth: 110);
            datumKolona.DefaultCellStyle.Format = "dd.MM.yyyy";

            AddColumn(nameof(IzvestajProlaznostiStavkaDto.BrojPokusajaTeorijski), "Teorijski pokusaji", fillWeight: 100, minWidth: 110);
            AddColumn(nameof(IzvestajProlaznostiStavkaDto.BrojPokusajaPrakticni), "Prakticni pokusaji", fillWeight: 100, minWidth: 110);

            dgv.CellFormatting -= ProlaznostGrid_CellFormatting;
            dgv.CellFormatting += ProlaznostGrid_CellFormatting;
        }

        private static readonly Color BojaPolozio = Color.FromArgb(200, 230, 201);
        private static readonly Color BojaPao = Color.FromArgb(255, 205, 210);
        private static readonly Color BojaUToku = Color.FromArgb(238, 238, 238);

        private void ProlaznostGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            DataGridView dgv = (DataGridView)sender;
            DataGridViewRow row = dgv.Rows[e.RowIndex];
            object rawStatus = row.Cells[nameof(IzvestajProlaznostiStavkaDto.Status)].Value;
            if (rawStatus is not StatusProlaznosti status)
            {
                return;
            }

            string columnName = dgv.Columns[e.ColumnIndex].DataPropertyName;
            if (columnName == nameof(IzvestajProlaznostiStavkaDto.Status))
            {
                e.Value = StatusDisplay(status);
                e.FormattingApplied = true;
            }

            switch (status)
            {
                case StatusProlaznosti.Polozio:
                    row.DefaultCellStyle.BackColor = BojaPolozio;
                    row.DefaultCellStyle.Font = new Font(dgv.Font, FontStyle.Regular);
                    break;
                case StatusProlaznosti.Pao:
                    row.DefaultCellStyle.BackColor = BojaPao;
                    row.DefaultCellStyle.Font = new Font(dgv.Font, FontStyle.Regular);
                    break;
                case StatusProlaznosti.UToku:
                    row.DefaultCellStyle.BackColor = BojaUToku;
                    row.DefaultCellStyle.Font = new Font(dgv.Font, FontStyle.Italic);
                    break;
            }
        }

        private static string StatusDisplay(StatusProlaznosti status)
        {
            switch (status)
            {
                case StatusProlaznosti.Polozio: return "Položio";
                case StatusProlaznosti.Pao: return "Pao";
                case StatusProlaznosti.UToku: return "U toku";
                default: return status.ToString();
            }
        }

        private void ConfigureDugovanjaGrid()
        {
            DataGridView dgv = _ucKreirajIzvestaj.DgvIzvestajIspita;
            dgv.AutoGenerateColumns = false;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.MultiSelect = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.Columns.Clear();

            AddColumn(nameof(IzvestajDugovanjaStavkaDto.Ime), "Ime");
            AddColumn(nameof(IzvestajDugovanjaStavkaDto.Prezime), "Prezime");
            AddColumn(nameof(IzvestajDugovanjaStavkaDto.Jmbg), "JMBG");
            AddColumn(nameof(IzvestajDugovanjaStavkaDto.Kategorija), "Kategorija", 90);
            AddColumn(nameof(IzvestajDugovanjaStavkaDto.BrojUpisa), "Broj upisa", 90);

            DataGridViewTextBoxColumn cena = AddColumn(nameof(IzvestajDugovanjaStavkaDto.UkupnaCenaObuke), "Ukupno zaduzenje", 120);
            cena.DefaultCellStyle.Format = "N2";

            DataGridViewTextBoxColumn placeno = AddColumn(nameof(IzvestajDugovanjaStavkaDto.UkupnoPlaceno), "Ukupno placeno", 120);
            placeno.DefaultCellStyle.Format = "N2";

            DataGridViewTextBoxColumn dug = AddColumn(nameof(IzvestajDugovanjaStavkaDto.Dugovanje), "Dugovanje", 110);
            dug.DefaultCellStyle.Format = "N2";

            AddColumn(nameof(IzvestajDugovanjaStavkaDto.StatusDuga), "Status", 90);
        }

        private DataGridViewTextBoxColumn AddColumn(string dataProperty, string headerText, int fillWeight = 100, int minWidth = 80)
        {
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn
            {
                Name = dataProperty,
                DataPropertyName = dataProperty,
                HeaderText = headerText,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = fillWeight,
                MinimumWidth = minWidth,
                SortMode = DataGridViewColumnSortMode.Automatic
            };

            _ucKreirajIzvestaj.DgvIzvestajIspita.Columns.Add(col);
            return col;
        }

        private void BindProlaznostResult(IzvestajProlaznostiResponseDto result)
        {
            List<IzvestajProlaznostiStavkaDto> stavke = result.Stavke ?? new List<IzvestajProlaznostiStavkaDto>();
            _poslednjeStavke = stavke;

            _ucKreirajIzvestaj.DgvIzvestajIspita.DataSource = new BindingList<IzvestajProlaznostiStavkaDto>(stavke);

            if (stavke.Count == 0)
            {
                ResetSummaryForProlaznost();
                _ucKreirajIzvestaj.LblNemaRezultata.Visible = true;
                _ucKreirajIzvestaj.LblNemaRezultata.BringToFront();
                _ucKreirajIzvestaj.BtnIzveziCsv.Enabled = false;
                return;
            }

            _ucKreirajIzvestaj.LblNemaRezultata.Visible = false;
            _ucKreirajIzvestaj.BtnIzveziCsv.Enabled = true;

            _ucKreirajIzvestaj.LblUkupnoKandidata.Text = string.Format("Prikazano kandidata: {0}", stavke.Count);
            _ucKreirajIzvestaj.LblVrednostPolozilo.Text = (result.Summary?.UkupnoPolozilo ?? 0).ToString();
            _ucKreirajIzvestaj.LblVrednostPalo.Text = (result.Summary?.UkupnoPalo ?? 0).ToString();
            _ucKreirajIzvestaj.LblVrednostUToku.Text = (result.Summary?.UkupnoUToku ?? 0).ToString();
            _ucKreirajIzvestaj.LblVrednostProcenat.Text = string.Format("{0:N2}%", result.Summary?.ProcenatProlaznosti ?? 0m);
        }

        private void ResetSummaryForProlaznost()
        {
            _ucKreirajIzvestaj.LblUkupnoKandidata.Text = "Prikazano kandidata: 0";
            _ucKreirajIzvestaj.LblVrednostPolozilo.Text = "0";
            _ucKreirajIzvestaj.LblVrednostPalo.Text = "0";
            _ucKreirajIzvestaj.LblVrednostUToku.Text = "0";
            _ucKreirajIzvestaj.LblVrednostProcenat.Text = "0,00%";
        }

        private void BtnIzveziCsv_Click(object sender, EventArgs e)
        {
            if (_poslednjeStavke == null || _poslednjeStavke.Count == 0)
            {
                ShowMessage.Info("Nema podataka za izvoz.");
                return;
            }

            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "CSV fajl (*.csv)|*.csv";
                dialog.FileName = string.Format("izvestaj_prolaznosti_{0:yyyyMMdd_HHmmss}.csv", DateTime.Now);

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    WriteProlaznostCsv(dialog.FileName, _poslednjeStavke);
                    ShowMessage.Success("Izvestaj je sacuvan.");
                }
                catch (Exception ex)
                {
                    ShowMessage.Error("Greska pri snimanju CSV fajla: " + ex.Message);
                }
            }
        }

        private static void WriteProlaznostCsv(string path, List<IzvestajProlaznostiStavkaDto> stavke)
        {
            using (StreamWriter writer = new StreamWriter(path, false, new UTF8Encoding(true)))
            {
                writer.WriteLine("Ime;Prezime;JMBG;Kategorija;Status;Poslednji ispit;Teorijski pokusaji;Prakticni pokusaji");

                foreach (IzvestajProlaznostiStavkaDto s in stavke)
                {
                    writer.Write(EscapeCsv(s.Ime));
                    writer.Write(';');
                    writer.Write(EscapeCsv(s.Prezime));
                    writer.Write(';');
                    writer.Write(EscapeCsv(s.Jmbg));
                    writer.Write(';');
                    writer.Write(EscapeCsv(s.Kategorija));
                    writer.Write(';');
                    writer.Write(EscapeCsv(StatusDisplay(s.Status)));
                    writer.Write(';');
                    writer.Write(s.DatumPoslednjegIspita.HasValue
                        ? s.DatumPoslednjegIspita.Value.ToString("dd.MM.yyyy")
                        : string.Empty);
                    writer.Write(';');
                    writer.Write(s.BrojPokusajaTeorijski);
                    writer.Write(';');
                    writer.WriteLine(s.BrojPokusajaPrakticni);
                }
            }
        }

        private static string EscapeCsv(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            if (value.IndexOfAny(new[] { ';', '"', '\n', '\r' }) < 0)
            {
                return value;
            }
            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }
    }
}
