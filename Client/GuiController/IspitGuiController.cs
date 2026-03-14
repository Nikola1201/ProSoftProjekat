using Client.UserControls.UCIspit;
using Client.Utils;
using Common.Communication;
using Common.Domain;
using Common.Domain.Izvestaji;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.GuiController
{
    internal class IspitGuiController
    {
        private UCKreirajIzvestajOIspitima _ucKreirajIzvestaj;
        private BindingList<Kategorija> _kategorije;

        private class TipIspitaOption
        {
            public string Naziv { get; set; }
            public TipIspitaFilter Vrednost { get; set; }
        }

        internal Control CreateIzvestajProlaznosti()
        {
            _ucKreirajIzvestaj = new UCKreirajIzvestajOIspitima();
            PrepareControl();

            _ucKreirajIzvestaj.BtnKreirajIzvestaj.Click += BtnKreirajIzvestaj_Click;

            return _ucKreirajIzvestaj;
        }

        private void PrepareControl()
        {
            _ucKreirajIzvestaj.DtpDatumOd.Value = DateTime.Today.AddMonths(-1);
            _ucKreirajIzvestaj.DtpDatumDo.Value = DateTime.Today;
            _ucKreirajIzvestaj.CbUtoku.Text = "Ukljuci i kandidate bez rezultata";

            _ucKreirajIzvestaj.TxtUkupnoPolozilo.ReadOnly = true;
            _ucKreirajIzvestaj.TxtUkupnoPalo.ReadOnly = true;
            _ucKreirajIzvestaj.TxtUkupnoUToku.ReadOnly = true;
            _ucKreirajIzvestaj.TxtProcenatProlaznosti.ReadOnly = true;

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
            try
            {
                Response response = Communication.Instance.KreirajIzvestajProlaznosti(kriterijum);
                if (response.Exception != null)
                {
                    ShowMessage.Error(response.Exception.Message);
                    return;
                }

                IzvestajProlaznostiResponseDto result = response.Result as IzvestajProlaznostiResponseDto;
                if (result == null)
                {
                    ShowMessage.Error("Sistem ne moze da kreira izvestaj prolaznosti.");
                    return;
                }

                BindProlaznostResult(result);
                if (result.Stavke == null || result.Stavke.Count == 0)
                {
                    ShowMessage.Info("Nema rezultata za zadate kriterijume.");
                }
            }
            catch (Exception)
            {
                ShowMessage.ServerDown();
            }
            finally
            {
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
                IncludeNoData = _ucKreirajIzvestaj.CbUtoku.Checked,
                IncludeOnlyAktivanUpis = false
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

            AddColumn(nameof(IzvestajProlaznostiStavkaDto.Ime), "Ime");
            AddColumn(nameof(IzvestajProlaznostiStavkaDto.Prezime), "Prezime");
            AddColumn(nameof(IzvestajProlaznostiStavkaDto.Jmbg), "JMBG");
            AddColumn(nameof(IzvestajProlaznostiStavkaDto.Kategorija), "Kategorija", 90);
            AddColumn(nameof(IzvestajProlaznostiStavkaDto.Status), "Status", 90);

            DataGridViewTextBoxColumn datumKolona = AddColumn(nameof(IzvestajProlaznostiStavkaDto.DatumPoslednjegIspita), "Poslednji ispit", 120);
            datumKolona.DefaultCellStyle.Format = "dd.MM.yyyy";

            AddColumn(nameof(IzvestajProlaznostiStavkaDto.BrojPokusajaTeorijski), "Teorijski pokusaji", 120);
            AddColumn(nameof(IzvestajProlaznostiStavkaDto.BrojPokusajaPrakticni), "Prakticni pokusaji", 120);
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

        private DataGridViewTextBoxColumn AddColumn(string dataProperty, string headerText, int width = 110)
        {
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn
            {
                DataPropertyName = dataProperty,
                HeaderText = headerText,
                Width = width,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            };

            _ucKreirajIzvestaj.DgvIzvestajIspita.Columns.Add(col);
            return col;
        }

        private void BindProlaznostResult(IzvestajProlaznostiResponseDto result)
        {
            ConfigureProlaznostGrid();
            _ucKreirajIzvestaj.DgvIzvestajIspita.DataSource = new BindingList<IzvestajProlaznostiStavkaDto>(result.Stavke ?? new List<IzvestajProlaznostiStavkaDto>());

            _ucKreirajIzvestaj.TxtUkupnoPolozilo.Text = (result.Summary?.UkupnoPolozilo ?? 0).ToString();
            _ucKreirajIzvestaj.TxtUkupnoPalo.Text = (result.Summary?.UkupnoPalo ?? 0).ToString();
            _ucKreirajIzvestaj.TxtUkupnoUToku.Text = (result.Summary?.UkupnoUToku ?? 0).ToString();
            _ucKreirajIzvestaj.TxtProcenatProlaznosti.Text = string.Format("{0:N2}%", result.Summary?.ProcenatProlaznosti ?? 0m);
        }

        private void ResetSummaryForProlaznost()
        {
            _ucKreirajIzvestaj.TxtUkupnoPolozilo.Text = "0";
            _ucKreirajIzvestaj.TxtUkupnoPalo.Text = "0";
            _ucKreirajIzvestaj.TxtUkupnoUToku.Text = "0";
            _ucKreirajIzvestaj.TxtProcenatProlaznosti.Text = "0.00%";
        }
    }
}
