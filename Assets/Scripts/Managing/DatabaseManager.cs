using Mono.Data.Sqlite;
using Project.Database;
using System.Data;
using UnityEngine;

namespace Project.Managing
{
    /// <summary>
    /// Singleton class responsible for managing the SQLite database connection and providing access to the DatabaseController.
    /// </summary>
    public class DatabaseManager : MonoBehaviour
    {
        /// <summary>
        /// Gets the singleton instance of the DatabaseManager.
        /// </summary>
        public static DatabaseManager Instance { get; private set; }

        /// <summary>
        /// Gets the DatabaseController instance for interacting with the SQLite database.
        /// </summary>
        public DatabaseController DatabaseController { get; private set; }

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            // Singleton pattern to ensure only one instance of DatabaseManager exists
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            // Create and initialize the DatabaseController with an open SQLite database connection
            DatabaseController = new DatabaseController(CreateAndOpenDatabase());

            // Don't destroy this GameObject when loading new scenes
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Creates and opens an SQLite database connection.
        /// </summary>
        /// <returns>The IDbConnection instance representing the open database connection.</returns>
        private IDbConnection CreateAndOpenDatabase()
        {
            // Connection string for the SQLite database file named "ApplicationDatabase.sqlite"
            string dbUri = "URI=file:ApplicationDatabase.sqlite";

            // Create and open the SQLite database connection
            IDbConnection dbConnection = new SqliteConnection(dbUri);
            dbConnection.Open();

            return dbConnection;
        }
    }
}