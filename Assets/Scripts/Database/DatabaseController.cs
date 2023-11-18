using Mono.Data.Sqlite;
using Project.UI.Menu;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Project.Database
{
    /// <summary>
    /// Manages interactions with a SQLite database to handle user authentication, registration,
    /// and user data management for a Unity application.
    /// </summary>
    /// <remarks>
    /// The DatabaseController class provides functionality for user registration, login, password updates,
    /// high-score record management, and other database-related operations. It utilizes SQLite as the database engine.
    /// </remarks>
    public class DatabaseController
    {
        private readonly IDbConnection _dbConnection;

        private UserData _userData = new();

        /// <summary>
        /// Gets or sets the currently logged-in user's data.
        /// </summary>
        /// <remarks>
        /// Setting this property updates the in-memory user data and persists the username using PlayerPrefs.
        /// </remarks>
        public UserData CurrentUserData
        {
            get => _userData;
           
            private set
            {
                _userData = value;
                PlayerPrefs.SetString("username", value.Username);
            }
        }

        /// <summary>
        /// Initializes a new instance of the DatabaseController class with the provided database connection.
        /// </summary>
        /// <param name="dbConnection">The database connection to be used by the controller.</param>
        /// <remarks>
        /// This constructor initializes the database, and sets the CurrentUserData based on the username stored in PlayerPrefs.
        /// </remarks>
        public DatabaseController(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
            InitializeDatabase();
            CurrentUserData = GetUserDataByUsername(PlayerPrefs.GetString("username"));
        }

        /// <summary>
        /// Attempts to register a new user in the database with the provided credentials.
        /// </summary>
        /// <param name="username">The username of the new user.</param>
        /// <param name="password">The password of the new user.</param>
        /// <param name="isAdministrator">A flag indicating whether the new user should have administrator privileges.</param>
        /// <returns>Returns true if the user is successfully registered; otherwise, returns false.</returns>
        /// <remarks>
        /// This function validates the username and password, checks for existing users with the same username,
        /// inserts the new user into the database, and sets the current user data.
        /// </remarks>
        public bool TryRegisterUser(string username, string password, bool isAdministrator)
        {
            try
            {
                ValidateUsername(username);
                ValidatePassword(password);

                var dbCommandCheckUser = _dbConnection.CreateCommand();
                dbCommandCheckUser.CommandText = "SELECT COUNT(*) FROM Users WHERE username=@username";
                dbCommandCheckUser.Parameters.Add(new SqliteParameter("@username", username));

                int count = Convert.ToInt32(dbCommandCheckUser.ExecuteScalar());

                if (count > 0)
                {
                    throw new InvalidOperationException("User already exists!");
                }

                var dbCommandInsertUser = _dbConnection.CreateCommand();
                dbCommandInsertUser.CommandText = "INSERT INTO Users (username, password, role) VALUES (@username, @password, @role)";
                dbCommandInsertUser.Parameters.Add(new SqliteParameter("@username", username));
                dbCommandInsertUser.Parameters.Add(new SqliteParameter("@password", password));
                dbCommandInsertUser.Parameters.Add(new SqliteParameter("@role", isAdministrator ? "Administrator" : "User"));
                dbCommandInsertUser.ExecuteNonQuery();

                CurrentUserData = GetUserDataByUsername(username);

                return true;
            }
            catch (ArgumentException e)
            {
                ShowMessage(e.Message);
            }
            catch (InvalidOperationException e)
            {
                ShowMessage(e.Message);
            }
            catch (SqliteException e)
            {
                ShowMessage("Error registering user in database: " + e.Message);
            }
            catch (Exception e)
            {
                ShowMessage("An unexpected error occurred: " + e.Message);
            }
            
            return false;
        }

        /// <summary>
        /// Attempts to log in a user with the provided username and password.
        /// </summary>
        /// <param name="username">The username of the user attempting to log in.</param>
        /// <param name="password">The password of the user attempting to log in.</param>
        /// <returns>Returns true if the login is successful; otherwise, returns false.</returns>
        /// <remarks>
        /// This function validates the username and password, checks the database for matching credentials,
        /// sets the current user data if the login is successful, and handles various exceptions.
        /// </remarks>
        public bool TryLoginUser(string username, string password)
        {
            try
            {
                ValidateUsername(username);
                ValidatePassword(password);

                var dbCommandCheckUser = _dbConnection.CreateCommand();
                dbCommandCheckUser.CommandText = "SELECT COUNT(*) FROM Users WHERE username=@username AND password=@password";
                dbCommandCheckUser.Parameters.Add(new SqliteParameter("@username", username));
                dbCommandCheckUser.Parameters.Add(new SqliteParameter("@password", password));

                int count = Convert.ToInt32(dbCommandCheckUser.ExecuteScalar());

                if (count <= 0)
                {
                    throw new InvalidOperationException("Invalid Username or Password.");
                }

                CurrentUserData = GetUserDataByUsername(username);
                return true;
            }

            catch (ArgumentException e)
            {
                ShowMessage(e.Message);
            }
            catch (InvalidOperationException e)
            {
                ShowMessage(e.Message);
            }
            catch (SqliteException e)
            {
                ShowMessage("Error registering user in database: " + e.Message);
            }
            catch (Exception e)
            {
                ShowMessage("An unexpected error occurred: " + e.Message);
            } 

            return false;
        }

        /// <summary>
        /// Retrieves user data for a given username from the database.
        /// </summary>
        /// <param name="username">The username for which to retrieve user data.</param>
        /// <returns>Returns the user data if the username exists; otherwise, returns empty user data.</returns>
        /// <remarks>
        /// This function performs a database query to fetch user data based on the provided username.
        /// It returns a UserData object containing user details if the user exists, or an empty UserData object otherwise.
        /// </remarks>
        public UserData GetUserDataByUsername(string username)
        {
            if (username != null)
            {
                var dbCommand = _dbConnection.CreateCommand();
                dbCommand.CommandText = "SELECT * FROM Users WHERE username=@Username";
                dbCommand.Parameters.Add(new SqliteParameter("@Username", username));

                var reader = dbCommand.ExecuteReader();

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
        /// Retrieves a list of user data from the database, sorted based on the specified parameter.
        /// </summary>
        /// <param name="type">The parameter by which to sort the user data. Allowed values: "Id", "Username", "Password", "Role", "Record".</param>
        /// <param name="desc">True to sort in descending order; false to sort in ascending order.</param>
        /// <returns>Returns a list of UserData objects representing all users, sorted according to the specified parameter and order.</returns>
        /// <remarks>
        /// This function performs a database query to fetch all users and sorts them based on the specified parameter.
        /// The returned list contains UserData objects with details for each user in the database.
        /// The parameter by which to sort the user data. Allowed values: "Id", "Username", "Password", "Role", "Record"
        /// </remarks>
        public List<UserData> GetAllUsersSortedByParameter(string type, bool desc)
        {
            var usersList = new List<UserData>();
            var dbCommand = _dbConnection.CreateCommand();

            if (desc)
            {
                dbCommand.CommandText = $"SELECT * FROM Users ORDER BY {type} DESC";
            }
            else
            {
                dbCommand.CommandText = $"SELECT * FROM Users ORDER BY {type}";
            }

            var reader = dbCommand.ExecuteReader();

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

        /// <summary>
        /// Updates the password for the currently logged-in user in the database.
        /// </summary>
        /// <param name="newPassword">The new password to set for the user.</param>
        /// <returns>Returns true if the password is successfully updated; otherwise, returns false.</returns>
        /// <remarks>
        /// This function validates the new password, performs a database update to change the password,
        /// updates the current user's password in memory, and displays a success message.
        /// </remarks>
        public bool UpdatePassword(string newPassword)
        {
            try
            {
                ValidatePassword(newPassword);

                var dbCommand = _dbConnection.CreateCommand();
                dbCommand.CommandText = "UPDATE Users SET password=@NewPassword WHERE id=@UserId";
                dbCommand.Parameters.Add(new SqliteParameter("@NewPassword", newPassword));
                dbCommand.Parameters.Add(new SqliteParameter("@UserId", CurrentUserData.Id));

                dbCommand.ExecuteNonQuery();

                _userData.Password = newPassword;
                ShowMessage("Successful password change! Your new password: " + newPassword);
                return true;
            }
            catch (ArgumentException e)
            {
                ShowMessage(e.Message);
            }
            catch (SqliteException e)
            {
                ShowMessage(e.Message);              
            }

            return false;
        }

        /// <summary>
        /// Attempts to set a new high score record for the currently logged-in user.
        /// </summary>
        /// <param name="score">The new score to set as the user's record.</param>
        /// <remarks>
        /// This function compares the provided score with the user's current record.
        /// If the provided score is higher, it updates the record in the database, in-memory, and displays a congratulatory message.
        /// </remarks>
        public void TrySetNewRecord(int score)
        {
            if (CurrentUserData.Record < score)
            {
                _userData.Record = score;

                var dbCommand = _dbConnection.CreateCommand();
                dbCommand.CommandText = "UPDATE Users SET record=@NewRecord WHERE id=@UserId";
                dbCommand.Parameters.Add(new SqliteParameter("@NewRecord", _userData.Record));
                dbCommand.Parameters.Add(new SqliteParameter("@UserId", CurrentUserData.Id));

                dbCommand.ExecuteNonQuery();

                ShowMessage("Congratulations! New Record: " + _userData.Record);
            }
        }

        /// <summary>
        /// Deletes a user from the database based on the provided username.
        /// </summary>
        /// <param name="username">The username of the user to be deleted.</param>
        /// <remarks>
        /// This function removes a user from the database by executing a DELETE SQL command with the specified username.
        /// </remarks>
        public void DeleteUserByUsername(string username)
        {
            if (username != null)
            {
                var dbCommand = _dbConnection.CreateCommand();
                dbCommand.CommandText = "DELETE FROM Users WHERE username=@Username";
                dbCommand.Parameters.Add(new SqliteParameter("@Username", username));

                dbCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Clears all user data from the database.
        /// </summary>
        /// <remarks>
        /// This function executes a DELETE SQL command to remove all user records from the Users table in the database.
        /// It also displays a message indicating that all user data has been cleared.
        /// </remarks>
        public void ClearUserData()
        {
            var dbCommandClearData = _dbConnection.CreateCommand();
            dbCommandClearData.CommandText = "DELETE FROM Users";
            dbCommandClearData.ExecuteNonQuery();
            ShowMessage("All user data cleared.");
        }

        /// <summary>
        /// Initializes the database by creating the Users table if it doesn't exist.
        /// </summary>
        /// <remarks>
        /// This function executes a CREATE TABLE SQL command to create the Users table with the required columns.
        /// It ensures that the table exists before interacting with user data.
        /// </remarks>
        private void InitializeDatabase()
        {
            var dbCommandCreateTable = _dbConnection.CreateCommand();
            dbCommandCreateTable.CommandText = "CREATE TABLE IF NOT EXISTS Users (id INTEGER PRIMARY KEY, username TEXT, password TEXT, role TEXT DEFAULT User, record INTEGER DEFAULT 0)";
            dbCommandCreateTable.ExecuteNonQuery();
        }

        /// <summary>
        /// Clears the currently logged-in user's data.
        /// </summary>
        /// <remarks>
        /// This function sets the CurrentUserData property to a new UserData object, effectively clearing the current user's data.
        /// </remarks>
        public void ClearCurrentUserData()
        {
            CurrentUserData = new();
        }

        /// <summary>
        /// Validates the provided username against a pattern.
        /// </summary>
        /// <param name="username">The username to be validated.</param>
        /// <exception cref="ArgumentException">Thrown if the username is invalid. It should consist of 5-20 alphanumeric characters and contain no white spaces.</exception>
        private void ValidateUsername(string username)
        {
            string pattern = "^[\\S]{5,20}$";

            if (!Regex.IsMatch(username, pattern))
            {
                throw new ArgumentException("Invalid username. It should consist of 5-20 alphanumeric characters.");
            }
        }

        /// <summary>
        /// Validates the provided password against a pattern.
        /// </summary>
        /// <param name="password">The password to be validated.</param>
        /// <exception cref="ArgumentException">Thrown if the password is invalid. It should be 4-20 characters long and contain no white spaces.</exception>
        private void ValidatePassword(string password)
        {
            string pattern = "^[\\S]{4,20}$";

            if (!Regex.IsMatch(password, pattern))
            {
                throw new ArgumentException("Invalid password. It should be 4-20 characters long.");
            }
        }

        /// <summary>
        /// Shows a message using the MessageUI component if available.
        /// </summary>
        /// <param name="text">The text of the message to be shown.</param>
        private void ShowMessage(string text)
        {
            try
            {
                if (UnityEngine.Object.FindObjectOfType<MessageUI>().TryGetComponent<MessageUI>(out var message))
                {
                    message.ShowMessage(text);
                    message.gameObject.SetActive(true);
                }
            }
            catch (NullReferenceException)
            {

            }
        }
    }
}