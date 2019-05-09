using SqlSchemaComparer.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSchemaComparer.DatabaseObjects
{
    internal class DatabaseSource
    {
        public class ScanProgressEventArgs { public string Message { get; set; } }

        public delegate void DatabaseObjectScanProgressHandler(object sender, ScanProgressEventArgs e);

        public DatabaseObjectScanProgressHandler DatabaseObjectScanProgress;

        public DatabaseConnection ConnectionDetails { get; set; }
        public List<DatabaseObject> DatabaseObjects { get; set; }

        public DatabaseSource(DatabaseConnection connectionDetails)
        {
            ConnectionDetails = connectionDetails;
        }

        public void GetDatabaseObjects()
        {
            if (ConnectionDetails.IsSqlServer)
            {
                if (DatabaseObjectScanProgress != null)
                    DatabaseObjectScanProgress(this, new ScanProgressEventArgs() { Message = string.Format("Scanning {0} on {1}...", ConnectionDetails.Database, ConnectionDetails.Server) });

                DatabaseSqlServer databaseSqlServer = new DatabaseSqlServer(ConnectionDetails);
                databaseSqlServer.ProgressMade += new DatabaseSqlServer.ProgressMadeHandler(SqlServerProgressMade);
                DatabaseObjects = databaseSqlServer.GetDatabaseObjects();

                if (DatabaseObjectScanProgress != null)
                    DatabaseObjectScanProgress(this, new ScanProgressEventArgs() { Message = string.Format("Scan complete") });
            }
            else if (ConnectionDetails.IsFolder)
            {
                if (DatabaseObjectScanProgress != null)
                    DatabaseObjectScanProgress(this, new ScanProgressEventArgs() { Message = string.Format("Scanning folder {0}...", ConnectionDetails.Server) });

                DatabaseFolder databaseFolder = new DatabaseFolder(ConnectionDetails);
                databaseFolder.ScanFolderStarted += new DatabaseFolder.ScanFolderStartedHandler(ScanFolderStarted);
                databaseFolder.ScanFileStarted += new DatabaseFolder.ScanFileStartedHandler(ScanFileStarted);
                DatabaseObjects = databaseFolder.GetDatabaseObjects();

                if (DatabaseObjectScanProgress != null)
                    DatabaseObjectScanProgress(this, new ScanProgressEventArgs() { Message = string.Format("Scan complete") });
            }
            else
            {
                DatabaseObjects = new List<DatabaseObject>();
            }

            DatabaseObjectScanProgress(this, new ScanProgressEventArgs() { Message = string.Format("Checking dependancies...") });
            DatabaseObjects = DatabaseObjects.OrderBy(o => o.Name).OrderBy(o => o.ObjectTypeSort).ToList();

            // Now sort the objects in the order of dependance
            for (int a = 0; a < DatabaseObjects.Count - 1; a++)
            {
                for (int b = a + 1; b < DatabaseObjects.Count - 1; b++)
                {
                    if (DatabaseObjects[b].CreateSQL.DependsOn(DatabaseObjects[b + 1].Name))
                    {
                        DatabaseObject t = DatabaseObjects[b];
                        DatabaseObjects[b] = DatabaseObjects[b+1];
                        DatabaseObjects[b+1] = t;
                    }
                }
            }

            DatabaseObjectScanProgress(this, new ScanProgressEventArgs() { Message = string.Format("Dependancies checked") });
        }

        public void ScanFolderStarted(object sender, DatabaseFolder.ScanFolderEventArgs e)
        {
            if (DatabaseObjectScanProgress != null)
            {
                DatabaseObjectScanProgress(this, new ScanProgressEventArgs() { Message = string.Format("Scanning folder {0}...", e.Folder) });
            }
        }
        public void ScanFileStarted(object sender, DatabaseFolder.ScanFileEventArgs e)
        {
            if (DatabaseObjectScanProgress != null)
            {
                //DatabaseObjectScanProgress(this, new ScanProgressEventArgs() { Message = string.Format("Scanning file {0}...", e.File) });
            }
        }
        public void SqlServerProgressMade(object sender, DatabaseSqlServer.ProgressEventArgs e)
        {
            DatabaseObjectScanProgress(this, new ScanProgressEventArgs() { Message = e.Message });
        }
    }
}
