using ECTDatev.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
            decimal d = 0;
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
                        case 6:
                            break;
                        case 19:
                            break;
                    }
                    break;
            }
            string oi = columnInfo.OptionalInfo;
            if (oi != null)
            {
                if (oi.StartsWith(Constants.MacroStart) && oi.EndsWith(Constants.MacroEnd))
                {
                    oi = oi.Substring(Constants.MacroStart.Length, oi.Length - Constants.MacroStart.Length - Constants.MacroEnd.Length);
                    if (oi.StartsWith(Constants.MacroKeyword_Abs))
                    {
                        d = Math.Abs(d);
                    }
                    else if (oi.StartsWith(Constants.MacroKeyword_NotAllowed))
                    {
                        if (oi.StartsWith(Constants.MacroKeyword_NotAllowed + Constants.FieldSeparator + 0))
                        {
                            if (d == 0)
                            {
                                if (columnInfo.IsMandatory)
                                {
                                    throw new InvalidOperationException(string.Format("{0}. booking, {1}. column: Mandatory data is zero, which is not allowed to be zero: {2}", bookingID, columnID, oi));
                                }
                                else
                                {
                                    // number is 0, but 0 is not allowed => empty return
                                    return ret.ToString();
                                }
                            }
                        }
                        else
                        {
                            if (columnInfo.IsMandatory)
                            {
                                throw new NotImplementedException(string.Format("{0}. booking, {1}. column: Interpreter for macro not implemented: {2}", bookingID, columnID, oi));
                            }
                        }
                    }
                    else
                    {
                        if (columnInfo.IsMandatory)
                        {
                            throw new NotImplementedException(string.Format("{0}. booking, {1}. column: Interpreter for macro not implemented: {2}", bookingID, columnID, oi));
                        }
                    }
                }
                else if (oi.Length > 0)
                {
                    throw new InvalidOperationException(string.Format("{0}. booking, {1}. column: Invalid optional data: {2}", bookingID, columnID, oi));
                }
            }
            string format = "0";
            if (columnInfo.DecimalPlaces > 0)
            {
                format += ".";
                format = format.PadRight(format.Length + columnInfo.DecimalPlaces, '0');
            }
            if (columnInfo.IsMandatory || d != 0)
            {
                ret.Append(d.ToString(format));
            }
            if (ret.ToString().Length > columnInfo.MaxLength)
            {
                throw new InvalidOperationException(string.Format("{0}. booking, {1}. column: The value is too long (expected max. {2} char): {3}", bookingID, columnID, columnInfo.MaxLength, ret.ToString()));
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
                string oi = columnInfo.OptionalInfo;
                if (!string.IsNullOrEmpty(oi))
                {
                    if (oi.StartsWith(Constants.MacroStart) && oi.EndsWith(Constants.MacroEnd))
                    {
                        oi = columnInfo.GetOptionalInfo();
                        if (oi.StartsWith(Constants.MacroKeyword_DateFormat))
                        {
                            string[] strArr = columnInfo.GetOptionalInfo(Constants.MacroKeyword_DateFormat);
                            string format = strArr[strArr.GetUpperBound(0)];
                            if (format.Length > 0)
                            {
                                ret.Append(d.ToString(format));
                            }
                        }
                    }
                    else if (oi.Length > 0)
                    {
                        throw new InvalidOperationException(string.Format("{0}. booking, {1}. column: Invalid optional data: {2}", bookingID, columnID, oi));
                    }
                }
                if (ret.ToString().Length > columnInfo.MaxLength)
                {
                    throw new InvalidOperationException(string.Format("{0}. booking, {1}. column: The value is too long (expected max. {2} char): {3}", bookingID, columnID, columnInfo.MaxLength, ret.ToString()));
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
                            // TODO Kontonummer
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
            ret.Append(str);
            if (columnInfo.MaxLength > 0)
            {
                if (ret.ToString().Length > columnInfo.MaxLength)
                {
                    throw new InvalidOperationException(string.Format("{0}. booking, {1}. column: Field value is too long (expected max. {2} chars): {3}", bookingID, columnID, columnInfo.MaxLength, str));
                }
            }
            return ret.ToString();
        }

        private static string ValidateText(int dataCategoryID, int columnID, Buchung buchung, ColumnInfo columnInfo, DatevPropertyItems propertyGridData, int bookingID)
        {
            StringBuilder ret = new StringBuilder();
            decimal d = 0;
            string str = string.Empty;
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
            string oi = columnInfo.OptionalInfo;
            if (!string.IsNullOrEmpty(oi))
            {
                if (oi.StartsWith(Constants.MacroStart) && oi.EndsWith(Constants.MacroEnd))
                {
                    oi = columnInfo.GetOptionalInfo();
                    if (oi.StartsWith(Constants.MacroKeyword_SetDebitOrCredit))
                    {
                        string[] strArr = columnInfo.GetOptionalInfo(Constants.MacroKeyword_SetDebitOrCredit);
                        if (strArr.Length != 3)
                        {
                            throw new InvalidOperationException(string.Format("{0}. booking, {1}. column: Unexpected macro parameters (3 fields were expected): {2}", bookingID, columnID, oi));
                        }
                        if (Math.Sign(d) >= 0)
                        {
                            ret.Append(strArr[2]);
                        }
                        else if (Math.Sign(d) < 0)
                        {
                            ret.Append(strArr[1]);
                        }
                    }
                    else if (oi.StartsWith(Constants.MacroKeyword_OneOf))
                    {
                        if (columnInfo.IsMandatory)
                        {
                            throw new NotImplementedException(string.Format("{0}. booking, {1}. column: Interpreter for macro not implemented: {2}", bookingID, columnID, oi));
                        }
                        else
                        {
                            if (str.Equals(string.Empty))
                            {
                                // nothing to do
                            }
                            else
                            {
                                if (columnInfo.IsMandatory)
                                {
                                    throw new NotImplementedException(string.Format("{0}. booking, {1}. column: Interpreter for macro not implemented: {2}", bookingID, columnID, oi));
                                }
                            }
                        }
                    }
                    else if (oi.StartsWith(Constants.MacroKeyword_AllowedChars))
                    {
                        // TODO
                        ret.Append(str);
                    }
                    else
                    {
                        if (columnInfo.IsMandatory)
                        {
                            throw new NotImplementedException(string.Format("{0}. booking, {1}. column: Interpreter for macro not implemented: {2}", bookingID, columnID, oi));
                        }
                    }
                }
                else if (oi.Length > 0)
                {
                    throw new InvalidOperationException(string.Format("{0}. booking, {1}. column: Invalid optional data: {2}", bookingID, columnID, oi));
                }
            }
            else
            {
                if (str.Length > 0)
                {
                    ret.Append(str);
                }
            }
            if (columnInfo.MaxLength > 0)
            {
                if (ret.ToString().Length > columnInfo.MaxLength)
                {
                    throw new InvalidOperationException(string.Format("{0}. booking, {1}. column: Field value is too long (expected max. {2} chars): {3}", bookingID, columnID, columnInfo.MaxLength, oi));
                }
            }
            return Tools.WrapData(ret.ToString());
        }

        private static string ValidateZahl(int dataCategoryID, int columnID, Buchung buchung, ColumnInfo columnInfo, DatevPropertyItems propertyGridData, int bookingID)
        {
            StringBuilder ret = new StringBuilder();
            decimal d = 0;
            string format = "0";
            bool doIt = false;
            switch (dataCategoryID)
            {
                case 21:
                    switch (columnID)
                    {
                        case 114:
                            doIt = true;
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
            string oi = columnInfo.OptionalInfo;
            if (oi != null)
            {
                if (oi.StartsWith(Constants.MacroStart) && oi.EndsWith(Constants.MacroEnd))
                {
                    oi = columnInfo.GetOptionalInfo();
                    if (oi.StartsWith(Constants.MacroKeyword_Abs))
                    {
                        d = Math.Abs(d);
                    }
                    else if (oi.StartsWith(Constants.MacroKeyword_NotAllowed))
                    {
                        if (oi.StartsWith(Constants.MacroKeyword_NotAllowed + Constants.FieldSeparator + 0))
                        {
                            if (d == 0)
                            {
                                if (columnInfo.IsMandatory)
                                {
                                    throw new InvalidOperationException(string.Format("{0}. booking, {1}. column: Mandatory data is zero, which is not allowed to be zero: {2}", bookingID, columnID, oi));
                                }
                                else
                                {
                                    // number is 0, but 0 is not allowed => empty return
                                    return ret.ToString();
                                }
                            }
                        }
                        else
                        {
                            if (columnInfo.IsMandatory)
                            {
                                throw new NotImplementedException(string.Format("{0}. booking, {1}. column: Interpreter for macro not implemented: {2}", bookingID, columnID, oi));
                            }
                        }
                    }
                    else if (oi.StartsWith(Constants.MacroKeyword_OneOf))
                    {
                        string[] strArr = columnInfo.GetOptionalInfo(Constants.MacroKeyword_OneOf);
                        bool found = false;
                        for (int i = 1; i < strArr.Length; i++)
                        {
                            if (strArr[i] == d.ToString(format))
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
                    else
                    {
                        if (columnInfo.IsMandatory)
                        {
                            throw new NotImplementedException(string.Format("{0}. booking, {1}. column: Interpreter for macro not implemented: {2}", bookingID, columnID, oi));
                        }
                    }
                }
                else if (oi.Length > 0)
                {
                    throw new InvalidOperationException(string.Format("{0}. booking, {1}. column: Invalid optional data: {2}", bookingID, columnID, oi));
                }
            }
            if (columnInfo.DecimalPlaces > 0)
            {
                format += ".";
                format = format.PadRight(format.Length + columnInfo.DecimalPlaces, '0');
            }
            if (columnInfo.IsMandatory || doIt)
            {
                ret.Append(d.ToString(format));
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
