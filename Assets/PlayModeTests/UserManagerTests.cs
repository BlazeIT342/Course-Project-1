using System.Collections;
using NUnit.Framework;
using Project.Database;
using Project.Database.Mock;
using UnityEngine.TestTools;

namespace Project.Testing
{
    public class UserManagerTests
    {
        [UnityTest]
        public IEnumerator TryRegisterUser_UserDoesNotExist_ReturnsTrue()
        {
            // Arrange
            MockDbConnection mockDbConnection = new();

            var userManager = new DatabaseController(mockDbConnection.CreateAndOpenDatabase());

            // Act
            bool result = userManager.TryRegisterUser("NewUser", "Password123", false);

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

            var userManager = new DatabaseController(mockDbConnection.CreateAndOpenDatabase());

            // Register a user first
            userManager.TryRegisterUser("ExistingUser", "Password123", false);

            // Act
            bool result = userManager.TryRegisterUser("ExistingUser", "Password456", false);

            // Assert
            Assert.IsFalse(result);

            // Clean up
            yield return null;
        }
    }
}