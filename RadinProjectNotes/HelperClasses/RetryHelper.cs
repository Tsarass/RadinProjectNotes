using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RadinProjectNotes
{
    class RetryHelper
    {
        /// <summary>Takes a function as an argument and loops it with
        /// the selected <paramref name="delay"/> until no exception occurs.
        /// </summary>
        /// <param name="times">maximum attempts</param>
        /// <param name="delay">delay between attempts</param>
        /// <param name="operation">function to be executed</param>
        public static bool RetryOnException(int times, TimeSpan delay, Action operation)
        {
            var attempts = 0;
            do
            {
                try
                {
                    attempts++;
                    Debug.WriteLine($"Executing operation: attempt {attempts}");
                    operation();
                    break;
                }
                catch (Exception ex)
                {
                    if (attempts == times)
                    {
                        Debug.WriteLine("Max attempts reached!", ex);
                        MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    Debug.WriteLine($"Exception caught on attempt {attempts} - will retry after delay {delay}", ex);

                    Task.Delay(delay).Wait();
                }
            } while (true);

            return true;
        }

    }
}
