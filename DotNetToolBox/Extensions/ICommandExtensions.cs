using System;
using System.Windows.Input;
using JetBrains.Annotations;

namespace DotNetToolBox.Extensions
{
    public static class ICommandExtensions
    {

        /// <summary>
        /// Executes the <see cref="ICommand"/> and passes a <see langword="null"/> parameter.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <seealso cref="ICommand"/>
        /// <seealso cref="ICommand.Execute"/>
        public static void Execute([NotNull] this ICommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            command.Execute(null);
        }

    }
}
