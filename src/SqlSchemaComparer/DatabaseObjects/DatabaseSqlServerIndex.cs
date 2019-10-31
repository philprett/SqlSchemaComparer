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
		public List<string> IncludeColumnNames { get; set; }

		public DatabaseSqlServerIndex()
		{
			SchemaName = string.Empty;
			IndexName = string.Empty;
			TableName = string.Empty;
			ColumnNames = new List<string>();
			IncludeColumnNames = new List<string>();
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
				bool includeColumn = (bool)dataRow["is_included_column"];

				DatabaseSqlServerIndex index = ret.FirstOrDefault(i => i.SchemaName.Equals(schemaName, StringComparison.CurrentCultureIgnoreCase) && i.IndexName.Equals(indexName, StringComparison.CurrentCultureIgnoreCase));
				if (index == null)
				{
					index = new DatabaseSqlServerIndex() { SchemaName = schemaName, IndexName = indexName, TableName = tableName };
					ret.Add(index);
				}

				if (includeColumn)
				{
					index.IncludeColumnNames.Add(columnName);
				}
				else
				{
					index.ColumnNames.Add(columnName);
				}
			}

			return ret;
		}

		public string CreateSQL
		{
			get
			{
				return string.Format(
					"CREATE INDEX {1} ON {0}.{2}({3}){4}",
					SchemaName, IndexName, TableName, string.Join(",", ColumnNames),
					IncludeColumnNames.Count == 0 ? "" : " INCLUDE (" + string.Join(",", IncludeColumnNames)
					);
			}
		}
	}
}
