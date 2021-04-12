﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECTDatev.Data
{
    public static class Constants
    {
        public const string FieldSeparator = ";";

        public const string SeparatorThousandsPlaces_EntryBatch = "";
        public const string SeparatorThousandsPlaces_RecurringEntries = "";
        public const string SeparatorThousandsPlaces_CustomersVendors = ".";
        public const string SeparatorThousandsPlaces_GLAccountLabels = "";
        public const string SeparatorThousandsPlaces_VariousAddresses = ".";
        public const string SeparatorThousandsPlaces_AssetAccountingEntryTemplates = "";
        public const string SeparatorDecimalPlaces = ",";
        public const string SeparatorDecimalPlaces_PaymentConditions = "";
        public const string SeparatorDecimalPlaces_VariousAddresses = ",";
        public const string DateFormat_EntryBatch = "ddMM";
        public const string DateFormat_RecurringEntries = "ddMM";
        public const string DateFormat_CustomersVendors = "ddMMyyyy";
        public const string DateFormat_DataCreated = "yyyyMMddhhmmssfff";
        public const string DateFormat_StartFiscalYear = "yyyyMMdd";
        public const string DateFormat_FromUntil = "yyyyMMdd";
        public const string CharactersAroundText = @"""";
        public const string SeparatorAtEndOfDataSet = "";
        public const string LineEndTerminator = "\r\n";
        public const bool ExportLineFeed = false;

        /// <summary>
        /// The start of the macro.
        /// </summary>
        public const string MacroStart = "{*|";
        /// <summary>
        /// The end of the macro.
        /// </summary>
        public const string MacroEnd = "|*}";
        /// <summary>
        /// The <code>OneOf</code> macro. The first item in the list is the default value and will be used if the empty column is mandatory.
        /// </summary>
        public const string MacroKeyword_OneOf = "ONE OF";
        /// <summary>
        /// The <code>NotAllowed</code> macro. List the not allowed items.
        /// </summary>
        public const string MacroKeyword_NotAllowed = "NOT ALLOWED";
        /// <summary>
        /// The <code>DateFormat</code> macro.
        /// </summary>
        public const string MacroKeyword_DateFormat = "DATE FORMAT";
        /// <summary>
        /// The <code>Abs</code> macro. value = abs(value)
        /// </summary>
        public const string MacroKeyword_Abs = "ABS";
        /// <summary>
        /// The <code>SetDebitOrCredit</code> macro.
        /// </summary>
        public const string MacroKeyword_SetDebitOrCredit = "SET DEBIT OR CREDIT";
        /// <summary>
        /// The <code>AllowedChars</code> macro.
        /// </summary>
        public const string MacroKeyword_AllowedChars = "ALLOWED CHARS";
        /// <summary>
        /// The <code>UpperCase</code> macro.
        /// </summary>
        public const string MacroKeyword_UpperCase = "UPPER CASE";
        /// <summary>
        /// The <code>LowerCase</code> macro.
        /// </summary>
        public const string MacroKeyword_LowerCase = "LOWER CASE";

        public const string DATEVFormatIdentifier = "EXTF";
        public const int DATEVFormatVersion = 700; // 7.00

        public const string FileNamePrefix = "EXTF_";
        public const string FileNameExtension = ".csv";

        public static int Length_ExportedBy = 25;
        
        /// <summary>
        /// Used in <see cref="Constants.MacroKeyword_AllowedChars"/>.
        /// </summary>
        public const string ReplacementForNotAllowedChars = "-";
    }
}
