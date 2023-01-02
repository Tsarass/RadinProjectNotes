using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdate
{
    public class AssemblyVersionChecker
    {
        /// <summary>
        /// Checks a candidate assembly's version against an original assembly.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="candidate"></param>
        /// <returns>true if the candidate assembly has a newer version</returns>
        public static bool Check(string original, string candidate)
        {
            //get the version of the orinial executable's assembly
            FileVersionInfo fv;
            try
            {
                fv = FileVersionInfo.GetVersionInfo(candidate);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not get assembly info on {original}. Aborting update of file.");
                return false;
            }

            Version currentVersion = new Version(FileVersionInfo.GetVersionInfo(original).FileVersion);

            if (FileVersionNewer(currentVersion, fv))
            {
                return true;
            }

            return false;
        }

        public static bool FileVersionNewer(Version original, FileVersionInfo candidate)
        {
            Version updateFileVersion = new Version(string.Format("{0}.{1}.{2}.{3}", candidate.FileMajorPart, candidate.FileMinorPart, candidate.FileBuildPart, candidate.FilePrivatePart));
            return updateFileVersion > original;
        }
    }
}
