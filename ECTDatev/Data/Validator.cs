using ECTDatev.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static ECTDatev.Data.DatevFields;

namespace ECTDatev.Data
{
    /// <summary>
    /// Validates the data of the columns and creates
    /// </summary>
    public static class Validator
    {
        /// <summary>
        /// Used for cropped overlong text.
        /// </summary>
        private static Collection<TextSafeInfo> m_TextSafe = new Collection<TextSafeInfo>();

        /// <summary>
        /// Used for cropped overlong text in <see cref="ValidateText(int, int, Buchung, ColumnInfo, DatevPropertyItems, int)"/>.
        /// </summary>
        internal static Collection<TextSafeInfo> TextSafe { get => m_TextSafe; set => m_TextSafe = value; }

        /// <summary>
        /// Just to save the <see cref="ColumnInfo"/> objects.
        /// </summary>
        internal static Dictionary<int, ColumnInfo> ColumnInfos { get; set; }

        /// <summary>
        /// Manages the export of the data.
        /// </summary>
        /// <param name="dataCategoryID"></param>
        /// <param name="buchungen"></param>
        /// <param name="propertyGridData"></param>
        /// <returns></returns>
        public static string Manager(int dataCategoryID, Collection<Buchung> buchungen, DatevPropertyItems propertyGridData)
        {
            if (buchungen == null)
                throw new ArgumentNullException("buchungen");

            StringBuilder ret = new StringBuilder();
            int bookingID = 0;
            TextSafe.Clear();

            switch (dataCategoryID)
            {
                case 21:
                    ColumnInfos = DatevFields.GenerateColumnInfos(dataCategoryID);
                    bookingID = 0;
                    if (TextSafe.Count > 0)
                    {
                        TextSafe.Clear(); // This may never happen! (only if there wasn´t enough space to save the content of the previous line)
                    }
                    foreach (Buchung buchung in buchungen)
                    {
                        bookingID++;
                        for (int j = 1; j <= ColumnInfos.Count; j++)
                        {
                            if (j > 1)
                            {
                                ret.Append(Constants.FieldSeparator);
                            }
                            switch (ColumnInfos[j].TypeText)
                            {
                                case "Betrag":
                                    ret.Append(ValidateBetrag(dataCategoryID, j, buchung, ColumnInfos[j], propertyGridData, bookingID));
                                    break;
                                case "Datum":
                                    ret.Append(ValidateDatum(dataCategoryID, j, buchung, ColumnInfos[j], propertyGridData, bookingID));
                                    break;
                                case "Konto":
                                    ret.Append(ValidateKonto(dataCategoryID, j, buchung, ColumnInfos[j], propertyGridData, bookingID));
                                    break;
                                case "Text":
                                    ret.Append(ValidateText(dataCategoryID, j, buchung, ColumnInfos[j], propertyGridData, bookingID));
                                    break;
                                case "Zahl":
                                    ret.Append(ValidateZahl(dataCategoryID, j, buchung, ColumnInfos[j], propertyGridData, bookingID));
                                    break;
                            }
                            if (j == ColumnInfos.Count)
                            {
                                ret.Append(Constants.LineEndTerminator);
                            }
                        }
                    }
                    break;
            }

            return ret.ToString();
        }

        private static string ValidateBetrag(int dataCategoryID, int columnID, Buchung buchung, ColumnInfo columnInfo, DatevPropertyItems propertyGridData, int bookingID)
        {
            StringBuilder ret = new StringBuilder();
            decimal? d = null;
            string format = "0";
            switch (dataCategoryID)
            {
                case 21:
                    switch (columnID)
                    {
                        case 1:
                            d = buchung.Betrag;
                            break;
                            //case 5:
                            //    break;
                            //case 13:
                            //    break;
                    }
                    break;
                case 65:
                    switch (columnID)
                    {
                        case 3:
                            d = buchung.Betrag;
                            break;
                        //case 6:
                        //    break;
                        //case 19:
                        //    break;
                    }
                    break;
            }
            if (columnInfo.HasMacro)
            {
                for (int mc = 1; mc <= columnInfo.GetNumberOfMacros(); mc++)
                {
                    string oi = columnInfo.GetOptionalInfo(mc);
                    if (oi.StartsWith(Constants.MacroKeyword_Abs))
                    {
                        if (d.HasValue)
                        {
                            d = Math.Abs(d.Value);
                        }
                    }
                    else if (oi.StartsWith(Constants.MacroKeyword_NotAllowedChars))
                    {
                        if (d.HasValue)
                        {
                            if (!string.IsNullOrEmpty(d.Value.ToString(format)))
                            {
                                string[] strArr = columnInfo.GetOptionalInfo(Constants.MacroKeyword_AllowedChars, mc);
                                string pattern = string.Empty;
                                for (int i = 1; i < strArr.Length; i++)
                                {
                                    switch (strArr[i])
                                    {
                                        case "Numbers":
                                            pattern += "0-9";
                                            break;
                                        case "Letters":
                                            pattern += "a-zA-Z";
                                            break;
                                        default:
                                            pattern += Regex.Escape(strArr[i]);
                                            break;
                                    }
                                }
                                // Ok, now we know what is no allowed. Do we have any match?
                                if (Regex.IsMatch(d.Value.ToString(format), "[" + pattern + "]"))
                                {
                                    throw new InvalidOperationException(string.Format("{0}. booking, {1}. column: Not allowed char was found: {2} (expected: {3})", bookingID, columnID, d.Value.ToString(format), pattern));
                                }
                                else
                                {
                                    if (!ret.ToString().EndsWith(d.Value.ToString(format)))
                                    {
                                        ret.Append(d.Value.ToString(format));
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // any new macro?
                        if (columnInfo.IsMandatory)
                        {
                            throw new NotImplementedException(string.Format("{0}. booking, {1}. column: Interpreter for macro not implemented: {2}", bookingID, columnID, oi));
                        }
                    }
                }
            }
            if (columnInfo.DecimalPlaces > 0)
            {
                format += ".";
                format = format.PadRight(format.Length + columnInfo.DecimalPlaces, '0');
            }
            if (/*columnInfo.IsMandatory && */ d.HasValue)
            {
                if (!ret.ToString().EndsWith(d.Value.ToString(format)))
                {
                    ret.Append(d.Value.ToString(format));
                }
            }
            if (columnInfo.MaxLength > 0)
            {
                if (ret.ToString().Length > columnInfo.MaxLength)
                {
                    throw new InvalidOperationException(string.Format("{0}. booking, {1}. column: The value is too long (expected max. {2} char): {3}", bookingID, columnID, columnInfo.MaxLength, ret.ToString()));
                }
            }

            return ret.ToString();
        }

        private static string ValidateDatum(int dataCategoryID, int columnID, Buchung buchung, ColumnInfo columnInfo, DatevPropertyItems propertyGridData, int bookingID)
        {
            StringBuilder ret = new StringBuilder();
            DateTime d = DateTime.MinValue;
            switch (dataCategoryID)
            {
                case 21:
                    switch (columnID)
                    {
                        case 10:
                            d = buchung.Datum.Date;
                            break;
                    }
                    break;
                case 65:
                    switch (columnID)
                    {
                        case 12:
                            break;
                        case 77:
                            break;
                        case 78:
                            break;
                        case 79:
                            break;
                        case 80:
                            break;
                    }
                    break;
            }
            if (d > DateTime.MinValue)
            {
                if (columnInfo.HasMacro)
                {
                    for (int mc = 1; mc <= columnInfo.GetNumberOfMacros(); mc++)
                    {
                        string oi = columnInfo.GetOptionalInfo(mc);
                        if (oi.StartsWith(Constants.MacroKeyword_DateFormat))
                        {
                            string[] strArr = columnInfo.GetOptionalInfo(Constants.MacroKeyword_DateFormat);
                            string format = strArr[strArr.GetUpperBound(0)];
                            if (format.Length > 0)
                            {
                                if (!ret.ToString().EndsWith(d.ToString(format)))
                                {
                                    ret.Append(d.ToString(format));
                                }
                            }
                        }
                    }
                }
                if (columnInfo.MaxLength > 0)
                {
                    if (ret.ToString().Length > columnInfo.MaxLength)
                    {
                        throw new InvalidOperationException(string.Format("{0}. booking, {1}. column: The value is too long (expected max. {2} char): {3}", bookingID, columnID, columnInfo.MaxLength, ret.ToString()));
                    }
                }
            }

            return ret.ToString();
        }

        private static string ValidateKonto(int dataCategoryID, int columnID, Buchung buchung, ColumnInfo columnInfo, DatevPropertyItems propertyGridData, int bookingID)
        {
            StringBuilder ret = new StringBuilder();
            string str = string.Empty;
            switch (dataCategoryID)
            {
                case 21:
                    switch (columnID)
                    {
                        case 7:
                            str = "k1234k"; // TODO Konto
                            break;
                        case 8:
                            str = "g1234g"; // TODO Gegenkonto
                            break;
                    }
                    break;
                case 65:
                    switch (columnID)
                    {
                        case 9:
                            // TODO Gegenkonto
                            break;
                        case 13:
                            // TODO Konto
                            break;
                    }
                    break;
                case 20:
                    switch (columnID)
                    {
                        case 1:
                            str = "k1234k"; // TODO Konto
                            break;
                    }
                    break;
            }
            if (columnInfo.HasMacro)
            {
                for (int mc = 1; mc <= columnInfo.GetNumberOfMacros(); mc++)
                {
                    string oi = columnInfo.GetOptionalInfo(mc);
                    // TODO validate konto & gegenkonto
                }
            }
            else
            {
                if (!ret.ToString().EndsWith(str))
                {
                    ret.Append(str);
                }
            }
            if (columnInfo.MaxLength > 0)
            {
                if (ret.ToString().Length > columnInfo.MaxLength)
                {
                    throw new InvalidOperationException(string.Format("{0}. booking, {1}. column: Field value is too long (expected max. {2} chars): {3}", bookingID, columnID, columnInfo.MaxLength, ret.ToString()));
                }
            }

            return ret.ToString();
        }

        internal class TextSafeInfo
        {
            /// <summary>
            /// The order number of the column.
            /// </summary>
            public int FieldID { get; set; }

            /// <summary>
            /// The name of the field.
            /// </summary>
            public string Fieldname { get; set; }

            /// <summary>
            /// The text saved in this object.
            /// </summary>
            public string Content { get; set; }

            /// <summary>
            /// If an overlong text has to be saved in more than one parts. This number tells, how many parts were already saved from this content.
            /// </summary>
            public int PartsCounter { get; set; }
            public TextSafeInfo(int fielID, string fieldname, string content, int partsCounter)
            {
                FieldID = FieldID;
                Fieldname = fieldname;
                Content = content;
                PartsCounter = partsCounter;
            }
        }

        private static string ValidateText(int dataCategoryID, int columnID, Buchung buchung, ColumnInfo columnInfo, DatevPropertyItems propertyGridData, int bookingID)
        {
            StringBuilder ret = new StringBuilder();
            decimal? d = null;
            string str = string.Empty;
            bool mayBeShortened = propertyGridData.ShortenOverlongTextValues;
            
            switch (dataCategoryID)
            {
                case 21:
                    switch (columnID)
                    {
                        case 2:
                            d = buchung.Betrag;
                            break;
                        case 9:
                            // TODO "BU-Schlüssel"
                            break;
                        case 11:
                            str = buchung.Belegnummer;
                            break;
                        case 14:
                            str = buchung.Beschreibung;
                            mayBeShortened = true;
                            break;
                        case 37:
                            // TODO "KOST1" nur für Ausgaben: behalten?
                            //if (buchung.IstAusgabe)
                            //{
                            str = buchung.Konto;
                            mayBeShortened = true;
                            //}
                            break;
                        case 38:
                            // TODO "KOST2" nur für Ausgaben: behalten?
                            //if (buchung.IstAusgabe)
                            //{
                            str = buchung.Belegnummer;
                            mayBeShortened = true;
                            //}
                            break;
                        case 48:
                        case 50:
                        case 52:
                        case 54:
                        case 56:
                        case 58:
                        case 60:
                        case 62:
                        case 64:
                        case 66:
                        case 68:
                        case 70:
                        case 72:
                        case 74:
                        case 76:
                        case 78:
                        case 80:
                        case 82:
                        case 84:
                        case 86:
                            // is it allowed to use textsafe for the cut off parts?
                            if (propertyGridData.SaveOverlongTextValues)
                            {
                                // do we have items in the safe?
                                if (TextSafe.Count > 0)
                                {
                                    // the increased parts number will be used at the end of the fieldname
                                    string partsSuffix = (++TextSafe.First<TextSafeInfo>().PartsCounter).ToString(Constants.FormatForShortenedPartsCounter);
                                    // is the name of colum in the safe overlong?
                                    if (TextSafe.First<TextSafeInfo>().Fieldname.Length + partsSuffix.Length > columnInfo.MaxLength)
                                    {
                                        StringBuilder sb = new StringBuilder();
                                        // take the allowed length -5 (or 6) (3 for "..." and 2 (or 3) for " <number of fields used for this content, can be 1 or 2 digits>")
                                        sb.Append(TextSafe.First<TextSafeInfo>().Fieldname.Substring(0, columnInfo.MaxLength - Constants.TextShortenedMarkerPostfix.Length - partsSuffix.Length));
                                        // append the shortened marker and the part counter
                                        sb.Append(Constants.TextShortenedMarkerPostfix);
                                        // append the part counter
                                        sb.Append(partsSuffix);
                                        str = sb.ToString();
                                    }
                                    else
                                    {
                                        StringBuilder sb = new StringBuilder();
                                        // take the allowed length -2 (or 3) (2 (or 3) for " <number of fields used for this content, can be 1 or 2 digits>")
                                        sb.Append(TextSafe.First<TextSafeInfo>().Fieldname);
                                        // append the part counter
                                        sb.Append(partsSuffix);
                                        str = sb.ToString();
                                    }
                                }
                            }
                            break;
                        case 49:
                        case 51:
                        case 53:
                        case 55:
                        case 57:
                        case 59:
                        case 61:
                        case 63:
                        case 65:
                        case 67:
                        case 69:
                        case 71:
                        case 73:
                        case 75:
                        case 77:
                        case 79:
                        case 81:
                        case 83:
                        case 85:
                        case 87:
                            // is it allowed to use textsafe for the cut off parts?
                            if (propertyGridData.SaveOverlongTextValues)
                            {
                                // is there any text saved in a textsafe?
                                if (TextSafe.Count > 0)
                                {
                                    if (Constants.CutOffTextMarkerPrefix.Length + TextSafe.First<TextSafeInfo>().Content.Length + Constants.CutOffTextMarkerPrefix.Length > columnInfo.MaxLength)
                                    {
                                        // we cannot save the whole content in this columnInfo => the remaining content shall be kept for the next possible block - if any (the field PartsCounter was already increased)
                                        str = Constants.CutOffTextMarkerPrefix + TextSafe.First<TextSafeInfo>().Content.Substring(0, columnInfo.MaxLength - Constants.CutOffTextMarkerPrefix.Length - Constants.TextShortenedMarkerPostfix.Length) + Constants.TextShortenedMarkerPostfix;
                                        TextSafe.First<TextSafeInfo>().Content = TextSafe.First<TextSafeInfo>().Content.Remove(0, columnInfo.MaxLength - Constants.CutOffTextMarkerPrefix.Length - Constants.TextShortenedMarkerPostfix.Length);
                                    }
                                    else
                                    {
                                        // we can save the whole content in this field => this TextSafeInfo shall be removed
                                        str = Constants.CutOffTextMarkerPrefix + TextSafe.First<TextSafeInfo>().Content;
                                        TextSafe.Remove(TextSafe.First<TextSafeInfo>());
                                    }
                                }
                            }
                            break;
                        case 102:
                            str = propertyGridData.Origin;
                            break;
                    }
                    break;
                case 65:
                    switch (columnID)
                    {
                        case 4:
                            d = buchung.Betrag;
                            break;
                    }
                    break;
            }
            if (columnInfo.HasMacro)
            {
                for (int mc = 1; mc <= columnInfo.GetNumberOfMacros(); mc++)
                {
                    string oi = columnInfo.GetOptionalInfo(mc);
                    if (oi.StartsWith(Constants.MacroKeyword_SetDebitOrCredit))
                    {
                        if (d.HasValue)
                        {
                            string[] strArr = columnInfo.GetOptionalInfo(Constants.MacroKeyword_SetDebitOrCredit);
                            if (strArr.Length != 3)
                            {
                                throw new InvalidOperationException(string.Format("{0}. booking, {1}. column: Unexpected macro parameters (3 fields were expected): {2}", bookingID, columnID, oi));
                            }
                            if (Math.Sign(d.Value) >= 0)
                            {
                                if (!ret.ToString().EndsWith(strArr[2]))
                                {
                                    ret.Append(strArr[2]);
                                }
                            }
                            else if (Math.Sign(d.Value) < 0)
                            {
                                if (!ret.ToString().EndsWith(strArr[1]))
                                {
                                    ret.Append(strArr[1]);
                                }
                            }
                        }
                    }
                    else if (oi.StartsWith(Constants.MacroKeyword_OneOf))
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            string[] strArr = columnInfo.GetOptionalInfo(Constants.MacroKeyword_AllowedChars, mc);
                            bool found = false;
                            for (int i = 1; i < strArr.Length; i++)
                            {
                                if (string.Equals(str, strArr[i]))
                                {
                                    found = true;
                                    break;
                                }
                            }
                            if (found)
                            {
                                if (!ret.ToString().EndsWith(str))
                                {
                                    ret.Append(str);
                                }
                            }
                            else
                            {
                                if (columnInfo.IsMandatory)
                                {
                                    throw new InvalidOperationException(string.Format("{0}. booking, {1}. column: A mandatory field was not found: {2} (macro: {3})", bookingID, columnID, str, oi));
                                }
                            }
                        }
                    }
                    else if (oi.StartsWith(Constants.MacroKeyword_AllowedChars))
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            string[] strArr = columnInfo.GetOptionalInfo(Constants.MacroKeyword_AllowedChars, mc);
                            string pattern = string.Empty;
                            for (int i = 1; i < strArr.Length; i++)
                            {
                                switch (strArr[i])
                                {
                                    case "Numbers":
                                        pattern += "0-9";
                                        break;
                                    case "Letters":
                                        pattern += "a-zA-Z";
                                        break;
                                    default:
                                        pattern += Regex.Escape(strArr[i]);
                                        break;
                                }
                            }
                            // Ok, now we know what is allowed. Do we have anything not allowed? => negate it
                            if (Regex.IsMatch(str, "[^" + pattern + "]"))
                            {
                                throw new InvalidOperationException(string.Format("{0}. booking, {1}. column: Char outside of the allowed range was found: {2} (expected: {3})", bookingID, columnID, str, pattern));
                            }
                            else
                            {
                                if (!ret.ToString().EndsWith(str))
                                {
                                    ret.Append(str);
                                }
                            }
                        }
                    }
                    else if (oi.StartsWith(Constants.MacroKeyword_NotAllowedChars))
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            string[] strArr = columnInfo.GetOptionalInfo(Constants.MacroKeyword_NotAllowedChars, mc);
                            string pattern = string.Empty;
                            for (int i = 1; i < strArr.Length; i++)
                            {
                                switch (strArr[i])
                                {
                                    case "Numbers":
                                        pattern += "0-9";
                                        break;
                                    case "Letters":
                                        pattern += "a-zA-Z";
                                        break;
                                    default:
                                        if (strArr[i].Length == 0)
                                        {
                                            // is the field separator char part of the field (is it doubbled)?
                                            if (oi.Contains(Constants.FieldSeparator + Constants.FieldSeparator))
                                            {
                                                // fill up this empty field
                                                strArr[i] = Constants.FieldSeparator;
                                            }
                                        }
                                        pattern += Regex.Escape(strArr[i]);
                                        break;
                                }
                            }
                            // Ok, now we know what is no allowed. Do we have any match?
                            if (Regex.IsMatch(str, "[" + pattern + "]"))
                            {
                                throw new InvalidOperationException(string.Format("{0}. booking, {1}. column: Not allowed char was found: {2} (expected: {3})", bookingID, columnID, str, pattern));
                            }
                            else
                            {
                                if (!ret.ToString().EndsWith(str))
                                {
                                    ret.Append(str);
                                }
                            }
                        }
                    }
                    else
                    {
                        // any new macro?
                        if (columnInfo.IsMandatory)
                        {
                            throw new NotImplementedException(string.Format("{0}. booking, {1}. column: Interpreter for macro not implemented: {2}", bookingID, columnID, oi));
                        }
                    }
                }
            }
            else
            {
                if (str.Length > 0)
                {
                    if (!ret.ToString().EndsWith(str))
                    {
                        ret.Append(str);
                    }
                }
            }
            if (columnInfo.MaxLength > 0)
            {
                // is the text too long?
                if (ret.ToString().Length > columnInfo.MaxLength)
                {
                    if (mayBeShortened)
                    {
                        // take the first part of the text for the normal export:
                        str = ret.ToString();
                        ret.Clear();
                        // take out that many text, which fits in this field (less the marker for shortened text´s length)
                        ret.Append(str.Substring(0, columnInfo.MaxLength - Constants.TextShortenedMarkerPostfix.Length));
                        // append the marker
                        ret.Append(Constants.TextShortenedMarkerPostfix);
                        // is the option for savin the cut off part switched on?
                        if (propertyGridData.SaveOverlongTextValues)
                        {
                            // put in the safe the cut off part
                            TextSafe.Add(new TextSafeInfo(columnID, columnInfo.Name.Split()[0], str.Remove(0, columnInfo.MaxLength - Constants.TextShortenedMarkerPostfix.Length), 0));
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException(string.Format("{0}. booking, {1}. column: Field value is too long (expected max. {2} chars): {3}", bookingID, columnID, columnInfo.MaxLength, ret.ToString()));
                    }
                }
            }

            return Tools.WrapData(ret.ToString());
        }

        private static string ValidateZahl(int dataCategoryID, int columnID, Buchung buchung, ColumnInfo columnInfo, DatevPropertyItems propertyGridData, int bookingID)
        {
            StringBuilder ret = new StringBuilder();
            decimal? d = null;
            string format = "0";
            switch (dataCategoryID)
            {
                case 21:
                    switch (columnID)
                    {
                        case 114:
                            d = 0; // TODO can we keep it as 0? 0 = keine Festschreibung, 1 = Festschreibung
                            break;
                        default:
                            break;
                    }
                    break;
                case 65:
                    switch (columnID)
                    {
                        default:
                            break;
                    }
                    break;
            }
            if (columnInfo.HasMacro)
            {
                for (int mc = 1; mc <= columnInfo.GetNumberOfMacros(); mc++)
                {
                    string oi = columnInfo.GetOptionalInfo(mc);
                    if (oi.StartsWith(Constants.MacroKeyword_Abs))
                    {
                        if (d.HasValue)
                        {
                            d = Math.Abs(d.Value);
                        }
                    }
                    else if (oi.StartsWith(Constants.MacroKeyword_NotAllowedChars))
                    {
                        if (d.HasValue)
                        {
                            if (!string.IsNullOrEmpty(d.Value.ToString(format)))
                            {
                                string[] strArr = columnInfo.GetOptionalInfo(Constants.MacroKeyword_NotAllowedChars, mc);
                                string pattern = string.Empty;
                                for (int i = 1; i < strArr.Length; i++)
                                {
                                    switch (strArr[i])
                                    {
                                        case "Numbers":
                                            pattern += "0-9";
                                            break;
                                        case "Letters":
                                            pattern += "a-zA-Z";
                                            break;
                                        default:
                                            pattern += Regex.Escape(strArr[i]);
                                            break;
                                    }
                                }
                                // Ok, now we know what is no allowed. Do we have any match?
                                if (Regex.IsMatch(d.Value.ToString(format), "[" + pattern + "]"))
                                {
                                    if (columnInfo.IsMandatory)
                                    {
                                        throw new InvalidOperationException(string.Format("{0}. booking, {1}. column: Not allowed char was found: {2} (expected: {3})", bookingID, columnID, d.Value.ToString(format), pattern));
                                    }
                                }
                                else
                                {
                                    if (!ret.ToString().EndsWith(d.Value.ToString(format)))
                                    {
                                        ret.Append(d.Value.ToString(format));
                                    }
                                }
                            }
                        }
                    }
                    else if (oi.StartsWith(Constants.MacroKeyword_OneOf))
                    {
                        if (d.HasValue)
                        {
                            string[] strArr = columnInfo.GetOptionalInfo(Constants.MacroKeyword_OneOf);
                            bool found = false;
                            for (int i = 1; i < strArr.Length; i++)
                            {
                                if (string.Equals(strArr[i], d.Value.ToString(format)))
                                {
                                    found = true;
                                    break;
                                }
                            }
                            if (!found)
                            {
                                if (columnInfo.IsMandatory)
                                {
                                    throw new InvalidOperationException(string.Format("{0}. booking, {1}. column: Mandatory data wasn´t found: {2}", bookingID, columnID, oi));
                                }
                            }
                        }
                    }
                    else
                    {
                        // any new macro?
                        if (columnInfo.IsMandatory)
                        {
                            throw new NotImplementedException(string.Format("{0}. booking, {1}. column: Interpreter for macro not implemented: {2}", bookingID, columnID, oi));
                        }
                    }
                }
            }
            if (columnInfo.DecimalPlaces > 0)
            {
                format += ".";
                format = format.PadRight(format.Length + columnInfo.DecimalPlaces, '0');
            }
            if (columnInfo.IsMandatory || d.HasValue)
            {
                if (!ret.ToString().EndsWith(d.Value.ToString(format)))
                {
                    ret.Append(d.Value.ToString(format));
                }
            }
            if (ret.ToString().Length > columnInfo.MaxLength)
            {
                throw new InvalidOperationException(string.Format("{0}. booking, {1}. column: The value is too long (expected max. {2} char): {3}", bookingID, columnID, columnInfo.MaxLength, ret.ToString()));
            }

            return ret.ToString();
        }

        public static string Validate(short value)
        {
            string ret = value.ToString();

            return ret;
        }

        public static string Validate(int value)
        {
            string ret = value.ToString();

            return ret;
        }

        public static string Validate(double value, string format = "0.00")
        {
            string ret = value.ToString();

            return ret;
        }

        public static string Validate(decimal value, string format = "0.00")
        {
            string ret = value.ToString();

            return ret;
        }

        public static string Validate(string value)
        {
            if (value == null)
            {
                value = string.Empty;
            }
            string ret = Tools.WrapData(value);

            return ret;
        }

        public static string Validate(DateTime value, string format = "ddMM")
        {
            string ret = value.ToString();

            return ret;
        }
    }
}
