using System;
using System.Collections.Generic;
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
        /// <param name="data"></param>
        /// <param name="columnInfos"></param>
        /// <returns></returns>
        public static string Manager(int dataCategoryID, Dictionary<int, object> data, Dictionary<int, ColumnInfo> columnInfos)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (columnInfos == null)
                throw new ArgumentNullException("columnInfos");
            if (data.Keys.Count > columnInfos.Keys.Count)
            {
                throw new ArgumentOutOfRangeException("Data shall have less or equal number of elements then columnInfo. Data: " + data.Keys.Count.ToString() + ", columnInfo: " + columnInfos.Keys.Count.ToString());
            }

            string ret = string.Empty;

            return ret;
        }
        
        public static string Validate(short value)
        {
            string ret = string.Empty;

            return ret;
        }

        public static string Validate(int value)
        {
            string ret = string.Empty;

            return ret;
        }

        public static string Validate(double value)
        {
            string ret = string.Empty;

            return ret;
        }

        public static string Validate(decimal value)
        {
            string ret = string.Empty;

            return ret;
        }

        public static string Validate(string value)
        {
            string ret = string.Empty;

            return ret;
        }

        public static string Validate(DateTime value)
        {
            string ret = string.Empty;

            return ret;
        }
    }
}
