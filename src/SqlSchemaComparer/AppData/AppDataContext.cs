using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSchemaComparer.AppData
{
    /// <summary>
    /// The main class to provide access to the AppData database
    /// </summary>
    internal class AppDataContext : DbContext
    {
        /// <summary>
        /// This is our single instance of this class.
        /// </summary>
        private static AppDataContext singleton;

        /// <summary>
        /// Provides a connection to the database file.
        /// </summary>
        private static System.Data.SQLite.SQLiteConnection Connection
        {
            get
            {
                string exeFilename = System.Reflection.Assembly.GetExecutingAssembly().Location;

                string filename = 
                        System.IO.Path.Combine(
                            System.IO.Path.GetDirectoryName(exeFilename),
                            "AppData.db");

                return
                    new System.Data.SQLite.SQLiteConnection(
                        string.Format(
                            "Data Source={0}",
                            filename));
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private AppDataContext() : base(Connection, true)
        {
            Database.SetInitializer<AppDataContext>(new CreateDatabaseIfNotExists<AppDataContext>());
        }

        /// <summary>
        /// Get the instance of the AppDataContext
        /// </summary>
        public static AppDataContext DB
        {
            get
            {
                if (singleton == null)
                {
                    singleton = new AppDataContext();
                    singleton.InitializeTables();
                }
                return singleton;
            }
        }

        /// <summary>
        /// Setup the modelbuilder.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Makes sure that the tables exist in the database file.
        /// </summary>
        public void InitializeTables()
        {
            Database.ExecuteSqlCommand(
                "CREATE TABLE IF NOT EXISTS DatabaseConnection " +
                "(" +
                "Id INTEGER NOT NULL PRIMARY KEY, " +
                "Name TEXT NOT NULL, " +
                "Server TEXT NOT NULL, " +
                "Username TEXT NOT NULL, " +
                "Password TEXT NOT NULL, " +
                "Database TEXT NOT NULL" +
                ")");

            Database.ExecuteSqlCommand(
                "CREATE TABLE IF NOT EXISTS SavedValue " +
                "(" +
                "Id INTEGER NOT NULL PRIMARY KEY, " +
                "Name TEXT NOT NULL, " +
                "Value TEXT NOT NULL " +
                ")");

        }

        public DbSet<DatabaseConnection> DatabaseConnections { get; set; }
        public DbSet<SavedValue> SavedValues { get; set; }

    }
}
