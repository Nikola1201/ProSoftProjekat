using Client.UserControls.UCCasVoznje;
using Client.Utils;
using Common.Communication;
using Common.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Client.GuiController
{
    internal class CasGuiController
    {
        private UCZakaziCasVoznje _ucZakaziCasVoznje;
        private UCOtkaziCasVoznje _ucOtkaziCasVoznje;


        private BindingList<Kandidat> kandidati;
        private BindingList<Instruktor> instruktori;
        private BindingList<VoziloRow> vozila;

        private List<Kandidat> sviKandidati;
        private List<Instruktor> sviInstruktori;
        private List<Vozilo> svaVozila;
        private List<Upis> sviUpisi;
        private List<PaketObuke> sviPaketi;
        private List<CasVoznje> sviCasovi;
        private List<Kategorija> sviKategorije;

        private BindingList<OtkaziCasGridRow> prikazaniCasovi;

        private bool isRefreshing;

        internal Control CreateZakaziCas()
        {
            _ucZakaziCasVoznje = new UCZakaziCasVoznje();
            PrepareUCZakaziCas();

            _ucZakaziCasVoznje.CmbKandidat.SelectedIndexChanged += SelectionChanged;
            _ucZakaziCasVoznje.CmbInstruktor.SelectedIndexChanged += SelectionChanged;
            _ucZakaziCasVoznje.CmbVozilo.SelectedIndexChanged += SelectionChanged;
            _ucZakaziCasVoznje.BtnZakazi.Click += ZakaziVoznju;

            return _ucZakaziCasVoznje;
        }

        private void PrepareUCZakaziCas()
        {
            try
            {
                sviKandidati = Communication.Instance.GetAllKandidati(true) ?? new List<Kandidat>();
                sviInstruktori = (Communication.Instance.GetAllInstruktori() ?? new List<Instruktor>())
                    .Where(i => i.Aktivan)
                    .ToList();
                svaVozila = (Communication.Instance.GetAllVozila() ?? new List<Vozilo>())
                    .Where(v => v.Aktivno)
                    .ToList();
                sviUpisi = (Communication.Instance.GetAllUpisi() ?? new List<Upis>())
                    .Where(u => string.Equals(u.Status, "aktivan", StringComparison.OrdinalIgnoreCase))
                    .ToList();
                sviPaketi = Communication.Instance.GetAllPaketiObuke() ?? new List<PaketObuke>();
                sviKategorije = Communication.Instance.GetAllKategorije() ?? new List<Kategorija>();

                BindFilteredCombos(null, null, null);

                bool imaPodataka = kandidati.Count > 0 && instruktori.Count > 0 && vozila.Count > 0;
                _ucZakaziCasVoznje.CmbKandidat.Enabled = kandidati.Count > 0;
                _ucZakaziCasVoznje.CmbInstruktor.Enabled = instruktori.Count > 0;
                _ucZakaziCasVoznje.CmbVozilo.Enabled = vozila.Count > 0;
                _ucZakaziCasVoznje.BtnZakazi.Enabled = imaPodataka;

                if (imaPodataka)
                {
                    _ucZakaziCasVoznje.LblPredusloviInfo.Visible = false;
                }
                else
                {
                    List<string> nedostaju = new List<string>();
                    if (kandidati.Count == 0) nedostaju.Add("kandidati");
                    if (instruktori.Count == 0) nedostaju.Add("instruktori");
                    if (vozila.Count == 0) nedostaju.Add("vozila");

                    _ucZakaziCasVoznje.LblPredusloviInfo.Text = "Nedostaju: " + string.Join(", ", nedostaju) + ".";
                    _ucZakaziCasVoznje.LblPredusloviInfo.Visible = true;
                }
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

        private void SelectionChanged(object sender, EventArgs e)
        {
            if (isRefreshing)
            {
                return;
            }

            Kandidat selectedKandidat = _ucZakaziCasVoznje.CmbKandidat.SelectedItem as Kandidat;
            Instruktor selectedInstruktor = _ucZakaziCasVoznje.CmbInstruktor.SelectedItem as Instruktor;
            Vozilo selectedVozilo = (_ucZakaziCasVoznje.CmbVozilo.SelectedItem as VoziloRow)?.Vozilo;

            BindFilteredCombos(selectedKandidat, selectedInstruktor, selectedVozilo);
        }

        private void BindFilteredCombos(Kandidat kandidatFilter, Instruktor instruktorFilter, Vozilo voziloFilter)
        {
            isRefreshing = true;

            List<Kandidat> filtriraniKandidati = FiltrirajKandidate(voziloFilter);
            List<Instruktor> filtriraniInstruktori = sviInstruktori.ToList();
            List<VoziloRow> filtriranaVozila = FiltrirajVozila(kandidatFilter);

            Kandidat prethodniKandidat = kandidatFilter;
            Instruktor prethodniInstruktor = instruktorFilter;
            Vozilo prethodnoVozilo = voziloFilter;

            kandidati = new BindingList<Kandidat>(filtriraniKandidati);
            instruktori = new BindingList<Instruktor>(filtriraniInstruktori);
            vozila = new BindingList<VoziloRow>(filtriranaVozila);

            _ucZakaziCasVoznje.CmbKandidat.DataSource = kandidati;
            _ucZakaziCasVoznje.CmbInstruktor.DataSource = instruktori;
            _ucZakaziCasVoznje.CmbVozilo.DataSource = vozila;

            if (prethodniKandidat != null && kandidati.Contains(prethodniKandidat))
            {
                _ucZakaziCasVoznje.CmbKandidat.SelectedItem = prethodniKandidat;
            }
            else
            {
                _ucZakaziCasVoznje.CmbKandidat.SelectedIndex = -1;
            }

            if (prethodniInstruktor != null && instruktori.Contains(prethodniInstruktor))
            {
                _ucZakaziCasVoznje.CmbInstruktor.SelectedItem = prethodniInstruktor;
            }
            else
            {
                _ucZakaziCasVoznje.CmbInstruktor.SelectedIndex = -1;
            }

            if (prethodnoVozilo != null)
            {
                VoziloRow restored = vozila.FirstOrDefault(vr => vr.Vozilo.VoziloId == prethodnoVozilo.VoziloId);
                if (restored != null)
                {
                    _ucZakaziCasVoznje.CmbVozilo.SelectedItem = restored;
                }
                else
                {
                    _ucZakaziCasVoznje.CmbVozilo.SelectedIndex = -1;
                }
            }
            else
            {
                _ucZakaziCasVoznje.CmbVozilo.SelectedIndex = -1;
            }

            _ucZakaziCasVoznje.CmbKandidat.Enabled = kandidati.Count > 0;
            _ucZakaziCasVoznje.CmbInstruktor.Enabled = instruktori.Count > 0;
            _ucZakaziCasVoznje.CmbVozilo.Enabled = vozila.Count > 0;

            _ucZakaziCasVoznje.BtnZakazi.Enabled =
                _ucZakaziCasVoznje.CmbKandidat.SelectedItem is Kandidat &&
                _ucZakaziCasVoznje.CmbInstruktor.SelectedItem is Instruktor &&
                _ucZakaziCasVoznje.CmbVozilo.SelectedItem is VoziloRow;

            isRefreshing = false;
        }

        private List<Kandidat> FiltrirajKandidate(Vozilo voziloFilter)
        {
            if (voziloFilter == null)
            {
                return KandidatiSaAktivnimUpisom().ToList();
            }

            return KandidatiSaAktivnimUpisom()
                .Where(k => KandidatImaAktivanUpisZaKategoriju(k.KandidatId, voziloFilter.KategorijaID))
                .ToList();
        }

        private List<VoziloRow> FiltrirajVozila(Kandidat kandidatFilter)
        {
            Dictionary<int, string> kategorijaNaziv = sviKategorije
                .GroupBy(k => k.KategorijaID)
                .ToDictionary(g => g.Key, g => g.First().NazivKategorije ?? string.Empty);

            IEnumerable<Vozilo> filtered;
            if (kandidatFilter == null)
            {
                filtered = svaVozila;
            }
            else
            {
                HashSet<int> dozvoljeneKategorije = AktivneKategorijeKandidata(kandidatFilter.KandidatId);
                filtered = svaVozila.Where(v => dozvoljeneKategorije.Contains(v.KategorijaID));
            }

            return filtered
                .Select(v => new VoziloRow { Vozilo = v, Display = BuildVoziloDisplay(v, kategorijaNaziv) })
                .ToList();
        }

        private IEnumerable<Kandidat> KandidatiSaAktivnimUpisom()
        {
            HashSet<int> kandidatiSaUpisom = new HashSet<int>(sviUpisi.Select(u => u.KandidatId));
            return sviKandidati.Where(k => kandidatiSaUpisom.Contains(k.KandidatId));
        }

        private HashSet<int> AktivneKategorijeKandidata(int kandidatId)
        {
            HashSet<int> kategorije = new HashSet<int>();

            List<Upis> upisiKandidata = sviUpisi.Where(u => u.KandidatId == kandidatId).ToList();
            foreach (Upis upis in upisiKandidata)
            {
                PaketObuke paket = sviPaketi.FirstOrDefault(p => p.PaketId == upis.PaketId);
                int kategorijaId = paket?.Kategorija?.KategorijaID ?? 0;
                if (kategorijaId <= 0)
                {
                    continue;
                }

                kategorije.Add(kategorijaId);
            }

            return kategorije;
        }

        private bool KandidatImaAktivanUpisZaKategoriju(int kandidatId, int kategorijaIdVozila)
        {
            if (kategorijaIdVozila <= 0)
            {
                return false;
            }

            return AktivneKategorijeKandidata(kandidatId).Contains(kategorijaIdVozila);
        }

        private void ZakaziVoznju(object sender, EventArgs e)
        {
            if (!(_ucZakaziCasVoznje.CmbKandidat.SelectedItem is Kandidat kandidat))
            {
                ShowMessage.Error("Izaberite kandidata.", _ucZakaziCasVoznje.CmbKandidat);
                return;
            }

            if (!(_ucZakaziCasVoznje.CmbInstruktor.SelectedItem is Instruktor instruktor))
            {
                ShowMessage.Error("Izaberite instruktora.", _ucZakaziCasVoznje.CmbInstruktor);
                return;
            }

            if (!(_ucZakaziCasVoznje.CmbVozilo.SelectedItem is VoziloRow voziloRow))
            {
                ShowMessage.Error("Izaberite vozilo.", _ucZakaziCasVoznje.CmbVozilo);
                return;
            }
            Vozilo vozilo = voziloRow.Vozilo;

            if (!int.TryParse(_ucZakaziCasVoznje.TextBox1.Text.Trim(), out int trajanjeMin) || trajanjeMin <= 0)
            {
                ShowMessage.Error("Trajanje casa mora biti ceo broj veci od 0.", _ucZakaziCasVoznje.TextBox1);
                return;
            }

            DateTime datumCasa = _ucZakaziCasVoznje.DateTimePicker1.Value;
            if (datumCasa < DateTime.Now)
            {
                ShowMessage.Error("Datum i vreme casa ne mogu biti u proslosti.", _ucZakaziCasVoznje.DateTimePicker1);
                return;
            }

            Upis aktivanUpis = PronadjiAktivanUpis(kandidat.KandidatId, vozilo.KategorijaID);
            if (aktivanUpis == null)
            {
                ShowMessage.Warning("Izabrani kandidat nema aktivan upis koji odgovara kategoriji vozila.", "Greska");
                return;
            }

            CasVoznje cas = new CasVoznje
            {
                UpisId = aktivanUpis.UpisId,
                InstruktorId = instruktor.InstruktorId,
                VoziloId = vozilo.VoziloId,
                DatumCas = datumCasa,
                TrajanjMin = trajanjeMin,
                Napomena = (_ucZakaziCasVoznje.TextBox2.Text ?? string.Empty).Trim()
            };

            try
            {
                Response response = Communication.Instance.ZakaziCasVoznje(cas);
                if (!string.IsNullOrEmpty(response.ErrorMessage))
                {
                    ShowMessage.Error(response.ErrorMessage);
                    return;
                }

                ShowMessage.Success("Sistem je uspesno zakazao cas voznje.");
                ResetForm();
                PrepareUCZakaziCas();
            }
            catch (Exception)
            {
                ShowMessage.ServerDown();
            }
        }

        private Upis PronadjiAktivanUpis(int kandidatId, int kategorijaIdVozila)
        {
            return sviUpisi.FirstOrDefault(u =>
                u.KandidatId == kandidatId &&
                string.Equals(u.Status, "aktivan", StringComparison.OrdinalIgnoreCase) &&
                sviPaketi.Any(p => p.PaketId == u.PaketId && (p.Kategorija?.KategorijaID ?? 0) == kategorijaIdVozila));
        }

        private void ResetForm()
        {
            _ucZakaziCasVoznje.TextBox1.Clear();
            _ucZakaziCasVoznje.TextBox2.Clear();
            _ucZakaziCasVoznje.DateTimePicker1.Value = DateTime.Now.AddMinutes(30);
        }

        internal Control CreateIzmeniCas()
        {
            _ucOtkaziCasVoznje = new UCOtkaziCasVoznje();
            PrepareUCOtkaziCasVoznje();
            _ucOtkaziCasVoznje.DateTimePicker1.ValueChanged += OtkaziCasDatumChanged;
            _ucOtkaziCasVoznje.BtnOtkazi.Click += OtkaziCasVoznje;

            return _ucOtkaziCasVoznje;
        }

        private void PrepareUCOtkaziCasVoznje()
        {
            try
            {
                sviCasovi = Communication.Instance.GetAllCasVoznje() ?? new List<CasVoznje>();
                sviUpisi = Communication.Instance.GetAllUpisi() ?? new List<Upis>();
                sviKandidati = Communication.Instance.GetAllKandidati(true) ?? new List<Kandidat>();
                sviInstruktori = Communication.Instance.GetAllInstruktori() ?? new List<Instruktor>();
                svaVozila = Communication.Instance.GetAllVozila() ?? new List<Vozilo>();
                sviKategorije = Communication.Instance.GetAllKategorije() ?? new List<Kategorija>();

                FormatDgvCasovi();
                RefreshOtkaziDgv();
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

        private void FormatDgvCasovi()
        {
            DataGridView dgv = _ucOtkaziCasVoznje.DgvCasovi;
            dgv.AutoGenerateColumns = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;

            dgv.Columns.Clear();

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Kandidat",
                HeaderText = "Kandidat",
                DataPropertyName = "KandidatImePrezime",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 35
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Instruktor",
                HeaderText = "Instruktor",
                DataPropertyName = "InstruktorImePrezime",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 30
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Vreme",
                HeaderText = "Vreme",
                DataPropertyName = "VremeZakazivanja",
                Width = 130
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Vozilo",
                HeaderText = "Vozilo",
                DataPropertyName = "VoziloMarkaModel",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 35
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "Status",
                DataPropertyName = "Status",
                Width = 90
            });
        }

        private void OtkaziCasDatumChanged(object sender, EventArgs e)
        {
            RefreshOtkaziDgv();
        }

        private void RefreshOtkaziDgv()
        {
            if (_ucOtkaziCasVoznje == null)
            {
                return;
            }

            DateTime izabraniDatum = _ucOtkaziCasVoznje.DateTimePicker1.Value.Date;

            List<OtkaziCasGridRow> casoviZaDatum = sviCasovi
                .Where(c => c.DatumCas.Date == izabraniDatum)
                .OrderBy(c => c.DatumCas)
                .Select(MapToGridRow)
                .ToList();

            prikazaniCasovi = new BindingList<OtkaziCasGridRow>(casoviZaDatum);
            _ucOtkaziCasVoznje.DgvCasovi.DataSource = prikazaniCasovi;

            _ucOtkaziCasVoznje.BtnOtkazi.Enabled = prikazaniCasovi.Count > 0;
            _ucOtkaziCasVoznje.LblNemaCasova.Visible = prikazaniCasovi.Count == 0;
            if (prikazaniCasovi.Count == 0)
            {
                _ucOtkaziCasVoznje.LblNemaCasova.BringToFront();
            }
        }

        private OtkaziCasGridRow MapToGridRow(CasVoznje cas)
        {
            Upis upis = sviUpisi.FirstOrDefault(u => u.UpisId == cas.UpisId);
            Kandidat kandidat = upis == null ? null : sviKandidati.FirstOrDefault(k => k.KandidatId == upis.KandidatId);
            Instruktor instruktor = sviInstruktori.FirstOrDefault(i => i.InstruktorId == cas.InstruktorId);
            Vozilo vozilo = svaVozila.FirstOrDefault(v => v.VoziloId == cas.VoziloId);

            return new OtkaziCasGridRow
            {
                Cas = cas,
                KandidatImePrezime = kandidat?.PunoIme ?? "-",
                InstruktorImePrezime = instruktor?.PunoIme ?? "-",
                VremeZakazivanja = cas.DatumCas.ToString("HH:mm"),
                VoziloMarkaModel = vozilo == null ? "-" : $"{vozilo.Marka} {vozilo.Model}",
                Status = cas.Status
            };
        }

        private void OtkaziCasVoznje(object sender, EventArgs e)
        {
            if (!(_ucOtkaziCasVoznje.DgvCasovi.CurrentRow?.DataBoundItem is OtkaziCasGridRow selected))
            {
                ShowMessage.Warning("Izaberite cas koji zelite da otkazete.", "Greska");
                return;
            }

            if (selected.Cas == null)
            {
                ShowMessage.Warning("Nije moguce pronaci izabrani cas.", "Greska");
                return;
            }

            try
            {
                selected.Cas.Status = "otkazan";
                Response response = Communication.Instance.OtkaziCasVoznje(selected.Cas);
                if (!string.IsNullOrEmpty(response.ErrorMessage))
                {
                    ShowMessage.Error(response.ErrorMessage);
                    return;
                }

                ShowMessage.Success("Sistem je uspesno otkazao rezervaciju casa voznje.");
                PrepareUCOtkaziCasVoznje();
            }
            catch (Exception)
            {
                ShowMessage.ServerDown();
            }
        }

        private class VoziloRow
        {
            public Vozilo Vozilo { get; set; }
            public string Display { get; set; }
            public override string ToString() => Display;
        }

        private static string BuildVoziloDisplay(Vozilo v, Dictionary<int, string> kategorijaNaziv)
        {
            string naziv = kategorijaNaziv != null
                           && kategorijaNaziv.TryGetValue(v.KategorijaID, out string n)
                           && !string.IsNullOrWhiteSpace(n)
                ? n
                : $"Kat. {v.KategorijaID}";

            return $"{v.Marka} {v.Model} ({v.Tablica}) - {naziv}";
        }

        private class OtkaziCasGridRow
        {
            public CasVoznje Cas { get; set; }
            public string KandidatImePrezime { get; set; }
            public string InstruktorImePrezime { get; set; }
            public string VremeZakazivanja { get; set; }
            public string VoziloMarkaModel { get; set; }
            public string Status { get; set; }
        }
    }
}
