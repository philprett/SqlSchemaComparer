﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSchemaComparer.AppData
{
    /// <summary>
    /// Provide details to the connection to a database
    /// </summary>
    internal class DatabaseConnection
    {
        /// <summary>
        /// The unique ID of the database connection
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// The name of the database
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The server host of the database
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// The username to connect to the server with.
        /// Leave empty to use the integrated security.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The password to connect to the server with.
        /// Leave empty to use the integrated security.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The database to use on the server.
        /// </summary>
        public string Database { get; set; }

        public DatabaseConnection()
        {
            Id.SetRandom();
            Server = string.Empty;
            Username = string.Empty;
            Password = string.Empty;
            Database = string.Empty;
        }

        public bool IsSqlServer { get { return !IsFolder; } }
        public bool IsFolder { get { return Server.Contains(":") || Server.Contains("\\\\"); } }
    }
}
