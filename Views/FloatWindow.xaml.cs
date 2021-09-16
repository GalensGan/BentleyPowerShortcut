/*--------------------------------------------------------------------------------------+
|
|  $Copyright: (c) 2015 Bentley Systems, Incorporated. All rights reserved. $
|
+--------------------------------------------------------------------------------------*/
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;

using Bentley.MstnPlatformNET.WPF;
using SwTools.PowerShortcut.Models;
using SwTools.PowerShortcut.ViewModels;

namespace SwTools.PowerShortcut.Views
{
    /// <summary>
    /// Interaction logic for FloatWindow.xaml
    /// </summary>
    public partial class FloatWindow : Window
    {
        private static FloatWindow s_window;
        private static WPFInteropHelper m_wndHelper;
        private FloatWindowViewModel _viewModel;

        public FloatWindow()
        {
            InitializeComponent();

            // Create the ViewModel and set the DataContext
            _viewModel = new ViewModels.FloatWindowViewModel();
            this.DataContext = _viewModel;
            CommandText.DataContext = _viewModel;

            // 向列表绑定数据
            ShortcutsList.ItemsSource = _viewModel.ShortcutResults;

            m_wndHelper = new WPFInteropHelper(this);
            m_wndHelper.Attach(PowerShortcutAddin.Instance, true, "PowerShortcut");

            this.Loaded += FloatWindow_Loaded;
            CommandText.KeyUp += CommandText_KeyUp;
            ShortcutsList.KeyUp += ShortcutsList_KeyUp;
            ShortcutsList.MouseDoubleClick += ShortcutsList_MouseDoubleClick;
        }

        // 双击打开命令
        private void ShortcutsList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ShortcutsList.SelectedItem != null)
            {
                // 获取选择的项
                Shortcut shortcut = ShortcutsList.SelectedItem as Shortcut;
                if (shortcut != null && !shortcut.RunKeyin())
                {
                    // 使输入框获得焦点
                    CommandText.Focus();
                }
                else
                {
                    // 保存到历史
                    _history.Push(shortcut);
                    _viewModel.InputText = string.Empty;
                }
            }
        }

        private void ShortcutsList_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // 在窗体中按空格时，触发选中的命令
            if (e.Key == System.Windows.Input.Key.Space || e.Key == System.Windows.Input.Key.Enter)
            {
                // 获取选择的项
                Shortcut shortcut = ShortcutsList.SelectedItem as Shortcut;
                if (shortcut!=null && !shortcut.RunKeyin())
                {                    
                    // 使输入框获得焦点
                    CommandText.Focus();
                }
                else
                {
                    // 保存到历史
                    _history.Push(shortcut);
                    _viewModel.InputText = string.Empty;
                }
            }else if (ShortcutsList.SelectedItem == null)
            {
                ShortcutsList.SelectedItem = _viewModel.ShortcutResults.FirstOrDefault();
            }
        }

        private ShortcutsHistory _history = new ShortcutsHistory();

        private bool _isForWakeup = false;
        private void CommandText_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (_isForWakeup)
            {
                _isForWakeup = false;
                return;
            }

            // 打开快捷
            if (e.Key == System.Windows.Input.Key.Space || e.Key == System.Windows.Input.Key.Enter)
            {
                // 打开命令
                // 按了确认键
                // 找到第一个命令
                Models.Shortcut shortcut = _viewModel.ShortcutResults.FirstOrDefault();

                // 重置为空
                _viewModel.InputText = string.Empty;
                if (shortcut == null)
                {
                    return;
                }

                if (!shortcut.RunKeyin())
                {
                    // 运行失败后,获取最后一次的快捷键
                    var lastShortcut = _history.History.LastOrDefault();
                    if (lastShortcut != null) lastShortcut.RunKeyin();
                }

                // 保存到历史
                _history.Push(shortcut);

                // 清空显示数据
                _viewModel.InputText = string.Empty;

                // 失去焦点
                // Helper.Win32Helper.SetForegroundWindow(m_wndHelper.MSWinIntPtr);
            }
            else if (e.Key == System.Windows.Input.Key.Tab)
            {
                // 自动补全
                var first = _viewModel.ShortcutResults.FirstOrDefault();
                if (first == null) return;

                _viewModel.InputText = first.Name;

                // 设置光标到末尾
                CommandText.Select(_viewModel.InputText.Length, 0);
            }
            else if (e.Key == System.Windows.Input.Key.Up)
            {
                // 上一个快捷键

                _viewModel.InputText = _history.Previous()?.Name;
                // 设置光标到末尾
                CommandText.Select(_viewModel.InputText.Length, 0);
            }
            else if (e.Key == System.Windows.Input.Key.Down)
            {
                _viewModel.InputText = _history.Next()?.Name;
                // 设置光标到末尾
                CommandText.Select(_viewModel.InputText.Length, 0);
            }
        }

        private void FloatWindow_Loaded(object sender, RoutedEventArgs e)
        {
            CommandText.Focus();
        }

        /*------------------------------------------------------------------------------------**/
        /// <summary>React to the Window closed</summary>
        /*--------------+---------------+---------------+---------------+---------------+------*/
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            ///
            /// PowerPlatform Integration
            /// 

            m_wndHelper.Detach();
            m_wndHelper.Dispose();
            s_window = null;
        }

        /*------------------------------------------------------------------------------------**/
        /// <summary>Creates and opens the Window</summary>
        /*--------------+---------------+---------------+---------------+---------------+------*/

        public static void OpenWindow(string unparsed)
        {
            if (null == s_window)
            {
                s_window = new FloatWindow();
                s_window.Show();
            }
            s_window._isForWakeup = true;

            Helper.Win32Helper.SetForegroundWindow(m_wndHelper.Handle);
            // 设置输入框为焦点
            s_window.CommandText.Focus();
        }

        /*------------------------------------------------------------------------------------**/
        /// <summary>Closes the Window</summary>
        /*--------------+---------------+---------------+---------------+---------------+------*/
        public static void CloseWindow(string unparsed)
        {
            if (null != s_window)
            {
                s_window.Close();
                s_window = null;
            }
        }
    }
}
