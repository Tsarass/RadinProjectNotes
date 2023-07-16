using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RadinProjectNotes
{
    public class RetryHelper<T>
    {
        /// <summary>Takes a function as an argument and loops it with
        /// the selected <paramref name="delayBetweenAttempts"/> until no exception occurs.
        /// </summary>
        /// <returns>Result of the operation of type T.</returns>
        /// <param name="maxAttempts">maximum attempts</param>
        /// <param name="delayBetweenAttempts">delay between attempts</param>
        /// <param name="operation">function to be executed with return type T</param>
        public static T RetryOnException(int maxAttempts, TimeSpan delayBetweenAttempts, Func<T> operation)
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
                    if (attempts == maxAttempts)
                    {
                        Debug.WriteLine("Max attempts reached!", ex.Message.ToString());
                        //MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return default(T);
                    }

                    Debug.WriteLine($"Exception caught on attempt {attempts} - will retry after delay {delayBetweenAttempts}", ex.Message.ToString());

                    Task.Delay(delayBetweenAttempts).Wait();
                }
            } while (true);
        }
    }

    public class RetryHelper
    {
        /// <summary>Takes a function as an argument and loops it with
        /// the selected <paramref name="delayBetweenAttempts"/> until no exception occurs.
        /// </summary>
        /// <returns>True if operation successful without exceptions.</returns>
        /// <param name="maxAttempts">maximum attempts</param>
        /// <param name="delayBetweenAttempts">delay between attempts</param>
        /// <param name="operation">function to be executed</param>
        public static bool RetryOnException(int maxAttempts, TimeSpan delayBetweenAttempts, Action operation)
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
                    if (attempts == maxAttempts)
                    {
                        Debug.WriteLine("Max attempts reached!", ex.Message.ToString());
                        //MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    Debug.WriteLine($"Exception caught on attempt {attempts} - will retry after delay {delayBetweenAttempts}", ex.Message.ToString());

                    Task.Delay(delayBetweenAttempts).Wait();
                }
            } while (true);

            return true;
        }
    }
}
