using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserManager : MonoBehaviour
{
    public UserData CurrentUserData;

    [SerializeField] private TMP_InputField _usernameField;
    [SerializeField] private TMP_InputField _passwordField;
    [SerializeField] private Toggle AdministratorToggle;
    [SerializeField] private TextMeshProUGUI textMessage;

    private IDbConnection dbConnection;

    private void OnEnable()
    {
        dbConnection = CreateAndOpenDatabase();
        InitializeDatabase();
    }

    private void OnDestroy()
    {
        dbConnection.Close();
    }

    public bool RegisterUser()
    {
        if (!IsUsernameValid(_usernameField.text))
        {
            ShowMessage("Invalid username. It should consist of 5-20 alphanumeric characters.");
            return false;
        }

        if (!IsPasswordValid(_passwordField.text))
        {
            ShowMessage("Invalid password. It should be 4-20 characters long.");
            return false;
        }

        IDbCommand dbCommandCheckUser = dbConnection.CreateCommand();
        dbCommandCheckUser.CommandText = "SELECT COUNT(*) FROM Users WHERE username=@username";
        dbCommandCheckUser.Parameters.Add(new SqliteParameter("@username", _usernameField.text));

        int count = Convert.ToInt32(dbCommandCheckUser.ExecuteScalar());

        if (count > 0)
        {
            ShowMessage("User already exists!");
            return false;
        }
        else
        {
            ShowMessage("Registration successful");
        }

        IDbCommand dbCommandInsertUser = dbConnection.CreateCommand();
        dbCommandInsertUser.CommandText = "INSERT INTO Users (username, password, role) VALUES (@username, @password, @role)";
        dbCommandInsertUser.Parameters.Add(new SqliteParameter("@username", _usernameField.text));
        dbCommandInsertUser.Parameters.Add(new SqliteParameter("@password", _passwordField.text));
        dbCommandInsertUser.Parameters.Add(new SqliteParameter("@role", AdministratorToggle.isOn ? "Administrator" : "User"));
        dbCommandInsertUser.ExecuteNonQuery();

        return true;
    }

    public bool LoginUser()
    {
        if (!IsUsernameValid(_usernameField.text))
        {
            ShowMessage("Invalid Username. It should consist of 5-20 alphanumeric characters.");
            return false;
        }

        if (!IsPasswordValid(_passwordField.text))
        {
            ShowMessage("Invalid Password. It should be 4-20 characters long.");
            return false;
        }

        IDbCommand dbCommandCheckUser = dbConnection.CreateCommand();
        dbCommandCheckUser.CommandText = "SELECT COUNT(*) FROM Users WHERE username=@username AND password=@password";
        dbCommandCheckUser.Parameters.Add(new SqliteParameter("@username", _usernameField.text));
        dbCommandCheckUser.Parameters.Add(new SqliteParameter("@password", _passwordField.text));

        int count = Convert.ToInt32(dbCommandCheckUser.ExecuteScalar());

        if (count > 0)
        {
            CurrentUserData = GetUserDataByUsername(_usernameField.text);
            ShowMessage("Login successful! " + CurrentUserData.Id);
            return true;
        }
        else
        {
            ShowMessage("Invalid Username or Password.");
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

    public UserData GetUserDataByUsername(string username)
    {
        if (username != null)
        {
            IDbCommand dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = "SELECT * FROM Users WHERE username=@Username";
            dbCommand.Parameters.Add(new SqliteParameter("@Username", username));

            IDataReader reader = dbCommand.ExecuteReader();

            if (reader.Read())
            {
                UserData user = new()
                {
                    Id = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    Password = reader.GetString(2),
                    Role = reader.GetString(3),
                    Record = reader.GetInt32(4),
                };

                reader.Close();

                return user;
            }
        }

        ShowMessage("User with this username not found");
        return new UserData();
    }

    /// <summary>
    /// Allowed Parameters: "Id", "Username", "Password", "Role", "Record"
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public List<UserData> GetAllUsersSortedByParameter(ColumnType type)
    {
        var usersList = new List<UserData>();
        IDbCommand dbCommand = dbConnection.CreateCommand();

        if (type == ColumnType.Record)
        {
            dbCommand.CommandText = $"SELECT * FROM Users ORDER BY {type} DESC";
        }
        else
        {
            dbCommand.CommandText = $"SELECT * FROM Users ORDER BY {type}";
        }

        IDataReader reader = dbCommand.ExecuteReader();

        while (reader.Read())
        {
            UserData user = new()
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                Password = reader.GetString(2),
                Role = reader.GetString(3),
                Record = reader.GetInt32(4),
            };

            usersList.Add(user);
        }

        reader.Close();

        return usersList;
    }

    public bool UpdatePassword(string newPassword)
    {
        if (!IsPasswordValid(newPassword))
        {
            ShowMessage("Invalid password. It should be 4-20 characters long.");
            return false;
        }

        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = "UPDATE Users SET password=@NewPassword WHERE id=@UserId";
        dbCommand.Parameters.Add(new SqliteParameter("@NewPassword", newPassword));
        dbCommand.Parameters.Add(new SqliteParameter("@UserId", CurrentUserData.Id));

        dbCommand.ExecuteNonQuery();

        CurrentUserData.Password = newPassword;
        ShowMessage("Successful password change! Your new password: " + newPassword);
        return true;
    }

    public void ClearUserData()
    {
        IDbCommand dbCommandClearData = dbConnection.CreateCommand();
        dbCommandClearData.CommandText = "DELETE FROM Users";
        dbCommandClearData.ExecuteNonQuery();
        ShowMessage("All user data cleared.");
    }

    // ... Другие методы взаимодействия с базой данных ...

    private void InitializeDatabase()
    {
        IDbCommand dbCommandCreateTable = dbConnection.CreateCommand();
        dbCommandCreateTable.CommandText = "CREATE TABLE IF NOT EXISTS Users (id INTEGER PRIMARY KEY, username TEXT, password TEXT, role TEXT DEFAULT User, record INTEGER DEFAULT 0)";
        dbCommandCreateTable.ExecuteNonQuery();
    }

    private IDbConnection CreateAndOpenDatabase()
    {
        string dbUri = "URI=file:ApplicationDatabase.sqlite";
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

    private void ShowMessage(string text)
    {
        textMessage.text = text;
        textMessage.GetComponent<Animation>().Stop();
        textMessage.GetComponent<Animation>().Play();
    }
}