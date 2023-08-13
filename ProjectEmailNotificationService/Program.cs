using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEmailNotificationService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            EmailSender.SendEmail();
        }
    }
}
