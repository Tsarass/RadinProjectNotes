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

        private void addMockUser()
        {
            mockUser = Credentials.Instance.userDatabase.AddUser(mockUsername, mockPassword, Permissions.Low);
        }

        private bool couldDeleteMockUser()
        {
            return Credentials.Instance.DeleteUser(mockUser.ID);
        }

        [Test]
        public void testAddAndDeleteUser()
        {
            addMockUser();
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
            addMockUser();
            Assert.DoesNotThrow(() => Credentials.Instance.CheckUsernameAndPassword(mockUsername, mockPassword));
            Credentials.Instance.userDatabase.ChangePassword(mockUsername, newPassword);
            Assert.DoesNotThrow(() => Credentials.Instance.CheckUsernameAndPassword(mockUsername, newPassword));
        }
    }
}
