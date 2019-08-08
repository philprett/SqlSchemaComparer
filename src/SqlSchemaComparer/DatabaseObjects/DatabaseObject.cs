﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSchemaComparer.DatabaseObjects
{
	internal class DatabaseObject
	{
		public string Filename { get; set; }
		public string Schema { get; set; }
		public string Name { get; set; }
		public string ObjectType { get; set; }
		public int ObjectTypeSort
		{
			get
			{
				if (ObjectType == "U") return 1;
				if (ObjectType == "V") return 2;
				if (ObjectType == "TF") return 3;
				if (ObjectType == "FN") return 3;
				if (ObjectType == "P") return 4;
				return 5;
			}
		}
		public string ObjectTypeNice
		{
			get
			{
				if (ObjectType == "U") return "TABLE";
				if (ObjectType == "V") return "VIEW";
				if (ObjectType == "FN") return "FUNCTION";
				if (ObjectType == "TF") return "FUNCTION";
				if (ObjectType == "P") return "PROCEDURE";
				return "";
			}
		}
		public SqlString CreateSQL { get; set; }

		public string DropSQL
		{
			get
			{
				return string.Format("IF OBJECT_ID('{0}') IS NOT NULL DROP {1} {0}\r\nGO\r\n\r\n", Name, ObjectTypeNice);
			}
		}


		public DatabaseObject()
		{
			Filename = string.Empty;
			Schema = string.Empty;
			Name = string.Empty;
			ObjectType = string.Empty;
			CreateSQL = new SqlString(string.Empty);
		}

		public override string ToString()
		{
			return Schema + "." + Name;
		}
	}
}
