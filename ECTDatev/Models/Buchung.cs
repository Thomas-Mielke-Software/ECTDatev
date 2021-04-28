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
        private int id;
        public int ID { get => id; set => id = value; }  
        public decimal Betrag { get { buchCtrl.ID = id; return buchCtrl.Betrag; } set { buchCtrl.ID = id; buchCtrl.Betrag = value; } }
        public double MWSt { get { buchCtrl.ID = id; return buchCtrl.MWSt; } set { buchCtrl.ID = id; buchCtrl.MWSt = value; } }
        public short AbschreibungNr { get { buchCtrl.ID = id; return buchCtrl.AbschreibungNr; } set { buchCtrl.ID = id; buchCtrl.AbschreibungNr = value; } }
        public short AbschreibungJahre { get { buchCtrl.ID = id; return buchCtrl.AbschreibungJahre; } set { buchCtrl.ID = id; buchCtrl.AbschreibungJahre = value; } }
        public string Beschreibung { get { buchCtrl.ID = id; return buchCtrl.Beschreibung; } set { buchCtrl.ID = id; buchCtrl.Beschreibung = value; } }
        public DateTime Datum { get { buchCtrl.ID = id; return buchCtrl.Datum; } set { buchCtrl.ID = id; buchCtrl.Datum = value; } }
        public string Konto { get { buchCtrl.ID = id; return buchCtrl.Konto; } set { buchCtrl.ID = id; buchCtrl.Konto = value; } }
        public string Belegnummer { get { buchCtrl.ID = id; return buchCtrl.Belegnummer; } set { buchCtrl.ID = id; buchCtrl.Belegnummer = value; } }
        public decimal AbschreibungRestwert { get { buchCtrl.ID = id; return buchCtrl.AbschreibungRestwert; } set { buchCtrl.ID = id; buchCtrl.AbschreibungRestwert = value; } }
        public bool AbschreibungDegressiv { get { buchCtrl.ID = id; return buchCtrl.AbschreibungDegressiv; } set { buchCtrl.ID = id; buchCtrl.AbschreibungDegressiv = value; } }
        public double AbschreibungSatz { get { buchCtrl.ID = id; return buchCtrl.AbschreibungSatz; } set { buchCtrl.ID = id; buchCtrl.AbschreibungSatz = value; } }
        public int AbschreibungGenauigkeit { get { buchCtrl.ID = id; return buchCtrl.AbschreibungGenauigkeit; } set { buchCtrl.ID = id; buchCtrl.AbschreibungGenauigkeit = value; } }
        public decimal HoleNetto { get { buchCtrl.ID = id; return buchCtrl.HoleNetto(); } }
        public decimal HoleBuchungsjahrNetto(int dokID) { buchCtrl.ID = id; return buchCtrl.HoleBuchungsjahrNetto(dokID); }  // dokID wird für die Berechnung des Abschreibungswerts im spezifischen Jahr benötigt
        public string Betrieb { get { buchCtrl.ID = id; return buchCtrl.Betrieb; } set { buchCtrl.ID = id; buchCtrl.Betrieb = value; } }
        public string Bestandskonto { get { buchCtrl.ID = id; return buchCtrl.Bestandskonto; } set { buchCtrl.ID = id; buchCtrl.Bestandskonto = value; } }
        public Dictionary<string,string> BenutzerdefWert { get { throw new NotImplementedException(); } }  // TODO
    }
}
