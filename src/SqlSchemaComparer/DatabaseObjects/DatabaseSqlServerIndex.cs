using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSchemaComparer.DatabaseObjects
{
	class DatabaseSqlServerIndex
	{
		public string SchemaName { get; set; }
		public string IndexName { get; set; }
		public string TableName { get; set; }
		public List<string> ColumnNames { get; set; }

		public DatabaseSqlServerIndex()
		{
			SchemaName = string.Empty;
			IndexName = string.Empty;
			TableName = string.Empty;
			ColumnNames = new List<string>();
		}

		public static List<DatabaseSqlServerIndex> GetFromDataTable(DataTable dataTable)
		{
			List<DatabaseSqlServerIndex> ret = new List<DatabaseSqlServerIndex>();
			foreach (DataRow dataRow in dataTable.Rows)
			{
				string schemaName = (string)dataRow["SchemaName"];
				string indexName = (string)dataRow["IndexName"];
				string tableName = (string)dataRow["tablename"];
				string columnName = (string)dataRow["ColumnName"];

				DatabaseSqlServerIndex index = ret.FirstOrDefault(i => i.SchemaName.Equals(schemaName, StringComparison.CurrentCultureIgnoreCase) && i.IndexName.Equals(indexName, StringComparison.CurrentCultureIgnoreCase));
				if (index == null)
				{
					index = new DatabaseSqlServerIndex() { SchemaName = schemaName, IndexName = indexName, TableName = tableName };
					ret.Add(index);
				}

				index.ColumnNames.Add(columnName);
			}

			return ret;
		}

		public string CreateSQL
		{
			get
			{
				return string.Format(
					"CREATE INDEX {1} ON {0}.{2}({3})",
					SchemaName, IndexName, TableName, string.Join(",", ColumnNames)
					);
			}
		}
	}
}
