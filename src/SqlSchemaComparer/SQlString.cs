using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SqlSchemaComparer
{
    /// <summary>
    /// Provide methods to update SQL scripts
    /// </summary>
    internal class SqlString
    {
        /// <summary>
        /// The SQL script
        /// </summary>
        public string Str { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="str"></param>
        public SqlString(string str)
        {
            Str = str;
        }

        /// <summary>
        /// Get the string value
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Str;
        }

        /// <summary>
        /// Replace all commments with spaces making it easier to search scripts
        /// </summary>
        /// <returns></returns>
        public SqlString CommentsToSpaces()
        {
            char[] chars = Str.ToCharArray();
            int charsLength = chars.Length;
            bool inComment = false;
            for (int c = 0; c < charsLength - 1; c++)
            {
                if (!inComment && chars[c] == '-' && chars[c] == '-')
                {
                    inComment = true;
                }
                if (chars[c] == '\r' || chars[c] == '\n')
                {
                    inComment = false;
                }
                if (inComment)
                {
                    chars[c] = ' ';
                }
            }
            return new SqlString(new string(chars));
        }

        /// <summary>
        /// Reduce all multiple whitespaces with a single space
        /// </summary>
        /// <returns></returns>
        public SqlString ReduceWhiteSpace()
        {
            string ret = Str;
            ret = ret.Replace("\t", " ");
            ret = ret.Replace("\r", " ");
            ret = ret.Replace("\n", " ");

            while (ret.Contains("  ")) ret = ret.Replace("  ", " ");

            return new SqlString(ret);
        }

        public bool IsCreateTable { get { return this.CommentsToSpaces().ReduceWhiteSpace().ToString().IndexOf("CREATE TABLE", StringComparison.CurrentCultureIgnoreCase) >= 0; } }
        public bool IsCreateView { get { return this.CommentsToSpaces().ReduceWhiteSpace().ToString().IndexOf("CREATE VIEW", StringComparison.CurrentCultureIgnoreCase) >= 0; } }
        public bool IsCreateFunction { get { return this.CommentsToSpaces().ReduceWhiteSpace().ToString().IndexOf("CREATE FUNC", StringComparison.CurrentCultureIgnoreCase) >= 0; } }
        public bool IsCreateProcedure { get { return this.CommentsToSpaces().ReduceWhiteSpace().ToString().IndexOf("CREATE PROC", StringComparison.CurrentCultureIgnoreCase) >= 0; } }
    }
}
