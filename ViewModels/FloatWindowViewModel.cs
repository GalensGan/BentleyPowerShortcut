/*--------------------------------------------------------------------------------------+
|
|  $Copyright: (c) 2015 Bentley Systems, Incorporated. All rights reserved. $
|
+--------------------------------------------------------------------------------------*/
using System.ComponentModel;
using System.Windows.Input;

using Bentley.UI.Mvvm;

using SwTools.PowerShortcut.Properties;
using SwTools.PowerShortcut.Views;

namespace SwTools.PowerShortcut.ViewModels
{
    class FloatWindowViewModel : ViewModelBase, IDataErrorInfo
    {
        public PropertyObserver<FloatWindowViewModel> PropertyObserver { get; set; }

        public Models.Shortcut Shortcut { get; set; }

        public FloatWindowViewModel()
        {
            Shortcut = new Models.Shortcut();  // Example
            Shortcut.Name = "";
            PropertyObserver = new PropertyObserver<FloatWindowViewModel>(this);

            // Greeting has a dependency on Name. 
            // Call NotifyOfPropertyChange for Greeting whenever Name changes.
            PropertyObserver.RegisterHandler(t => t.Name, t => NotifyOfPropertyChange(() => ShowDescription));
        }

        public string Name
        {
            get
            {
                return Shortcut.Name;
            }
            set
            {
                // 如果后面有空格，说明确认命令
                Shortcut.Name = value.Trim(' ');
                Shortcut.Update();

                NotifyOfPropertyChange(() => Name);
            }
        }

        public string ShowDescription
        {
            get
            {
                return Shortcut.Description;
            }
        }

        #region IDataErrorInfo Members

        private string _error;

        public string Error
        {
            get { return _error; }
        }

        public string this[string propertyName]
        {
            get
            {
                _error = null;

                if (propertyName == "Name")
                {
                    _error = Shortcut.Update();
                }

                // Dirty the commands registered with CommandManager,
                // such as our Save command, so that they are queried
                // to see if they can execute now.
                CommandManager.InvalidateRequerySuggested();

                return _error;
            }
        }

        #endregion
    }
}
