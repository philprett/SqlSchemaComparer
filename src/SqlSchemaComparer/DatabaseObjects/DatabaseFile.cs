using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SqlSchemaComparer.DatabaseObjects
{
    internal class DatabaseFile
    {
        public string Filename { get; set; }

        public SqlString Contents { get; set; }

        public SqlString ContentsStripped { get; set; }

        public SqlString ContentsReducedWhiteSpace { get; set; }

        public string ObjectType { get; set; }
        public string ObjectName { get; set; }

        public DatabaseFile(string filename)
        {
            Filename = filename;
            Contents = new SqlString(File.ReadAllText(Filename));
            ContentsStripped = Contents.CommentsToSpaces();
            ContentsReducedWhiteSpace = ContentsStripped.ReduceWhiteSpace();

            int pos = ContentsReducedWhiteSpace.ToString().IndexOf("CREATE TABLE", StringComparison.CurrentCultureIgnoreCase);
            if (pos >= 0)
            {
                ObjectType = "U";
                ObjectName = ContentsReducedWhiteSpace.ToString().Substring(pos + 13);
                if (ObjectName.IndexOf(" ") >= 0) ObjectName = ObjectName.Substring(0, ObjectName.IndexOf(" "));
                if (ObjectName.IndexOf("(") >= 0) ObjectName = ObjectName.Substring(0, ObjectName.IndexOf("("));
                if (ObjectName.IndexOf(".") >= 0) ObjectName = ObjectName.Substring(ObjectName.LastIndexOf(".") + 1);
                if (ObjectName[0] == '[') ObjectName = ObjectName.Substring(1);
                if (ObjectName[ObjectName.Length - 1] == ']') ObjectName = ObjectName.Substring(0, ObjectName.Length - 1);
            }
            else if (ContentsReducedWhiteSpace.ToString().IndexOf("CREATE VIEW", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                ObjectType = "V";
            }
            else if (ContentsReducedWhiteSpace.ToString().IndexOf("CREATE FUNC", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                ObjectType = "FN";
            }
            else if (ContentsReducedWhiteSpace.ToString().IndexOf("CREATE PROC", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                ObjectType = "P";
            }
            else
            {
                ObjectType = "";
            }
        }


    }
}
