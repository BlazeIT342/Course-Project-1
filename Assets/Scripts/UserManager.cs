using Mono.Data.Sqlite;
using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    [SerializeField] TMP_InputField username;
    [SerializeField] TMP_InputField password;

    private IDbConnection dbConnection;

    void Start()
    {
        dbConnection = CreateAndOpenDatabase();
        InitializeDatabase();
    }

    void OnDestroy()
    {
        dbConnection.Close();
    }

    private void InitializeDatabase()
    {
        IDbCommand dbCommandCreateTable = dbConnection.CreateCommand();
        dbCommandCreateTable.CommandText = "CREATE TABLE IF NOT EXISTS Users (id INTEGER PRIMARY KEY, username TEXT, password TEXT, administrator INTEGER DEFAULT 0, completedLessons TEXT DEFAULT '', progress INTEGER DEFAULT 0)";
        dbCommandCreateTable.ExecuteNonQuery();
    }

    public bool RegisterUser()
    {
        if (!IsUsernameValid(username.text))
        {
            Debug.Log("Invalid username. It should consist of 5-20 alphanumeric characters.");
            return false;
        }

        if (!IsPasswordValid(password.text))
        {
            Debug.Log("Invalid password. It should be 4-20 characters long.");
            return false;
        }

        IDbCommand dbCommandCheckUser = dbConnection.CreateCommand();
        dbCommandCheckUser.CommandText = "SELECT COUNT(*) FROM Users WHERE username=@username";
        dbCommandCheckUser.Parameters.Add(new SqliteParameter("@username", username.text));

        int count = Convert.ToInt32(dbCommandCheckUser.ExecuteScalar());

        if (count > 0)
        {
            Debug.Log("User already exists!");
            return false;
        }
        else
        {
            Debug.Log("Registration successful");
        }

        IDbCommand dbCommandInsertUser = dbConnection.CreateCommand();
        dbCommandInsertUser.CommandText = "INSERT INTO Users (username, password) VALUES (@username, @password)";
        dbCommandInsertUser.Parameters.Add(new SqliteParameter("@username", username.text));
        dbCommandInsertUser.Parameters.Add(new SqliteParameter("@password", password.text));
        dbCommandInsertUser.ExecuteNonQuery();

        return true;
    }

    public bool LoginUser()
    {
        if (!IsUsernameValid(username.text))
        {
            Debug.Log("Invalid username. It should consist of 5-20 alphanumeric characters.");
            return false;
        }

        if (!IsPasswordValid(password.text))
        {
            Debug.Log("Invalid password. It should be 4-20 characters long.");
            return false;
        }

        IDbCommand dbCommandCheckUser = dbConnection.CreateCommand();
        dbCommandCheckUser.CommandText = "SELECT COUNT(*) FROM Users WHERE username=@username AND password=@password";
        dbCommandCheckUser.Parameters.Add(new SqliteParameter("@username", username.text));
        dbCommandCheckUser.Parameters.Add(new SqliteParameter("@password", password.text));

        int count = Convert.ToInt32(dbCommandCheckUser.ExecuteScalar());

        if (count > 0)
        {
            Debug.Log("Login successful!");
            return true;
        }
        else
        {
            Debug.Log("Invalid username or password.");
            return false;
        }
    }

    private bool IsUsernameValid(string username)
    {
        string pattern = "^[\\S]{5,20}$";
        return Regex.IsMatch(username, pattern);
    }

    private bool IsPasswordValid(string password)
    {
        string pattern = "^[\\S]{4,20}$";
        return Regex.IsMatch(password, pattern);
    }


    // ... Другие методы взаимодействия с базой данных ...

    private IDbConnection CreateAndOpenDatabase()
    {
        string dbUri = "URI=file:Assets/Resources/Database/ApplicationDatabase.sqlite";
        IDbConnection dbConnection = new SqliteConnection(dbUri);
        dbConnection.Open();

        return dbConnection;
    }

    public void ClearUserData()
    {
        IDbCommand dbCommandClearData = dbConnection.CreateCommand();
        dbCommandClearData.CommandText = "DELETE FROM Users";
        dbCommandClearData.ExecuteNonQuery();
        Debug.Log("All user data cleared.");
    }
}