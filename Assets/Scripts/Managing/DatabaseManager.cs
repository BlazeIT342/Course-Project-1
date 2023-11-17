using Mono.Data.Sqlite;
using Project.Database;
using System.Data;
using UnityEngine;

namespace Project.Managing
{
    public class DatabaseManager : MonoBehaviour
    {
        public static DatabaseManager Instance;

        public DatabaseController DatabaseController { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            DatabaseController = new DatabaseController(CreateAndOpenDatabase());

            DontDestroyOnLoad(gameObject);
        }

        private IDbConnection CreateAndOpenDatabase()
        {
            string dbUri = "URI=file:ApplicationDatabase.sqlite";
            IDbConnection dbConnection = new SqliteConnection(dbUri);
            dbConnection.Open();

            return dbConnection;
        }
    }
}