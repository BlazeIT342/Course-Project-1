using System;
using System.Data;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Project.Testing
{
    public class UserManagerr : MonoBehaviour
    {
        private UserData _userData = new();

        public UserData CurrentUserData
        {
            get => _userData;
            set
            {
                _userData = value;
            }
        }

        private IDbConnection _dbConnection;

        public UserManagerr(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        private void OnEnable()
        {
            _dbConnection = CreateAndOpenDatabase();
            InitializeDatabase();

        }

        private void OnDestroy()
        {
            _dbConnection.Close();
        }

        public bool TryRegisterUser(string username, string password, bool isAdministrator)
        {
            try
            {
                ValidateUsername(username);
                ValidatePassword(password);

                // Mocked check for user existence
                if (UserExists(username))
                {
                    throw new InvalidOperationException("User already exists!");
                }

                // Mocked user registration
                RegisterUser(username, password, isAdministrator);

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
            catch (Exception e)
            {
                ShowMessage("An unexpected error occurred: " + e.Message);
            }

            return false;
        }

        private void RegisterUser(string username, string password, bool isAdministrator)
        {
            // Mocked database insertion
            CurrentUserData = new UserData
            {
                Username = username,
                Password = password,
                Role = isAdministrator ? "Administrator" : "User",
                Record = 0
            };
        }

        private bool UserExists(string username)
        {
            // Mocked check for user existence in the database
            return CurrentUserData.Username == username;
        }

        private void ValidateUsername(string username)
        {
            string pattern = "^[\\S]{5,20}$";

            if (!Regex.IsMatch(username, pattern))
            {
                throw new ArgumentException("Invalid username. It should consist of 5-20 alphanumeric characters and contain no white spaces.");
            }
        }

        private void ValidatePassword(string password)
        {
            string pattern = "^[\\S]{4,20}$";

            if (!Regex.IsMatch(password, pattern))
            {
                throw new ArgumentException("Invalid password. It should be 4-20 characters long and contain no white spaces.");
            }
        }

        private void ShowMessage(string text)
        {
            // Mocked method to show a message
            Debug.Log(text);
        }

        // Other methods...

        private void InitializeDatabase()
        {
            // Mocked method to initialize the database
        }

        private IDbConnection CreateAndOpenDatabase()
        {
            // Mocked method to create and open the database
            return new MockDbConnection();
        }
    }
}