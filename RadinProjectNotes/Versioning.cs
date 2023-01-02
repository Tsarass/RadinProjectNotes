using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadinProjectNotes
{
    public class Versioning
    {

        //Save Structures    
        [ProtoContract]
        public class SaveStructureV1
        {
            [ProtoMember(1)]
            public int version;
            [ProtoMember(2)]
            public List<Notes.ProjectNote> noteData;
            [ProtoMember(3)]
            public long lastSavedTime;

            public bool IsEmpty
            {
                get{ return this.lastSavedTime < 0 || this.noteData.Count == 0; }
            }

            public SaveStructureV1(int version, List<Notes.ProjectNote> noteDataList)
            {
                this.version = version;
                this.noteData = noteDataList;
                this.lastSavedTime = DateTime.UtcNow.Ticks;
            }

            public SaveStructureV1()
            {
                this.version = -1;
                this.noteData = new List<Notes.ProjectNote>();
                this.lastSavedTime = -1;
            }

            public void DeleteNote(Notes.ProjectNote noteToDelete)
            {
                noteToDelete.attachmentLibrary.DeleteAttachmentsFromDisk();
                noteData.Remove(noteToDelete);
            }
        }

        
    }
}
