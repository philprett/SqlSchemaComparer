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
    }
}
