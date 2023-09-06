using System;

namespace RadinProjectNotesCommon.EncryptedDatabaseSerializer
{
    /// <summary>
    /// Throw when a data structure could not be saved to a file.
    /// </summary>
    public class CouldNotSaveDatabase : Exception
    {
        /// <summary>
        /// Database structure could not be saved to a file.
        /// </summary>
        public CouldNotSaveDatabase() : base("Database could not be saved.")
        { }
    }

    /// <summary>
    /// Throw when a data structure could not be loaded from a file.
    /// </summary>
    public class CouldNotLoadDatabase : Exception
    {
        /// <summary>
        /// Database structure could not be loaded from a file.
        /// </summary>
        public CouldNotLoadDatabase() : base("Database could not be loaded.")
        { }
    }

    /// <summary>
    /// Throw when a database file could not be found.
    /// </summary>
    public class DatabaseFileNotFound : Exception
    {
        /// <summary>
        /// Database file not found.
        /// </summary>
        public DatabaseFileNotFound() : base("Database file not found.")
        { }
    }
}
