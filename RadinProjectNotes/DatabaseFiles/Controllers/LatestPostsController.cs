using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using RadinProjectNotesCommon;

namespace RadinProjectNotes.DatabaseFiles.Controllers
{
    public class LatestPostsController
    {
        private static readonly string latestChangesFileName = @"latest.db";

        private static string LatestPostsDatabaseFullPath
        {
            get
            {
                return Path.Combine(ServerConnection.serverFolder, latestChangesFileName);
            }
        }

        private static long LatestPostsDatabaseFilesize
        {
            get
            {
                if (File.Exists(LatestPostsDatabaseFullPath))
                {
                    FileInfo info = new FileInfo(LatestPostsDatabaseFullPath);
                    return info.Length;
                }
                else
                {
                    return -1;
                }
                
            }
        }

        public static List<RecentChange> GetLatestPosts()
        {
            SaveLatestPostsDatabaseFilesize();
            return LoadListOfRecentChangesFromDatabase();
        }

        /// <summary>
        /// Posts a recent change to the latest changes database.
        /// </summary>
        /// <param name="change"></param>
        public static void PostRecentChange(RecentChange change)
        {
            List<RecentChange> database = LoadListOfRecentChangesFromDatabase();

            database.Add(change);

            SaveLatestChangesDatabase(database);
        }

        private static void SaveLatestChangesDatabase(List<RecentChange> database)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            // Encryption
            try
            {
                using (var fs = new FileStream(LatestPostsDatabaseFullPath, FileMode.Create, FileAccess.Write))
                using (var cryptoStream = new CryptoStream(fs, des.CreateEncryptor(Security.desKey, Security.desIV), CryptoStreamMode.Write))
                {

                    // This is where you serialize the class
                    ProtoBuf.Serializer.Serialize(cryptoStream, database);
                }

                //hide file
                File.SetAttributes(LatestPostsDatabaseFullPath, FileAttributes.Hidden);
            }
            catch (Exception e)
            {
                //MessageBox.Show(this, e.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Debug.WriteLine($"Exception caught while accessing file to SaveLatestChangesDatabase", e);
                throw;
            }
        }

        private static List<RecentChange> LoadListOfRecentChangesFromDatabase()
        {
            List<RecentChange> data = new List<RecentChange>();

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            //check to see if database exists
            if (File.Exists(LatestPostsDatabaseFullPath))
            {
                try
                {
                    // Decryption
                    Debug.WriteLine("Trying to load latest changes database file.");
                    using (var fs = new FileStream(LatestPostsDatabaseFullPath, FileMode.Open, FileAccess.Read))
                    using (var cryptoStream = new CryptoStream(fs, des.CreateDecryptor(Security.desKey, Security.desIV), CryptoStreamMode.Read))
                    {
                        // This is where you deserialize the class
                        data = ProtoBuf.Serializer.Deserialize<List<RecentChange>>(cryptoStream);
                        Debug.WriteLine("Successfully loaded database file.");
                    }
                }
                catch (Exception e)
                {
                    //MessageBox.Show(this, e.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Debug.WriteLine("Error while loading database file..", e);
                    throw;
                }

            }
            else
            {
                return new List<RecentChange>();
            }

            return data;
        }

        private static void SaveLatestPostsDatabaseFilesize()
        {
            RegistryFunctions.SetRegistryKeyValue(RegistryEntry.LatestPostsDatabaseLastSize, LatestPostsDatabaseFilesize.ToString());
        }

        public static bool IsUpdateRequired()
        {
            string lastSizeSavedInRegistry = RegistryFunctions.GetRegistryKeyValue(RegistryEntry.LatestPostsDatabaseLastSize);
            long.TryParse(lastSizeSavedInRegistry, out long parsedSize);

            //if filesize is exactly equal, skip the check
            //if filesize is greater, latest posts were added
            //if filesize was smaller or negative, something went wrong, update regardless
            if (parsedSize == LatestPostsDatabaseFilesize)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
    }
}
