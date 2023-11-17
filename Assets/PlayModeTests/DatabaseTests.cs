#if UNITY_EDITOR
using System.Collections;
using NUnit.Framework;
using Project.Database;
using Project.Database.Mock;
using UnityEngine.TestTools;

namespace Project.Testing
{
    public class DatabaseTests
    {
        [UnityTest]
        public IEnumerator TryRegisterUser_UserDoesNotExist_ReturnsTrue()
        {
            // Arrange
            MockDbConnection mockDbConnection = new();

            var databaseController = new DatabaseController(mockDbConnection.CreateAndOpenDatabase());

            // Act
            var result = databaseController.TryRegisterUser("NewUser", "Password123", false);

            // Assert
            Assert.IsTrue(result);

            // Clean up
            yield return null;
        }

        [UnityTest]
        public IEnumerator TryRegisterUser_UserAlreadyExists_ReturnsFalse()
        {
            // Arrange
            MockDbConnection mockDbConnection = new();

            var databaseController = new DatabaseController(mockDbConnection.CreateAndOpenDatabase());

            // Register a user first
            databaseController.TryRegisterUser("ExistingUser", "Password123", false);

            // Act
            var result = databaseController.TryRegisterUser("ExistingUser", "Password456", false);

            // Assert
            Assert.IsFalse(result);

            // Clean up
            yield return null;
        }

        [UnityTest]
        public IEnumerator TryLoginUser_ValidCredentials_ReturnsTrue()
        {
            // Arrange
            MockDbConnection mockDbConnection = new();

            var databaseController = new DatabaseController(mockDbConnection.CreateAndOpenDatabase());

            // Register a user first
            databaseController.TryRegisterUser("TestUser", "Password123", false);

            // Act
            var result = databaseController.TryLoginUser("TestUser", "Password123");

            // Assert
            Assert.IsTrue(result);

            // Clean up
            yield return null;
        }

        [UnityTest]
        public IEnumerator TryLoginUser_InvalidUsername_ReturnsFalse()
        {
            // Arrange
            MockDbConnection mockDbConnection = new();

            var databaseController = new DatabaseController(mockDbConnection.CreateAndOpenDatabase());

            // Act
            var result = databaseController.TryLoginUser("NonExistingUser", "Password123");

            // Assert
            Assert.IsFalse(result);

            // Clean up
            yield return null;
        }

        [UnityTest]
        public IEnumerator TryLoginUser_InvalidPassword_ReturnsFalse()
        {
            // Arrange
            MockDbConnection mockDbConnection = new();

            var databaseController = new DatabaseController(mockDbConnection.CreateAndOpenDatabase());

            // Register a user first
            databaseController.TryRegisterUser("TestUser", "Password123", false);

            // Act
            var result = databaseController.TryLoginUser("TestUser", "InvalidPassword");

            // Assert
            Assert.IsFalse(result);

            // Clean up
            yield return null;
        }

        [UnityTest]
        public IEnumerator GetUserDataByUsername_ExistingUser_ReturnsUserData()
        {
            // Arrange
            MockDbConnection mockDbConnection = new();

            var databaseController = new DatabaseController(mockDbConnection.CreateAndOpenDatabase());

            // Register a user first
            databaseController.TryRegisterUser("TestUser", "Password123", false);

            // Act
            var userData = databaseController.GetUserDataByUsername("TestUser");

            // Assert
            Assert.IsNotNull(userData);
            Assert.AreEqual("TestUser", userData.Username);

            // Clean up
            yield return null;
        }

        [UnityTest]
        public IEnumerator GetUserDataByUsername_NonExistingUser_ReturnsEmptyUserData()
        {
            // Arrange
            MockDbConnection mockDbConnection = new();

            var databaseController = new DatabaseController(mockDbConnection.CreateAndOpenDatabase());

            // Act
            var userData = databaseController.GetUserDataByUsername("NonExistingUser");

            // Assert
            Assert.IsNotNull(userData);
            Assert.AreEqual(default(int), userData.Id); // Assuming Id is an int

            // Clean up
            yield return null;
        }

        [UnityTest]
        public IEnumerator GetUserDataByUsername_NullUsername_ReturnsEmptyUserData()
        {
            // Arrange
            MockDbConnection mockDbConnection = new();

            var databaseController = new DatabaseController(mockDbConnection.CreateAndOpenDatabase());

            // Act
            var userData = databaseController.GetUserDataByUsername(null);

            // Assert
            Assert.IsNotNull(userData);
            Assert.AreEqual(default(int), userData.Id); // Assuming Id is an int

            // Clean up
            yield return null;
        }
    }
}
#endif