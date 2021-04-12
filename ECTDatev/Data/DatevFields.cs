using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECTDatev.Data
{
    /// <summary>
    /// s. Datev-Doku 3.5
    /// </summary>
    public static class DatevFields
    {
        public static readonly Dictionary<int, string> DataCategory = new Dictionary<int, string>
        {
            { 21, "Buchungsstapel" },
            { 65, "Wiederkehrende Buchungen" },
            { 67, "Buchungstextkonstanten" },
            { 20, "Sachkontenbeschriftungen" },
            { 47, "Konto - Notizen" },
            { 16, "Debitoren -/ Kreditoren" },
            { 44, "Textschlüssel" },
            { 46, "Zahlungsbedingungen" },
            { 48, "Diverse Adressen" },
            { 63, "Buchungssätze der Anlagenbuchführung" },
            { 62, "Filialen der Anlagenbuchführung" }
        };

        public static bool IsDataCategoryValid(int dataCategoryID)
        {
            return DatevFields.DataCategory.ContainsKey(dataCategoryID);
        }

        public static readonly Dictionary<int, string> FormatName = new Dictionary<int, string>
        {
            { 21, "Buchungsstapel" },
            { 65, "Wiederkehrende Buchungen" },
            { 67, "Buchungstextkonstanten" },
            { 20, "Kontenbeschriftungen" },
            { 16, "Debitoren/Kreditoren" },
            { 44, "Textschlüssel" },
            { 46, "Zahlungsbedingungen" },
            { 48, "Diverse Adressen" },
            { 63, "Anlagenbuchführung – Buchungssätze" },
            { 62, "Anlagenbuchführung – Filialen" }
        };

        public static readonly Dictionary<string, int> FormatVersion = new Dictionary<string, int>
        {
            { "Buchungsstapel", 9 },
            { "Wiederkehrende Buchungen", 3 },
            { "Buchungstextkonstanten", 1 },
            { "Kontenbeschriftungen", 2 },
            { "Konto - Notizen", 1 },
            { "Debitoren / Kreditoren", 5 },
            { "Zahlungsbedingungen", 2 },
            { "Diverse Adressen", 2 },
            { "Buchungssätze der Anlagenbuchführung", 1 },
            { "Filialen der Anlagenbuchführung", 1 }
        };

        public static readonly Dictionary<int, string> RecordType = new Dictionary<int, string>
        {
            { 1, "Finanzbuchführung" },
            { 2, "Jahresabschluss" }
        };

        public static readonly Dictionary<int, string> AccountingReason = new Dictionary<int, string>
        {
            { 0, "vom Rechnungslegungszweck unabhängig" },
            { 50, "Handelsrecht" },
            { 30, "Steuerrecht" },
            { 64, "IFRS" },
            { 40, "Kalkulatorik" },
            { 11, "Reserviert" },
            { 12, "Reserviert" }
        };

        public static readonly Dictionary<int, string> Locking = new Dictionary<int, string>
        {
            { 0, "keine Festschreibung" },
            { 1, "Festschreibung" }
        };

        public class ColumnInfo
        {
            //public int OrderNumber { get; set; }
            public string Name { get; set; }
            public string TypeText { get; set; }
            public int Length { get; set; }
            public int DecimalPlaces { get; set; }
            public int MaxLength { get; set; }
            public string MandatoryText { get; set; }
            public string OptionalInfo { get; set; }

            public bool IsMandatory
            {
                get
                {
                    return (MandatoryText.ToLower().Equals("ja") || MandatoryText.ToLower().Equals("yes"));
                }
            }

            /// <summary>
            /// Optional info for the verification of the given column (given in DATEV documentation)
            /// </summary>
            public bool HasOptionalInfo
            {
                get
                {
                    return
                        OptionalInfo != null && OptionalInfo.StartsWith(Constants.MacroStart) && OptionalInfo.EndsWith(Constants.MacroEnd) &&
                        OptionalInfo.Length > Constants.MacroStart.Length + Constants.MacroEnd.Length;
                }
            }

            /// <summary>
            /// Returns the optional info as a string ohne <see cref="Constants.MacroStart"/> und <see cref="Constants.MacroEnd"/>.
            /// </summary>
            ///
            /// <returns>The optional info without the leading and ending macro recognizer chars (but starting with a macro keyword as one string with possible with separators) if there is any, otherwise an empty string.</returns>
            ///
            public string GetOptionalInfo()
            {
                if (HasOptionalInfo)
                {
                    return OptionalInfo.Substring(Constants.MacroStart.Length, OptionalInfo.Length - Constants.MacroStart.Length - Constants.MacroEnd.Length);
                }
                else
                    return string.Empty;
            }

            /// <summary>
            /// Returns the optional info as a string array for but without the given macro keyword e.g. <see cref="Constants.MacroKeyword_OneOf"/>.
            /// Both <see cref="Constants.MacroStart"/> und <see cref="Constants.MacroEnd"/> aren´t in that array.
            /// If the optional info has another macro in it than the given keyword, then an empty array will be returned.
            /// </summary>
            /// <param name="macroKeyword">The given macro keyword</param>
            /// <returns>This is already without the leading and ending macro recognizer chars and without the given macro keyword (as a string array without separators) if there is any, otherwise an empty string.</returns>
            public string[] GetOptionalInfo(string macroKeyword)
            {
                if (GetOptionalInfo().StartsWith(macroKeyword))
                {
                    return OptionalInfo.Substring(macroKeyword.Length + Constants.FieldSeparator.Length).Split(new string[] { Constants.FieldSeparator }, StringSplitOptions.RemoveEmptyEntries);
                }
                else
                    return new string[] { };
            }

            /// <summary>
            /// Returns the full macro as string array.
            /// Both <see cref="Constants.MacroStart"/> und <see cref="Constants.MacroEnd"/> aren´t in that array.
            /// </summary>
            /// <returns>The full macro.</returns>
            private string[] GetMacro()
            {
                if (HasOptionalInfo)
                {
                    return GetOptionalInfo().Split(new string[] { Constants.FieldSeparator }, StringSplitOptions.None);
                }
                else
                    return new string[] { };
            }

            public string getMacroKeyword()
            {
                if (HasOptionalInfo)
                {
                    return GetMacro()[0];
                }
                else
                    return string.Empty;
            }

            public ColumnInfo(string name, string typeText, int length, int decimalPlaces, int maxLength, string mandatoryText = "No", string optionalInfo = "")
            {
                Name = name;
                TypeText = typeText;
                Length = length;
                DecimalPlaces = decimalPlaces;
                MaxLength = maxLength;
                MandatoryText = mandatoryText;
                OptionalInfo = optionalInfo;
            }
        }

        public static Dictionary<int, ColumnInfo> GenerateColumnInfos(int dataCategoryID)
        {
            Dictionary<int, ColumnInfo> columns = new Dictionary<int, ColumnInfo>();

            switch (dataCategoryID)
            {
                case 21: // Buchungsstapel

                    Dictionary<int, ColumnInfo> columns21 = new Dictionary<int, ColumnInfo>()
                    {
                        // No., Field Name, Type, Length, DP, Max. Length, Required
                        { 1, new ColumnInfo( "Umsatz (ohne Soll/Haben-Kz)", "Betrag", 10, 2, 13, "Ja", optionalInfo: Constants.MacroStart + Constants.MacroKeyword_Abs + Constants.MacroEnd) },
                        { 2, new ColumnInfo( "Soll/Haben-Kennzeichen", "Text", 1, 0, 1, "Ja", optionalInfo: Constants.MacroStart + Constants.MacroKeyword_SetDebitOrCredit + Constants.FieldSeparator + "S" + Constants.FieldSeparator + "H" + Constants.MacroEnd) },
                        { 3, new ColumnInfo( "WKZ Umsatz", "Text", 3, 0, 3) },
                        { 4, new ColumnInfo( "Kurs", "Zahl", 4, 6, 11, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_NotAllowed + Constants.FieldSeparator + 0 + Constants.MacroEnd) },
                        { 5, new ColumnInfo( "Basis-Umsatz", "Betrag", 10, 2, 13) },
                        { 6, new ColumnInfo( "WKZ Basis-Umsatz", "Text", 3, 0, 3) },
                        { 7, new ColumnInfo( "Konto", "Konto", 9, 0, 9, "Ja") },
                        { 8, new ColumnInfo( "Gegenkonto (ohne BU-Schlüssel)", "Konto", 9, 0, 9, "Ja") },
                        { 9, new ColumnInfo( "BU-Schlüssel", "Text", 4, 0, 4) },
                        { 10, new ColumnInfo("Belegdatum", "Datum", 4, 0, 4, "Ja", optionalInfo: Constants.MacroStart + Constants.MacroKeyword_DateFormat + Constants.FieldSeparator + "ddMM" + Constants.MacroEnd) },
                        { 11, new ColumnInfo("Belegfeld 1", "Text", 36, 0, 36, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_AllowedChars + Constants.FieldSeparator + "Numbers" + Constants.FieldSeparator + "Letters" + Constants.FieldSeparator + @"$&%*+-/" + Constants.MacroEnd) },
                        { 12, new ColumnInfo("Belegfeld 2", "Text", 12, 0, 12) },
                        { 13, new ColumnInfo("Skonto", "Betrag", 8, 2, 11, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_NotAllowed + Constants.FieldSeparator + 0 + Constants.MacroEnd) },
                        { 14, new ColumnInfo("Buchungstext", "Text", 60, 0, 60) },
                        { 15, new ColumnInfo("Postensperre", "Zahl", 1, 0, 1, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_OneOf + Constants.FieldSeparator + 0 + Constants.FieldSeparator + 1 + Constants.MacroEnd) },
                        { 16, new ColumnInfo("Diverse Adressnummer", "Text", 9, 0, 9) },
                        { 17, new ColumnInfo("Geschäftspartnerbank", "Zahl", 3, 0, 3) },
                        { 18, new ColumnInfo("Sachverhalt", "Zahl", 2, 0, 2) },
                        { 19, new ColumnInfo("Zinssperre", "Zahl", 1, 0, 1, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_OneOf + Constants.FieldSeparator + 0 + Constants.FieldSeparator + 1 + Constants.MacroEnd) },
                        { 20, new ColumnInfo("Beleglink", "Text", 210, 0, 210) },
                        { 21, new ColumnInfo("Beleginfo - Art 1", "Text", 20, 0, 20) },
                        { 22, new ColumnInfo("Beleginfo - Inhalt 1", "Text", 210, 0, 210) },
                        { 23, new ColumnInfo("Beleginfo - Art 2", "Text", 20, 0, 20) },
                        { 24, new ColumnInfo("Beleginfo - Inhalt 2", "Text", 210, 0, 210) },
                        { 25, new ColumnInfo("Beleginfo - Art 3", "Text", 20, 0, 20) },
                        { 26, new ColumnInfo("Beleginfo - Inhalt 3", "Text", 210, 0, 210) },
                        { 27, new ColumnInfo("Beleginfo - Art 4", "Text", 20, 0, 20) },
                        { 28, new ColumnInfo("Beleginfo - Inhalt 4", "Text", 210, 0, 210) },
                        { 29, new ColumnInfo("Beleginfo - Art 5", "Text", 20, 0, 20) },
                        { 30, new ColumnInfo("Beleginfo - Inhalt 5", "Text", 210, 0, 210) },
                        { 31, new ColumnInfo("Beleginfo - Art 6", "Text", 20, 0, 20) },
                        { 32, new ColumnInfo("Beleginfo - Inhalt 6", "Text", 210, 0, 210) },
                        { 33, new ColumnInfo("Beleginfo - Art 7", "Text", 20, 0, 20) },
                        { 34, new ColumnInfo("Beleginfo - Inhalt 7", "Text", 210, 0, 210) },
                        { 35, new ColumnInfo("Beleginfo - Art 8", "Text", 20, 0, 20) },
                        { 36, new ColumnInfo("Beleginfo - Inhalt 8", "Text", 210, 0, 210) },
                        { 37, new ColumnInfo("KOST1 - Kostenstelle", "Text", 36, 0, 36) },
                        { 38, new ColumnInfo("KOST2 - Kostenstelle", "Text", 36, 0, 36) },
                        { 39, new ColumnInfo("Kost-Menge", "Zahl", 12, 4, 17) },
                        { 40, new ColumnInfo("EU-Land u. UStID", "Text", 15, 0, 15) },
                        { 41, new ColumnInfo("EU-Steuersatz", "Zahl", 2, 2, 5) },
                        { 42, new ColumnInfo("Abw. Versteuerungsart", "Text", 1, 0, 1, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_OneOf + Constants.FieldSeparator + "I" + Constants.FieldSeparator + "K" + Constants.FieldSeparator + "P" + Constants.FieldSeparator + "S" + Constants.MacroEnd) },
                        { 43, new ColumnInfo("Sachverhalt L+L", "Zahl", 3, 0, 3, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_NotAllowed + Constants.FieldSeparator + 0 + Constants.MacroEnd) },
                        { 44, new ColumnInfo("Funktionsergänzung L+L", "Zahl", 3, 0, 3, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_NotAllowed + Constants.FieldSeparator + 0 + Constants.MacroEnd) },
                        { 45, new ColumnInfo("BU 49 Hauptfunktionstyp", "Zahl", 1, 0, 1) },
                        { 46, new ColumnInfo("BU 49 Hauptfunktionsnummer", "Zahl", 2, 0, 2) },
                        { 47, new ColumnInfo("BU 49 Funktionsergänzung", "Zahl", 3, 0, 3) },
                        { 48, new ColumnInfo("Zusatzinformation - Art 1", "Text", 20, 0, 20) },
                        { 49, new ColumnInfo("Zusatzinformation- Inhalt 1", "Text", 210, 0, 210) },
                        { 50, new ColumnInfo("Zusatzinformation - Art 2", "Text", 20, 0, 20) },
                        { 51, new ColumnInfo("Zusatzinformation- Inhalt 2", "Text", 210, 0, 210) },
                        { 52, new ColumnInfo("Zusatzinformation - Art 3", "Text", 20, 0, 20) },
                        { 53, new ColumnInfo("Zusatzinformation- Inhalt 3", "Text", 210, 0, 210) },
                        { 54, new ColumnInfo("Zusatzinformation - Art 4", "Text", 20, 0, 20) },
                        { 55, new ColumnInfo("Zusatzinformation- Inhalt 4", "Text", 210, 0, 210) },
                        { 56, new ColumnInfo("Zusatzinformation - Art 5", "Text", 20, 0, 20) },
                        { 57, new ColumnInfo("Zusatzinformation- Inhalt 5", "Text", 210, 0, 210) },
                        { 58, new ColumnInfo("Zusatzinformation - Art 6", "Text", 20, 0, 20) },
                        { 59, new ColumnInfo("Zusatzinformation- Inhalt 6", "Text", 210, 0, 210) },
                        { 60, new ColumnInfo("Zusatzinformation - Art 7", "Text", 20, 0, 20) },
                        { 61, new ColumnInfo("Zusatzinformation- Inhalt 7", "Text", 210, 0, 210) },
                        { 62, new ColumnInfo("Zusatzinformation - Art 8", "Text", 20, 0, 20) },
                        { 63, new ColumnInfo("Zusatzinformation- Inhalt 8", "Text", 210, 0, 210) },
                        { 64, new ColumnInfo("Zusatzinformation - Art 9", "Text", 20, 0, 20) },
                        { 65, new ColumnInfo("Zusatzinformation- Inhalt 9", "Text", 210, 0, 210) },
                        { 66, new ColumnInfo("Zusatzinformation - Art 10", "Text", 20, 0, 20) },
                        { 67, new ColumnInfo("Zusatzinformation- Inhalt 10", "Text", 210, 0, 210) },
                        { 68, new ColumnInfo("Zusatzinformation - Art 11", "Text", 20, 0, 20) },
                        { 69, new ColumnInfo("Zusatzinformation- Inhalt 11", "Text", 210, 0, 210) },
                        { 70, new ColumnInfo("Zusatzinformation - Art 12", "Text", 20, 0, 20) },
                        { 71, new ColumnInfo("Zusatzinformation- Inhalt 12", "Text", 210, 0, 210) },
                        { 72, new ColumnInfo("Zusatzinformation - Art 13", "Text", 20, 0, 20) },
                        { 73, new ColumnInfo("Zusatzinformation- Inhalt 13", "Text", 210, 0, 210) },
                        { 74, new ColumnInfo("Zusatzinformation - Art 14", "Text", 20, 0, 20) },
                        { 75, new ColumnInfo("Zusatzinformation- Inhalt 14", "Text", 210, 0, 210) },
                        { 76, new ColumnInfo("Zusatzinformation - Art 15", "Text", 20, 0, 20) },
                        { 77, new ColumnInfo("Zusatzinformation- Inhalt 15", "Text", 210, 0, 210) },
                        { 78, new ColumnInfo("Zusatzinformation - Art 16", "Text", 20, 0, 20) },
                        { 79, new ColumnInfo("Zusatzinformation- Inhalt 16", "Text", 210, 0, 210) },
                        { 80, new ColumnInfo("Zusatzinformation - Art 17", "Text", 20, 0, 20) },
                        { 81, new ColumnInfo("Zusatzinformation- Inhalt 17", "Text", 210, 0, 210) },
                        { 82, new ColumnInfo("Zusatzinformation - Art 18", "Text", 20, 0, 20) },
                        { 83, new ColumnInfo("Zusatzinformation- Inhalt 18", "Text", 210, 0, 210) },
                        { 84, new ColumnInfo("Zusatzinformation - Art 19", "Text", 20, 0, 20) },
                        { 85, new ColumnInfo("Zusatzinformation- Inhalt 19", "Text", 210, 0, 210) },
                        { 86, new ColumnInfo("Zusatzinformation - Art 20", "Text", 20, 0, 20) },
                        { 87, new ColumnInfo("Zusatzinformation- Inhalt 20", "Text", 210, 0, 210) },
                        { 88, new ColumnInfo("Stück", "Zahl", 8, 0, 8) },
                        { 89, new ColumnInfo("Gewicht", "Zahl", 8, 2, 11) },
                        { 90, new ColumnInfo("Zahlweise", "Zahl", 2, 0, 2, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_OneOf + Constants.FieldSeparator + 1 + Constants.FieldSeparator + 2 + Constants.FieldSeparator + 3 + Constants.MacroEnd) },
                        { 91, new ColumnInfo("Forderungsart", "Text", 10, 0, 10) },
                        { 92, new ColumnInfo("Veranlagungsjahr", "Zahl", 4, 0, 4, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_DateFormat + Constants.FieldSeparator + "yyyy" + Constants.MacroEnd) },
                        { 93, new ColumnInfo("Zugeordnete Fälligkeit", "Datum", 8, 0, 8, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_DateFormat + Constants.FieldSeparator + "ddMMyyyy" + Constants.MacroEnd) },
                        { 94, new ColumnInfo("Skontotyp", "Zahl", 1, 0, 1, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_OneOf + Constants.FieldSeparator + 1 + Constants.FieldSeparator + 2 + Constants.MacroEnd) },
                        { 95, new ColumnInfo("Auftragsnummer", "Text", 30, 0, 30) },
                        { 96, new ColumnInfo("Buchungstyp", "Text", 2, 0, 2, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_OneOf + Constants.FieldSeparator + "AA" + Constants.FieldSeparator + "AG" + Constants.FieldSeparator + "AV" + Constants.FieldSeparator + "SR" + Constants.FieldSeparator + "SU" + Constants.FieldSeparator + "SG" + Constants.FieldSeparator + "SO" + Constants.MacroEnd) },
                        { 97, new ColumnInfo("USt-Schlüssel (Anzahlungen)", "Zahl", 2, 0, 2) },
                        { 98, new ColumnInfo("EU-Land (Anzahlungen)", "Text", 2, 0, 2) },
                        { 99, new ColumnInfo("Sachverhalt L+L (Anzahlungen)", "Zahl", 3, 0, 3, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_NotAllowed + Constants.FieldSeparator + 0 + Constants.MacroEnd) },
                        { 100, new ColumnInfo("EU-Steuersatz (Anzahlungen)", "Zahl", 2, 2, 5) },
                        { 101, new ColumnInfo("Erlöskonto (Anzahlungen)", "Konto", 8, 0, 8) },
                        { 102, new ColumnInfo("Herkunft-Kz", "Text", 2, 0, 2) },
                        { 103, new ColumnInfo("Buchungs GUID", "Text", 36, 0, 36) },
                        { 104, new ColumnInfo("KOST-Datum", "Datum", 8, 0, 8, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_DateFormat + Constants.FieldSeparator + "ddMMyyyy" + Constants.MacroEnd) },
                        { 105, new ColumnInfo("SEPA-Mandatsreferenz", "Text", 35, 0, 35) },
                        { 106, new ColumnInfo("Skontosperre", "Zahl", 1, 0, 1, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_OneOf + Constants.FieldSeparator + 0 + Constants.FieldSeparator + 1 + Constants.MacroEnd) },
                        { 107, new ColumnInfo("Gesellschaftername", "Text", 76, 0, 76) },
                        { 108, new ColumnInfo("Beteiligtennummer", "Zahl", 4, 0, 4) },
                        { 109, new ColumnInfo("Identifikationsnummer", "Text", 11, 0, 11) },
                        { 110, new ColumnInfo("Zeichnernummer", "Text", 20, 0, 20) },
                        { 111, new ColumnInfo("Postensperre bis", "Datum", 8, 0, 8, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_DateFormat + Constants.FieldSeparator + "ddMMyyyy" + Constants.MacroEnd) },
                        { 112, new ColumnInfo("Bezeichnung SoBil-Sachverhalt", "Text", 30, 0, 30) },
                        { 113, new ColumnInfo("Kennzeichen SoBil-Buchung", "Zahl", 2, 0, 2, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_OneOf + Constants.FieldSeparator + 0 + Constants.FieldSeparator + 1 + Constants.MacroEnd) },
                        { 114, new ColumnInfo("Festschreibung", "Zahl", 1, 0, 1, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_OneOf + Constants.FieldSeparator + 0 + Constants.FieldSeparator + 1 + Constants.MacroEnd) },
                        { 115, new ColumnInfo("Leistungsdatum", "Datum", 8, 0, 8, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_DateFormat + Constants.FieldSeparator + "ddMMyyyy" + Constants.MacroEnd) },
                        { 116, new ColumnInfo("Datum Zuord. Steuerperiode", "Datum", 8, 0, 8, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_DateFormat + Constants.FieldSeparator + "ddMMyyyy" + Constants.MacroEnd) },
                        { 117, new ColumnInfo("Fälligkeit", "Datum", 8, 0, 8, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_DateFormat + Constants.FieldSeparator + "ddMMyyyy" + Constants.MacroEnd) },
                        { 118, new ColumnInfo("Generalumkehr (GU)", "Text", 1, 0, 1, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_OneOf + Constants.FieldSeparator + 0 + Constants.FieldSeparator + 1 + Constants.MacroEnd) },
                        { 119, new ColumnInfo("Steuersatz", "Zahl", 2, 2, 5) },
                        { 120, new ColumnInfo("Land", "Text", 2, 0, 2, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_UpperCase+ Constants.MacroEnd) }
                    };

                    columns = columns21;
                    break;
                case 65: // Wiederkehrende Buchungen
                    Dictionary<int, ColumnInfo> columns65 = new Dictionary<int, ColumnInfo>()
                    {
                        { 1, new ColumnInfo( "B1", "Zahl", 1, 0, 1, "Ja", optionalInfo: Constants.MacroStart + Constants.MacroKeyword_OneOf + Constants.FieldSeparator + 1 + Constants.FieldSeparator + 2 + Constants.FieldSeparator + 3 + Constants.MacroEnd) },
                        { 2, new ColumnInfo( "WKZ (Umsatz)", "Text", 3, 0, 3) },
                        { 3, new ColumnInfo( "Umsatz (ohne Soll/Haben-Kz)", "Betrag", 10, 2, 13, "Ja", optionalInfo: Constants.MacroStart + Constants.MacroKeyword_Abs + Constants.MacroEnd) },
                        { 4, new ColumnInfo( "Soll/Haben-Kennzeichen", "Text", 1, 0, 1, "Ja", optionalInfo: Constants.MacroStart + Constants.MacroKeyword_SetDebitOrCredit + Constants.FieldSeparator + "S" + Constants.FieldSeparator + "H" + Constants.MacroEnd) },
                        { 5, new ColumnInfo( "Kurs", "Zahl", 4, 6, 11, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_NotAllowed + Constants.FieldSeparator + 0 + Constants.MacroEnd) },
                        { 6, new ColumnInfo( "Basis-Umsatz", "Betrag", 10, 2, 13) },
                        { 7, new ColumnInfo( "WKZ Basis-Umsatz", "Text", 3, 0, 3) },
                        { 8, new ColumnInfo( "BU-Schlüssel", "Text", 4, 0, 4) },
                        { 9, new ColumnInfo( "Gegenkonto (ohne BU-Schlüssel)", "Konto", 9, 0, 9, "Ja") },
                        { 10, new ColumnInfo("Belegfeld1", "Text", 36, 0, 36, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_AllowedChars + Constants.FieldSeparator + "Numbers" + Constants.FieldSeparator + "Letters" + Constants.FieldSeparator + @"$&%*+-/" + Constants.MacroEnd) },
                        { 11, new ColumnInfo("Belegfeld2", "Text", 12, 0, 12) },
                        { 12, new ColumnInfo("Beginndatum", "Datum", 8, 0, 8, "Ja", optionalInfo: Constants.MacroStart + Constants.MacroKeyword_DateFormat + Constants.FieldSeparator + "ddMMyyyy" + Constants.MacroEnd) },
                        { 13, new ColumnInfo("Kontonummer", "Konto", 9, 0, 9, "Ja") },
                        { 14, new ColumnInfo("Stück", "Zahl", 8, 0, 8) },
                        { 15, new ColumnInfo("Gewicht", "Zahl", 8, 2, 11) },
                        { 16, new ColumnInfo("KOST1-Kostenstelle", "Text", 36, 0, 36) },
                        { 17, new ColumnInfo("KOST2-Kostenstelle", "Text", 36, 0, 36) },
                        { 18, new ColumnInfo("KOST-Menge", "Zahl", 12, 4, 17) },
                        { 19, new ColumnInfo("Skonto", "Betrag", 8, 2, 11, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_NotAllowed + Constants.FieldSeparator + 0 + Constants.MacroEnd) },
                        { 20, new ColumnInfo("Buchungstext", "Text", 60, 0, 60) },
                        { 21, new ColumnInfo("Postensperre", "Zahl", 1, 0, 1, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_OneOf + Constants.FieldSeparator + 0 + Constants.FieldSeparator + 1 + Constants.MacroEnd) },
                        { 22, new ColumnInfo("Diverse Adressnummer", "Text", 9, 0, 9) },
                        { 23, new ColumnInfo("Geschäftspartnerbank", "Zahl", 3, 0, 3) },
                        { 24, new ColumnInfo("Sachverhalt", "Zahl", 2, 0, 2) },
                        { 25, new ColumnInfo("Zinssperre", "Zahl", 1, 0, 1, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_OneOf + Constants.FieldSeparator + 0 + Constants.FieldSeparator + 1 + Constants.MacroEnd) },
                        { 26, new ColumnInfo("Beleglink", "Text", 210, 0, 210) },
                        { 27, new ColumnInfo("EU-Land u. UStId", "Text", 15, 0, 15) },
                        { 28, new ColumnInfo("EU-Steuersatz", "Zahl", 2, 2, 5) },
                        { 29, new ColumnInfo("Leerfeld", "Text", 1, 0, 1) },
                        { 30, new ColumnInfo("Sachverhalt L+L", "Zahl", 3, 0, 3, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_NotAllowed + Constants.FieldSeparator + 0 + Constants.MacroEnd) },
                        { 31, new ColumnInfo("BU 49 Hauptfunktionstyp", "Zahl", 1, 0, 1) },
                        { 32, new ColumnInfo("BU 49 Hauptfunktionsnummer", "Zahl", 2, 0, 2) },
                        { 33, new ColumnInfo("BU 49 Funktionsergänzung", "Zahl", 3, 0, 3) },
                        { 34, new ColumnInfo("Zusatzinformation - Art 1", "Text", 20, 0, 20) },
                        { 35, new ColumnInfo("Zusatzinformation- Inhalt 1", "Text", 210, 0, 210) },
                        { 36, new ColumnInfo("Zusatzinformation - Art 2", "Text", 20, 0, 20) },
                        { 37, new ColumnInfo("Zusatzinformation- Inhalt 2", "Text", 210, 0, 210) },
                        { 38, new ColumnInfo("Zusatzinformation - Art 3", "Text", 20, 0, 20) },
                        { 39, new ColumnInfo("Zusatzinformation- Inhalt 3", "Text", 210, 0, 210) },
                        { 40, new ColumnInfo("Zusatzinformation - Art 4", "Text", 20, 0, 20) },
                        { 41, new ColumnInfo("Zusatzinformation- Inhalt 4", "Text", 210, 0, 210) },
                        { 42, new ColumnInfo("Zusatzinformation - Art 5", "Text", 20, 0, 20) },
                        { 43, new ColumnInfo("Zusatzinformation- Inhalt 5", "Text", 210, 0, 210) },
                        { 44, new ColumnInfo("Zusatzinformation - Art 6", "Text", 20, 0, 20) },
                        { 45, new ColumnInfo("Zusatzinformation- Inhalt 6", "Text", 210, 0, 210) },
                        { 46, new ColumnInfo("Zusatzinformation - Art 7", "Text", 20, 0, 20) },
                        { 47, new ColumnInfo("Zusatzinformation- Inhalt 7", "Text", 210, 0, 210) },
                        { 48, new ColumnInfo("Zusatzinformation - Art 8", "Text", 20, 0, 20) },
                        { 49, new ColumnInfo("Zusatzinformation- Inhalt 8", "Text", 210, 0, 210) },
                        { 50, new ColumnInfo("Zusatzinformation - Art 9", "Text", 20, 0, 20) },
                        { 51, new ColumnInfo("Zusatzinformation- Inhalt 9", "Text", 210, 0, 210) },
                        { 52, new ColumnInfo("Zusatzinformation - Art 10", "Text", 20, 0, 20) },
                        { 53, new ColumnInfo("Zusatzinformation- Inhalt 10", "Text", 210, 0, 210) },
                        { 54, new ColumnInfo("Zusatzinformation - Art 11", "Text", 20, 0, 20) },
                        { 55, new ColumnInfo("Zusatzinformation- Inhalt 11", "Text", 210, 0, 210) },
                        { 56, new ColumnInfo("Zusatzinformation - Art 12", "Text", 20, 0, 20) },
                        { 57, new ColumnInfo("Zusatzinformation- Inhalt 12", "Text", 210, 0, 210) },
                        { 58, new ColumnInfo("Zusatzinformation - Art 13", "Text", 20, 0, 20) },
                        { 59, new ColumnInfo("Zusatzinformation- Inhalt 13", "Text", 210, 0, 210) },
                        { 60, new ColumnInfo("Zusatzinformation - Art 14", "Text", 20, 0, 20) },
                        { 61, new ColumnInfo("Zusatzinformation- Inhalt 14", "Text", 210, 0, 210) },
                        { 62, new ColumnInfo("Zusatzinformation - Art 15", "Text", 20, 0, 20) },
                        { 63, new ColumnInfo("Zusatzinformation- Inhalt 15", "Text", 210, 0, 210) },
                        { 64, new ColumnInfo("Zusatzinformation - Art 16", "Text", 20, 0, 20) },
                        { 65, new ColumnInfo("Zusatzinformation- Inhalt 16", "Text", 210, 0, 210) },
                        { 66, new ColumnInfo("Zusatzinformation - Art 17", "Text", 20, 0, 20) },
                        { 67, new ColumnInfo("Zusatzinformation- Inhalt 17", "Text", 210, 0, 210) },
                        { 68, new ColumnInfo("Zusatzinformation - Art 18", "Text", 20, 0, 20) },
                        { 69, new ColumnInfo("Zusatzinformation- Inhalt 18", "Text", 210, 0, 210) },
                        { 70, new ColumnInfo("Zusatzinformation - Art 19", "Text", 20, 0, 20) },
                        { 71, new ColumnInfo("Zusatzinformation- Inhalt 19", "Text", 210, 0, 210) },
                        { 72, new ColumnInfo("Zusatzinformation - Art 20", "Text", 20, 0, 20) },
                        { 73, new ColumnInfo("Zusatzinformation- Inhalt 20", "Text", 210, 0, 210) },
                        { 74, new ColumnInfo("Zahlweise", "Zahl", 2, 0, 2) },
                        { 75, new ColumnInfo("Forderungsart", "Text", 10, 0, 10) },
                        { 76, new ColumnInfo("Veranlagungsjahr", "Zahl", 4, 0, 4, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_DateFormat + Constants.FieldSeparator + "yyyy" + Constants.MacroEnd) },
                        { 77, new ColumnInfo("Zugeordnete Fälligkeit", "Datum", 8, 0, 8, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_DateFormat + Constants.FieldSeparator + "ddMMyyyy" + Constants.MacroEnd) },
                        { 78, new ColumnInfo("Zuletzt per", "Datum", 8, 0, 8, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_DateFormat + Constants.FieldSeparator + "ddMMyyyy" + Constants.MacroEnd) },
                        { 79, new ColumnInfo("Nächste Fälligkeit", "Datum", 8, 0, 8, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_DateFormat + Constants.FieldSeparator + "ddMMyyyy" + Constants.MacroEnd) },
                        { 80, new ColumnInfo("Enddatum", "Datum", 8, 0, 8, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_DateFormat + Constants.FieldSeparator + "ddMMyyyy" + Constants.MacroEnd) },
                        { 81, new ColumnInfo("Zeitintervallart", "Text", 3, 0, 3, "Ja", optionalInfo: Constants.MacroStart + Constants.MacroKeyword_OneOf + Constants.FieldSeparator + "TAG" + Constants.FieldSeparator + "MON" + Constants.MacroEnd) },
                        { 82, new ColumnInfo("Zeitabstand", "Zahl", 3, 0, 3, "Ja") },
                        { 83, new ColumnInfo("Wochentag", "Zahl", 3, 0, 3) },
                        { 84, new ColumnInfo("Monat", "Zahl", 2, 0, 2) },
                        { 85, new ColumnInfo("Ordnungszahl Tag im Monat", "Zahl", 2, 0, 2, "Ja") },
                        { 86, new ColumnInfo("Ordnungszahl Wochentag", "Zahl", 1, 0, 1) },
                        { 87, new ColumnInfo("EndeTyp", "Zahl", 1, 0, 1, "Ja", optionalInfo: Constants.MacroStart + Constants.MacroKeyword_OneOf + Constants.FieldSeparator + 1 + Constants.FieldSeparator + 2 + Constants.FieldSeparator + 3 + Constants.MacroEnd) },
                        { 88, new ColumnInfo("Gesellschaftername", "Text", 76, 0, 76) },
                        { 89, new ColumnInfo("Beteiligtennummer", "Zahl", 4, 0, 4) },
                        { 90, new ColumnInfo("Identifikationsnummer", "Text", 11, 0, 11) },
                        { 91, new ColumnInfo("Zeichnernummer", "Text", 20, 0, 20) },
                        { 92, new ColumnInfo("SEPA-Mandatsreferenz", "Text", 35, 0, 35) },
                        { 93, new ColumnInfo("Postensperre bis", "Datum", 8, 0, 8, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_DateFormat + Constants.FieldSeparator + "ddMMyyyy" + Constants.MacroEnd) },
                        { 94, new ColumnInfo("KOST-Datum", "Datum", 8, 0, 8, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_DateFormat + Constants.FieldSeparator + "ddMMyyyy" + Constants.MacroEnd) },
                        { 95, new ColumnInfo("Bezeichnung SoBil-Sachverhalt", "Text", 30, 0, 30) },
                        { 96, new ColumnInfo("Kennzeichen SoBil-Buchung", "Zahl", 2, 0, 2) },
                        { 97, new ColumnInfo("Generalumkehr (GU)", "Text", 1, 0, 1, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_OneOf + Constants.FieldSeparator + 0 + Constants.FieldSeparator + 1 + Constants.MacroEnd) },
                        { 98, new ColumnInfo("Steuersatz", "Zahl", 2, 2, 5) },
                        { 99, new ColumnInfo("Land", "Text", 2, 0, 2, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_UpperCase+ Constants.MacroEnd) }
                    };

                    columns = columns65;
                    break;
                case 67: // Buchungstextkonstanten
                    Dictionary<int, ColumnInfo> columns67 = new Dictionary<int, ColumnInfo>()
                    {
                        { 1, new ColumnInfo("Nummer", "Zahl", 4, 0, 4, "Ja") },
                        { 2, new ColumnInfo("Buchungstext", "Text", 60, 0, 60) }
                    };

                    columns = columns67;
                    break;
                case 20: // Sachkontenbeschriftungen
                    Dictionary<int, ColumnInfo> columns20 = new Dictionary<int, ColumnInfo>()
                    {
                        { 1, new ColumnInfo("Konto", "Konto", 9, 0, 9, "Ja") },
                        { 2, new ColumnInfo("Kontenbeschriftung", "Text", 40, 0, 40) },
                        { 3, new ColumnInfo("Sprach-ID", "Text", 5, 0, 5, optionalInfo: Constants.MacroStart + "OneOf" + Constants.FieldSeparator + "de-DE" + Constants.FieldSeparator + "en-GB" + Constants.MacroEnd) }
                    };

                    columns = columns20;
                    break;
                case 47: // Konto - Notizen
                    throw new NotImplementedException("GenerateColumns: dataCategoryID=" + dataCategoryID.ToString() + " => " + DataCategory[dataCategoryID]);

                case 16: // Debitoren -/ Kreditoren
                    throw new NotImplementedException("GenerateColumns: dataCategoryID=" + dataCategoryID.ToString() + " => " + DataCategory[dataCategoryID]);

                case 44: // Textschlüssel
                    Dictionary<int, ColumnInfo> columns44 = new Dictionary<int, ColumnInfo>()
                    {
                        { 1, new ColumnInfo("TS-Nr.", "Zahl", 4, 0, 4, "Ja") },
                        { 2, new ColumnInfo("Beschriftung", "Text", 40, 0, 40) },
                        { 3, new ColumnInfo("Ref.-TS", "Text", 4, 0, 4, "Ja") },
                        { 4, new ColumnInfo("Konto Soll", "Konto", 8, 0, 8) },
                        { 5, new ColumnInfo("Konto Haben", "Konto", 8, 0, 8) },
                        { 6, new ColumnInfo("Sprach-ID", "Text", 5, 0, 5, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_OneOf + Constants.FieldSeparator + "de-DE" + Constants.FieldSeparator + "en-EN" + Constants.MacroEnd) }
                    };

                    columns = columns44;
                    break;
                case 46: // Zahlungsbedingungen
                    throw new NotImplementedException("GenerateColumns: dataCategoryID=" + dataCategoryID.ToString() + " => " + DataCategory[dataCategoryID]);

                case 48: // Diverse Adressen
                    throw new NotImplementedException("GenerateColumns: dataCategoryID=" + dataCategoryID.ToString() + " => " + DataCategory[dataCategoryID]);

                case 63: // Buchungssätze der Anlagenbuchführung
                    Dictionary<int, ColumnInfo> columns63 = new Dictionary<int, ColumnInfo>()
                    {
                        { 1, new ColumnInfo("Bereich", "Zahl", 2, 0, 2, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_OneOf + Constants.FieldSeparator + 0 + Constants.FieldSeparator + 50 + Constants.FieldSeparator + 30 + Constants.FieldSeparator + 40 + Constants.FieldSeparator + 11 + Constants.FieldSeparator + 12 + Constants.FieldSeparator + 64 + Constants.MacroEnd) },
                        { 2, new ColumnInfo("Kontonummer", "Zahl", 8, 0, 8, "Ja", optionalInfo: Constants.MacroStart + Constants.MacroKeyword_NotAllowed + Constants.FieldSeparator + 0 + Constants.MacroEnd) },
                        { 3, new ColumnInfo("Buchungssatztyp", "Text", 3, 0, 3, "Ja") },
                        { 4, new ColumnInfo("KontonummerSoll", "Zahl", 8, 0, 8, "Ja") },
                        { 5, new ColumnInfo("KontonummerHaben", "Zahl", 8, 0, 8, "Ja") },
                        { 6, new ColumnInfo("Buchungstext", "Text", 60, 0, 60, "Ja") }
                    };

                    columns = columns63;
                    break;
                case 62: // Filialen der Anlagenbuchführung
                    Dictionary<int, ColumnInfo> columns62 = new Dictionary<int, ColumnInfo>()
                    {
                        { 1, new ColumnInfo("Filialnummer", "Zahl", 6, 0, 6, "Ja") },
                        { 2, new ColumnInfo("Filialbezeichnung", "Text", 60, 0, 60, "Ja") }
                    };

                    columns = columns62;
                    break;
            }
            return columns;
        }

    }
}
