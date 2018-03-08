using System.Diagnostics;

namespace FunWithAutofixture
{
    public class Logger : ILogger
    {
        public void LogMessage(string address)
        {
            Debug.WriteLine($"Important shit Sending email to {address}");
        }
        public void LogAsImportant(string address)
        {
            Debug.WriteLine($"Sending email to {address}");
        }
    }
}
