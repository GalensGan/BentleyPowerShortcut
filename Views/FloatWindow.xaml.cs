/*--------------------------------------------------------------------------------------+
|
|  $Copyright: (c) 2015 Bentley Systems, Incorporated. All rights reserved. $
|
+--------------------------------------------------------------------------------------*/
using System;
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

            ///
            /// PowerPlatform Integration
            /// 
            /// Bentley.MstnPlatformNET.WPF.WPFInteropHelper

            // Create the MicroStation Interop Helper and Attach the Window
            m_wndHelper = new WPFInteropHelper(this);
            m_wndHelper.Attach(PowerShortcutAddin.Instance, true, "PowerShortcut");

            this.Loaded += FloatWindow_Loaded;
            CommandText.KeyUp += CommandText_KeyUp;
        }

        private ShortcutsHistory _history = new ShortcutsHistory();
        private void CommandText_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // 打开快捷
            if (e.Key == System.Windows.Input.Key.Space || e.Key == System.Windows.Input.Key.Enter)
            {
                // 打开命令
                // 按了确认键
                Bentley.MstnPlatformNET.Session.Instance.Keyin(_viewModel.Shortcut.Keyin);

                // 保存快捷键
                _history.Push(_viewModel.Shortcut.Name);

                // 重置为空
                _viewModel.Name = string.Empty;

                // 失去焦点
                Helper.Win32Helper.SetForegroundWindow(m_wndHelper.MSWinIntPtr);
            }
            else if (e.Key == System.Windows.Input.Key.Tab)
            {
                // 自动补全
                _viewModel.Name = Models.ShortcutConfig.Instance.TabFullName(_viewModel.Name);
                // 设置光标到末尾
                CommandText.Select(_viewModel.Name.Length, 0);
            }
            else if (e.Key == System.Windows.Input.Key.Up)
            {
                _viewModel.Name = _history.Previous();
                // 设置光标到末尾
                CommandText.Select(_viewModel.Name.Length, 0);
            }
            else if (e.Key == System.Windows.Input.Key.Down)
            {
                _viewModel.Name = _history.Next();
                // 设置光标到末尾
                CommandText.Select(_viewModel.Name.Length, 0);
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

            Helper.Win32Helper.SetForegroundWindow(m_wndHelper.Handle);
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
