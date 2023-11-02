using Mono.Data.Sqlite;
using System;
using System.Data;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserManager : MonoBehaviour
{
    public int CurrentUserId;
    public string Username;
    public string Password;
    public string Role;

    [SerializeField] private TMP_InputField _usernameField;
    [SerializeField] private TMP_InputField _passwordField;
    [SerializeField] private Toggle AdministratorToggle;

    private IDbConnection dbConnection;

    private void Start()
    {
        dbConnection = CreateAndOpenDatabase();
        InitializeDatabase();
    }

    private void OnDestroy()
    {
        dbConnection.Close();
    }

    private void InitializeDatabase()
    {
        IDbCommand dbCommandCreateTable = dbConnection.CreateCommand();
        dbCommandCreateTable.CommandText = "CREATE TABLE IF NOT EXISTS Users (id INTEGER PRIMARY KEY, username TEXT, password TEXT, administrator INTEGER DEFAULT 0)";
        dbCommandCreateTable.ExecuteNonQuery();
    }

    public bool RegisterUser()
    {
        if (!IsUsernameValid(_usernameField.text))
        {
            Debug.Log("Invalid _usernameField. It should consist of 5-20 alphanumeric characters.");
            return false;
        }

        if (!IsPasswordValid(_passwordField.text))
        {
            Debug.Log("Invalid _passwordField. It should be 4-20 characters long.");
            return false;
        }

        IDbCommand dbCommandCheckUser = dbConnection.CreateCommand();
        dbCommandCheckUser.CommandText = "SELECT COUNT(*) FROM Users WHERE username=@username";
        dbCommandCheckUser.Parameters.Add(new SqliteParameter("@username", _usernameField.text));

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
        dbCommandInsertUser.CommandText = "INSERT INTO Users (username, password, administrator) VALUES (@username, @password, @administrator)";
        dbCommandInsertUser.Parameters.Add(new SqliteParameter("@username", _usernameField.text));
        dbCommandInsertUser.Parameters.Add(new SqliteParameter("@password", _passwordField.text));
        dbCommandInsertUser.Parameters.Add(new SqliteParameter("@administrator", AdministratorToggle.isOn ? 1 : 0));
        dbCommandInsertUser.ExecuteNonQuery();

        return true;
    }

    public bool LoginUser()
    {
        if (!IsUsernameValid(_usernameField.text))
        {
            Debug.Log("Invalid Username. It should consist of 5-20 alphanumeric characters.");
            return false;
        }

        if (!IsPasswordValid(_passwordField.text))
        {
            Debug.Log("Invalid Password. It should be 4-20 characters long.");
            return false;
        }

        IDbCommand dbCommandCheckUser = dbConnection.CreateCommand();
        dbCommandCheckUser.CommandText = "SELECT COUNT(*) FROM Users WHERE username=@username AND password=@password";
        dbCommandCheckUser.Parameters.Add(new SqliteParameter("@username", _usernameField.text));
        dbCommandCheckUser.Parameters.Add(new SqliteParameter("@password", _passwordField.text));

        int count = Convert.ToInt32(dbCommandCheckUser.ExecuteScalar());

        if (count > 0)
        {
            LoadUserData(_usernameField.text);
            Debug.Log("Login successful! " + CurrentUserId);
            return true;
        }
        else
        {
            Debug.Log("Invalid Username or Password.");
            return false;
        }
    }

    public int GetUserIdByUsername(string searchUsername)
    {
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = "SELECT id FROM Users WHERE username=@username";
        dbCommand.Parameters.Add(new SqliteParameter("@username", searchUsername));

        object result = dbCommand.ExecuteScalar();

        if (result != null)
        {
            int userId = Convert.ToInt32(result);
            return userId;
        }
        else
        {
            return -1;
        }
    }

    public void LoadUserData(string username)
    {
        var id = GetUserIdByUsername(username);
        if (username != null)
        {
            IDbCommand dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = "SELECT * FROM Users WHERE id=@UserId";
            dbCommand.Parameters.Add(new SqliteParameter("@UserId", id));

            IDataReader reader = dbCommand.ExecuteReader();

            if (reader.Read())
            {
                CurrentUserId = reader.GetInt32(0);
                Username = reader.GetString(1);
                Password = reader.GetString(2);
                Role = reader.GetInt32(3) == 1 ? "Administrator" : "User";
            }

            reader.Close();
        }
        else
        {
            Debug.Log("ѕользователь с таким именем не найден");
        }
    }

    public void UpdatePassword(int userId, string newPassword)
    {
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = "UPDATE Users SET password=@NewPassword WHERE id=@UserId";
        dbCommand.Parameters.Add(new SqliteParameter("@NewPassword", newPassword));
        dbCommand.Parameters.Add(new SqliteParameter("@UserId", userId));

        dbCommand.ExecuteNonQuery();

        LoadUserData(Username);
    }

    // ... ƒругие методы взаимодействи€ с базой данных ...

    private IDbConnection CreateAndOpenDatabase()
    {
        string dbUri = "URI=file:Assets/Resources/Database/ApplicationDatabase.sqlite";
        IDbConnection dbConnection = new SqliteConnection(dbUri);
        dbConnection.Open();

        return dbConnection;
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


    public void ClearUserData()
    {
        IDbCommand dbCommandClearData = dbConnection.CreateCommand();
        dbCommandClearData.CommandText = "DELETE FROM Users";
        dbCommandClearData.ExecuteNonQuery();
        Debug.Log("All user data cleared.");
    }
}