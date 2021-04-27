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
        /// Manages the export
        /// </summary>
        /// <param name="dataCategoryID"></param>
        /// <param name="buchungen"></param>
        /// <param name="columnInfos"></param>
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
                            switch (columnInfo[i].TypeText)
                            {
                                case "Betrag":
                                    ret.Append(ValidateBetrag(buchung, columnInfo[i]));
                                    break;
                                case "Datum":
                                    ret.Append(ValidateDatum(buchung, columnInfo[i]));
                                    break;
                                case "Konto":
                                    ret.Append(ValidateKonto(buchung, columnInfo[i]));
                                    break;
                                case "Text":
                                    ret.Append(ValidateText(buchung, columnInfo[i]));
                                    break;
                                case "Zahl":
                                    ret.Append(ValidateZahl(buchung, columnInfo[i]));
                                    break;
                            }
                        }
                    }
                    break;

            }
        

            return ret.ToString();
        }

        private static string ValidateBetrag(Buchung buchung, ColumnInfo columnInfo)
        {
            StringBuilder ret = new StringBuilder(); 


            return ret.ToString();
        }

        private static string ValidateDatum(Buchung buchung, ColumnInfo columnInfo)
        {
            StringBuilder ret = new StringBuilder();

            return ret.ToString();
        }

        private static string ValidateKonto(Buchung buchung, ColumnInfo columnInfo)
        {
            StringBuilder ret = new StringBuilder();

            return ret.ToString();
        }

        private static string ValidateText(Buchung buchung, ColumnInfo columnInfo)
        {
            StringBuilder ret = new StringBuilder();

            return ret.ToString();
        }

        private static string ValidateZahl(Buchung buchung, ColumnInfo columnInfo)
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
