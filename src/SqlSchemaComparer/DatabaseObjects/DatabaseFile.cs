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

			ObjectName = ContentsReducedWhiteSpace.GetObjectTypeFromCreateSql();
			if (ContentsReducedWhiteSpace.IsCreateTable)
            {
                ObjectType = "U";
            }
            else if (ContentsReducedWhiteSpace.IsCreateView)
            {
                ObjectType = "V";
            }
            else if (ContentsReducedWhiteSpace.IsCreateFunction)
            {
                ObjectType = "FN";
            }
            else if (ContentsReducedWhiteSpace.IsCreateProcedure)
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
