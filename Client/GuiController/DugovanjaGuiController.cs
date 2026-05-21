using Client.UserControls.UCDugovanja;
using Client.Utils;
using Common.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

namespace Client.GuiController
{
    internal class DugovanjaGuiController
    {
        private UCPregledDugovanja _ucPregled;
        private BindingList<KandidatDugovanjeDto> _prikazaneStavke;
        private UCEvidentirajUplatu _ucEvidentirajUplatu;
        private KandidatDugovanjeDto _trenutnoIzabraniKandidat;

        internal Control CreatePregledDugovanja()
        {
            _ucPregled = new UCPregledDugovanja();
            FormatDgvDugovanja();
            UcitajPodatke();
            _ucPregled.DgvDugovanja.SelectionChanged += DgvDugovanja_SelectionChanged;
            _ucPregled.BtnEvidentirajUplatu.Click += BtnEvidentirajUplatu_Click;
            return _ucPregled;
        }

        private void FormatDgvDugovanja()
        {
            DataGridView dgv = _ucPregled.DgvDugovanja;
            dgv.AutoGenerateColumns = false;
            dgv.Columns.Clear();

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "KandidatId",
                HeaderText = "ID",
                DataPropertyName = "KandidatId",
                Width = 60
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PunoIme",
                HeaderText = "Kandidat",
                DataPropertyName = "PunoIme",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 40
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "JMBG",
                HeaderText = "JMBG",
                DataPropertyName = "JMBG",
                Width = 120
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "UkupnaCena",
                HeaderText = "Ukupna cena",
                DataPropertyName = "UkupnaCena",
                Width = 110,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "UkupnoPlaceno",
                HeaderText = "Placeno",
                DataPropertyName = "UkupnoPlaceno",
                Width = 110,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Dugovanje",
                HeaderText = "Dugovanje",
                DataPropertyName = "Dugovanje",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "N2",
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    ForeColor = System.Drawing.Color.DarkRed,
                    Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold)
                }
            });
        }

        private void UcitajPodatke()
        {
            try
            {
                List<KandidatDugovanjeDto> stavke = Communication.Instance.VratiKandidatiSaDugovanjem()
                    ?? new List<KandidatDugovanjeDto>();
                _prikazaneStavke = new BindingList<KandidatDugovanjeDto>(stavke);
                _ucPregled.DgvDugovanja.DataSource = _prikazaneStavke;

                bool imaPodataka = _prikazaneStavke.Count > 0;
                _ucPregled.LblNemaPodataka.Visible = !imaPodataka;
                _ucPregled.LblBrojRedova.Text = imaPodataka
                    ? $"Prikazano: {_prikazaneStavke.Count}"
                    : "";
                _ucPregled.BtnEvidentirajUplatu.Enabled = false;
            }
            catch (Exception ex) when (ex is System.Net.Sockets.SocketException || ex is System.IO.IOException)
            {
                ShowMessage.ServerDown();
            }
            catch (Exception ex)
            {
                ShowMessage.Error(ex.Message);
            }
        }

        private void DgvDugovanja_SelectionChanged(object sender, EventArgs e)
        {
            _ucPregled.BtnEvidentirajUplatu.Enabled =
                _ucPregled.DgvDugovanja.CurrentRow?.DataBoundItem is KandidatDugovanjeDto;
        }

        private void BtnEvidentirajUplatu_Click(object sender, EventArgs e)
        {
            if (!(_ucPregled.DgvDugovanja.CurrentRow?.DataBoundItem is KandidatDugovanjeDto stavka))
            {
                ShowMessage.Warning("Izaberite kandidata iz tabele.", "Greska");
                return;
            }
            MainCoordinator.Instance.ShowEvidentirajUplatuPanel(stavka);
        }

        internal Control CreateEvidentirajUplata(KandidatDugovanjeDto stavka)
        {
            _trenutnoIzabraniKandidat = stavka;
            _ucEvidentirajUplatu = new UCEvidentirajUplatu();
            PripremiFormu();
            _ucEvidentirajUplatu.BtnSacuvaj.Click += BtnSacuvajUplatu_Click;
            return _ucEvidentirajUplatu;
        }

        private void PripremiFormu()
        {
            _ucEvidentirajUplatu.LblKandidat.Text = $"Kandidat: {_trenutnoIzabraniKandidat.PunoIme} ({_trenutnoIzabraniKandidat.JMBG})";
            _ucEvidentirajUplatu.LblUpis.Text = "Upis: najnoviji (automatski)";
            _ucEvidentirajUplatu.LblPreostalo.Text = $"Preostalo dugovanje: {_trenutnoIzabraniKandidat.Dugovanje:N2} RSD";

            _ucEvidentirajUplatu.CmbNacin.DisplayMember = "Naziv";
            _ucEvidentirajUplatu.CmbNacin.ValueMember = "Vrednost";
            _ucEvidentirajUplatu.CmbNacin.DataSource = new List<NacinOption>
            {
                new NacinOption { Naziv = "Gotovina", Vrednost = "gotovina" },
                new NacinOption { Naziv = "Kartica", Vrednost = "kartica" },
                new NacinOption { Naziv = "Transfer", Vrednost = "transfer" }
            };

            _ucEvidentirajUplatu.DtpDatum.Value = DateTime.Today;
            _ucEvidentirajUplatu.TxtIznos.Text = "";
            _ucEvidentirajUplatu.TxtNapomena.Text = "";
        }

        private void BtnSacuvajUplatu_Click(object sender, EventArgs e)
        {
            string rawIznos = (_ucEvidentirajUplatu.TxtIznos.Text ?? string.Empty).Trim().Replace(',', '.');
            if (!decimal.TryParse(rawIznos, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal iznos) || iznos <= 0m)
            {
                ShowMessage.Error("Iznos mora biti pozitivan broj.", _ucEvidentirajUplatu.TxtIznos);
                return;
            }

            NacinOption nacin = _ucEvidentirajUplatu.CmbNacin.SelectedItem as NacinOption;
            if (nacin == null)
            {
                ShowMessage.Error("Izaberite nacin placanja.", _ucEvidentirajUplatu.CmbNacin);
                return;
            }

            EvidentirajUplatuRequest req = new EvidentirajUplatuRequest
            {
                KandidatId = _trenutnoIzabraniKandidat.KandidatId,
                UpisId = null,
                Iznos = iznos,
                NacinPlacanja = nacin.Vrednost,
                DatumPlacanja = _ucEvidentirajUplatu.DtpDatum.Value.Date,
                Napomena = (_ucEvidentirajUplatu.TxtNapomena.Text ?? string.Empty).Trim()
            };

            try
            {
                EvidentirajUplatuResponse resp = Communication.Instance.EvidentirajUplatu(req);
                ShowMessage.Success($"{resp.Poruka} Preostalo: {resp.PreostaloDugovanje:N2} RSD.");
                MainCoordinator.Instance.ShowPregledDugovanjaPanel();
            }
            catch (Exception ex) when (ex is System.Net.Sockets.SocketException || ex is System.IO.IOException)
            {
                ShowMessage.ServerDown();
            }
            catch (Exception ex)
            {
                ShowMessage.Error(ex.Message);
            }
        }

        private class NacinOption
        {
            public string Naziv { get; set; }
            public string Vrednost { get; set; }
        }
    }
}
