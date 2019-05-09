using SqlSchemaComparer.AppData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSchemaComparer.DatabaseObjects
{
    internal class DatabaseSqlServer
    {
        #region SQL Statements
        private const string SQL_GET_DATABASE_OBJECTS =
            "SELECT USER_NAME(o.uid) AS uid, o.name as name, c.text as objsql, LTRIM(RTRIM(o.type)) as type " +
            "FROM sysobjects o " +
            "LEFT JOIN sys.objects o2 ON o2.object_id = o.id " +
            "LEFT JOIN syscomments c " +
            "ON c.id = o.id " +
            "WHERE o2.is_ms_shipped = 0 " +
            "      AND RTRIM(o.type) IN ('U', 'V', 'P', 'TF', 'FN') " +
            "      AND NOT (o.name = 'dtproperties' AND LTRIM(RTRIM(o.type)) = 'U') " +
            "      AND NOT (o.name = 'sysdiagrams' AND LTRIM(RTRIM(o.type)) = 'U') " +
            "      AND NOT (o.name LIKE 'fn_%' AND LTRIM(RTRIM(o.type)) = 'FN') " +
            "      AND NOT (o.name LIKE 'sp_%' AND LTRIM(RTRIM(o.type)) = 'P') " +
            "ORDER BY CASE WHEN LTRIM(RTRIM(o.type)) = 'U' THEN 1 WHEN LTRIM(RTRIM(o.type)) = 'V' THEN 2 ELSE 3 END ASC, o.name ASC, c.number ASC, c.colid ASC";
        private const string SQL_GET_TABLE_COLUMNS =
            "SELECT		USER_NAME(o.uid) AS SchemaName, o.name as ObjectName, " +
            "           col.colid AS ColumnID, col.name AS ColumnName, col.xtype as xtypeid, typ.name as xtype, col.length, " +
            "           col.isnullable, col2.is_identity as IsIdentity, " +
            "           col.xprec, col.xscale, ISNULL(dc.definition, '') as DefaultValue, dc.name AS DefaultConstraintName, " +
            "           cc.definition AS ComputedDefinition " +
            "FROM		syscolumns col " +
            "INNER JOIN sysobjects o ON o.id = col.id " +
            "INNER JOIN	sys.columns col2 ON col2.object_id = col.id AND col2.column_id = col.colid " +
            "INNER JOIN	systypes typ ON col.xtype = typ.xtype " +
            "LEFT JOIN  sys.default_constraints def ON def.object_id = 1 " +
            "LEFT JOIN  sys.default_constraints dc on dc.object_id = col2.default_object_id " +
            "LEFT JOIN  sys.computed_columns cc on cc.object_id = col2.object_id and cc.column_id = col2.column_id " +
            "WHERE		col.id = o.id AND typ.status != 1 " +
            "ORDER BY	USER_NAME(o.uid), o.name, col.colorder ASC";
        private const string SQL_GET_KEY_CONSTRAINTS =
            "select     k.name as ConstraintName, i.is_unique as IsUnique, i.is_primary_key as IsPrimaryKey, ic.is_descending_key as IsDescending, c.name as ColumnName, o.name as tablename, s.name as schemaname  " +
            "from       sys.key_constraints k " +
            "inner join sys.objects o on o.object_id = k.parent_object_id " +
            "inner join sys.schemas s on o.schema_id = s.schema_id " +
            "inner join sys.indexes i on i.object_id = k.parent_object_id " +
            "inner join sys.index_columns ic on ic.object_id = i.object_id and ic.index_id = i.index_id " +
            "inner join sys.columns c on c.object_id = i.object_id and c.column_id = ic.column_id " +
            "where      i.is_primary_key = 1 " +
            "order by   s.name, o.name, c.name";
        private const string SQL_GET_FOREIGNKEYS =
            "SELECT      o.name AS FKName, p.name AS TableName, c.name AS FieldName, ro.name AS ReferencesTableName, rc.name AS ReferencedFieldName, fk.delete_referential_action " +
            "FROM        sys.foreign_keys fk " +
            "INNER JOIN  sys.objects o                ON o.object_id = fk.object_id " +
            "INNER JOIN  sys.objects p                ON p.object_id = fk.parent_object_id " +
            "INNER JOIN  sys.foreign_key_columns fkc  ON fkc.constraint_object_id = fk.object_id and fkc.parent_object_id = fk.parent_object_id " +
            "INNER JOIN  sys.columns c                ON c.object_id = fk.parent_object_id AND c.column_id = fkc.parent_column_id " +
            "INNER JOIN  sys.objects ro               ON ro.object_id = fkc.referenced_object_id " +
            "INNER JOIN  sys.columns rc               ON rc.object_id = fkc.referenced_object_id AND rc.column_id = fkc.referenced_column_id " +
            "WHERE       fk.type = 'F' " +
            "ORDER BY    p.name, c.name ";
        private const string SQL_GET_INDEXES =
            "select     distinct i.name as IndexName, c.name as ColumnName, o.name as tablename " +
            "from       sys.indexes i " +
            "inner join sys.index_columns ic on ic.object_id = i.object_id and ic.index_id = i.index_id " +
            "inner join sys.objects o on o.object_id = i.object_id and o.is_ms_shipped = 0 " +
            "inner join sys.columns c on c.object_id = o.object_id and c.column_id = ic.column_id " +
            "where      i.is_primary_key = 0 " +
            "order by   o.name, i.name, c.name";
        private const string SQL_GET_PERMISSIONS =
            "select o.name, " +
            "       (p.state_desc COLLATE Latin1_General_CI_AS) + ' ' + " +
            "       (p.permission_name COLLATE Latin1_General_CI_AS) + ' ON ' + " +
            "       '['+(o.name COLLATE Latin1_General_CI_AS)+']' + ' TO ' + " +
            "       '['+(u.name COLLATE Latin1_General_CI_AS)+']'" +
            "from sys.database_permissions p " +
            "inner join sys.objects o ON o.object_id = p.major_id " +
            "inner join sys.sysusers u ON u.uid = p.grantee_principal_id " +
            "where p.class_desc = 'OBJECT_OR_COLUMN' AND p.grantee_principal_id != 0 " +
            "order by o.name, u.name, p.state_desc, p.permission_name";
        #endregion SQL Statements

        public class ProgressEventArgs { public string Message { get; set; } }
        public delegate void ProgressMadeHandler(object sender, ProgressEventArgs e);
        public ProgressMadeHandler ProgressMade;


        public DatabaseConnection DatabaseConnection { get; set; }

        public DatabaseSqlServer(DatabaseConnection databaseConnection)
        {
            ProgressMade = null;
            DatabaseConnection = databaseConnection;
        }

        private void ReportProgress(string message, params object[] args)
        {
            if (ProgressMade != null) ProgressMade(this, new ProgressEventArgs() { Message = string.Format(message, args) });
        }

        public List<DatabaseObject> GetDatabaseObjects()
        {
            List<DatabaseObject> ret = new List<DatabaseObject>();

            using (SqlConnection sql = new SqlConnection(DatabaseConnection.ConnectionString))
            {
                ReportProgress("Connecting to database {0} on {1}", DatabaseConnection.Database, DatabaseConnection.Server);
                try
                {
                    sql.Open();
                    ReportProgress("Retrieving database objects from {0} on {1}...", DatabaseConnection.Database, DatabaseConnection.Server);

                    DataTable rsObjects = new DataTable();
                    using (SqlCommand com = sql.CreateCommand())
                    {
                        // USER_NAME(o.uid) AS uid, o.name as name, c.text as objsql, LTRIM(RTRIM(o.type)) as type
                        com.CommandText = SQL_GET_DATABASE_OBJECTS;
                        using (SqlDataAdapter da = new SqlDataAdapter(com)) da.Fill(rsObjects);
                    }

                    DataTable rsTableColumns = new DataTable();
                    using (SqlCommand com = sql.CreateCommand())
                    {
                        // SchemaName, ObjectName, ColumnID, ColumnName, xtypeid, xtype, length, isnullable, IsIdentity, xprec, xscale, 
                        // DefaultValue, DefaultConstraintName, ComputedDefinition
                        com.CommandText = SQL_GET_TABLE_COLUMNS;
                        using (SqlDataAdapter da = new SqlDataAdapter(com)) da.Fill(rsTableColumns);
                    }

                    DataTable rsTablesPKs = new DataTable();
                    using (SqlCommand com = sql.CreateCommand())
                    {
                        // ConstraintName, IsUnique, IsPrimaryKey, IsDescending, ColumnName, tablename, schemaname 
                        com.CommandText = SQL_GET_KEY_CONSTRAINTS;
                        using (SqlDataAdapter da = new SqlDataAdapter(com)) da.Fill(rsTablesPKs);
                    }

                    DataTable rsIndexes = new DataTable();
                    using (SqlCommand com = sql.CreateCommand())
                    {
                        // IndexName, ColumnName, tablename
                        com.CommandText = SQL_GET_INDEXES;
                        using (SqlDataAdapter da = new SqlDataAdapter(com)) da.Fill(rsIndexes);
                    }

                    List<DatabaseSqlServerTableColumn> columns = DatabaseSqlServerTableColumn.GetFromDataTable(rsTableColumns, rsTablesPKs, rsIndexes);

                    foreach (DataRow row in rsObjects.Rows)
                    {
                        string objSchema = row["uid"].ToString();
                        string objName = row["name"].ToString();
                        string objSql = row["objsql"].ToString();
                        string objType = row["type"].ToString();

                        DatabaseObject existing = ret
                            .FirstOrDefault(o =>
                                o.Schema.Equals(objSchema, StringComparison.CurrentCultureIgnoreCase) &&
                                o.Name.Equals(objName, StringComparison.CurrentCultureIgnoreCase));

                        if (existing != null)
                        {
                            existing.CreateSQL.Append(objSql);
                        }
                        else
                        {
                            if (objType == "U")
                            {
                                objSql = CreateTableSQL(objSchema, objName, columns);
                            }
                            ret.Add(new DatabaseObject() { Schema = objSchema, Name = objName, ObjectType = objType, CreateSQL = new SqlString(objSql) });
                        }
                    }
                    sql.Close();
                    ReportProgress("Retrieved all objects from database {0} on {1}", DatabaseConnection.Database, DatabaseConnection.Server);
                    return ret;
                }
                catch (Exception ex)
                {
                    ReportProgress("Could not connect to database {0} on {1}: {2}", DatabaseConnection.Database, DatabaseConnection.Server, ex.Message);
                    throw ex;
                }
            }
        }
        private string CreateTableSQL(string schema, string name, List<DatabaseSqlServerTableColumn> allColumns)
        {
            List<DatabaseSqlServerTableColumn> columns = allColumns
                .Where(c => c.SchemaName == schema && c.ObjectName == name)
                .OrderBy(c => c.ColumnID)
                .ToList();

            List<string> rawColumnTypes = columns.Select(c => c.GetDefinitionType().ToUpper()).ToList();

            int maxColumnName = columns.Max(c => c.ColumnName.Length);
            int maxColumnType = rawColumnTypes.Max(c => c.Length);

            List<string> columnNames = columns.Select(c => c.ColumnName + new string(' ', maxColumnName - c.ColumnName.Length)).ToList();
            List<string> columnTypes = rawColumnTypes.Select(c => c + new string(' ', maxColumnType - c.Length)).ToList();

            StringBuilder sql = new StringBuilder();

            sql.Append(string.Format("CREATE TABLE {0}.{1}\r\n", schema, name));
            sql.Append(string.Format("(\r\n"));

            // SchemaName, ObjectName, ColumnName, xtypeid, xtype, length, isnullable, IsIdentity, xprec, xscale, 
            // DefaultValue, DefaultConstraintName, ComputedDefinition
            for (int c = 0; c < columns.Count; c++)
            {
                DatabaseSqlServerTableColumn column = columns[c];

                if (!string.IsNullOrWhiteSpace(column.ComputedDefinition))
                {
                    sql.Append(string.Format(
                        "    {0} AS {1}{2}\r\n",
                        columnNames[c],
                        column.ComputedDefinition,
                        c == columns.Count - 1 ? "" : ","));

                }
                else
                {
                    string colDef = column.GetDefinitionType();
                    sql.Append(string.Format(
                        "    {0} {1} {2}{3}{4}{5}\r\n",
                        columnNames[c],
                        columnTypes[c],
                        column.IsNullable ? "    NULL" : "NOT NULL",
                        string.IsNullOrWhiteSpace(column.DefaultValue) ? "" : " DEFAULT " + column.DefaultValue,
                        column.IsPrimaryKey ? " PRIMARY KEY" : "",
                        c == columns.Count - 1 ? "" : ","));
                }
            }

            sql.Append(string.Format(")\r\n"));

            List<DatabaseSqlServerTableColumn> indexColumns = columns.Where(c => !string.IsNullOrWhiteSpace(c.IndexName)).ToList();
            if (indexColumns.Count > 0)
            {
                sql.Append("GO\r\n\r\n");

                foreach (DatabaseSqlServerTableColumn column in indexColumns.OrderBy(c => c.ColumnName).OrderBy(c => c.IndexName))
                {
                    sql.Append(string.Format("CREATE INDEX {0} ON {1}.{2}({3})\r\nGO\r\n", column.IndexName, schema, name, column.ColumnName));
                }
            }

            return sql.ToString();
        }
    }
}