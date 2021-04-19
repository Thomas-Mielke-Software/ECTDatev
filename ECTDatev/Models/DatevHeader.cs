using ECTDatev.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECTDatev.Models
{
    /// <summary>
    /// The “header” is the first line of the text file. It contains information used to interpret 
    /// the file during batch processing (information pertaining to import automation).
    /// s. Datev docu 3.3
    /// </summary>
    public class DatevHeader
    {
        private readonly int dataCategoryID;
        private readonly DateTime createdOn;
        /// <summary>
        /// s. Datev-Doku 3.3: 
        /// The header for master data contains less information than the header for transaction data.
        /// </summary>
        private readonly bool isShortHeader;

        public DatevHeader(int dataCategoryID)
        {
            if (!DatevFields.IsDataCategoryValid(dataCategoryID))
            {
                throw new KeyNotFoundException("dataCategoryID: " + dataCategoryID.ToString());
            }

            this.dataCategoryID = dataCategoryID;
            this.createdOn = DateTime.Now;
            isShortHeader = dataCategoryID != 21 && dataCategoryID != 65;
        }

        // 1 DATEV Format Identifier
        public string DATEVFormatIdentifier { get => Constants.DATEVFormatIdentifier; }
        // 2 Version Number
        public int VersionNumber { get => Constants.DATEVFormatVersion; }
        // 3 Data Category
        public int DataCategoryID { get => this.dataCategoryID; }
        // 4 Format Name
        public string FormatName { get => DatevFields.DataCategory[this.DataCategoryID]; }
        // 5 Format Version
        public int FormatVersion { get => DatevFields.FormatVersion[DatevFields.DataCategory[this.DataCategoryID]]; }
        // 6 Created On
        public DateTime CreatedOn { get => this.createdOn; }
        // 7 Imported
        public int? Imported { get => null; }
        // 8 Origin
        public string Origin { get => ToDo.Origin; }
        // 9 Exported By
        public string ExportedBy { get => ToDo.ExportedBy; }
        // 10 Imported By
        public string ImportedBy { get => string.Empty; }
        // 11 Consultant
        public int Consultant { get => ToDo.Consultant; }
        // 12 Client
        public int Client { get => ToDo.Client; }
        // 13 Beginning of FY
        // TODO: check the possible alternatives for the start of FY
        public DateTime BeginningOfFY { get => new DateTime(ToDo.Buchungsjahr, 1, 1); }
 
        // Following header columns might be empty, s. isShortHeader
        // TODO Check out whether the property values are needed in case of empty columns!

        // 14 G/L Account Number Length
        // TODO: set the length for for the remaing cases (possible values: 5(if data category==16) , 8-9(if personal accounts?))
        public int GLAccountNumberLength { get => 4; }
        // 15 Date From
        public DateTime DateFrom { get => ToDo.DateFrom; }
        // 16 Date Until
        public DateTime DateUntil { get => ToDo.DateUntil; }
        // 17 Label (entry batch)
        public string LabelEntryBatch { get => ToDo.LabelEntryBatch; }
        // 18 Initials
        public string Initials { get => ToDo.Initials; }
        // 19 Record Type
        //TODO Set the correct Record Type, currently: Keep it empty, thus, it will be interpreted as 1 = Financial accounting (alternative would be: 2 = Annual financial statements)
        public int? RecordType { get => ToDo.RecordType; }
        // 20 Accounting Reason
        //TODO Check out whether other reasons are also needed! s. DatevFiels.AccountingReason
        public int AccountingReason { get => 0; }
        // 21 Locking
        public int Locking { get => ToDo.Locking; }
        // 22 Currency Code
        public string CurrencyCode { get => ToDo.CurrencyCode; }
        // 23 Reserved
        public int? Reserved23 { get => null; }
        // 24 Derivatives Flag
        public string DerivativesFlag { get => string.Empty; }
        // 25 Reserved
        public int? Reserved25 { get => null; }
        // 26 Reserved
        public int? Reserved26 { get => null; }
        // 27 COA (Datev-SKR)
        public string DatevSKR { get => ToDo.DatevSKR; }
        // 28 Industry Solution ID
        public int? IndustrySolutionID { get => null; }
        // 29 Reserved
        public int? Reserved29 { get => null; }
        // 30 Reserved
        public string Reserved30 { get => string.Empty; }
        // 31 Application Information
        public string ApplicationInformation { get => ToDo.ApplicationInformation; }

        /// <summary>
        /// Creates the header line.
        /// </summary>
        /// <returns>The line ends by the given line end terminator (<seealso cref="Data.Constants.LineEndTerminator"/>).</returns>
        public string GetHeader()
        {
            StringBuilder header = new StringBuilder();
            // The columns (see the Datev documentation for more information)
            // 1 DATEV Format Identifier
            header.Append(Tools.WrapData(this.DATEVFormatIdentifier));
            // 2 Version Number
            header.Append(Constants.FieldSeparator + Tools.WrapData(this.VersionNumber));
            // 3 Data Category
            header.Append(Constants.FieldSeparator + Tools.WrapData(this.DataCategoryID));
            // 4 Format Name
            header.Append(Constants.FieldSeparator + Tools.WrapData(DatevFields.DataCategory[this.DataCategoryID]));
            // 5 Format Version
            header.Append(Constants.FieldSeparator + Tools.WrapData(DatevFields.FormatVersion[DatevFields.DataCategory[this.DataCategoryID]]));
            // 6 Created On
            header.Append(Constants.FieldSeparator + Tools.WrapData(DateTime.Now.ToString(Constants.DateFormat_DataCreated), false));
            // 7 Imported
            header.Append(Constants.FieldSeparator + Tools.WrapData(this.Imported) /* Imported: shall contain no data */ );
            // 8 Origin
            header.Append(Constants.FieldSeparator + Tools.WrapData(this.Origin));
            // 9 Exported By
            header.Append(Constants.FieldSeparator + Tools.WrapData(this.ExportedBy));
            // 10 Imported By
            header.Append(Constants.FieldSeparator + /* Imported by: shall contain no data */ Tools.WrapData(this.ImportedBy));
            // 11 Consultant
            header.Append(Constants.FieldSeparator + Tools.WrapData(this.Consultant));
            // 12 Client
            header.Append(Constants.FieldSeparator + Tools.WrapData(this.Client));
            // 13 Beginning of FY
            //TODO: check the possible alternatives for the start of FY
            header.Append(Constants.FieldSeparator + Tools.WrapData(this.BeginningOfFY.ToString(Constants.DateFormat_StartFiscalYear), false));

            // Following header columns might be empty, s. isShortHeader

            // 14 G/L Account Number Length
            header.Append(Constants.FieldSeparator + (this.isShortHeader ? Tools.WrapData(string.Empty, false) : Tools.WrapData(this.GLAccountNumberLength)));
            // 15 Date From
            header.Append(Constants.FieldSeparator + (this.isShortHeader ? Tools.WrapData(string.Empty, false) : Tools.WrapData(this.DateFrom.ToString(Constants.DateFormat_FromUntil), false)));
            // 16 Date Until
            header.Append(Constants.FieldSeparator + (this.isShortHeader ? Tools.WrapData(string.Empty, false) : Tools.WrapData(this.DateUntil.ToString(Constants.DateFormat_FromUntil), false)));
            // 17 Label (entry batch)
            header.Append(Constants.FieldSeparator + (this.isShortHeader ? Tools.WrapData(string.Empty, true) : Tools.WrapData(this.LabelEntryBatch)));
            // 18 Initials
            header.Append(Constants.FieldSeparator + (this.isShortHeader ? Tools.WrapData(string.Empty, true) : Tools.WrapData(this.Initials)));
            // 19 Record Type
            header.Append(Constants.FieldSeparator + (this.isShortHeader ? Tools.WrapData(string.Empty, false) : Tools.WrapData(this.RecordType) /* Keep it empty, thus, it will be  1 = Financial accounting (alternative would be: 2 = Annual financial statements) */ ));
            // 20 Accounting Reason
            header.Append(Constants.FieldSeparator + (this.isShortHeader ? Tools.WrapData(string.Empty, false) : Tools.WrapData(this.AccountingReason) /* Keep it empty, thus, it becomes 0 = not tied to a specific assessment area */ ));
            // 21 Locking
            header.Append(Constants.FieldSeparator + (this.isShortHeader ? Tools.WrapData(string.Empty, false) : Tools.WrapData(this.Locking)));
            // 22 Currency Code
            header.Append(Constants.FieldSeparator + (this.isShortHeader ? Tools.WrapData(string.Empty, true) : Tools.WrapData(this.CurrencyCode)));
            // 23 Reserved
            header.Append(Constants.FieldSeparator + (this.isShortHeader ? Tools.WrapData(string.Empty, false) : Tools.WrapData(this.Reserved23)));
            // 24 Derivatives Flag
            header.Append(Constants.FieldSeparator + (this.isShortHeader ? Tools.WrapData(string.Empty, true) : Tools.WrapData(this.DerivativesFlag)));
            // 25 Reserved
            header.Append(Constants.FieldSeparator + (this.isShortHeader ? Tools.WrapData(string.Empty, false) : Tools.WrapData(this.Reserved25)));
            // 26 Reserved
            header.Append(Constants.FieldSeparator + (this.isShortHeader ? Tools.WrapData(string.Empty, false) : Tools.WrapData(this.Reserved26)));
            // 27 COA (Datev-SKR)
            header.Append(Constants.FieldSeparator + (this.isShortHeader ? Tools.WrapData(string.Empty, true) : Tools.WrapData(this.DatevSKR)));
            // 28 Industry Solution ID
            header.Append(Constants.FieldSeparator + (this.isShortHeader ? Tools.WrapData(string.Empty, false) : Tools.WrapData(this.IndustrySolutionID)));
            // 29 Reserved
            header.Append(Constants.FieldSeparator + (this.isShortHeader ? Tools.WrapData(string.Empty, false) : Tools.WrapData(this.Reserved29)));
            // 30 Reserved
            header.Append(Constants.FieldSeparator + (this.isShortHeader ? Tools.WrapData(string.Empty, true) : Tools.WrapData(this.Reserved30)));
            // 31 Application Information
            header.Append(Constants.FieldSeparator + (this.isShortHeader ? Tools.WrapData(string.Empty, true) : Tools.WrapData(this.ApplicationInformation)));

            header.Append(Constants.LineEndTerminator);

            return header.ToString();
        }
    }
}
