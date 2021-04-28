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
        /// <returns></returns>
        public static string Manager(int dataCategoryID, Collection<Buchung> buchungen)
        {
            if (buchungen == null)
                throw new ArgumentNullException("buchungen");

            StringBuilder ret = new StringBuilder();

            switch (dataCategoryID)
            {
                case 21:
                    foreach (Buchung buchung in buchungen)
                    {
                        Dictionary<int, ColumnInfo> columnInfo = DatevFields.GenerateColumnInfos(dataCategoryID);
                        for (int i = 0; i < columnInfo.Count; i++)
                        {
                            if (i > 0)
                            {
                                ret.Append(Constants.FieldSeparator);
                            }
                            switch (columnInfo[i].TypeText)
                            {
                                case "Betrag":
                                    ret.Append(ValidateBetrag(i, buchung, columnInfo[i]));
                                    break;
                                case "Datum":
                                    ret.Append(ValidateDatum(i, buchung, columnInfo[i]));
                                    break;
                                case "Konto":
                                    ret.Append(ValidateKonto(i, buchung, columnInfo[i]));
                                    break;
                                case "Text":
                                    ret.Append(ValidateText(i, buchung, columnInfo[i]));
                                    break;
                                case "Zahl":
                                    ret.Append(ValidateZahl(i, buchung, columnInfo[i]));
                                    break;
                            }
                            if (i == columnInfo.Count - 1)
                            {
                                ret.Append(Constants.LineEndTerminator);
                            }
                        }
                    }
                    break;
            }
            return ret.ToString();
        }

        private static string ValidateBetrag(int i, Buchung buchung, ColumnInfo columnInfo)
        {
            StringBuilder ret = new StringBuilder();

            decimal d = buchung.Betrag;
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
                                    throw new InvalidOperationException(string.Format("Round {0}: Mandatory data is zero, which is not allowed to be zero: {1}", i, oi));
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new NotImplementedException(string.Format("Round {0}: Interpreter for macro not implemented: {1}", i, oi));
                    }
                }
                else
                {
                    throw new InvalidOperationException(string.Format("Round {0}: Invalid optional data: {1}", i, oi));
                }
            }
            string format = "0.".PadRight(columnInfo.DecimalPlaces, '0');
            if (d == 0 && columnInfo.IsMandatory || d != 0)
            {
                ret.Append(d.ToString(format));
            }
            if (ret.ToString().Length > columnInfo.MaxLength)
            {
                throw new InvalidOperationException(string.Format("Round {0}: The value is too long (expected max. {1} char): {2}", i, columnInfo.MaxLength, ret.ToString()));
            }

            return ret.ToString();
        }

        private static string ValidateDatum(int i, Buchung buchung, ColumnInfo columnInfo)
        {
            StringBuilder ret = new StringBuilder();

            return ret.ToString();
        }

        private static string ValidateKonto(int i, Buchung buchung, ColumnInfo columnInfo)
        {
            StringBuilder ret = new StringBuilder();

            return ret.ToString();
        }

        private static string ValidateText(int i, Buchung buchung, ColumnInfo columnInfo)
        {
            StringBuilder ret = new StringBuilder();

            return ret.ToString();
        }

        private static string ValidateZahl(int i, Buchung buchung, ColumnInfo columnInfo)
        {
            StringBuilder ret = new StringBuilder();

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
