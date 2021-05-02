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

            switch (dataCategoryID)
            {
                case 21:
                    Dictionary<int, ColumnInfo> columnInfo = DatevFields.GenerateColumnInfos(dataCategoryID);
                    bookingID = 0;
                    foreach (Buchung buchung in buchungen)
                    {
                        bookingID++;
                        for (int j = 1; j <= columnInfo.Count; j++)
                        {
                            if (j > 1)
                            {
                                ret.Append(Constants.FieldSeparator);
                            }
                            switch (columnInfo[j].TypeText)
                            {
                                case "Betrag":
                                    ret.Append(ValidateBetrag(dataCategoryID, j, buchung, columnInfo[j], propertyGridData, bookingID));
                                    break;
                                case "Datum":
                                    ret.Append(ValidateDatum(dataCategoryID, j, buchung, columnInfo[j], propertyGridData, bookingID));
                                    break;
                                case "Konto":
                                    ret.Append(ValidateKonto(dataCategoryID, j, buchung, columnInfo[j], propertyGridData, bookingID));
                                    break;
                                case "Text":
                                    ret.Append(ValidateText(dataCategoryID, j, buchung, columnInfo[j], propertyGridData, bookingID));
                                    break;
                                case "Zahl":
                                    ret.Append(ValidateZahl(dataCategoryID, j, buchung, columnInfo[j], propertyGridData, bookingID));
                                    break;
                            }
                            if (j == columnInfo.Count)
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
                            d = buchung.Datum;
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

        private static string ValidateText(int dataCategoryID, int columnID, Buchung buchung, ColumnInfo columnInfo, DatevPropertyItems propertyGridData, int bookingID)
        {
            StringBuilder ret = new StringBuilder();
            decimal? d = null;
            string str = string.Empty;
            bool mayBeShortened = propertyGridData.ShortenTextValuesWithoutException;
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
                if (ret.ToString().Length > columnInfo.MaxLength)
                {
                    if (mayBeShortened)
                    {
                        // take the first part of the text
                        str = ret.ToString();
                        ret.Clear();
                        ret.Append(str.Substring(0, columnInfo.MaxLength));
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
