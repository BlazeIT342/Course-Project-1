using Mono.Data.Sqlite;
using System.Data;
using UnityEngine;

namespace Project.Managing
{
    public class GameManager : MonoBehaviour
    {
        public UserManager UserManager { get; private set; }

        private void OnEnable()
        {
            UserManager = new UserManager(CreateAndOpenDatabase());
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