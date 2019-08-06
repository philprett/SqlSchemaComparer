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
        private string _str;
        private string _strNoComments;
        /// <summary>
        /// The SQL script
        /// </summary>
        public string Str
        {
            get { return _str; }
            set
            {
                _str = value;
                _strNoComments = null;
            }
        }

        public string StrNoComments
        {
            get
            {
                if (_strNoComments == null)
                    _strNoComments = commentsToSpaces(_str);
                return _strNoComments;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="str"></param>
        public SqlString(string str)
        {
            Str = str;
        }

        public void Append(string str)
        {
            Str += str;
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
        /// Replace all comments with spaces making it easier to search scripts
        /// </summary>
        /// <returns></returns>
        public SqlString CommentsToSpaces()
        {
            return new SqlString(commentsToSpaces(Str));
        }
        private string commentsToSpaces(string str)
        {
            char[] chars = str.ToCharArray();
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
            return new string(chars);
        }

        public SqlString RemoveSquareBrackets()
        {
            string ret = Str;
            ret = ret.Replace("[", "");
            ret = ret.Replace("]", "");
            return new SqlString(ret);
        }

        public SqlString RemoveGo()
        {
            string ret = Str;
            ret = ret.Replace("\nGO\r\n", "\n\r\n");
            ret = ret.Replace("\nGO\n", "\n\n");
            if (ret.EndsWith("\r\nGO")) ret = ret.Substring(0, ret.Length - 4);
            if (ret.EndsWith("\nGO")) ret = ret.Substring(0, ret.Length - 3);
            return new SqlString(ret);
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

            return new SqlString(ret.Trim());
        }

        public bool IsCreateTable { get { return this.GetObjectTypeFromCreateSql() == "TABLE"; } }
        public bool IsCreateView { get { return this.GetObjectTypeFromCreateSql() == "VIEW"; } }
        public bool IsCreateFunction { get { return this.GetObjectTypeFromCreateSql() == "FUNC"; } }
        public bool IsCreateProcedure { get { return this.GetObjectTypeFromCreateSql() == "PROC"; } }

        public string GetObjectTypeFromCreateSql()
        {
            SqlString noComments = this.CommentsToSpaces();
            SqlString reducedWhitespace = noComments.ReduceWhiteSpace();

			int posTable = reducedWhitespace.Str.IndexOf("CREATE TABLE", StringComparison.CurrentCultureIgnoreCase);
			int posView = reducedWhitespace.Str.IndexOf("CREATE VIEW", StringComparison.CurrentCultureIgnoreCase);
			int posFunc = reducedWhitespace.Str.IndexOf("CREATE FUNC", StringComparison.CurrentCultureIgnoreCase);
            int posProc = reducedWhitespace.Str.IndexOf("CREATE PROC", StringComparison.CurrentCultureIgnoreCase);

			if (posTable >= 0 &&
				(posView < 0 || posTable < posView) &&
				(posFunc < 0 || posTable < posFunc) &&
				(posProc < 0 || posTable < posProc))
			{
				return "TABLE";
			}
			if (posView >= 0 &&
				(posTable < 0 || posView < posTable) &&
				(posFunc < 0 || posView < posFunc) &&
				(posProc < 0 || posView < posProc))
			{
				return "VIEW";
			}
			if (posFunc >= 0 &&
				(posTable < 0 || posFunc < posTable) &&
				(posView < 0 || posFunc < posView) &&
				(posProc < 0 || posFunc < posProc))
			{
				return "FUNC";
			}
			if (posProc >= 0 &&
				(posTable < 0 || posProc < posTable) &&
				(posView < 0 || posProc < posView) &&
				(posFunc < 0 || posProc < posFunc))
			{
				return "PROC";
			}

			return string.Empty;
        }

		public string GetNameFromCreateSQL()
		{
			SqlString noComments = this.CommentsToSpaces();
			SqlString reducedWhitespace = noComments.ReduceWhiteSpace();

			string name = string.Empty;
			int pos = reducedWhitespace.Str.IndexOf("CREATE TABLE", StringComparison.CurrentCultureIgnoreCase);
			if (pos >= 0)
			{
				name = reducedWhitespace.Str.Substring(pos + 13);
				int spacePos = name.IndexOf(" ");
				int bracketPos = name.IndexOf("(");
				if (spacePos >= 0 && bracketPos >= 0)
				{
					if (spacePos < bracketPos)
					{
						name = name.Substring(0, spacePos);
					}
					else if (spacePos > bracketPos)
					{
						name = name.Substring(0, bracketPos);
					}
				}
				else if (spacePos >= 0)
				{
					name = name.Substring(0, spacePos);
				}
				else if (bracketPos >= 0)
				{
					name = name.Substring(0, bracketPos);
				}
				else
				{
					name = string.Empty;
				}
				if (name.StartsWith("#"))
				{
					name = string.Empty;
					pos = -1;
				}
			}
			if (pos < 0)
			{
				pos = reducedWhitespace.Str.IndexOf("CREATE VIEW", StringComparison.CurrentCultureIgnoreCase);
				if (pos < 0) pos = reducedWhitespace.Str.IndexOf("CREATE FUNC", StringComparison.CurrentCultureIgnoreCase);
				if (pos < 0) pos = reducedWhitespace.Str.IndexOf("CREATE PROC", StringComparison.CurrentCultureIgnoreCase);
				if (pos >= 0)
				{
					if (reducedWhitespace.Str.Substring(pos, 15).Equals("CREATE FUNCTION", StringComparison.CurrentCultureIgnoreCase))
						name = reducedWhitespace.Str.Substring(pos + 16);
					else if (reducedWhitespace.Str.Substring(pos, 16).Equals("CREATE PROCEDURE", StringComparison.CurrentCultureIgnoreCase))
						name = reducedWhitespace.Str.Substring(pos + 17);
					else
						name = reducedWhitespace.Str.Substring(pos + 12);
					int spacePos = name.IndexOf(" ");
					if (spacePos >= 0)
					{
						name = name.Substring(0, spacePos);
					}
					else
					{
						name = string.Empty;
					}
				}
			}

			if (!string.IsNullOrWhiteSpace(name))
			{
				name = name.Replace("[", "");
				name = name.Replace("]", "");
			}

			return name;
		}
		public bool FunctionallyEquals(SqlString otherSqlString, bool ignoreComments, bool ignoreGo, bool caseSensitive)
        {
            SqlString sql1 = ignoreComments ? CommentsToSpaces() : new SqlString(Str);
            SqlString sql2 = ignoreComments ? otherSqlString.CommentsToSpaces() : new SqlString(otherSqlString.Str);

            if (ignoreGo && (sql1.Str.IndexOf("GO") >= 0 || sql2.Str.IndexOf("GO") >= 0))
            {
                sql1 = sql1.RemoveGo();
                sql2 = sql2.RemoveGo();
            }
            if (!caseSensitive)
            {
                sql1.Str = sql1.Str.ToUpper();
                sql2.Str = sql2.Str.ToUpper();
            }

            sql1 = sql1.RemoveSquareBrackets().ReduceWhiteSpace();
            sql2 = sql2.RemoveSquareBrackets().ReduceWhiteSpace();

            return sql1.Str.Trim() == sql2.Str.Trim();
        }

        public bool DependsOn(string objectName)
        {
			string objectNameNoSchema = objectName.Contains(".") ? objectName.Substring(objectName.IndexOf(".") + 1) : objectName;

			if (StrNoComments.IndexOf(objectNameNoSchema + " ", StringComparison.CurrentCultureIgnoreCase) >= 0) return true;
			if (StrNoComments.IndexOf(objectNameNoSchema + "\t", StringComparison.CurrentCultureIgnoreCase) >= 0) return true;
			if (StrNoComments.IndexOf(objectNameNoSchema + "\r", StringComparison.CurrentCultureIgnoreCase) >= 0) return true;
			if (StrNoComments.IndexOf(objectNameNoSchema + "\n", StringComparison.CurrentCultureIgnoreCase) >= 0) return true;
			if (StrNoComments.IndexOf(objectNameNoSchema + "(", StringComparison.CurrentCultureIgnoreCase) >= 0) return true;
			return false;
		}

		public SqlString ChangeCreateToAlter()
        {
            int createPos = StrNoComments.IndexOf("CREATE", StringComparison.CurrentCultureIgnoreCase);
            if (createPos < 0) throw new Exception("Not a CREATE SQL statement");

            string alterSql = Str.Substring(0, createPos) + "ALTER " + Str.Substring(createPos + 6);
            return new SqlString(alterSql);
        }

        public SqlString ChangeCreateToDropCreate()
        {
            string strippedSQL = new SqlString(this.StrNoComments).ReduceWhiteSpace().StrNoComments;
            int createPos = StrNoComments.IndexOf("CREATE", StringComparison.CurrentCultureIgnoreCase);
            if (createPos < 0) throw new Exception("Not a CREATE SQL statement");

            string createSql = strippedSQL.Substring(strippedSQL.IndexOf("CREATE")).TrimStart().Replace("(", " (").Replace("\t", " \t").Replace("\r", " \r").Replace("\n", " \n");
            int spacePos = createSql.IndexOf(" ");
            spacePos = createSql.IndexOf(" ", spacePos + 1);
            spacePos = createSql.IndexOf(" ", spacePos + 1);
            createSql = createSql.Substring(0, spacePos);

            string dropCreateSql = "DROP" + createSql.Substring(6)+"\r\nGO\r\n"+Str;
            return new SqlString(dropCreateSql);
        }
    }
}
