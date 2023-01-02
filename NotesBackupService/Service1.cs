using System;
using System.ServiceProcess;
using System.Timers;

namespace NotesBackupService
{
    public partial class Service1 : ServiceBase
    {

        Timer timer = new Timer(); // name space(using System.Timers;)  
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 300000; //number in miliseconds
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
            RegistryFunctions.SetRegistryKeyValue(RegistryEntry.ServiceStoppedOn, DateTime.Now.ToString()) ;
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            
            if ((BackupOnThisDayRequired()) && (ValidHourForBackup()))
            {
                timer.Enabled = false;
                
                try
                {
                    RegistryFunctions.ClearRegistryValues();
                    FileBackup fileBackup = new FileBackup();
                    fileBackup.Start();
                    RegistryFunctions.SetRegistryKeyValue(RegistryEntry.LastBackupDate, DateTime.Now.ToString());
                }
                catch (Exception ex)
                {
                    ExceptionRecorder exRecorder = new ExceptionRecorder(ex);
                    exRecorder.SaveExceptionRecordFile();
                }

                timer.Enabled = true;
            }
        }

        private bool BackupOnThisDayRequired()
        {

            string lastBackupDate = RegistryFunctions.GetRegistryKeyValue(RegistryEntry.LastBackupDate);
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
            DateTime now = DateTime.Now;

            if ((now.Hour >= 22) || (now.Hour <= 5))
            {
                return true;
            }

            return false;
        }
    }
}
