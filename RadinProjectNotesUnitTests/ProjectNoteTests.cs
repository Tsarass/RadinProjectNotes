using NUnit.Framework;
using RadinProjectNotes;
using System.Collections.Generic;

namespace RadinProjectNotesUnitTests
{
    public class ProjectNoteTests
    {
        [SetUp]
        public void Setup()
        {
            ServerConnection.credentials = new Credentials();
            ServerConnection.credentials.TryLoadUserDatabase();
        }

        [Test]
        public void testProjectNoteConstructor()
        {
            var projectNote = new Notes.ProjectNote("dummy text");
            Assert.AreEqual(projectNote.dateLastEdited, -1);
            Assert.AreEqual(projectNote.attachmentLibrary.attachments.Count, 0);
        }

        [Test]
        public void testProjectNoteConstructorWithAttachments()
        {
            string attachmentPath = "dummypath";
            var attachment = new Attachment(attachmentPath);
            var projectNote = new Notes.ProjectNote("dummy text", new List<Attachment> { attachment });
            Assert.AreEqual(projectNote.dateLastEdited, -1);
            Assert.AreEqual(projectNote.attachmentLibrary.attachments.Count, 1);
            Assert.AreEqual(projectNote.attachmentLibrary.attachments[0].OriginalFilePath, attachmentPath);
        }
    }
}