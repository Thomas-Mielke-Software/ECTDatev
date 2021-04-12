using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECTDatev
{
    /// <summary>
    /// Hier ist zunächst alles, was entweder unklar oder noch undefiniert ist.
    /// Wenn wir fertig sind, sollte hier nichts mehr drin sein und die Klasse
    /// kann gelöscht werden.
    /// </summary>
    public class ToDo
    {
        /// <summary>
        /// s. Header 3
        /// Wird je nach Aufgabenstellung ermittelt.
        /// </summary>
        public static int DataCategoryID { get => 21; }

        /// <summary>
        /// s. UI -> Herkunft
        /// </summary>
        public static string Origin { get => "EC"; }

        /// <summary>
        /// s. Header 9
        /// Kann aus ECT-Daten ermittelt werden.
        /// </summary>
        public static string ExportedBy { get => "JohnDoe"; }
        
        /// <summary>
        /// s. Header 11
        /// Soll in UI eingegeben werden. Speichern + vorschlagen für nächste Session.
        /// </summary>
        public static int Consultant { get => 29098; }

        /// <summary>
        /// s. Header 12
        /// Soll in UI eingegeben werden. Speichern + vorschlagen für nächste Session.
        /// </summary>
        public static int Client { get => 55003; }

        /// <summary>
        /// s. UI -> Buchungsjahr
        /// </summary>
        public static int Buchungsjahr { get => 2021; }

        /// <summary>
        /// s. Header 15
        /// Soll in UI eingegeben werden.
        /// </summary>
        public static DateTime DateFrom { get => new DateTime(2021, 1, 1); }

        /// <summary>
        /// s. Header 16
        /// Soll in UI eingegeben werden.
        /// </summary>
        public static DateTime DateUntil { get => new DateTime(2021, 12, 31); }

        /// <summary>
        /// s. Header 17
        /// Soll in UI eingegeben werden.
        /// </summary>
        public static string LabelEntryBatch { get => "Rechnungen"; }

        /// <summary>
        /// s. Header 18
        /// Soll in UI eingegeben werden.
        /// </summary>
        public static string Initials { get => "JS"; }

        /// <summary>
        /// s. Header 19
        /// Wahrscheinlich soll es int und immer 1 sein.
        /// </summary>
        public static int? RecordType { get => 1; }

        /// <summary>
        /// s. Header 21
        /// "Not locked" (0) is used as default, but it might be also "Locked" (1).
        /// </summary>
        public static int Locking { get => 0; }

        /// <summary>
        /// s. Header 22
        /// Kann aus ECT-Daten ermittelt werden.
        /// </summary>
        public static string CurrencyCode { get => "EUR"; }

        /// <summary>
        /// s. Header 27
        /// </summary>
        public static string DatevSKR { get => string.Empty; }

        /// <summary>
        /// s. Header 31: Processing flag of the transferring application
        /// Soll in UI eingegeben werden.
        /// </summary>
        public static string ApplicationInformation { get => "Some information"; }
    }
}
