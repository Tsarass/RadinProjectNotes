using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdate
{
    public class MainExecutableUpdater
    {
        string executableFromPath = ""; //path from where to copy the file
        string executableToPath = "";   //path to which we copy the file

        public MainExecutableUpdater(string executableFromPath, string executableToPath)
        {
            this.executableFromPath = executableFromPath;
            this.executableToPath = executableToPath;
            Update();
        }

        private void Update()
        {
            if (AssemblyVersionChecker.CheckIfNewerVersion(executableToPath, executableFromPath))
            {
                FilesUpdater.UpdateFile(executableFromPath, executableToPath);
            }
        }
    }
}
