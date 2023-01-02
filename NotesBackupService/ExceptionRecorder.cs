using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesBackupService
{
    public class ExceptionRecorder
    {
        public Exception ex;

        static readonly string exceptionRecordFilename = @"exception.log";
        static readonly string exceptionRecordFolder = @"exceptions";

        private string ExceptionRecordFilePath
        {
            get
            {
                return Path.Combine(Logger.AppdataFolder(), exceptionRecordFolder, exceptionRecordFilename);
            }
        }

        private string ExceptionRecordSaveFileName
        {
            get
            {
                return ExceptionRecordFilePath + $"_{DateTime.Now.ToString("yyyymmddhhmmss")}";
            }
        }

        public ExceptionRecorder(Exception ex)
        {
            this.ex = ex;
            CreateExceptionRecordFolder();
        }

        private void CreateExceptionRecordFolder()
        {
            string path = Path.Combine(Logger.AppdataFolder(), exceptionRecordFolder);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public void SaveExceptionRecordFile()
        {
            using (var file = File.Open(ExceptionRecordSaveFileName, FileMode.Append, FileAccess.Write))
            using (var writer = new StreamWriter(file))
            {
                writer.WriteLine($"Date: {DateTime.Now.ToString()}");
                writer.WriteLine($"Message: {ex.Message}");
                writer.WriteLine($"Source: {ex.Source}");
                writer.WriteLine($"Inner exception: {ex.InnerException}");
                writer.WriteLine($"Stacktrace: {ex.StackTrace}");
            }
        }
    }
}
