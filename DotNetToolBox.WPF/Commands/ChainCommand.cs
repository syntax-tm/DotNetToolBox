using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace DotNetToolBox.WPF.Commands
{
    public class ChainCommand : ICommand
    {
        public ChainCommand(params ICommand[] commands)
        {
            Commands = commands.ToList();
        }

        public IEnumerable<ICommand> Commands { get; set; }

        public void Execute(object parameter)
        {
            foreach (var cmd in Commands)
                cmd.Execute(parameter);
        }

        public bool CanExecute(object parameter)
        {
            return Commands.All(c => c.CanExecute(parameter));
        }

        public event EventHandler CanExecuteChanged
        {
            add { }

            remove { }
        }
    }
}