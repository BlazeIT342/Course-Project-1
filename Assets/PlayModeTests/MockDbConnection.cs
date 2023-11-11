using System;
using System.Data;

namespace Project.Testing
{
    public class MockDbConnection : IDbConnection
    {
        public string ConnectionString { get; set; }

        // �������� �������� ������ CreateCommand
        public IDbCommand CreateCommand()
        {
            return new MockDbCommand();
        }

        // �������� �������� ������ Open
        public void Open()
        {
            State = ConnectionState.Open;
        }

        // �������� �������� ������ Close
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