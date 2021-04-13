using ECTDatev.Data;
using System;
using System.Collections.Generic;
using System.IO;
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

        /// <summary>
        /// Wites the given <see cref="String"/> into the given <see cref="FileStream"/>.
        /// Use this as the wrapper to writing data into a file.
        /// </summary>
        /// <param name="fs">The FileStream to write in.</param>
        /// <param name="str">The String to write.</param>
        public static void AddTextToUTF8Stream(FileStream fs, string str)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(str);
            fs.Write(info, 0, info.Length);
        }
    }
}
