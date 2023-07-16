using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RadinProjectNotes
{
    public class RetryHelper<T>
    {
        /// <summary>Takes a function as an argument and loops it with
        /// the selected <paramref name="delay"/> until no exception occurs.
        /// </summary>
        /// <returns></returns>
        /// <param name="times">maximum attempts</param>
        /// <param name="delay">delay between attempts</param>
        /// <param name="operation">function to be executed</param>
        public static T RetryOnException(int times, TimeSpan delay, Func<T> operation)
        {
            var attempts = 0;
            do
            {
                try
                {
                    attempts++;
                    Debug.WriteLine($"Executing operation: attempt {attempts}");
                    return operation();
                }
                catch (Exception ex)
                {
                    if (attempts == times)
                    {
                        Debug.WriteLine("Max attempts reached!", ex.Message.ToString());
                        //MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return default(T);
                    }

                    Debug.WriteLine($"Exception caught on attempt {attempts} - will retry after delay {delay}", ex.Message.ToString());

                    Task.Delay(delay).Wait();
                }
            } while (true);
        }

    }
}
