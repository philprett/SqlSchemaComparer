using SqlSchemaComparer.AppData;
using System;
using System.Collections.Generic;
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
            "      AND NOT (o.name = 'dtproperties' AND LTRIM(RTRIM(o.type)) = 'U') " +
            "      AND NOT (o.name = 'sysdiagrams' AND LTRIM(RTRIM(o.type)) = 'U') " +
            "      AND NOT (o.name LIKE 'fn_%' AND LTRIM(RTRIM(o.type)) = 'FN') " +
            "      AND NOT (o.name LIKE 'sp_%' AND LTRIM(RTRIM(o.type)) = 'P') " +
            "      AND o.name LIKE '{0}' " +
            "ORDER BY CASE WHEN LTRIM(RTRIM(o.type)) = 'U' THEN 1 WHEN LTRIM(RTRIM(o.type)) = 'V' THEN 2 ELSE 3 END ASC, o.name ASC, c.number ASC, c.colid ASC";
        private const string SQL_GET_TABLE_COLUMNS =
            "SELECT		USER_NAME(o.uid) AS SchemaName, o.name as ObjectName, " +
            "           col.name AS ColumnName, col.xtype as xtypeid, typ.name as xtype, col.length, " +
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
            "WHERE		col.id = o.id AND typ.status != 1 AND o.name LIKE '{0}' " +
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

        public DatabaseConnection DatabaseConnection { get; set; }

        public DatabaseSqlServer(DatabaseConnection databaseConnection)
        {
            DatabaseConnection = databaseConnection;
        }

        public List<DatabaseObject> GetDatabaseObjects()
        {
            return new List<DatabaseObject>();
        }
    }
}
