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
    internal class CasGuiKontroler
    {
        private UCZakaziCasVoznje _ucZakaziCasVoznje;

        private BindingList<Kandidat> kandidati;
        private BindingList<Instruktor> instruktori;
        private BindingList<Vozilo> vozila;

        private List<Kandidat> sviKandidati;
        private List<Instruktor> sviInstruktori;
        private List<Vozilo> svaVozila;
        private List<Upis> sviUpisi;
        private List<PaketObuke> sviPaketi;

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

                BindFilteredCombos(null, null, null);

                bool imaPodataka = kandidati.Count > 0 && instruktori.Count > 0 && vozila.Count > 0;
                _ucZakaziCasVoznje.CmbKandidat.Enabled = kandidati.Count > 0;
                _ucZakaziCasVoznje.CmbInstruktor.Enabled = instruktori.Count > 0;
                _ucZakaziCasVoznje.CmbVozilo.Enabled = vozila.Count > 0;
                _ucZakaziCasVoznje.BtnZakazi.Enabled = imaPodataka;

                if (!imaPodataka)
                {
                    ShowMessage.Info("Nema dovoljno podataka za zakazivanje casa voznje.");
                }
            }
            catch (Exception)
            {
                ShowMessage.ServerDown();
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
            Vozilo selectedVozilo = _ucZakaziCasVoznje.CmbVozilo.SelectedItem as Vozilo;

            BindFilteredCombos(selectedKandidat, selectedInstruktor, selectedVozilo);
        }

        private void BindFilteredCombos(Kandidat kandidatFilter, Instruktor instruktorFilter, Vozilo voziloFilter)
        {
            isRefreshing = true;

            List<Kandidat> filtriraniKandidati = FiltrirajKandidate(instruktorFilter, voziloFilter);
            List<Instruktor> filtriraniInstruktori = FiltrirajInstruktore(kandidatFilter, voziloFilter);
            List<Vozilo> filtriranaVozila = FiltrirajVozila(kandidatFilter, instruktorFilter);

            Kandidat prethodniKandidat = kandidatFilter;
            Instruktor prethodniInstruktor = instruktorFilter;
            Vozilo prethodnoVozilo = voziloFilter;

            kandidati = new BindingList<Kandidat>(filtriraniKandidati);
            instruktori = new BindingList<Instruktor>(filtriraniInstruktori);
            vozila = new BindingList<Vozilo>(filtriranaVozila);

            _ucZakaziCasVoznje.CmbKandidat.DataSource = kandidati;
            _ucZakaziCasVoznje.CmbInstruktor.DataSource = instruktori;
            _ucZakaziCasVoznje.CmbVozilo.DataSource = vozila;

            if (prethodniKandidat != null && kandidati.Contains(prethodniKandidat))
            {
                _ucZakaziCasVoznje.CmbKandidat.SelectedItem = prethodniKandidat;
            }

            if (prethodniInstruktor != null && instruktori.Contains(prethodniInstruktor))
            {
                _ucZakaziCasVoznje.CmbInstruktor.SelectedItem = prethodniInstruktor;
            }

            if (prethodnoVozilo != null && vozila.Contains(prethodnoVozilo))
            {
                _ucZakaziCasVoznje.CmbVozilo.SelectedItem = prethodnoVozilo;
            }

            _ucZakaziCasVoznje.CmbKandidat.Enabled = kandidati.Count > 0;
            _ucZakaziCasVoznje.CmbInstruktor.Enabled = instruktori.Count > 0;
            _ucZakaziCasVoznje.CmbVozilo.Enabled = vozila.Count > 0;

            _ucZakaziCasVoznje.BtnZakazi.Enabled =
                _ucZakaziCasVoznje.CmbKandidat.SelectedItem is Kandidat &&
                _ucZakaziCasVoznje.CmbInstruktor.SelectedItem is Instruktor &&
                _ucZakaziCasVoznje.CmbVozilo.SelectedItem is Vozilo;

            isRefreshing = false;
        }

        private List<Kandidat> FiltrirajKandidate(Instruktor instruktorFilter, Vozilo voziloFilter)
        {
            if (voziloFilter == null)
            {
                return KandidatiSaAktivnimUpisom().ToList();
            }

            return KandidatiSaAktivnimUpisom()
                .Where(k => KandidatImaAktivanUpisZaKategoriju(k.KandidatId, voziloFilter.KategorijaID))
                .ToList();
        }

        private List<Instruktor> FiltrirajInstruktore(Kandidat kandidatFilter, Vozilo voziloFilter)
        {
            return sviInstruktori.Where(i => i.Aktivan).ToList();
        }

        private List<Vozilo> FiltrirajVozila(Kandidat kandidatFilter, Instruktor instruktorFilter)
        {
            if (kandidatFilter == null)
            {
                return svaVozila.Where(v => v.Aktivno).ToList();
            }

            HashSet<int> dozvoljeneKategorije = AktivneKategorijeKandidata(kandidatFilter.KandidatId);
            return svaVozila
                .Where(v => v.Aktivno && dozvoljeneKategorije.Contains(v.KategorijaID))
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

            if (!(_ucZakaziCasVoznje.CmbVozilo.SelectedItem is Vozilo vozilo))
            {
                ShowMessage.Error("Izaberite vozilo.", _ucZakaziCasVoznje.CmbVozilo);
                return;
            }

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
                if (response.Exception != null)
                {
                    ShowMessage.Error(response.Exception.Message);
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
            Upis upisZaKategoriju = sviUpisi
                .FirstOrDefault(u =>
                    u.KandidatId == kandidatId &&
                    sviPaketi.Any(p => p.PaketId == u.PaketId && (p.Kategorija?.KategorijaID ?? 0) == kategorijaIdVozila));

            if (upisZaKategoriju != null)
            {
                return upisZaKategoriju;
            }

            return sviUpisi.FirstOrDefault(u => u.KandidatId == kandidatId);
        }

        private void ResetForm()
        {
            _ucZakaziCasVoznje.TextBox1.Clear();
            _ucZakaziCasVoznje.TextBox2.Clear();
            _ucZakaziCasVoznje.DateTimePicker1.Value = DateTime.Now.AddMinutes(30);
        }
    }
}
