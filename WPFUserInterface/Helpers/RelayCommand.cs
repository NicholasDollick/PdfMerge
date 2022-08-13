using System;
using System.Windows.Input;

namespace WPFUserInterface.Helpers
{
    class RelayCommand : ICommand
    {
        //execute determines the action to be performed by the command, canExecute lets us know if the action should be performed
        private Action<object> execute;
        private object parameter;
        private Predicate<object> canExecute;

        private event EventHandler CanExecuteChangedInternal;

        /// <summary>
        ///     Default constructor. Takes in action object, but not a canExecute value, we redirect to our main constructor with a default value
        /// </summary>
        /// <param name="execute"></param>
        public RelayCommand(Action<object> execute)
            : this(execute, DefaultCanExecute)
        { }

        /// <summary>
        /// Main constructor. Sets our properties.
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            //validate arguments
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            if (canExecute == null)
            {
                throw new ArgumentNullException("canExecute");
            }

            this.execute = execute;
            this.canExecute = canExecute;
            this.parameter = null;
        }

        public RelayCommand(Action<object> execute, object parameter, Predicate<object> canExecute)
            : this(execute, canExecute)
        {
            this.parameter = parameter;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                this.CanExecuteChangedInternal += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
                this.CanExecuteChangedInternal -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute != null && this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            if (parameter == null)
            {
                this.execute(this.parameter);
            }
            else
            {
                this.execute(parameter);
            }
        }

        public void OnCanExecuteChanged()
        {
            EventHandler handler = this.CanExecuteChangedInternal;
            if (handler != null)
            {
                handler.Invoke(this, EventArgs.Empty);
            }
        }

        public void Destroy()
        {
            this.canExecute = _ => false;
            this.execute = _ => { return; };
        }

        public static bool DefaultCanExecute(object parameter)
        {
            return true;
        }
    }
}
