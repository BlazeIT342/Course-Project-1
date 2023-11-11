using Mono.Data.Sqlite;
using System;
using System.Data;

namespace Project.Testing
{
    public class MockDbConnection : IDbConnection
    {
        public string ConnectionString { get; set; }

        public IDbConnection CreateAndOpenDatabase()
        {
            string dbUri = "URI=file:ApplicationDatabase.sqlite";
            IDbConnection dbConnection = new SqliteConnection(dbUri);
            dbConnection.Open();

            return dbConnection;
        }

        // Имитация базового метода CreateCommand
        public IDbCommand CreateCommand()
        {
            return new MockDbCommand();
        }

        // Имитация базового метода Open
        public void Open()
        {
            State = ConnectionState.Open;
        }

        // Имитация базового метода Close
        public void Close()
        {
            State = ConnectionState.Closed;
        }

        public int ConnectionTimeout { get; }
        public string Database { get; }
        public ConnectionState State { get; private set; }

        public IDbTransaction BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            throw new NotImplementedException();
        }

        public void ChangeDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}