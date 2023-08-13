using NUnit.Framework;
using RadinProjectNotes;
using System.Security.Permissions;

namespace RadinProjectNotesUnitTests
{
    [TestFixture]
    public class CredentialsTests
    {
        private const string mockUsername = "mock user";
        private const string mockPassword = "mock password";
        private User mockUser;

        private void addMockUser()
        {
            mockUser = Credentials.Instance.userDatabase.AddUser(mockUsername, mockPassword, Permissions.Read);
        }

        private bool couldDeleteMockUser()
        {
            return Credentials.Instance.DeleteUser(mockUser.ID);
        }

        [SetUp]
        public void SetUp()
        {
            addMockUser();
        }

        [TearDown] 
        public void TearDown() 
        {
            couldDeleteMockUser();
        }

        [Test]
        public void testAddAndDeleteUser()
        {
            Assert.AreEqual(Credentials.Instance.userDatabase.NumUsers, 1);
            Assert.IsTrue(Credentials.Instance.UsernameExists(mockUsername));
            Assert.IsTrue(couldDeleteMockUser());
            Assert.IsFalse(Credentials.Instance.UsernameExists(mockUsername));
            Assert.AreEqual(Credentials.Instance.userDatabase.NumUsers, 0);
        }

        [Test]
        public void testChangePassword()
        {
            string newPassword = "new mock password";
            Assert.DoesNotThrow(() => Credentials.Instance.LogInUser(mockUsername, mockPassword));
            Credentials.Instance.userDatabase.ChangePassword(mockUsername, newPassword);
            Assert.DoesNotThrow(() => Credentials.Instance.LogInUser(mockUsername, newPassword));
        }

        [Test]
        public void testLoginUserWithPassword()
        {
            Assert.DoesNotThrow(() => Credentials.Instance.LogInUser(mockUsername, mockPassword));
            Assert.IsTrue(Credentials.Instance.currentUser == mockUser);
            mockUser.ResetPassword();
            Assert.IsTrue(mockUser.HasResetPassword());
            Assert.Throws<UserDatabase.UserHasResetPassword>(() => Credentials.Instance.LogInUser(mockUsername, mockPassword));
        }
    }
}
