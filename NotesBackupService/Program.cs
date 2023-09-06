using System.ServiceProcess;

namespace NotesBackupService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new BackupService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
