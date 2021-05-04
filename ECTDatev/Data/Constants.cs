using System;
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
        public const string OriginDefault = "EC";

        /// <summary>
        /// The start of the macro.
        /// </summary>
        public const string MacroStart = "{*|";
        /// <summary>
        /// The end of the macro.
        /// </summary>
        public const string MacroEnd = "|*}";
        /// <summary>
        /// You can join macros by this separator.
        /// </summary>
        public const string MacroSeparator = "|;|";
        /// <summary>
        /// The <code>OneOf</code> macro. The first item in the list is the default value and will be used if the empty column is mandatory.
        /// </summary>
        public const string MacroKeyword_OneOf = "ONE OF";
        /// <summary>
        /// The <code>NotAllowedChars</code> macro. Lists the not allowed chars.
        /// </summary>
        public const string MacroKeyword_NotAllowedChars = "NOT ALLOWED CHARS";
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
        public const string ReplacementForNotAllowedChars = "";

        /// <summary>
        /// If an overlong text get shortened, then this marker will be put to the end of the shortened text in order to show it was cut off
        /// </summary>
        public const string TextShortenedMarkerPostfix = "...";

        /// <summary>
        ///The prefix for the cut off text.
        /// </summary>
        public const string CutOffTextMarkerPrefix = "...";

        /// <summary>
        /// If overlong cut off field content get saved in one of those 20 <c>Additional invoice information</c> fields
        /// (if <see cref="DatevPropertyItems.SaveOverlongTextValues"/> is true),
        /// the <c>Additional invoice information type</c> will contain the name of the cut off field (the text is coming from)
        /// followed by a space char and the part counter (in how many parts that text was saved).
        /// </summary>
        public const string FormatForShortenedPartsCounter = " 0";
    }
}
