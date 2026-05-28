using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Communication
{
    /// <summary>
    /// Tip zahteva koji klijent šalje serveru. Svaka vrednost odgovara
    /// po jednoj sistemskoj operaciji u <c>Server.Kontroler</c>.
    /// </summary>
    public enum Operation
    {
        /// <summary>Prijava administratora na sistem.</summary>
        Login,
        /// <summary>Odjava trenutno prijavljenog administratora.</summary>
        Logout,
        /// <summary>Kreiranje novog kandidata u sistemu.</summary>
        KreirajKandidata,
        /// <summary>Preuzimanje svih kategorija vozačke dozvole.</summary>
        GetAllKategorije,
        /// <summary>Preuzimanje liste svih kandidata.</summary>
        GetAllKandidati,
        /// <summary>Preuzimanje svih dostupnih paketa obuke.</summary>
        GetAllPaketiObuke,
        /// <summary>Upis kandidata u paket obuke.</summary>
        UpisiKandidata,
        /// <summary>Brisanje kandidata iz sistema.</summary>
        ObrisiKandidata,
        /// <summary>Kreiranje novog instruktora vožnje.</summary>
        KreirajInstruktora,
        /// <summary>Brisanje instruktora iz sistema.</summary>
        ObrisiInstruktora,
        /// <summary>Preuzimanje liste svih instruktora.</summary>
        GetAllInstruktori,
        /// <summary>Preuzimanje liste svih vozila u autoškoli.</summary>
        GetAllVozila,
        /// <summary>Preuzimanje svih upisa kandidata u pakete obuke.</summary>
        GetAllUpisi,
        /// <summary>Zakazivanje časa vožnje za kandidata.</summary>
        ZakaziCasVoznje,
        /// <summary>Preuzimanje liste svih zakazanih časova vožnje.</summary>
        GetAllCasVoznje,
        /// <summary>Otkazivanje prethodno zakazanog časa vožnje.</summary>
        OtkaziCasVoznje,
        /// <summary>Pretraga kandidata po zadatim kriterijumima.</summary>
        PretraziKandidate,
        /// <summary>Evidentiranje rezultata ispita za kandidata.</summary>
        EvidentirajIspit,
        /// <summary>Generisanje izveštaja prolaznosti kandidata na ispitima.</summary>
        KreirajIzvestajProlaznosti,
        /// <summary>Generisanje izveštaja dugovanja kandidata.</summary>
        KreirajIzvestajDugovanja,
        /// <summary>Preuzimanje liste kandidata koji imaju dugovanja.</summary>
        VratiKandidatiSaDugovanjem,
        /// <summary>Evidentiranje uplate kandidata.</summary>
        EvidentirajUplatu,
        /// <summary>Preuzimanje svih kombinacija instruktor–kategorija.</summary>
        GetAllInstrKat,
    }
}
