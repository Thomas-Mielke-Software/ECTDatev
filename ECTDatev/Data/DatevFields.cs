using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            { 20, "Kontenbeschriftungen" },
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
            /// <param name="number">The (1 based) order number of the macro in this <see cref="OptionalInfo"/></param>
            /// <returns>The optional info without the leading and ending macro recognizer chars (but starting with a macro keyword as one string with possible with separators) if there is any, otherwise an empty string.</returns>           
            public string GetOptionalInfo(int number = 1)
            {
                if (HasOptionalInfo)
                {
                    if (GetNumberOfMacros() == 1)
                    {
                        return OptionalInfo.Substring(Constants.MacroStart.Length, OptionalInfo.Length - Constants.MacroStart.Length - Constants.MacroEnd.Length);
                    }
                    else
                    {
                        string[] strArr = Regex.Split(OptionalInfo, Regex.Escape(Constants.MacroSeparator));
                        return strArr[number - 1].Substring(Constants.MacroStart.Length, strArr[number - 1].Length - Constants.MacroStart.Length - Constants.MacroEnd.Length);
                    }
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
                    return GetOptionalInfo().Split(Constants.FieldSeparator.ToCharArray(), StringSplitOptions.None);
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

            public string GetMacroKeyword()
            {
                if (HasOptionalInfo)
                {
                    return GetMacro()[0];
                }
                else
                    return string.Empty;
            }

            public bool HasMacro
            {
                get
                {
                    bool ret = false;
                    if (HasOptionalInfo)
                    {
                        if (OptionalInfo.StartsWith(Constants.MacroStart) && OptionalInfo.EndsWith(Constants.MacroEnd))
                        {
                            ret = true;
                        }
                        else
                        {
                            throw new FormatException(string.Format("Seems to be not a well formed macro: {0}", OptionalInfo));
                        }
                    }
                    
                    return ret;
                }
            }

            /// <summary>
            /// Tells the number of joined macros in the given <see cref="OptionalInfo"/>.
            /// </summary>
            /// <returns>The number of joined macros.</returns>
            public int GetNumberOfMacros()
            {
                int ret = 0;

                if (HasOptionalInfo)
                {
                    MatchCollection mc = Regex.Matches(OptionalInfo, Regex.Escape(Constants.MacroSeparator));
                    if (mc.Count == 0)
                    {
                        if (HasMacro)
                        {
                            ret = 1;
                        }
                    }
                    else
                    {
                        ret = mc.Count + 1;
                    }
                }
                
                return ret;
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
                        { 4, new ColumnInfo( "Kurs", "Zahl", 4, 6, 11, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_NotAllowedChars + Constants.FieldSeparator + 0 + Constants.MacroEnd) },
                        { 5, new ColumnInfo( "Basis-Umsatz", "Betrag", 10, 2, 13) },
                        { 6, new ColumnInfo( "WKZ Basis-Umsatz", "Text", 3, 0, 3) },
                        { 7, new ColumnInfo( "Konto", "Konto", 9, 0, 9, "Ja") },
                        { 8, new ColumnInfo( "Gegenkonto (ohne BU-Schlüssel)", "Konto", 9, 0, 9, "Ja") },
                        { 9, new ColumnInfo( "BU-Schlüssel", "Text", 4, 0, 4) },
                        { 10, new ColumnInfo("Belegdatum", "Datum", 4, 0, 4, "Ja", optionalInfo: Constants.MacroStart + Constants.MacroKeyword_DateFormat + Constants.FieldSeparator + "ddMM" + Constants.MacroEnd) },
                        { 11, new ColumnInfo("Belegfeld 1", "Text", 36, 0, 36, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_AllowedChars + Constants.FieldSeparator + "Numbers" + Constants.FieldSeparator + "Letters" + Constants.FieldSeparator + @"$&%*+-/" + Constants.MacroEnd + Constants.MacroSeparator + Constants.MacroStart + Constants.MacroKeyword_NotAllowedChars + Constants.FieldSeparator + " äöüß.,;:" + Constants.MacroEnd) },
                        { 12, new ColumnInfo("Belegfeld 2", "Text", 12, 0, 12) },
                        { 13, new ColumnInfo("Skonto", "Betrag", 8, 2, 11, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_NotAllowedChars + Constants.FieldSeparator + 0 + Constants.MacroEnd) },
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
                        { 43, new ColumnInfo("Sachverhalt L+L", "Zahl", 3, 0, 3, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_NotAllowedChars + Constants.FieldSeparator + 0 + Constants.MacroEnd) },
                        { 44, new ColumnInfo("Funktionsergänzung L+L", "Zahl", 3, 0, 3, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_NotAllowedChars + Constants.FieldSeparator + 0 + Constants.MacroEnd) },
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
                        { 99, new ColumnInfo("Sachverhalt L+L (Anzahlungen)", "Zahl", 3, 0, 3, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_NotAllowedChars + Constants.FieldSeparator + 0 + Constants.MacroEnd) },
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
                        { 5, new ColumnInfo( "Kurs", "Zahl", 4, 6, 11, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_NotAllowedChars + Constants.FieldSeparator + 0 + Constants.MacroEnd) },
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
                        { 19, new ColumnInfo("Skonto", "Betrag", 8, 2, 11, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_NotAllowedChars + Constants.FieldSeparator + 0 + Constants.MacroEnd) },
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
                        { 30, new ColumnInfo("Sachverhalt L+L", "Zahl", 3, 0, 3, optionalInfo: Constants.MacroStart + Constants.MacroKeyword_NotAllowedChars + Constants.FieldSeparator + 0 + Constants.MacroEnd) },
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
                        { 2, new ColumnInfo("Kontonummer", "Zahl", 8, 0, 8, "Ja", optionalInfo: Constants.MacroStart + Constants.MacroKeyword_NotAllowedChars + Constants.FieldSeparator + 0 + Constants.MacroEnd) },
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

        public class TaxInfo
        {
            public int TaxKey { get; set; }
            public string Description { get; set; }
            public string TaxRateDescription { get; set; }
            public decimal TaxRate { get; set; }
            public Collection<object> UStVA { get; set; }
            public Collection<string> Function { get; set; }
            public String GroupName { get; set; }

            public TaxInfo(
                int taxKey, string description, string taxRateDescription, 
                Collection<object> uStVA, Collection<string> function, String groupName
                )
            {
                this.TaxKey = taxKey;
                this.Description = description;
                this.TaxRateDescription = taxRateDescription;
                this.TaxRate = 0;
                this.UStVA = uStVA;
                this.Function = function;
                this.GroupName = groupName;
            }

            public TaxInfo(
                int taxKey, string description, decimal taxRate,
                Collection<object> uStVA, Collection<string> function, String groupName
                )
            {
                this.TaxKey = taxKey;
                this.Description = description;
                this.TaxRateDescription = null;
                this.TaxRate = taxRate;
                this.UStVA = uStVA;
                this.Function = function;
                this.GroupName = groupName;
            }

            public static Collection<TaxInfo> GetTaxInfos(int year)
            {
                Collection<TaxInfo> taxInfos = new Collection<TaxInfo>();

                switch (year)
                {
                    case 2021:
                        Collection<TaxInfo> taxInfos2021 = new Collection<TaxInfo>()
                        {
                            new TaxInfo( 1,"Umsatzsteuerfrei (mit Vorsteuerausweis)",0,new Collection<object>(){ 43 },new Collection<string>(){ "AM 82000" },"steuerfreie Umsätze" ),
                            new TaxInfo( 2,"Umsatzsteuer",7,new Collection<object>(){ 86 },new Collection<string>(){ "AM 80002", "AM 80070" },"Lieferungen und sonstige Leistungen" ),
                            new TaxInfo( 3,"Umsatzsteuer",19,new Collection<object>(){ 81 },new Collection<string>(){ "AM 80001", "AM 80190" },"Lieferungen und sonstige Leistungen" ),
                            new TaxInfo( 4,"Umsatzsteuer",5,new Collection<object>(){ 35 },new Collection<string>(){ "AM 80050" },"Lieferungen und sonstige Leistungen" ),
                            new TaxInfo( 5,"Umsatzsteuer",16,new Collection<object>(){ 35 },new Collection<string>(){ "AM 80160" },"Lieferungen und sonstige Leistungen" ),
                            new TaxInfo( 6,"Vorsteuer",5,new Collection<object>(){ 66 },new Collection<string>(){ "AV 30050" },"Vorsteuerbeträge aus Rechnungen von anderen Unternehmen" ),
                            new TaxInfo( 7,"Vorsteuer",16,new Collection<object>(){ 66 },new Collection<string>(){ "AV 30160" },"Vorsteuerbeträge aus Rechnungen von anderen Unternehmen" ),
                            new TaxInfo( 8,"Vorsteuer",7,new Collection<object>(){ 66 },new Collection<string>(){ "AV 30002", "AV 30070" },"Vorsteuerbeträge aus Rechnungen von anderen Unternehmen" ),
                            new TaxInfo( 9,"Vorsteuer",19,new Collection<object>(){ 66 },new Collection<string>(){ "AV 30001", "AV 30190" },"Vorsteuerbeträge aus Rechnungen von anderen Unternehmen" ),
                            new TaxInfo( 10,"nicht steuerbarer Umsatz in Deutschland (Steuerpflicht im anderen EU-Land)","wählbar EU",new Collection<object>(){ 45 },new Collection<string>(){ "AM 62xxx" },"nicht steuerbare Umsätze" ),
                            new TaxInfo( 11,"steuerfreie innergem. Lieferung mit USt-IdNr. § 4 Nr. 1 b) UStG","-",new Collection<object>(){ 41, "und ZM" },new Collection<string>(){ "AM 74000" },"innergem. Lieferung" ),
                            new TaxInfo( 12,"innergem. Lieferung ohne USt-IdNr.",7,new Collection<object>(){ 86 },new Collection<string>(){ "AM 60002", "AM 60070" },"innergem. Lieferung" ),
                            new TaxInfo( 13,"innergem. Lieferung ohne USt-IdNr.",19,new Collection<object>(){ 81 },new Collection<string>(){ "AM 60001", "AM 60190" },"innergem. Lieferung" ),
                            new TaxInfo( 14,"innergem. Lieferung ohne USt-IdNr.",5,new Collection<object>(){ 35 },new Collection<string>(){ "AM 60050" },"innergem. Lieferung" ),
                            new TaxInfo( 15,"innergem. Lieferung ohne USt-IdNr.",16,new Collection<object>(){ 35 },new Collection<string>(){ "AM 60160" },"innergem. Lieferung" ),
                            new TaxInfo( 16,"steuerpflichtiger innergem. Erwerb § 1a UStG",5,new Collection<object>(){ 95, 61 },new Collection<string>(){ "AV 54050" },"innergem. Erwerb" ),
                            new TaxInfo( 17,"steuerpflichtiger innergem. Erwerb § 1a UStG",16,new Collection<object>(){ 95, 61 },new Collection<string>(){ "AV 54160" },"innergem. Erwerb" ),
                            new TaxInfo( 18,"steuerpflichtiger innergem. Erwerb § 1a UStG",7,new Collection<object>(){ 93, 61 },new Collection<string>(){ "AV 54002", "AV 54070" },"innergem. Erwerb" ),
                            new TaxInfo( 19,"steuerpflichtiger innergem. Erwerb § 1a UStG",19,new Collection<object>(){ 89, 61 },new Collection<string>(){ "AV 54001", "AV 54190" },"innergem. Erwerb" ),
                            new TaxInfo( 44,"elektronische Dienstleistungen, MOSS","wählbar EU",new Collection<object>(){ 45 },new Collection<string>(){ "AM 50xxx" },"nicht steuerbare Umsätze" ),
                            new TaxInfo( 46,"erbrachte Leistung § 13b UStG","-",new Collection<object>(){ 60 },new Collection<string>(){ "wählbar" },"§ 13b UStG Leistungserbringer" ),
                            new TaxInfo( 47,"nicht steuerbare sonstige Leistung § 18b Satz 1 Nr. 2 UStG","-",new Collection<object>(){ 21, "und ZM" },new Collection<string>(){ "AM 34000" },"nicht steuerbare Umsätze" ),
                            new TaxInfo( 91,"erhaltene Leistung § 13b UStG, alle Steuertatbestände",7,new Collection<object>(){ 46, 73, 84, 67 },new Collection<string>(){ "wählbar" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 92,"erhaltene Leistung § 13b UStG, alle Steuertatbestände (ohne Vorsteuerabzug)",7,new Collection<object>(){ 46, 73, 84,  },new Collection<string>(){ "wählbar" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 94,"erhaltene Leistung § 13b UStG, alle Steuertatbestände",19,new Collection<object>(){ 46, 73, 84, 67 },new Collection<string>(){ "wählbar" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 95,"erhaltene Leistung § 13b UStG, alle Steuertatbestände (ohne Vorsteuerabzug)",19,new Collection<object>(){ 46, 73, 84,  },new Collection<string>(){ "wählbar" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 96,"Vorsteuer (aufzuteilende Vorsteuer)",5,new Collection<object>(){ },new Collection<string>(){ "AV 30050" },"Vorsteuerbeträge aus Rechnungen von anderen Unternehmen" ),
                            new TaxInfo( 97,"Vorsteuer (aufzuteilende Vorsteuer)",16,new Collection<object>(){ },new Collection<string>(){ "AV 30160" },"Vorsteuerbeträge aus Rechnungen von anderen Unternehmen" ),
                            new TaxInfo( 98,"Vorsteuer (aufzuteilende Vorsteuer)",7,new Collection<object>(){ },new Collection<string>(){ "AV 30002" },"Vorsteuerbeträge aus Rechnungen von anderen Unternehmen" ),
                            new TaxInfo( 99,"Vorsteuer (aufzuteilende Vorsteuer)",19,new Collection<object>(){ },new Collection<string>(){ "AV 30001" },"Vorsteuerbeträge aus Rechnungen von anderen Unternehmen" ),
                            new TaxInfo( 100,"Umsatzsteuer","wählbar",new Collection<object>(){ },new Collection<string>(){ "AM 80070", "AM 80190", "AM 80160" },"Lieferungen und sonstige Leistungen" ),
                            new TaxInfo( 101,"Umsatzsteuer",19,new Collection<object>(){ 81 },new Collection<string>(){ "AM 80001", "AM 80190" },"Lieferungen und sonstige Leistungen" ),
                            new TaxInfo( 102,"Umsatzsteuer",7,new Collection<object>(){ 86 },new Collection<string>(){ "AM 80002", "AM 80070" },"Lieferungen und sonstige Leistungen" ),
                            new TaxInfo( 110,"Umsatzsteuer Entnahme von Gegenständen","wählbar",new Collection<object>(){ },new Collection<string>(){ "AM 84070", "AM 84190", "AM 84160" },"unentgeltliche Wertabgabe" ),
                            new TaxInfo( 111,"Umsatzsteuer Entnahme von Gegenständen",19,new Collection<object>(){ 81 },new Collection<string>(){ "AM 84001", "AM 84190" },"unentgeltliche Wertabgabe" ),
                            new TaxInfo( 112,"Umsatzsteuer Entnahme von Gegenständen",7,new Collection<object>(){ 86 },new Collection<string>(){ "AM 84002", "AM 84070" },"unentgeltliche Wertabgabe" ),
                            new TaxInfo( 120,"Umsatzsteuer Verwendung von Gegenständen","wählbar",new Collection<object>(){ },new Collection<string>(){ "AM 85070", "AM 85190", "AM 85160" },"unentgeltliche Wertabgabe" ),
                            new TaxInfo( 121,"Umsatzsteuer Verwendung von Gegenständen",19,new Collection<object>(){ 81 },new Collection<string>(){ "AM 85001", "AM 85190" },"unentgeltliche Wertabgabe" ),
                            new TaxInfo( 122,"Umsatzsteuer Verwendung von Gegenständen",7,new Collection<object>(){ 86 },new Collection<string>(){ "AM 85002", "AM 85070" },"unentgeltliche Wertabgabe" ),
                            new TaxInfo( 130,"Umsatzsteuer sonstige Leistung unentgeltlich","wählbar",new Collection<object>(){ },new Collection<string>(){ "AM 86070", "AM 86190", "AM 86160" },"unentgeltliche Wertabgabe" ),
                            new TaxInfo( 131,"Umsatzsteuer sonstige Leistung unentgeltlich",19,new Collection<object>(){ 81 },new Collection<string>(){ "AM 86001", "AM 86190" },"unentgeltliche Wertabgabe" ),
                            new TaxInfo( 132,"Umsatzsteuer sonstige Leistung unentgeltlich",7,new Collection<object>(){ 86 },new Collection<string>(){ "AM 86002", "AM 86070" },"unentgeltliche Wertabgabe" ),
                            new TaxInfo( 140,"Umsatzsteuer unentgeltliche Zuwendung","wählbar",new Collection<object>(){ },new Collection<string>(){ "AM 87070", "AM 87190", "AM 87160" },"unentgeltliche Wertabgabe" ),
                            new TaxInfo( 141,"Umsatzsteuer unentgeltliche Zuwendung",19,new Collection<object>(){ 81 },new Collection<string>(){ "AM 87001", "AM 87190" },"unentgeltliche Wertabgabe" ),
                            new TaxInfo( 142,"Umsatzsteuer unentgeltliche Zuwendung",7,new Collection<object>(){ 86 },new Collection<string>(){ "AM 87002", "AM 87070" },"unentgeltliche Wertabgabe" ),
                            new TaxInfo( 171,"umsatzsteuerfrei mit Vorsteuerabzug nicht EU, § 4 Nr. 2-7 UStG","-",new Collection<object>(){ 43 },new Collection<string>(){ "AM 82000" },"steuerfreie Umsätze" ),
                            new TaxInfo( 172,"umsatzsteuerfrei mit Vorsteuerabzug nicht EU, Offshore-Steuerabkommen usw.","-",new Collection<object>(){ 43 },new Collection<string>(){ "AM 82510" },"steuerfreie Umsätze" ),
                            new TaxInfo( 173,"umsatzsteuerfrei mit Vorsteuerabzug nicht EU, § 4 Nr. 1a UStG","-",new Collection<object>(){ 43 },new Collection<string>(){ "AM 82511" },"steuerfreie Umsätze" ),
                            new TaxInfo( 174,"umsatzsteuerfrei mit Vorsteuerabzug nicht EU, § 25 Abs. 2 UStG","-",new Collection<object>(){ 43 },new Collection<string>(){ "AM 82513" },"steuerfreie Umsätze" ),
                            new TaxInfo( 181,"umsatzsteuerfrei ohne Vorsteuerabzug § 4 Nr. 8 ff. UStG","-",new Collection<object>(){ 48 },new Collection<string>(){ "AM 83000" },"steuerfreie Umsätze" ),
                            new TaxInfo( 182,"umsatzsteuerfrei ohne Vorsteuerabzug § 4 UStG","-",new Collection<object>(){ 48 },new Collection<string>(){ "AM 83510" },"steuerfreie Umsätze" ),
                            new TaxInfo( 183,"umsatzsteuerfrei ohne Vorsteuerabzug","-",new Collection<object>(){ 48 },new Collection<string>(){ "AM 83511" },"steuerfreie Umsätze" ),
                            new TaxInfo( 184,"umsatzsteuerfrei ohne Vorsteuerabzug § 4 Nr. 12 UStG","-",new Collection<object>(){ 48 },new Collection<string>(){ "AM 83512" },"steuerfreie Umsätze" ),
                            new TaxInfo( 191,"nicht steuerbare Leistung, Drittland, Nettobetrag","-",new Collection<object>(){ 45 },new Collection<string>(){ "AM 62000" },"nicht steuerbare Umsätze" ),
                            new TaxInfo( 200,"erbrachte Leistung § 13b UStG","wählbar",new Collection<object>(){ },new Collection<string>(){ "AM 72510", "AM 72000" },"§ 13b UStG Leistungserbringer" ),
                            new TaxInfo( 201,"erbrachte Leistung § 13b Abs. 2 Nr. 10 UStG, Mobilfunkgeräte usw.","-",new Collection<object>(){ 60 },new Collection<string>(){ "AM 72510" },"§ 13b UStG Leistungserbringer" ),
                            new TaxInfo( 202,"erbrachte Leistung § 13b Abs. 5 UStG, übrige Umsätze","-",new Collection<object>(){ 60 },new Collection<string>(){ "AM 72000" },"§ 13b UStG Leistungserbringer" ),
                            new TaxInfo( 220,"innergem. Lieferung ohne USt-IdNr.","wählbar",new Collection<object>(){ 81, 86,  35, 36 },new Collection<string>(){ "AM 60070", "AM 60190", "AM 60160" },"innergem. Lieferung" ),
                            new TaxInfo( 221,"innergem. Lieferung ohne USt-IdNr.",19,new Collection<object>(){ 81 },new Collection<string>(){ "AM 60001", "AM 60190" },"innergem. Lieferung" ),
                            new TaxInfo( 222,"innergem. Lieferung ohne USt-IdNr.",7,new Collection<object>(){ 86 },new Collection<string>(){ "AM 60002", "AM 60070" },"innergem. Lieferung" ),
                            new TaxInfo( 225,"elektronische Dienstleistungen § 3a Abs. 5 Sätze 3 und 4 UStG","wählbar",new Collection<object>(){ },new Collection<string>(){ "AM 66070", "AM 66190" },"Lieferungen und Leistungen" ),
                            new TaxInfo( 226,"elektronische Dienstleistungen § 3a Abs. 5 Sätze 3 und 4 UStG",19,new Collection<object>(){ 81 },new Collection<string>(){ "AM 66001", "AM 66190" },"Lieferungen und Leistungen" ),
                            new TaxInfo( 227,"elektronische Dienstleistungen § 3a Abs. 5 Sätze 3 und 4 UStG",7,new Collection<object>(){ 86 },new Collection<string>(){ "AM 66002", "AM 66070" },"Lieferungen und Leistungen" ),
                            new TaxInfo( 231,"steuerfreie innergem. Lieferung mit USt-IdNr. § 4 Nr. 1 b) UStG","-",new Collection<object>(){ 41, "und ZM" },new Collection<string>(){ "AM 74000" },"innergem. Lieferung" ),
                            new TaxInfo( 232,"steuerfreie innergem. Lieferung ohne USt-IdNr., Neufahrzeuge","-",new Collection<object>(){ 44 },new Collection<string>(){ "AM 76000" },"innergem. Lieferung" ),
                            new TaxInfo( 233,"innergem. Dreiecksgeschäft § 25b Abs. 2 UStG","-",new Collection<object>(){ 42, "und ZM" },new Collection<string>(){ "AM 68000" },"innergem. Dreiecksgeschäft" ),
                            new TaxInfo( 240,"nicht steuerbare Lieferung EU","wählbar EU",new Collection<object>(){ 45 },new Collection<string>(){ "AM 62xxx" },"nicht steuerbare Umsätze" ),
                            new TaxInfo( 250,"nicht steuerbare sonstige Leistung EU","wählbar EU",new Collection<object>(){ 45 },new Collection<string>(){ "AM 64xxx" },"nicht steuerbare Umsätze" ),
                            new TaxInfo( 260,"nicht steuerbare Lieferung/sonstige Leistung EU, Nettobetrag","-",new Collection<object>(){ 45 },new Collection<string>(){ "AM 64000" },"nicht steuerbare Umsätze" ),
                            new TaxInfo( 270,"nicht steuerbare sonstige Leistung § 18b Satz 1 Nr. 2 UStG","-",new Collection<object>(){ 21, "und ZM" },new Collection<string>(){ "AM 34000" },"nicht steuerbare Umsätze" ),
                            new TaxInfo( 280,"elektronische Dienstleistungen, MOSS","wählbar EU",new Collection<object>(){ 45 },new Collection<string>(){ "AM 50xxx" },"nicht steuerbare Umsätze" ),
                            new TaxInfo( 310,"Umsatzsteuer LuF, Pauschalierung mit Regelbesteuerung","wählbar",new Collection<object>(){ },new Collection<string>(){ "AM 73070", "AM 73190", "AM 73160" },"land- und forstwirtschaftliche Betriebe" ),
                            new TaxInfo( 311,"Umsatzsteuer LuF, Pauschalierung mit Regelbesteuerung",19,new Collection<object>(){ 81 },new Collection<string>(){ "AM 73001", "AM 73190" },"land- und forstwirtschaftliche Betriebe" ),
                            new TaxInfo( 312,"Umsatzsteuer LuF, Pauschalierung mit Regelbesteuerung",7,new Collection<object>(){ 86 },new Collection<string>(){ "AM 73002", "AM 73070" },"land- und forstwirtschaftliche Betriebe" ),
                            new TaxInfo( 350,"Pauschalierung und unrichtig ausgewiesene USt","wählbar",new Collection<object>(){ 69 },new Collection<string>(){ "AM 71004", "AM 71008" },"land- und forstwirtschaftliche Betriebe" ),
                            new TaxInfo( 352,"Pauschalierung und unrichtig ausgewiesene USt",7,new Collection<object>(){ 69 },new Collection<string>(){ "AM 71004" },"land- und forstwirtschaftliche Betriebe" ),
                            new TaxInfo( 381,"Steuer auf Altteile",(decimal)20.9,new Collection<object>(){ 81 },new Collection<string>(){ "AM 80990" },"Sonstige" ),
                            new TaxInfo( 400,"Vorsteuer","wählbar",new Collection<object>(){ 66 },new Collection<string>(){ "AV 30070", "AV 30190", "AV 30160" },"Vorsteuerbeträge aus Rechnungen von anderen Unternehmen" ),
                            new TaxInfo( 401,"Vorsteuer",19,new Collection<object>(){ 66 },new Collection<string>(){ "AV 30001", "AV 30190" },"Vorsteuerbeträge aus Rechnungen von anderen Unternehmen" ),
                            new TaxInfo( 402,"Vorsteuer",7,new Collection<object>(){ 66 },new Collection<string>(){ "AV 30002", "AV 30070" },"Vorsteuerbeträge aus Rechnungen von anderen Unternehmen" ),
                            new TaxInfo( 408,"Vorsteuer",(decimal)10.7,new Collection<object>(){ 66 },new Collection<string>(){ "AV 30003", "AV 30107" },"Vorsteuerbeträge aus Rechnungen von anderen Unternehmen" ),
                            new TaxInfo( 409,"Vorsteuer",(decimal)5.5,new Collection<object>(){ 66 },new Collection<string>(){ "AV 30009", "AV 30055" },"Vorsteuerbeträge aus Rechnungen von anderen Unternehmen" ),
                            new TaxInfo( 480,"Steuerschuldner bei Auslagerung § 13a Abs. 1 Nr. 6 UStG","wählbar",new Collection<object>(){ 66, 69 },new Collection<string>(){ "AV 35070", "AV 35190", "AV 35160" },"§ 13a UStG" ),
                            new TaxInfo( 481,"Steuerschuldner bei Auslagerung § 13a Abs. 1 Nr. 6 UStG",19,new Collection<object>(){ 66, 69 },new Collection<string>(){ "AV 35001", "AV 35190" },"§ 13a UStG" ),
                            new TaxInfo( 482,"Steuerschuldner bei Auslagerung § 13a Abs. 1 Nr. 6 UStG",7,new Collection<object>(){ 66, 69 },new Collection<string>(){ "AV 35002", "AV 35070" },"§ 13a UStG" ),
                            new TaxInfo( 490,"ohne Vorsteuerabzug",0,new Collection<object>(){ },new Collection<string>(){ },"Sonstige" ),
                            new TaxInfo( 501,"erhaltene Leistung § 13b UStG, alle Steuertatbestände",19,new Collection<object>(){ },new Collection<string>(){ "wählbar" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 502,"erhaltene Leistung § 13b UStG, alle Steuertatbestände",7,new Collection<object>(){ },new Collection<string>(){ "wählbar" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 505,"sonstige EU-Leistung § 13b Abs. 1 UStG","wählbar",new Collection<object>(){ 46, 47, 67 },new Collection<string>(){ "AV 24070", "AV 24190", "AV 24160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 506,"sonstige EU-Leistung § 13b Abs. 1 UStG",19,new Collection<object>(){ 46, 47, 67 },new Collection<string>(){ "AV 24001", "AV 24190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 507,"sonstige EU-Leistung § 13b Abs. 1 UStG",7,new Collection<object>(){ 46, 47, 67 },new Collection<string>(){ "AV 24002", "AV 24070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 510,"Leistungen ausländischer Unternehmer § 13b Abs. 2 Nr. 1 UStG","wählbar",new Collection<object>(){ 84, 85, 67 },new Collection<string>(){ "AV 36070", "AV 36190", "AV 36160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 511,"Leistungen ausländischer Unternehmer § 13b Abs. 2 Nr. 1 UStG",19,new Collection<object>(){ 84, 85, 67 },new Collection<string>(){ "AV 36001", "AV 36190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 512,"Leistungen ausländischer Unternehmer § 13b Abs. 2 Nr. 1 UStG",7,new Collection<object>(){ 84, 85, 67 },new Collection<string>(){ "AV 36002", "AV 36070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 515,"sicherungsübereignete Gegenstände § 13b Abs. 2 Nr. 2 UStG","wählbar",new Collection<object>(){ 84, 85, 67 },new Collection<string>(){ "AV 42070", "AV 42190", "AV 42160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 516,"sicherungsübereignete Gegenstände § 13b Abs. 2 Nr. 2 UStG",19,new Collection<object>(){ 84, 85, 67 },new Collection<string>(){ "AV 42001", "AV 42190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 517,"sicherungsübereignete Gegenstände § 13b Abs. 2 Nr. 2 UStG",7,new Collection<object>(){ 84, 85, 67 },new Collection<string>(){ "AV 42002", "AV 42070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 520,"Umsätze Grunderwerbsteuergesetz § 13b Abs. 2 Nr. 3 UStG","wählbar",new Collection<object>(){ 73, 74, 67 },new Collection<string>(){ "AV 44070", "AV 44190", "AV 44160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 521,"Umsätze Grunderwerbsteuergesetz § 13b Abs. 2 Nr. 3 UStG",19,new Collection<object>(){ 73, 74, 67 },new Collection<string>(){ "AV 44001", "AV 44190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 522,"Umsätze Grunderwerbsteuergesetz § 13b Abs. 2 Nr. 3 UStG",7,new Collection<object>(){ 73, 74, 67 },new Collection<string>(){ "AV 44002", "AV 44070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 525,"Bauleistungen § 13b Abs. 2 Nr. 4 UStG","wählbar",new Collection<object>(){ 84, 85, 67 },new Collection<string>(){ "AV 46070", "AV 46190", "AV 46160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 526,"Bauleistungen § 13b Abs. 2 Nr. 4 UStG",19,new Collection<object>(){ 84, 85, 67 },new Collection<string>(){ "AV 46001", "AV 46190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 527,"Bauleistungen § 13b Abs. 2 Nr. 4 UStG",7,new Collection<object>(){ 84, 85, 67 },new Collection<string>(){ "AV 46002", "AV 46070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 530,"Gas/Elektrizität ausländischer Unternehmer § 13b Abs. 2 Nr. 5 UStG","wählbar",new Collection<object>(){ 84, 85, 67 },new Collection<string>(){ "AV 38070", "AV 38190", "AV 38160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 531,"Gas/Elektrizität ausländischer Unternehmer § 13b Abs. 2 Nr. 5 UStG",19,new Collection<object>(){ 84, 85, 67 },new Collection<string>(){ "AV 38001", "AV 38190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 532,"Gas/Elektrizität ausländischer Unternehmer § 13b Abs. 2 Nr. 5 UStG",7,new Collection<object>(){ 84, 85, 67 },new Collection<string>(){ "AV 38002", "AV 38070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 535,"Gas/Elektrizität inländischer Unternehmer § 13b Abs. 2 Nr. 5 b) UStG","wählbar",new Collection<object>(){ 84, 85, 67 },new Collection<string>(){ "AV 26070", "AV 26190", "AV 26160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 536,"Gas/Elektrizität inländischer Unternehmer § 13b Abs. 2 Nr. 5 b) UStG",19,new Collection<object>(){ 84, 85, 67 },new Collection<string>(){ "AV 26001", "AV 26190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 537,"Gas/Elektrizität inländischer Unternehmer § 13b Abs. 2 Nr. 5 b) UStG",7,new Collection<object>(){ 84, 85, 67 },new Collection<string>(){ "AV 26002", "AV 26070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 540,"Treibhausgasemissionszertifikate § 13b Abs. 2 Nr. 6 UStG","wählbar",new Collection<object>(){ 84, 85, 67 },new Collection<string>(){ "AV 26070", "AV 26190", "AV 26160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 541,"Treibhausgasemissionszertifikate § 13b Abs. 2 Nr. 6 UStG",19,new Collection<object>(){ 84, 85, 67 },new Collection<string>(){ "AV 26001", "AV 26190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 542,"Treibhausgasemissionszertifikate § 13b Abs. 2 Nr. 6 UStG",7,new Collection<object>(){ 84, 85, 67 },new Collection<string>(){ "AV 26002", "AV 26070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 545,"Anlage 3: Altmetall/Schrott/Plastik oder Ähnliches § 13b Abs. 2 Nr. 7 UStG","wählbar",new Collection<object>(){ 84, 85, 67 },new Collection<string>(){ "AV 26070", "AV 26190", "AV 26160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 546,"Anlage 3: Altmetall/Schrott/Plastik oder Ähnliches § 13b Abs. 2 Nr. 7 UStG",19,new Collection<object>(){ 84, 85, 67 },new Collection<string>(){ "AV 26001", "AV 26190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 547,"Anlage 3: Altmetall/Schrott/Plastik oder Ähnliches § 13b Abs. 2 Nr. 7 UStG",7,new Collection<object>(){ 84, 85, 67 },new Collection<string>(){ "AV 26002", "AV 26070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 550,"Gebäudereinigung § 13b Abs. 2 Nr. 8 UStG","wählbar",new Collection<object>(){ 84, 85, 67 },new Collection<string>(){ "AV 26070", "AV 26190", "AV 26160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 551,"Gebäudereinigung § 13b Abs. 2 Nr. 8 UStG",19,new Collection<object>(){ 84, 85, 67 },new Collection<string>(){ "AV 26001", "AV 26190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 552,"Gebäudereinigung § 13b Abs. 2 Nr. 8 UStG",7,new Collection<object>(){ 84, 85, 67 },new Collection<string>(){ "AV 26002", "AV 26070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 555,"Lieferung von Gold § 13b Abs. 2 Nr. 9 UStG","wählbar",new Collection<object>(){ 84, 85, 67 },new Collection<string>(){ "AV 26070", "AV 26190", "AV 26160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 556,"Lieferung von Gold § 13b Abs. 2 Nr. 9 UStG",19,new Collection<object>(){ 84, 85, 67 },new Collection<string>(){ "AV 26001", "AV 26190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 557,"Lieferung von Gold § 13b Abs. 2 Nr. 9 UStG",7,new Collection<object>(){ 84, 85, 67 },new Collection<string>(){ "AV 26002", "AV 26070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 560,"Mobilfunkgeräte usw. § 13b Abs. 2 Nr. 10 UStG","wählbar",new Collection<object>(){ 84, 85, 67 },new Collection<string>(){ "AV 26070", "AV 26190", "AV 26160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 561,"Mobilfunkgeräte usw. § 13b Abs. 2 Nr. 10 UStG",19,new Collection<object>(){ 84, 85, 67 },new Collection<string>(){ "AV 26001", "AV 26190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 562,"Mobilfunkgeräte usw. § 13b Abs. 2 Nr. 10 UStG",7,new Collection<object>(){ 84, 85, 67 },new Collection<string>(){ "AV 26002", "AV 26070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 565,"Gegenstände aus Anlage 4, § 13b Abs. 2 Nr. 11 UStG","wählbar",new Collection<object>(){ 84, 85, 67 },new Collection<string>(){ "AV 26070", "AV 26190", "AV 26160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 566,"Gegenstände aus Anlage 4, § 13b Abs. 2 Nr. 11 UStG",19,new Collection<object>(){ 84, 85, 67 },new Collection<string>(){ "AV 26001", "AV 26190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 567,"Gegenstände aus Anlage 4, § 13b Abs. 2 Nr. 11 UStG",7,new Collection<object>(){ 84, 85, 67 },new Collection<string>(){ "AV 26002", "AV 26070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 700,"steuerpflichtiger innergem. Erwerb § 1a UStG","wählbar",new Collection<object>(){ 89, 93, 61 },new Collection<string>(){ "AV 54070", "AV 54190", "AV 54160" },"innergem. Erwerb" ),
                            new TaxInfo( 701,"steuerpflichtiger innergem. Erwerb § 1a UStG",19,new Collection<object>(){ 89, 61 },new Collection<string>(){ "AV 54001", "AV 54190" },"innergem. Erwerb" ),
                            new TaxInfo( 702,"steuerpflichtiger innergem. Erwerb § 1a UStG",7,new Collection<object>(){ 93, 61 },new Collection<string>(){ "AV 54002", "AV 54070" },"innergem. Erwerb" ),
                            new TaxInfo( 720,"steuerpflichtiger innergem. Erwerb Neufahrzeuge § 1b Abs. 2 und 3 UStG","wählbar",new Collection<object>(){ 94, 96, 61 },new Collection<string>(){ "AV 53190", "AV 53160" },"innergem. Erwerb" ),
                            new TaxInfo( 721,"steuerpflichtiger innergem. Erwerb Neufahrzeuge § 1b Abs. 2 und 3 UStG",19,new Collection<object>(){ 94, 96, 61 },new Collection<string>(){ "AV 53001", "AV 53190" },"innergem. Erwerb" ),
                            new TaxInfo( 730,"innergem. Dreiecksgeschäft § 25b UStG, letzter Abnehmer","wählbar",new Collection<object>(){ 66, 69 },new Collection<string>(){ "AV 47070", "AV 47190" },"innergem. Dreiecksgeschäft" ),
                            new TaxInfo( 731,"innergem. Dreiecksgeschäft § 25b UStG, letzter Abnehmer",19,new Collection<object>(){ 66, 69 },new Collection<string>(){ "AV 47001", "AV 47190" },"innergem. Dreiecksgeschäft" ),
                            new TaxInfo( 732,"innergem. Dreiecksgeschäft § 25b UStG, letzter Abnehmer",7,new Collection<object>(){ 66, 69 },new Collection<string>(){ "AV 47002", "AV 47070" },"innergem. Dreiecksgeschäft" ),
                            new TaxInfo( 750,"Vorsteuer für EU-Steuersätze","wählbar EU",new Collection<object>(){ },new Collection<string>(){ "AV 29xxx" },"Vorsteuerbeträge aus Rechnungen von anderen Unternehmen" ),
                            new TaxInfo( 781,"steuerfreier innergem. Erwerb §§ 4b und 25c UStG","-",new Collection<object>(){ 91 },new Collection<string>(){ "AV 52000" },"innergem. Erwerb" ),
                            new TaxInfo( 800,"Vorsteuer LuF, Pauschalierung mit Regelbesteuerung","wählbar",new Collection<object>(){ 66 },new Collection<string>(){ "AV 56070", "AV 56190", "AV 56160", "AV 56107" },"land- und forstwirtschaftliche Betriebe" ),
                            new TaxInfo( 801,"Vorsteuer LuF, Pauschalierung mit Regelbesteuerung",19,new Collection<object>(){ 66 },new Collection<string>(){ "AV 56001", "AV 56190" },"land- und forstwirtschaftliche Betriebe" ),
                            new TaxInfo( 802,"Vorsteuer LuF, Pauschalierung mit Regelbesteuerung",7,new Collection<object>(){ 66 },new Collection<string>(){ "AV 56002", "AV 56070" },"land- und forstwirtschaftliche Betriebe" ),
                            new TaxInfo( 808,"Vorsteuer LuF, Pauschalierung mit Regelbesteuerung",(decimal)10.7,new Collection<object>(){ 66 },new Collection<string>(){ "AV 56003", "AV 56107" },"land- und forstwirtschaftliche Betriebe" ),
                            new TaxInfo( 881,"Vorsteuer auf Altteile",(decimal)20.9,new Collection<object>(){ 66 },new Collection<string>(){ "AV 30990" },"Sonstige" ),
                            new TaxInfo( 6501,"erhaltene Leistung § 13b UStG, alle Steuertatbestände (ohne Vorsteuerabzug)",19,new Collection<object>(){ },new Collection<string>(){ "wählbar" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6502,"erhaltene Leistung § 13b UStG, alle Steuertatbestände (ohne Vorsteuerabzug)",7,new Collection<object>(){ },new Collection<string>(){ "wählbar" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6505,"sonstige EU-Leistung § 13b Abs. 1 UStG (ohne Vorsteuerabzug)","wählbar",new Collection<object>(){ 46, 47 },new Collection<string>(){ "AV 25070", "AV 25190", "AV 25160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6506,"sonstige EU-Leistung § 13b Abs. 1 UStG (ohne Vorsteuerabzug)",19,new Collection<object>(){ 46, 47 },new Collection<string>(){ "AV 25001", "AV 25190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6507,"sonstige EU-Leistung § 13b Abs. 1 UStG (ohne Vorsteuerabzug)",7,new Collection<object>(){ 46, 47 },new Collection<string>(){ "AV 25002", "AV 25070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6510,"Leistungen ausländischer Unternehmer § 13b Abs. 2 Nr. 1 UStG (ohne Vorsteuerabzug)","wählbar",new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 37070", "AV 37190", "AV 37160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6511,"Leistungen ausländischer Unternehmer § 13b Abs. 2 Nr. 1 UStG (ohne Vorsteuerabzug)",19,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 37001", "AV 37190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6512,"Leistungen ausländischer Unternehmer § 13b Abs. 2 Nr. 1 UStG (ohne Vorsteuerabzug)",7,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 37002", "AV 37070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6515,"sicherungsübereignete Gegenstände § 13b Abs. 2 Nr. 2 UStG (ohne Vorsteuerabzug)","wählbar",new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 43070", "AV 43190", "AV 43160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6516,"sicherungsübereignete Gegenstände § 13b Abs. 2 Nr. 2 UStG (ohne Vorsteuerabzug)",19,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 43001", "AV 43190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6517,"sicherungsübereignete Gegenstände § 13b Abs. 2 Nr. 2 UStG (ohne Vorsteuerabzug)",7,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 43002", "AV 43070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6520,"Umsätze Grunderwerbsteuergesetz § 13b Abs. 2 Nr. 3 UStG (ohne Vorsteuerabzug)","wählbar",new Collection<object>(){ 73, 74 },new Collection<string>(){ "AV 45070", "AV 45190", "AV 45160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6521,"Umsätze Grunderwerbsteuergesetz § 13b Abs. 2 Nr. 3 UStG (ohne Vorsteuerabzug)",19,new Collection<object>(){ 73, 74 },new Collection<string>(){ "AV 45001", "AV 45190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6522,"Umsätze Grunderwerbsteuergesetz § 13b Abs. 2 Nr. 3 UStG (ohne Vorsteuerabzug)",7,new Collection<object>(){ 73, 74 },new Collection<string>(){ "AV 45002", "AV 45070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6525,"Bauleistungen § 13b Abs. 2 Nr. 4 UStG (ohne Vorsteuerabzug)","wählbar",new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 48070", "AV 48190", "AV 48160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6526,"Bauleistungen § 13b Abs. 2 Nr. 4 UStG (ohne Vorsteuerabzug)",19,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 48001", "AV 48190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6527,"Bauleistungen § 13b Abs. 2 Nr. 4 UStG (ohne Vorsteuerabzug)",7,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 48002", "AV 48070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6530,"Gas/Elektrizität ausländischer Unternehmer § 13b Abs. 2 Nr. 5 UStG (ohne Vorsteuerabzug)","wählbar",new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 39070", "AV 39190", "AV 39160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6531,"Gas/Elektrizität ausländischer Unternehmer § 13b Abs. 2 Nr. 5 UStG (ohne Vorsteuerabzug)",19,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 39001", "AV 39190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6532,"Gas/Elektrizität ausländischer Unternehmer § 13b Abs. 2 Nr. 5 UStG (ohne Vorsteuerabzug)",7,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 39002", "AV 39070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6535,"Gas/Elektrizität inländischer Unternehmer § 13b Abs. 2 Nr. 5 b) UStG (ohne Vorsteuerabzug)","wählbar",new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 27070", "AV 27190", "AV 27160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6536,"Gas/Elektrizität inländischer Unternehmer § 13b Abs. 2 Nr. 5 b) UStG (ohne Vorsteuerabzug)",19,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 27001", "AV 27190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6537,"Gas/Elektrizität inländischer Unternehmer § 13b Abs. 2 Nr. 5 b) UStG (ohne Vorsteuerabzug)",7,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 27002", "AV 27070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6540,"Treibhausgasemissionszertifikate § 13b Abs. 2 Nr. 6 UStG (ohne Vorsteuerabzug)","wählbar",new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 27070", "AV 27190", "AV 27160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6541,"Treibhausgasemissionszertifikate § 13b Abs. 2 Nr. 6 UStG (ohne Vorsteuerabzug)",19,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 27001", "AV 27190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6542,"Treibhausgasemissionszertifikate § 13b Abs. 2 Nr. 6 UStG (ohne Vorsteuerabzug)",7,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 27002", "AV 27070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6545,"Anlage 3: Altmetall/Schrott/Plastik oder Ähnliches § 13b Abs. 2 Nr. 7 UStG (ohne Vorsteuerabzug)","wählbar",new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 27070", "AV 27190", "AV 27160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6546,"Anlage 3: Altmetall/Schrott/Plastik oder Ähnliches § 13b Abs. 2 Nr. 7 UStG (ohne Vorsteuerabzug)",19,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 27001", "AV 27190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6547,"Anlage 3: Altmetall/Schrott/Plastik oder Ähnliches § 13b Abs. 2 Nr. 7 UStG (ohne Vorsteuerabzug)",7,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 27002", "AV 27070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6550,"Gebäudereinigung § 13b Abs. 2 Nr. 8 UStG (ohne Vorsteuerabzug)","wählbar",new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 27070", "AV 27190", "AV 27160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6551,"Gebäudereinigung § 13b Abs. 2 Nr. 8 UStG (ohne Vorsteuerabzug)",19,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 27001", "AV 27190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6552,"Gebäudereinigung § 13b Abs. 2 Nr. 8 UStG (ohne Vorsteuerabzug)",7,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 27002", "AV 27070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6555,"Lieferung von Gold § 13b Abs. 2 Nr. 9 UStG (ohne Vorsteuerabzug)","wählbar",new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 27070", "AV 27190", "AV 27160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6556,"Lieferung von Gold § 13b Abs. 2 Nr. 9 UStG (ohne Vorsteuerabzug)",19,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 27001", "AV 27190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6557,"Lieferung von Gold § 13b Abs. 2 Nr. 9 UStG (ohne Vorsteuerabzug)",7,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 27002", "AV 27070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6560,"Mobilfunkgeräte usw. § 13b Abs. 2 Nr. 10 UStG (ohne Vorsteuerabzug)","wählbar",new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 27070", "AV 27190", "AV 27160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6561,"Mobilfunkgeräte usw. § 13b Abs. 2 Nr. 10 UStG (ohne Vorsteuerabzug)",19,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 27001", "AV 27190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6562,"Mobilfunkgeräte usw. § 13b Abs. 2 Nr. 10 UStG (ohne Vorsteuerabzug)",7,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 27002", "AV 27070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6565,"Gegenstände aus Anlage 4, § 13b Abs. 2 Nr. 11 UStG (ohne Vorsteuerabzug)","wählbar",new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 27070", "AV 27190", "AV 27160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6566,"Gegenstände aus Anlage 4, § 13b Abs. 2 Nr. 11 UStG (ohne Vorsteuerabzug)",19,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 27001", "AV 27190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6567,"Gegenstände aus Anlage 4, § 13b Abs. 2 Nr. 11 UStG (ohne Vorsteuerabzug)",7,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 27002", "AV 27070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 6700,"steuerpflichtiger innergem. Erwerb § 1a UStG (ohne Vorsteuerabzug)","wählbar",new Collection<object>(){ 89, 93 },new Collection<string>(){ "AV 58070", "AV 58190", "AV 58160" },"innergem. Erwerb" ),
                            new TaxInfo( 6701,"steuerpflichtiger innergem. Erwerb § 1a UStG (ohne Vorsteuerabzug)",19,new Collection<object>(){ 89 },new Collection<string>(){ "AV 58001", "AV 58190" },"innergem. Erwerb" ),
                            new TaxInfo( 6702,"steuerpflichtiger innergem. Erwerb § 1a UStG (ohne Vorsteuerabzug)",7,new Collection<object>(){ 93 },new Collection<string>(){ "AV 58002", "AV 58070" },"innergem. Erwerb" ),
                            new TaxInfo( 6730,"innergem. Dreiecksgeschäft § 25b UStG, letzter Abnehmer (ohne Vorsteuerabzug)","wählbar",new Collection<object>(){ 69 },new Collection<string>(){ "AV 49070", "AV 49190", "AV 49160" },"innergem. Dreiecksgeschäft" ),
                            new TaxInfo( 6731,"innergem. Dreiecksgeschäft § 25b UStG, letzter Abnehmer (ohne Vorsteuerabzug)",19,new Collection<object>(){ 69 },new Collection<string>(){ "AV 49001", "AV 49190" },"innergem. Dreiecksgeschäft" ),
                            new TaxInfo( 6732,"innergem. Dreiecksgeschäft § 25b UStG, letzter Abnehmer (ohne Vorsteuerabzug)",7,new Collection<object>(){ 69 },new Collection<string>(){ "AV 49002", "AV 49070" },"innergem. Dreiecksgeschäft" ),
                            new TaxInfo( 9400,"Vorsteuer (aufzuteilende Vorsteuer)","wählbar",new Collection<object>(){ },new Collection<string>(){ "AV 30070", "AV 30190", "AV 30160" },"Vorsteuerbeträge aus Rechnungen von anderen Unternehmen" ),
                            new TaxInfo( 9401,"Vorsteuer (aufzuteilende Vorsteuer)",19,new Collection<object>(){ },new Collection<string>(){ "AV 30001", "AV 30190" },"Vorsteuerbeträge aus Rechnungen von anderen Unternehmen" ),
                            new TaxInfo( 9402,"Vorsteuer (aufzuteilende Vorsteuer)",7,new Collection<object>(){ },new Collection<string>(){ "AV 30002", "AV 30070" },"Vorsteuerbeträge aus Rechnungen von anderen Unternehmen" ),
                            new TaxInfo( 9408,"Vorsteuer (aufzuteilende Vorsteuer)",(decimal)10.7,new Collection<object>(){ },new Collection<string>(){ "AV 30003", "AV 30107" },"Vorsteuerbeträge aus Rechnungen von anderen Unternehmen" ),
                            new TaxInfo( 9409,"Vorsteuer (aufzuteilende Vorsteuer)",(decimal)5.5,new Collection<object>(){ },new Collection<string>(){ "AV 30009", "AV 30055" },"Vorsteuerbeträge aus Rechnungen von anderen Unternehmen" ),
                            new TaxInfo( 9480,"Steuerschuldner bei Auslagerung § 13a Abs. 1 Nr. 6 UStG (aufzuteilende Vorsteuer)","wählbar",new Collection<object>(){ 69 },new Collection<string>(){ "AV 35070", "AV 35190", "AV 35160" },"§ 13a UStG" ),
                            new TaxInfo( 9481,"Steuerschuldner bei Auslagerung § 13a Abs. 1 Nr. 6 UStG (aufzuteilende Vorsteuer)",19,new Collection<object>(){ 69 },new Collection<string>(){ "AV 35001", "AV 35190" },"§ 13a UStG" ),
                            new TaxInfo( 9482,"Steuerschuldner bei Auslagerung § 13a Abs. 1 Nr. 6 UStG (aufzuteilende Vorsteuer)",7,new Collection<object>(){ 69 },new Collection<string>(){ "AV 35002", "AV 35070" },"§ 13a UStG" ),
                            new TaxInfo( 9501,"erhaltene Leistung § 13b UStG, alle Steuertatbestände (aufzuteilende Vorsteuer)",19,new Collection<object>(){ },new Collection<string>(){ "wählbar" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9502,"erhaltene Leistung § 13b UStG, alle Steuertatbestände (aufzuteilende Vorsteuer)",7,new Collection<object>(){ },new Collection<string>(){ "wählbar" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9505,"sonstige EU-Leistung § 13b Abs. 1 UStG (aufzuteilende Vorsteuer)","wählbar",new Collection<object>(){ 46, 47 },new Collection<string>(){ "AV 24070", "AV 24190", "AV 24160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9506,"sonstige EU-Leistung § 13b Abs. 1 UStG (aufzuteilende Vorsteuer)",19,new Collection<object>(){ 46, 47 },new Collection<string>(){ "AV 24001", "AV 24190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9507,"sonstige EU-Leistung § 13b Abs. 1 UStG (aufzuteilende Vorsteuer)",7,new Collection<object>(){ 46, 47 },new Collection<string>(){ "AV 24002", "AV 24070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9510,"Leistungen ausländischer Unternehmer § 13b Abs. 2 Nr. 1 UStG (aufzuteilende Vorsteuer)","wählbar",new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 36070", "AV 36190", "AV 36160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9511,"Leistungen ausländischer Unternehmer § 13b Abs. 2 Nr. 1 UStG (aufzuteilende Vorsteuer)",19,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 36001", "AV 36190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9512,"Leistungen ausländischer Unternehmer § 13b Abs. 2 Nr. 1 UStG (aufzuteilende Vorsteuer)",7,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 36002", "AV 36070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9515,"sicherungsübereignete Gegenstände § 13b Abs. 2 Nr. 2 UStG (aufzuteilende Vorsteuer)","wählbar",new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 42070", "AV 42190", "AV 42160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9516,"sicherungsübereignete Gegenstände § 13b Abs. 2 Nr. 2 UStG (aufzuteilende Vorsteuer)",19,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 42001", "AV 42190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9517,"sicherungsübereignete Gegenstände § 13b Abs. 2 Nr. 2 UStG (aufzuteilende Vorsteuer)",7,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 42002", "AV 42070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9520,"Umsätze Grunderwerbsteuergesetz § 13b Abs. 2 Nr. 3 UStG (aufzuteilende Vorsteuer)","wählbar",new Collection<object>(){ 73, 74 },new Collection<string>(){ "AV 44070", "AV 44190", "AV 44160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9521,"Umsätze Grunderwerbsteuergesetz § 13b Abs. 2 Nr. 3 UStG (aufzuteilende Vorsteuer)",19,new Collection<object>(){ 73, 74 },new Collection<string>(){ "AV 44001", "AV 44190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9522,"Umsätze Grunderwerbsteuergesetz § 13b Abs. 2 Nr. 3 UStG (aufzuteilende Vorsteuer)",7,new Collection<object>(){ 73, 74 },new Collection<string>(){ "AV 44002", "AV 44070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9525,"Bauleistungen § 13b Abs. 2 Nr. 4 UStG (aufzuteilende Vorsteuer)","wählbar",new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 46070", "AV 46190", "AV 46160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9526,"Bauleistungen § 13b Abs. 2 Nr. 4 UStG (aufzuteilende Vorsteuer)",19,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 46001", "AV 46190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9527,"Bauleistungen § 13b Abs. 2 Nr. 4 UStG (aufzuteilende Vorsteuer)",7,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 46002", "AV 46070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9530,"Gas/Elektrizität ausländischer Unternehmer § 13b Abs. 2 Nr. 5 UStG (aufzuteilende Vorsteuer)","wählbar",new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 38070", "AV 38190", "AV 38160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9531,"Gas/Elektrizität ausländischer Unternehmer § 13b Abs. 2 Nr. 5 UStG (aufzuteilende Vorsteuer)",19,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 38001", "AV 38190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9532,"Gas/Elektrizität ausländischer Unternehmer § 13b Abs. 2 Nr. 5 UStG (aufzuteilende Vorsteuer)",7,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 38002", "AV 38070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9535,"Gas/Elektrizität inländischer Unternehmer § 13b Abs. 2 Nr. 5 b) UStG (aufzuteilende Vorsteuer)","wählbar",new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 26070", "AV 26190", "AV 26160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9536,"Gas/Elektrizität inländischer Unternehmer § 13b Abs. 2 Nr. 5 b) UStG (aufzuteilende Vorsteuer)",19,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 26001", "AV 26190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9537,"Gas/Elektrizität inländischer Unternehmer § 13b Abs. 2 Nr. 5 b) UStG (aufzuteilende Vorsteuer)",7,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 26002", "AV 26070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9540,"Treibhausgasemissionszertifikate § 13b Abs. 2 Nr. 6 UStG (aufzuteilende Vorsteuer)","wählbar",new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 26070", "AV 26190", "AV 26160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9541,"Treibhausgasemissionszertifikate § 13b Abs. 2 Nr. 6 UStG (aufzuteilende Vorsteuer)",19,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 26001", "AV 26190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9542,"Treibhausgasemissionszertifikate § 13b Abs. 2 Nr. 6 UStG (aufzuteilende Vorsteuer)",7,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 26002", "AV 26070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9545,"Anlage 3: Altmetall/Schrott/Plastik oder Ähnliches § 13b Abs. 2 Nr. 7 UStG (aufzuteilende Vorsteuer)","wählbar",new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 26070", "AV 26190", "AV 26160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9546,"Anlage 3: Altmetall/Schrott/Plastik oder Ähnliches § 13b Abs. 2 Nr. 7 UStG (aufzuteilende Vorsteuer)",19,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 26001", "AV 26190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9547,"Anlage 3: Altmetall/Schrott/Plastik oder Ähnliches § 13b Abs. 2 Nr. 7 UStG (aufzuteilende Vorsteuer)",7,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 26002", "AV 26070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9550,"Gebäudereinigung § 13b Abs. 2 Nr. 8 UStG (aufzuteilende Vorsteuer)","wählbar",new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 26070", "AV 26190", "AV 26160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9551,"Gebäudereinigung § 13b Abs. 2 Nr. 8 UStG (aufzuteilende Vorsteuer)",19,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 26001", "AV 26190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9552,"Gebäudereinigung § 13b Abs. 2 Nr. 8 UStG (aufzuteilende Vorsteuer)",7,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 26002", "AV 26070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9555,"Lieferung von Gold § 13b Abs. 2 Nr. 9 UStG (aufzuteilende Vorsteuer)","wählbar",new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 26070", "AV 26190", "AV 26160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9556,"Lieferung von Gold § 13b Abs. 2 Nr. 9 UStG (aufzuteilende Vorsteuer)",19,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 26001", "AV 26190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9557,"Lieferung von Gold § 13b Abs. 2 Nr. 9 UStG (aufzuteilende Vorsteuer)",7,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 26002", "AV 26070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9560,"Mobilfunkgeräte usw. § 13b Abs. 2 Nr. 10 UStG (aufzuteilende Vorsteuer)","wählbar",new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 26070", "AV 26190", "AV 26160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9561,"Mobilfunkgeräte usw. § 13b Abs. 2 Nr. 10 UStG (aufzuteilende Vorsteuer)",19,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 26001", "AV 26190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9562,"Mobilfunkgeräte usw. § 13b Abs. 2 Nr. 10 UStG (aufzuteilende Vorsteuer)",7,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 26002", "AV 26070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9565,"Gegenstände aus Anlage 4, § 13b Abs. 2 Nr. 11 UStG (aufzuteilende Vorsteuer)","wählbar",new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 26070", "AV 26190", "AV 26160" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9566,"Gegenstände aus Anlage 4, § 13b Abs. 2 Nr. 11 UStG (aufzuteilende Vorsteuer)",19,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 26001", "AV 26190" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9567,"Gegenstände aus Anlage 4, § 13b Abs. 2 Nr. 11 UStG (aufzuteilende Vorsteuer)",7,new Collection<object>(){ 84, 85 },new Collection<string>(){ "AV 26002", "AV 26070" },"§ 13b UStG Leistungsempfänger" ),
                            new TaxInfo( 9700,"steuerpflichtiger innergem. Erwerb § 1a UStG (aufzuteilende Vorsteuer)","wählbar",new Collection<object>(){ 89, 93 },new Collection<string>(){ "AV 54070", "AV 54190", "AV 54160" },"innergem. Erwerb" ),
                            new TaxInfo( 9701,"steuerpflichtiger innergem. Erwerb § 1a UStG (aufzuteilende Vorsteuer)",19,new Collection<object>(){ 89 },new Collection<string>(){ "AV 54001", "AV 54190" },"innergem. Erwerb" ),
                            new TaxInfo( 9702,"steuerpflichtiger innergem. Erwerb § 1a UStG (aufzuteilende Vorsteuer)",7,new Collection<object>(){ 93 },new Collection<string>(){ "AV 54002", "AV 54070" },"innergem. Erwerb" ),
                            new TaxInfo( 9720,"steuerpflichtiger innergem. Erwerb § 1b Abs. 2 und 3 UStG (aufzuteilende Vorsteuer)","wählbar",new Collection<object>(){ 94, 96 },new Collection<string>(){ "AV 53070", "AV 53190", "AV 53160" },"innergem. Erwerb" ),
                            new TaxInfo( 9721,"steuerpflichtiger innergem. Erwerb § 1b Abs. 2 und 3 UStG (aufzuteilende Vorsteuer)",19,new Collection<object>(){ 94, 96 },new Collection<string>(){ "AV 53001", "AV 53190" },"innergem. Erwerb" ),
                            new TaxInfo( 9730,"innergem. Dreiecksgeschäft § 25b UStG, letzter Abnehmer (aufzuteilende Vorsteuer)","wählbar",new Collection<object>(){ 69 },new Collection<string>(){ "AV 47070", "AV 47190" },"innergem. Dreiecksgeschäft" ),
                            new TaxInfo( 9731,"innergem. Dreiecksgeschäft § 25b UStG, letzter Abnehmer (aufzuteilende Vorsteuer)",19,new Collection<object>(){ 69 },new Collection<string>(){ "AV 47001", "AV 47190" },"innergem. Dreiecksgeschäft" ),
                            new TaxInfo( 9732,"innergem. Dreiecksgeschäft § 25b UStG, letzter Abnehmer (aufzuteilende Vorsteuer)",7,new Collection<object>(){ 69 },new Collection<string>(){ "AV 47002", "AV 47070" },"innergem. Dreiecksgeschäft" ),
                            new TaxInfo( 9800,"Vorsteuer LuF, Pauschalierung mit Regelbesteuerung (aufzuteilende Vorsteuer)","wählbar",new Collection<object>(){ },new Collection<string>(){ "AV 56070", "AV 56190", "AV 56160", "AV 56107" },"land- und forstwirtschaftliche Betriebe" ),
                            new TaxInfo( 9801,"Vorsteuer LuF, Pauschalierung mit Regelbesteuerung (aufzuteilende Vorsteuer)",19,new Collection<object>(){ },new Collection<string>(){ "AV 56001", "AV 56190" },"land- und forstwirtschaftliche Betriebe" ),
                            new TaxInfo( 9802,"Vorsteuer LuF, Pauschalierung mit Regelbesteuerung (aufzuteilende Vorsteuer)",7,new Collection<object>(){ },new Collection<string>(){ "AV 56002", "AV 56070" },"land- und forstwirtschaftliche Betriebe" ),
                            new TaxInfo( 9808,"Vorsteuer LuF, Pauschalierung mit Regelbesteuerung (aufzuteilende Vorsteuer)",(decimal)10.7,new Collection<object>(){ },new Collection<string>(){ "AV 56003", "AV 56107" },"land- und forstwirtschaftliche Betriebe" ),
                            new TaxInfo( 9881,"Vorsteuer auf Altteile (aufzuteilende Vorsteuer)",(decimal)20.9,new Collection<object>(){ },new Collection<string>(){ "AV 30990" },"Sonstige" )
                        };
                        taxInfos = taxInfos2021;
                        break;
                    default:
                        break;
                }

                return taxInfos;
            }
        }
    }
}
