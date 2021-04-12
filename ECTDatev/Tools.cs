using ECTDatev.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECTDatev
{
    public static class Tools
    {
        public static string WrapData(int data)
        {
            return data.ToString();
        }

        public static string WrapData(int? data)
        {
            if (data == null)
            {
                return string.Empty;
            }
            else
            {
                return data.ToString();
            }    

        }

        public static string WrapData(string data, bool wrap = true)
        {
            string ret;
            if (data == null)
                data = string.Empty;

            if (wrap)
                ret = Constants.CharactersAroundText + DoubleTheDoubleQuotes(data) + Constants.CharactersAroundText;
            else
                ret = DoubleTheDoubleQuotes(data);

            return ret;
        }

        /// <summary>
        /// Doubles the double quotes in the given string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string DoubleTheDoubleQuotes(string str)
        {
            return str.Replace(Constants.CharactersAroundText, Constants.CharactersAroundText + Constants.CharactersAroundText);
        }

        /// <summary>
        /// Removes the double quotes at the beginning and end of the string.
        /// </summary>
        /// <param name="str">The parameter to unwrap.</param>
        /// <returns>The unwrapped string.</returns>
        public static string UnWrap(string str)
        {
            return str.Trim(Constants.CharactersAroundText.ToCharArray());
        }
    }
}
