using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;

namespace AutoUpdate
{
    class Program
    {
        //args[0] should contain the installation directory path
        //args[1] should contain the executable path to be updated

        static string executablePath = "";
        static string updateFromPath = "";
        static string updateToPath = "";
        static string executableName = "";
        static void Main(string[] args)
        {
            string assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            string assemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Console.WriteLine($"{assemblyName} v.{assemblyVersion}\n");

            Thread.Sleep(500);

            if (!ErrorCheckArguments(args))
            {
                Console.ReadLine();
                return;
            }
            SetPaths(args);

            //wait a bit for the main app to close
            Console.WriteLine("Waiting for main application to close...");
            WaitForMainApplicationClose();

            //run the updater for the auto updater application
            AutoUpdaterUpdater updater = new AutoUpdaterUpdater(updateFromPath, updateToPath, executablePath);

            //run the updater for the executable
            FileInfo fi = new FileInfo(executablePath);
            string executableDestination = Path.Combine(updateToPath, fi.Name);
            MainExecutableUpdater update = new MainExecutableUpdater(executablePath, executableDestination);

            FilesUpdater filesUpdater = new FilesUpdater(updateFromPath, updateToPath);

            //start process anew
            Process.Start(executableDestination);

            Console.WriteLine("Files successfully updated.");
            return;
        }

        private static void WaitForMainApplicationClose()
        {
            Thread.Sleep(100);
            bool processClosed = false;
            while (!processClosed)
            {
                Process[] processes = Process.GetProcesses();
                bool found = false;
                foreach (var process in processes)
                {
                    if (process.ProcessName.Contains(executableName))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    processClosed = true;
                }

                Thread.Sleep(100);
            }
        }

        private static void SetPaths(string[] args)
        {
            updateToPath = args[0];
            executablePath = args[1];

            FileInfo fi = new FileInfo(args[1]);
            updateFromPath = fi.DirectoryName;

            executableName = Path.GetFileNameWithoutExtension(fi.Name);
            
        }

        static bool ErrorCheckArguments(string[] args)
        {
            //check for argument of filepath for the updated files
            if (args.Length < 2)
            {
                Console.WriteLine("Incorrect path arguments passed. Exiting...");
                return false;
            }

            if (!Directory.Exists(args[0]))
            {
                Console.WriteLine("Incorrect update source path. Exiting...");
                return false;
            }

            return true;
        }

    }
}
