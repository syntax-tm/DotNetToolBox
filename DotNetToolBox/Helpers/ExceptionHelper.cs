using System;
using System.Text;
using DotNetToolBox.Extensions;
using JetBrains.Annotations;

namespace DotNetToolBox.Helpers
{
    public static class ExceptionHelper
    {

        public static string GetErrorMessage([NotNull] string message, [CanBeNull] Exception innerException = null)
        {
            var messageSb = new StringBuilder();
            
            messageSb.Append(message);
            messageSb.EndSentence();

            var innerExMessage = innerException?.Message;
            if (string.IsNullOrEmpty(innerExMessage)) return messageSb.ToString();
            
            if (messageSb.ToString().ContainsIgnoreCase(innerExMessage))
            {
                return messageSb.ToString();
            }

            messageSb.StartSentence();
            messageSb.Append(innerException.Message);
            messageSb.EndSentence();

            return messageSb.ToString();
        }

    }
}
