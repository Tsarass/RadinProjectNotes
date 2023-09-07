using RadinProjectNotesCommon;
using RadinProjectNotesCommon.EncryptedDatabaseSerializer;
using System;
using System.ServiceProcess;
using System.Text;
using System.Timers;

namespace NotesBackupService
{
    public partial class BackupService : ServiceBase
    {

        public static byte[] desKey = { 10, 25, 11, 35, 207, 108, 56, 99 };
        public static byte[] desIV = { 38, 13, 7, 1, 116, 222, 111, 6 };

        Timer timer = new Timer();
        public BackupService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 60000; //number in miliseconds
            timer.Enabled = true;

            // Set up.
            EncryptionKeys.SetEncryptionKeys(desKey, desIV);
            RegistryFunctions.SetAppKeyPath("Notes Backup Service");
        }

        protected override void OnStop()
        {
            RegistryFunctions.SetRegistryKeyValue(RegistryEntries.ServiceStoppedOn, DateTime.Now.ToString()) ;
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            
            if ((BackupOnThisDayRequired()) && (ValidHourForBackup()))
            {
                timer.Enabled = false;
                
                try
                {
                    FileBackup fileBackup = new FileBackup(Filepaths.GetAppDataFolder());
                    fileBackup.Start();
                    RegistryFunctions.SetRegistryKeyValue(RegistryEntries.LastBackupDate, DateTime.Now.ToString());
                }
                catch (Exception ex)
                {
                    ExceptionLogger exRecorder = new ExceptionLogger(ex, Filepaths.GetAppDataFolder());
                    exRecorder.SaveExceptionLogFile();
                }

                timer.Enabled = true;
            }
        }

        private bool BackupOnThisDayRequired()
        {
            string lastBackupDate = RegistryFunctions.GetRegistryKeyValue(RegistryEntries.LastBackupDate);
            if ((lastBackupDate is null) || (lastBackupDate == String.Empty))
            {
                return true;
            }

            //check if backup was already done for this day
            DateTime now = DateTime.Now;
            DateTime lastDate;
            DateTime.TryParse(lastBackupDate, out lastDate);

            if (lastDate != null)
            {
                double hoursElapsed = now.Subtract(lastDate).TotalHours;
                if (hoursElapsed >= 22)
                {
                    return true;
                }
            }
            else
            {
                return true;
            }

            return false;
        }

        private bool ValidHourForBackup()
        {
            return true;
            DateTime now = DateTime.Now;

            if ((now.Hour >= 22) || (now.Hour <= 5))
            {
                return true;
            }

            return false;
        }
    }
}
