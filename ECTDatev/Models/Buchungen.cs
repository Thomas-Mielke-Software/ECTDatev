using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECTDatev.Models
{
    public class EinnahmenBuchungen : Buchungen
    {
        public EinnahmenBuchungen(AxEASYCTXLib.AxDokument dokCtrl, AxEASYCTXLib.AxBuchung buchCtrl) : base(dokCtrl, buchCtrl)
        {
            base.ersteBuchungsID = dokCtrl.FindeErsteEinnahmenBuchung();
        }
    }

    public class AusgabenBuchungen : Buchungen
    {
        public AusgabenBuchungen(AxEASYCTXLib.AxDokument dokCtrl, AxEASYCTXLib.AxBuchung buchCtrl) : base(dokCtrl, buchCtrl)
        {
            base.ersteBuchungsID = dokCtrl.FindeErsteAusgabenBuchung();
        }
    }

    /// <summary>
    /// Wrapper-Klasse zum Iterieren über die Buchungen
    /// </summary>
    abstract public class Buchungen : IEnumerable
    {
        protected int ersteBuchungsID;
        protected AxEASYCTXLib.AxDokument dokCtrl;
        protected AxEASYCTXLib.AxBuchung buchCtrl;

        /// <summary>
        /// Konstruktor für Wrapper-Klasse zum Iterieren über die Buchungen.
        /// </summary>
        /// <remarks>
        /// Bitte mit den Klassen <see cref="EinnahmenBuchungen"/> oder <see cref="AusgabenBuchungen"/> instanzieren.
        /// </remarks>
        /// <param name='dokID'>
        /// Dokument-ID, wie sie in der Init-Methode des ActiveX übermittelt wurde.
        /// </param>
        /// <param name='dokCtrl'>
        /// ActeveX-Steuerelement für das Dokument.
        /// </param>
        /// <param name='buchCtrl'>
        /// ActeveX-Steuerelement für Buchung.
        /// </param>
        public Buchungen(AxEASYCTXLib.AxDokument dokCtrl, AxEASYCTXLib.AxBuchung buchCtrl)
        {
            if (dokCtrl.ID == 0)
                throw new ArgumentNullException();  // Bitte zuerst die in der Init()-Methode empfangene ID in das dokCtrl einspeisen.

            this.dokCtrl = dokCtrl;
            this.buchCtrl = buchCtrl;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public BuchungenEnum GetEnumerator()
        {
            return new BuchungenEnum(ersteBuchungsID, dokCtrl, buchCtrl);
        }

        /*
        public IEnumerator GetEnumerator()
        {
            return this.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int id = ersteBuchungsID; id != 0; id = dokCtrl.FindeNaechsteBuchung(id))
                yield return new Buchung(id, buchCtrl);
        }
        */
    }

    public class BuchungenEnum : IEnumerator
    {
        protected int ersteBuchungsID;
        private int currentBuchungsID = -1;  // Enumerators are positioned before the first element until the first MoveNext() call.
        protected AxEASYCTXLib.AxDokument dokCtrl;
        protected AxEASYCTXLib.AxBuchung buchCtrl;

        

        public BuchungenEnum(int ersteBuchungsID, AxEASYCTXLib.AxDokument dokCtrl, AxEASYCTXLib.AxBuchung buchCtrl)
        {
            this.ersteBuchungsID = ersteBuchungsID;
            this.dokCtrl = dokCtrl;
            this.buchCtrl = buchCtrl;
        }

        public bool MoveNext()
        {
            int naechsteBuchungsID;
            if (currentBuchungsID == -1)
                naechsteBuchungsID = ersteBuchungsID;
            else
                naechsteBuchungsID = dokCtrl.FindeNaechsteBuchung(currentBuchungsID);

            if (naechsteBuchungsID == 0)
                return false;  // keine weiteren Buchungen vorhanden
            else
            {
                currentBuchungsID = naechsteBuchungsID;
                return true;
            }
        }

        public void Reset()
        {
            currentBuchungsID = ersteBuchungsID;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public Buchung Current
        {
            get
            {
                return new Buchung(currentBuchungsID, buchCtrl);
            }
        }
    }
}
