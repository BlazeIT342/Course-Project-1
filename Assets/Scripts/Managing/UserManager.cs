using Mono.Data.Sqlite;
using Project.UI.Menu;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Project.Managing
{
    public class UserManager : MonoBehaviour
    {
        private UserData _userData = new();

        public UserData CurrentUserData
        {
            get => _userData;
            set
            {
                _userData = value;
                PlayerPrefs.SetString("username", value.Username);
            }
        }

        private IDbConnection _dbConnection;

        private void OnEnable()
        {
            _dbConnection = CreateAndOpenDatabase();
            InitializeDatabase();

            CurrentUserData = GetUserDataByUsername(PlayerPrefs.GetString("username"));
        }

        private void OnDestroy()
        {
            _dbConnection.Close();
        }

        public bool TryRegisterUser(string username, string password, bool isAdministrator)
        {
            if (!IsUsernameValid(username))
            {
                ShowMessage("Invalid username. It should consist of 5-20 alphanumeric characters.");
                return false;
            }

            if (!IsPasswordValid(password))
            {
                ShowMessage("Invalid password. It should be 4-20 characters long.");
                return false;
            }

            IDbCommand dbCommandCheckUser = _dbConnection.CreateCommand();
            dbCommandCheckUser.CommandText = "SELECT COUNT(*) FROM Users WHERE username=@username";
            dbCommandCheckUser.Parameters.Add(new SqliteParameter("@username", username));

            int count = Convert.ToInt32(dbCommandCheckUser.ExecuteScalar());

            if (count > 0)
            {
                ShowMessage("User already exists!");
                return false;
            }

            IDbCommand dbCommandInsertUser = _dbConnection.CreateCommand();
            dbCommandInsertUser.CommandText = "INSERT INTO Users (username, password, role) VALUES (@username, @password, @role)";
            dbCommandInsertUser.Parameters.Add(new SqliteParameter("@username", username));
            dbCommandInsertUser.Parameters.Add(new SqliteParameter("@password", password));
            dbCommandInsertUser.Parameters.Add(new SqliteParameter("@role", isAdministrator ? "Administrator" : "User"));
            dbCommandInsertUser.ExecuteNonQuery();

            CurrentUserData = GetUserDataByUsername(username);

            return true;
        }

        public bool TryLoginUser(string username, string password)
        {
            if (!IsUsernameValid(username))
            {
                ShowMessage("Invalid Username. It should consist of 5-20 alphanumeric characters.");
                return false;
            }

            if (!IsPasswordValid(password))
            {
                ShowMessage("Invalid Password. It should be 4-20 characters long.");
                return false;
            }

            IDbCommand dbCommandCheckUser = _dbConnection.CreateCommand();
            dbCommandCheckUser.CommandText = "SELECT COUNT(*) FROM Users WHERE username=@username AND password=@password";
            dbCommandCheckUser.Parameters.Add(new SqliteParameter("@username", username));
            dbCommandCheckUser.Parameters.Add(new SqliteParameter("@password", password));

            int count = Convert.ToInt32(dbCommandCheckUser.ExecuteScalar());

            if (count > 0)
            {
                CurrentUserData = GetUserDataByUsername(username);
                return true;
            }
            else
            {
                ShowMessage("Invalid Username or Password.");
                return false;
            }
        }

        public UserData GetUserDataByUsername(string username)
        {
            if (username != null)
            {
                IDbCommand dbCommand = _dbConnection.CreateCommand();
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

            return new UserData();
        }

        /// <summary>
        /// Allowed Parameters: "Id", "Username", "Password", "_role", "Record"
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public List<UserData> GetAllUsersSortedByParameter(ColumnType type)
        {
            var usersList = new List<UserData>();
            IDbCommand dbCommand = _dbConnection.CreateCommand();

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

            IDbCommand dbCommand = _dbConnection.CreateCommand();
            dbCommand.CommandText = "UPDATE Users SET password=@NewPassword WHERE id=@UserId";
            dbCommand.Parameters.Add(new SqliteParameter("@NewPassword", newPassword));
            dbCommand.Parameters.Add(new SqliteParameter("@UserId", CurrentUserData.Id));

            dbCommand.ExecuteNonQuery();

            _userData.Password = newPassword;
            ShowMessage("Successful password change! Your new password: " + newPassword);
            return true;
        }

        public void Logout()
        {
            CurrentUserData = new();
        }

        public void TrySetNewRecord(int score)
        {
            if (CurrentUserData.Record < score)
            {
                _userData.Record = score;

                IDbCommand dbCommand = _dbConnection.CreateCommand();
                dbCommand.CommandText = "UPDATE Users SET record=@NewRecord WHERE id=@UserId";
                dbCommand.Parameters.Add(new SqliteParameter("@NewRecord", _userData.Record));
                dbCommand.Parameters.Add(new SqliteParameter("@UserId", CurrentUserData.Id));

                dbCommand.ExecuteNonQuery();

                ShowMessage("Congratulations! New Record: " + _userData.Record);
            }
        }

        public void ClearUserData()
        {
            IDbCommand dbCommandClearData = _dbConnection.CreateCommand();
            dbCommandClearData.CommandText = "DELETE FROM Users";
            dbCommandClearData.ExecuteNonQuery();
            ShowMessage("All user data cleared.");
        }

        // ... Другие методы взаимодействия с базой данных ...

        private void InitializeDatabase()
        {
            IDbCommand dbCommandCreateTable = _dbConnection.CreateCommand();
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
            var message = GameObject.Find("Message").GetComponent<MessageUI>();
            message.ShowMessage(text);
            message.gameObject.SetActive(true);
        }
    }
}