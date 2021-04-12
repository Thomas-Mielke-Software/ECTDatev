using ECTDatev.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECTDatev.Models
{
    /// <summary>
    /// The second line of the text file contains a headline which documents the individual fields.
    /// s. Datev docu 3.4
    /// </summary>
    public static class DatevHeadline
    {
        /// <summary>
        /// Creates the headline.
        /// </summary>
        /// <param name="dataCategoryID">The given data category ID</param>
        /// <returns>The line ends by the given line end terminator (<seealso cref="Data.Constants.LineEndTerminator"/>).</returns>
        public static string GetHeadline(int dataCategoryID)
        {
            if (!DatevFields.IsDataCategoryValid(dataCategoryID))
            {
                throw new KeyNotFoundException("dataCategoryID: " + dataCategoryID.ToString());
            }

            StringBuilder headLine = new StringBuilder();
            Dictionary<int, DatevFields.ColumnInfo> columns = DatevFields.GenerateColumnInfos(dataCategoryID);
            for (int i = 1; i <= columns.Count; i++)
            {
                headLine.Append(columns[i].Name);
                if (i < columns.Count)
                {
                    headLine.Append(Constants.FieldSeparator);
                }
            }

            headLine.Append(Constants.LineEndTerminator);

            return headLine.ToString();
        }
    }
}
