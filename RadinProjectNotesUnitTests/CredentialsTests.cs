using NUnit.Framework;
using RadinProjectNotes;

namespace RadinProjectNotesUnitTests
{
    [TestFixture]
    public class CredentialsTests
    {
        private const string mockUsername = "mock user";
        private const string mockPassword = "mock password";
        private User mockUser;

        [SetUp]
        public void Setup()
        {
            ServerConnection.credentials = new Credentials();
        }

        private void addMockUser()
        {
            mockUser = ServerConnection.credentials.userDatabase.AddUser(mockUsername, mockPassword, Permissions.Low);
        }

        private bool couldDeleteMockUser()
        {
            return ServerConnection.credentials.DeleteUser(mockUser.ID);
        }

        [Test]
        public void testAddAndDeleteUser()
        {
            addMockUser();
            Assert.AreEqual(ServerConnection.credentials.userDatabase.NumUsers, 1);
            Assert.IsTrue(ServerConnection.credentials.UsernameExists(mockUsername));
            Assert.IsTrue(couldDeleteMockUser());
            Assert.IsFalse(ServerConnection.credentials.UsernameExists(mockUsername));
            Assert.AreEqual(ServerConnection.credentials.userDatabase.NumUsers, 0);
        }

        [Test]
        public void testChangePassword()
        {
            string newPassword = "new mock password";
            addMockUser();
            Assert.IsNotNull(ServerConnection.credentials.MatchUserOrNull(mockUsername, mockPassword));
            ServerConnection.credentials.userDatabase.ChangePassword(mockUsername, newPassword);
            Assert.IsNotNull(ServerConnection.credentials.MatchUserOrNull(mockUsername, newPassword));
        }
    }
}
