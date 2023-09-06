using System.Text.RegularExpressions;

namespace RadinProjectNotes.HelperClasses
{
    internal static class Emails
    {
        /// <summary>
        /// Verify that a given string is a valid e-mail address.
        /// ref 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool VerifyEmailFormat(string email)
        {
            // ref https://www.rhyous.com/2010/06/15/csharp-email-regular-expression/
            string theEmailPattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                   + "@"
                                   + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z";

            return Regex.IsMatch(email, theEmailPattern);
        }
    }
}
