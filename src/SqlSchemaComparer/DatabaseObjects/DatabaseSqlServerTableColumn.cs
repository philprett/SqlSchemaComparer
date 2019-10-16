using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSchemaComparer.DatabaseObjects
{
	internal class DatabaseSqlServerTableColumn
	{
		public string SchemaName { get; set; }
		public string ObjectName { get; set; }
		public string ColumnName { get; set; }
		public int ColumnID { get; set; }
		public int ColumnTypeId { get; set; }
		public string ColumnType { get; set; }
		public int MaxLength { get; set; }
		public int Precision { get; set; }
		public int Scale { get; set; }
		public bool IsNullable { get; set; }
		public bool IsIdentity { get; set; }
		public string DefaultValue { get; set; }
		public string DefaultConstraintName { get; set; }
		public string ComputedDefinition { get; set; }
		public bool IsPrimaryKey { get; set; }
		public string IndexName { get; set; }

		public DatabaseSqlServerTableColumn()
		{
			SchemaName = string.Empty;
			ObjectName = string.Empty;
			ColumnName = string.Empty;
			ColumnID = -1;
			ColumnTypeId = -1;
			ColumnType = string.Empty;
			MaxLength = -1;
			Precision = -1;
			Scale = -1;
			IsNullable = true;
			IsIdentity = false;
			DefaultValue = string.Empty;
			DefaultConstraintName = string.Empty;
			ComputedDefinition = string.Empty;
			IsPrimaryKey = false;
			IndexName = string.Empty;
		}

		public string GetDefinitionType()
		{
			if (ColumnType == "real" || ColumnType == "money" || ColumnType == "decimal" || ColumnType == "numeric" || ColumnType == "smallmoney")
			{
				return string.Format("{0}({1},{2})", ColumnType, Precision, Scale);
			}
			else if (ColumnType == "varbinary" || ColumnType == "varchar" || ColumnType == "binary" || ColumnType == "char")
			{
				return string.Format("{0}({1})", ColumnType, MaxLength == -1 ? "MAX" : MaxLength.ToString());
			}
			else if (ColumnType == "nvarchar" || ColumnType == "nchar")
			{
				return string.Format("{0}({1})", ColumnType, MaxLength == -1 ? "MAX" : (MaxLength / 2).ToString());
				//return string.Format("{0}({1})", ColumnType, MaxLength == -1 ? "MAX" : MaxLength.ToString());
			}
			else
			{
				return ColumnType;
			}
		}

		public static List<DatabaseSqlServerTableColumn> GetFromDataTable(DataTable columns, DataTable pks)
		{
			// columns
			// SchemaName, ObjectName, ColumnName, xtypeid, xtype, length, isnullable, IsIdentity, xprec, xscale, 
			// DefaultValue, DefaultConstraintName, ComputedDefinition

			// pks
			// ConstraintName, IsUnique, IsPrimaryKey, IsDescending, ColumnName, tablename, schemaname 

			// indexes
			// IndexName, ColumnName, tablename

			if (!columns.Columns.Contains("SchemaName") || !columns.Columns.Contains("ObjectName") || !columns.Columns.Contains("ColumnName") ||
				!columns.Columns.Contains("ColumnID") || !columns.Columns.Contains("xtypeid") || !columns.Columns.Contains("xtype") ||
				!columns.Columns.Contains("length") || !columns.Columns.Contains("isnullable") || !columns.Columns.Contains("IsIdentity") ||
				!columns.Columns.Contains("xprec") || !columns.Columns.Contains("xscale") || !columns.Columns.Contains("DefaultValue") ||
				!columns.Columns.Contains("DefaultConstraintName") || !columns.Columns.Contains("ComputedDefinition") || columns.Columns.Count != 14)
			{
				throw new EvaluateException("Invalid columns in DataTable argument. Should contain SchemaName, ObjectName, ColumnName, ColumnID, xtypeid, xtype, length, isnullable, IsIdentity, xprec, xscale, DefaultValue, DefaultConstraintName, ComputedDefinition");
			}

			List<DatabaseSqlServerTableColumn> ret = new List<DatabaseSqlServerTableColumn>();
			foreach (DataRow row in columns.Rows)
			{
				DatabaseSqlServerTableColumn newColumn = new DatabaseSqlServerTableColumn()
				{
					SchemaName = row["SchemaName"].ToString(),
					ObjectName = row["ObjectName"].ToString(),
					ColumnID = (int)(short)row["ColumnID"],
					ColumnName = row["ColumnName"].ToString(),
					ColumnTypeId = (int)(byte)row["xtypeid"],
					ColumnType = row["xtype"].ToString(),
					MaxLength = (int)(short)row["length"],
					IsNullable = (int)row["isnullable"] != 0,
					IsIdentity = (bool)row["IsIdentity"],
					Precision = (int)(byte)row["xprec"],
					Scale = (int)(byte)row["xscale"],
					DefaultValue = row["DefaultValue"] == DBNull.Value ? string.Empty : row["DefaultValue"].ToString(),
					DefaultConstraintName = row["DefaultConstraintName"] == DBNull.Value ? string.Empty : row["DefaultConstraintName"].ToString(),
					ComputedDefinition = row["ComputedDefinition"] == DBNull.Value ? string.Empty : row["ComputedDefinition"].ToString(),
				};

				while (newColumn.DefaultValue.StartsWith("(") && newColumn.DefaultValue.EndsWith(")"))
					newColumn.DefaultValue = newColumn.DefaultValue.Substring(1, newColumn.DefaultValue.Length - 2);

				while (newColumn.ComputedDefinition.StartsWith("(") && newColumn.ComputedDefinition.EndsWith(")"))
					newColumn.ComputedDefinition = newColumn.ComputedDefinition.Substring(1, newColumn.ComputedDefinition.Length - 2);

				foreach (DataRow pk in pks.Rows)
				{
					if ((string)pk["schemaname"] == newColumn.SchemaName &&
						(string)pk["tablename"] == newColumn.ObjectName &&
						(string)pk["ColumnName"] == newColumn.ColumnName &&
						(bool)pk["IsPrimaryKey"])
					{
						newColumn.IsPrimaryKey = true;
						break;
					}
				}

				ret.Add(newColumn);
			}

			return ret;
		}
	}
}
