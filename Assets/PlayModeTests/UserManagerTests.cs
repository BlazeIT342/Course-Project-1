using System.Collections;
using System.Data;
using NUnit.Framework;
using Project.Managing;
using UnityEngine.TestTools;

namespace Project.Testing
{
    public class UserManagerTests
    {
        [UnityTest]
        public IEnumerator TryRegisterUser_UserDoesNotExist_ReturnsTrue()
        {
            // Arrange
            IDbConnection mockDbConnection = new MockDbConnection();
            var userManager = new UserManager(mockDbConnection);

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
            IDbConnection mockDbConnection = new MockDbConnection();
            var userManager = new UserManager(mockDbConnection);

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