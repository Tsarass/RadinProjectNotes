using ProtoBuf;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Traverses the list of attachments and returns the matching attachment,
        /// otherwise, returns null.
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        private Attachment FindAttachment(Attachment attachment)
        {
            foreach (var _attachment in attachments)
            {
                if (_attachment.Id == attachment.Id)
                {
                    return _attachment;
                }
            }

            return null;
        }

        /// <summary>
        /// Traverses the list of attachments and returns the matching attachment,
        /// otherwise, returns null.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Attachment FindAttachment(Guid Id)
        {
            foreach (var _attachment in attachments)
            {
                if (_attachment.Id == Id)
                {
                    return _attachment;
                }
            }

            return null;
        }

        public Attachment AddAttachment(Attachment attachment)
        {
            if (FindAttachment(attachment) is null)
            {
                attachments.Add(attachment);
                return attachment;
            }

            return null;
        }

        public Attachment RemoveAttachment(Attachment attachment, bool deleteFromDisk = false)
        {
            Attachment match = FindAttachment(attachment);
            if (match != null)
            {
                attachments.Remove(match);

                if (deleteFromDisk)
                {
                    match.DeleteFromDisk();
                }

                return match;
            }

            return null;
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
    }
   
}
