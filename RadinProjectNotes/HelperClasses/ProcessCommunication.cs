using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RadinProjectNotes
{
    public class ProcessCommunication
    {
        public const string ThreadIDFileName = @"RadinProjektNotes";
        static uint message = (uint)0xab1;

        static MemoryMappedFile mmf;

        /// <summary>
        /// Sends a message via the MemoryMappedFile to the open process of RadinProjektNotes
        /// </summary>
        public static void SendMessageToOpenProcess()
        {
            uint threadId;
            using (var mmf = MemoryMappedFile.OpenExisting(ThreadIDFileName, MemoryMappedFileRights.Read))
            using (var accessor = mmf.CreateViewAccessor(0, IntPtr.Size, MemoryMappedFileAccess.Read))
            {
                accessor.Read(0, out threadId);
            }

            PostThreadMessage(threadId, message, IntPtr.Zero, IntPtr.Zero);

        }

        public static void SetupMemoryMappedFile()
        {
            uint mainThreadId = GetCurrentThreadId();
            Console.WriteLine(mainThreadId);
            mmf = MemoryMappedFile.CreateNew(ProcessCommunication.ThreadIDFileName, IntPtr.Size, MemoryMappedFileAccess.ReadWrite);
            using (var accessor = mmf.CreateViewAccessor(0, IntPtr.Size, MemoryMappedFileAccess.ReadWrite))
            {
                accessor.Write(0, mainThreadId);
            }
            Application.AddMessageFilter(new ProcessCommunication.ProcessMessageFilter());
        }

        public class ProcessMessageFilter : IMessageFilter
        {
            public bool PreFilterMessage(ref Message m)
            {
                if ((uint)m.Msg == message)
                {
                    Form1.mainForm.RestoreAppFromTray();
                    return true;
                }

                return false;
            }
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostThreadMessage(uint threadId, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        static extern uint GetCurrentThreadId();
    }

}
