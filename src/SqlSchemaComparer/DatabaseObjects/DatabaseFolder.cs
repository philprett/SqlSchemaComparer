using SqlSchemaComparer.AppData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSchemaComparer.DatabaseObjects
{
    internal class DatabaseFolder
    {
        public class ScanFolderEventArgs { public string Folder { get; set; } }
        public class ScanFileEventArgs { public string File { get; set; } }

        public delegate void ScanFolderStartedHandler(object sender, ScanFolderEventArgs e);
        public delegate void ScanFileStartedHandler(object sender, ScanFileEventArgs e);

        public ScanFolderStartedHandler ScanFolderStarted;
        public ScanFileStartedHandler ScanFileStarted;

        public DatabaseConnection DatabaseConnection { get; set; }
        private List<DatabaseObject> databaseObjects;
        public DatabaseFolder(DatabaseConnection databaseConnection)
        {
            DatabaseConnection = databaseConnection;
            databaseObjects = new List<DatabaseObject>();
            ScanFolderStarted = null;
            ScanFileStarted = null;
        }

        public List<DatabaseObject> GetDatabaseObjects()
        {
            databaseObjects = new List<DatabaseObject>();
            ScanFolder(DatabaseConnection.Server);
            return databaseObjects;
        }

        private void ScanFolder(string folder)
        {
            DirectoryInfo dir = new DirectoryInfo(folder);
            if (ScanFolderStarted != null) ScanFolderStarted(this, new ScanFolderEventArgs() { Folder = dir.FullName });

            foreach (FileInfo file in dir.GetFiles("*.sql"))
            {
                if (!file.Name.EndsWith(".sql")) continue;
                if (ScanFileStarted != null) ScanFileStarted(this, new ScanFileEventArgs() { File = file.FullName });

                DatabaseFile databaseFile = new DatabaseFile(file.FullName);
                DatabaseObject databaseObject = new DatabaseObject
                {
                    Schema = string.Empty,
                    Name = file.Name,
                    ObjectType = databaseFile.ObjectType,
                    CreateSQL = databaseFile.Contents
                };
                if (!string.IsNullOrWhiteSpace(databaseObject.ObjectType))
                {
                    databaseObjects.Add(databaseObject);
                }
            }
            foreach (DirectoryInfo sub in dir.GetDirectories())
            {
                if (sub.Name != ".vs" && sub.Name != "bin" && sub.Name != "obj")
                {
                    ScanFolder(sub.FullName);
                }
            }
        }
    }
}
