using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSchemaComparer.DatabaseObjects
{
    internal class DatabaseObject
    {
        public string Schema { get; set; }
        public string Name { get; set; }
        public string ObjectType { get; set; }
        public string ObjectTypeNice
        {
            get
            {
                if (ObjectType == "U") return "TABLE";
                if (ObjectType == "V") return "VIEW";
                if (ObjectType == "FN") return "FUNC";
                if (ObjectType == "P") return "PROC";
                return "";
            }
        }
        public SqlString CreateSQL { get; set; }


        public DatabaseObject()
        {
            Schema = string.Empty;
            Name = string.Empty;
            ObjectType = string.Empty;
            CreateSQL = new SqlString(string.Empty);
        }
    }
}
