using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace RadinProjectNotes
{
    [ProtoContract]
    public class AttachmentLibrary
    {
        [ProtoMember(1)]
        public List<Attachment> attachments = new List<Attachment>();

        public bool AttachmentIconsLoaded { get; set; }

        public AttachmentLibrary()
        {
            //parameterless constructor for protobuf
            AttachmentIconsLoaded = false;
        }

        /// <summary>
        /// Returns a stream of an icon to be used on files that have an unknown system icon.
        /// </summary>
        /// <returns></returns>
        private static Stream MakeUknownFileIcon()
        {
            System.Reflection.Assembly a = System.Reflection.Assembly.GetExecutingAssembly();
            System.IO.Stream st = a.GetManifestResourceStream("RadinProjectNotes.icons.unknown-file.ico");
            return st;
        }

        /// <summary>
        /// Loads the icons of all attachments in the library
        /// </summary>
        public void LoadAttachmentIcons()
        {
            if (AttachmentIconsLoaded)
                return;

            Dictionary<string, Icon> iconPerExtension = new Dictionary<string, Icon>();

            foreach (var attachment in this.attachments)
            {
                FileInfo fi = new FileInfo(attachment.FileName);
                string extension = fi.Extension;
                string attachmentFilePath = attachment.SavedToDisk ? attachment.AttachmentSavedToDiskFilePath : attachment.OriginalFilePath;

                Stream unknownIconStream = MakeUknownFileIcon();

                if (!iconPerExtension.ContainsKey(extension))
                {
                    Icon icon;
                    if (File.Exists(attachmentFilePath))
                    {
                        icon = ExtractIcon.ExtractAssociatedIcon(attachmentFilePath);
                        iconPerExtension.Add(extension, icon);
                    }
                    else
                    {
                        icon = new Icon(unknownIconStream);
                    }

                    attachment.Icon = icon;
                }
                else
                {
                    attachment.Icon = File.Exists(attachmentFilePath) ? iconPerExtension[extension] : new Icon(unknownIconStream);
                }
            }

            AttachmentIconsLoaded = true;
        }

        private Attachment FindAttachment(Attachment attachment)
        {
            return FindAttachmentById(attachment.Id);
        }

        /// <summary>
        /// Find the attachment in the library by id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>The matching attachment.</returns>
        /// <exception cref="AttachmentNotFound"></exception>
        public Attachment FindAttachmentById(Guid Id)
        {
            foreach (var _attachment in attachments)
            {
                if (_attachment.Id == Id)
                {
                    return _attachment;
                }
            }

            throw new AttachmentNotFound();
        }

        private bool AttachmentExistsInLibrary(Attachment attachment)
        {
            try
            {
                FindAttachment(attachment);
            }
            catch (AttachmentNotFound)
            {
                return false;
            }

            return true;
        }

        public void AddAttachment(Attachment attachment)
        {
            if (AttachmentExistsInLibrary(attachment))
            {
                throw new AttachmentAlreadyExists();
            }
            else
            {
                attachments.Add(attachment);
            }
        }

        public int NumberOfValidAttachments()
        {
            int count = 0;
            foreach (var attachment in attachments)
            {
                if (attachment.ExistsInDisk())
                {
                    count++;
                }
            }

            return count;
        }

        public void RemoveAttachmentById(Guid id, bool deleteFromDisk = false)
        {
            Attachment attachmentToRemove;
            try
            {
                attachmentToRemove = FindAttachmentById(id);
            }
            catch (AttachmentNotFound ex)
            {
                Debug.Write(ex.Message.ToString());
                return;
            }

            attachments.Remove(attachmentToRemove);

            if (deleteFromDisk)
            {
                attachmentToRemove.DeleteFromDisk();
            }
        }

        public List<Attachment> GetAttachments()
        {
            return attachments;
        }

        public void SetAttachments(List<Attachment> attachments)
        {
            this.attachments = attachments;
        }

        public void SaveAttachmentsToDisk()
        {
            foreach (var attachment in attachments)
            {
                attachment.SaveToDisk();
            }
        }

        public void DeleteAttachmentsFromDisk()
        {
            foreach (var attachment in attachments)
            {
                attachment.DeleteFromDisk();
            }
        }

        public class AttachmentNotFound : Exception
        {
            public AttachmentNotFound() : base("Attachment not found.") { }
        }

        public class AttachmentAlreadyExists : Exception
        {
            public AttachmentAlreadyExists() : base("Attachment already exists in library.") { }
        }
    }
   
}
