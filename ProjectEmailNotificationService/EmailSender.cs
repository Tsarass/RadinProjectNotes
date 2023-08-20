using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEmailNotificationService
{
    internal class EmailSender
    {

        public static void SendEmail(string content)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("christsaridis@gmail.com", "yfobnnvvihogqczk"),
                EnableSsl = true,
            };

            smtpClient.Send("christsaridis@gmail.com", "rr_tsaras@hotmail.com", "AUTOMATED MAIL", content);
        }
    }
}
