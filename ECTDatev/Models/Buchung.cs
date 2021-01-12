using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECTDatev.Models
{
    /// <summary>
    /// Wrapper-Klasse für Buchungs-ActiveX. Indizierung der Buchung über das member ID.
    /// für mehr Info siehe https://www.easyct.de/readarticle.php?article_id=23#Buchung
    /// </summary>
    public class Buchung
    {
        private AxEASYCTXLib.AxBuchung buchCtrl;  // ActiveX, wo wir die Daten herbekommen

        /// <summary>
        ///  Konstruktor
        /// </summary>
        /// <param name="buchCtrl">ActiveX-Objekt</param>
        public Buchung(int id, AxEASYCTXLib.AxBuchung buchCtrl)
        {
            this.buchCtrl = buchCtrl;
            this.ID = id;  // handle zur Buchung setzen; tatsächlich ist es die 32-bit Speicheradresse des C++-Objekts, also Vorsicht damit!
        }
        public int ID { get => buchCtrl.ID; set => buchCtrl.ID = value; }  
        public decimal Betrag { get => buchCtrl.Betrag; set => buchCtrl.Betrag = value; }
        public double MWSt { get => buchCtrl.MWSt; set => buchCtrl.MWSt = value; }
        public short AbschreibungNr { get => buchCtrl.AbschreibungNr; set => buchCtrl.AbschreibungNr = value; }
        public short AbschreibungJahre { get => buchCtrl.AbschreibungJahre; set => buchCtrl.AbschreibungJahre = value; }
        public string Beschreibung { get => buchCtrl.Beschreibung; set => buchCtrl.Beschreibung = value; }
        public DateTime Datum { get => buchCtrl.Datum; set => buchCtrl.Datum = value; }
        public string Konto { get => buchCtrl.Konto; set => buchCtrl.Konto = value; }
        public string Belegnummer { get => buchCtrl.Belegnummer; set => buchCtrl.Belegnummer = value; }
        public decimal AbschreibungRestwert { get => buchCtrl.AbschreibungRestwert; set => buchCtrl.AbschreibungRestwert = value; }
        public bool AbschreibungDegressiv { get => buchCtrl.AbschreibungDegressiv; set => buchCtrl.AbschreibungDegressiv = value; }
        public double AbschreibungSatz { get => buchCtrl.AbschreibungSatz; set => buchCtrl.AbschreibungSatz = value; }
        public int AbschreibungGenauigkeit { get => buchCtrl.AbschreibungGenauigkeit; set => buchCtrl.AbschreibungGenauigkeit = value; }
        public decimal HoleNetto { get => buchCtrl.HoleNetto(); }
        public decimal HoleBuchungsjahrNetto(int dokID) { return buchCtrl.HoleBuchungsjahrNetto(dokID); }  // dokID wird für die Berechnung des Abschreibungswerts im spezifischen Jahr benötigt
        public string Betrieb { get => buchCtrl.Betrieb; set => buchCtrl.Betrieb = value; }
        public string Bestandskonto { get => buchCtrl.Bestandskonto; set => buchCtrl.Bestandskonto = value; }
        public Dictionary<string,string> BenutzerdefWert { get => throw new NotImplementedException(); }  // TODO
    }
}
