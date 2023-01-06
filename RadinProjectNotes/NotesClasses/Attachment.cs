using ProtoBuf;
using System;
using System.Drawing;
using System.IO;
using System.Net.Mail;

namespace RadinProjectNotes
{
    [ProtoContract]
    public class Attachment
    {
        [ProtoMember(1)]
        public string OriginalFilePath { get; set; }
        [ProtoMember(2)]
        public string FileName{ get; set; }
        [ProtoMember(3)]
        public string AttachmentSavedToDiskFilePath { get; set;}
        [ProtoMember(4)]
        public bool SavedToDisk { get; set; }
        [ProtoMember(5)]
        public Guid Id { get; set; }
        public Icon Icon { get; set; }

        public Attachment()
        {
            //parameterless constructor for protobuf
        }

        public Attachment(string filepath)
        {
            this.OriginalFilePath = filepath;
            this.FileName = Path.GetFileName(filepath);
            this.SavedToDisk = false;
            this.Id = Guid.NewGuid();
        }

        public void SaveToDisk()
        {
            if (SavedToDisk)
            {
                return;
            }

            string attachmentsFolder = Notes.GetCurrentProjectAttachmentsFolder();
            if (!Directory.Exists(attachmentsFolder))
            {
                Directory.CreateDirectory(attachmentsFolder);
            }

            string trialFileName = Path.Combine(attachmentsFolder, this.FileName);
            string validFilename = GetValidFilename(trialFileName);

            File.Copy(this.OriginalFilePath, validFilename);
            this.AttachmentSavedToDiskFilePath = validFilename;
            SavedToDisk = true;
        }

        public void DeleteFromDisk()
        {
            if (SavedToDisk)
            {
                try
                {
                    File.Delete(AttachmentSavedToDiskFilePath);
                }
                catch
                {

                }
            }
        }

        public bool ExistsInDisk()
        {
            return File.Exists(this.AttachmentSavedToDiskFilePath);
        }

        /// <summary>
        /// Tries to open the file associated with current attachment.
        /// </summary>
        /// <returns>True if file could be opened.</returns>
        public bool TryOpenFile()
        {
            try
            {
                if (this.AttachmentSavedToDiskFilePath != null)
                {
                    if (File.Exists(this.AttachmentSavedToDiskFilePath))
                    {
                        System.Diagnostics.Process.Start(this.AttachmentSavedToDiskFilePath);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    System.Diagnostics.Process.Start(this.OriginalFilePath);
                }
            }
            catch
            {
                throw;
            }

            return true;
        }

        /// <summary>
        /// Checks if a file with the same <paramref name="fileName"/> exists and returns a valid filename with a number appended to it.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static string GetValidFilename(string fileName)
        {
            if (!File.Exists(fileName)) return fileName;

            FileInfo fi = new FileInfo(fileName);
            string ext = fi.Extension;
            string name = fi.FullName.Substring(0, fi.FullName.Length - ext.Length);

            int i = 2;
            while (File.Exists($"{name}_{i}{ext}"))
            {
                i++;
            }


            return $"{name}_{i}{ext}";
        }
        
    }
}
