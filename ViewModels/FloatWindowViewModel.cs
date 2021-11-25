/*--------------------------------------------------------------------------------------+
|
|  $Copyright: (c) 2015 Bentley Systems, Incorporated. All rights reserved. $
|
+--------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Input;

using Bentley.UI.Mvvm;

using WowuTool.PowerShortcut.Properties;
using WowuTool.PowerShortcut.Views;

namespace WowuTool.PowerShortcut.ViewModels
{
    class FloatWindowViewModel : ViewModelBase
    {
        public PropertyObserver<FloatWindowViewModel> PropertyObserver { get; set; }

        /// <summary>
        /// 快捷键结果
        /// </summary>
        public ObservableCollection<Models.Shortcut> ShortcutResults = new ObservableCollection<Models.Shortcut>();

        public FloatWindowViewModel()
        {
            PropertyObserver = new PropertyObserver<FloatWindowViewModel>(this);

            // Greeting has a dependency on Name. 
            // Call NotifyOfPropertyChange for Greeting whenever Name changes.
            // PropertyObserver.RegisterHandler(t => t.InputText, t => NotifyOfPropertyChange(() => ShowDescription));

            // 读取所有的快捷键

        }

        private string _inputText = string.Empty;
        public string InputText
        {
            get
            {
                return _inputText;
            }
            set
            {
                _inputText = value??string.Empty;

                // 通过关键词搜索快捷键，并根据使用频次进行排序
                List<Models.Shortcut> results = Models.ShortcutConfig.Instance.GetShortcuts(value);
                // 清空全部并重新添加
                ShortcutResults.Clear();
                results.ForEach(s => ShortcutResults.Add(s));

                // 如果为空的话，添加一个未匹配快捷键
                if (ShortcutResults.Count == 0)
                {
                    ShortcutResults.Add(Models.Shortcut.GetNullShortcut());
                }

                NotifyOfPropertyChange(() => InputText);
            }
        }
    }
}
