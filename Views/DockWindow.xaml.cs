/*--------------------------------------------------------------------------------------+
|
|     $Source: MstnExamples/WPF/WPFSample/Views/DockWindow.xaml.cs $
|
|  $Copyright: (c) 2015 Bentley Systems, Incorporated. All rights reserved. $
|
+--------------------------------------------------------------------------------------*/
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SD = System.Drawing;

using Bentley.MstnPlatformNET.WPF;

namespace WPFSample.Views
    {
    /// <summary>
    /// Interaction logic for DockWindow.xaml
    /// </summary>
    public partial class DockWindow : DockableWindow
        {
        private static DockWindow s_window;

        public DockWindow ()
            {
            InitializeComponent ();

            this.Attach (WPFSampleApp.Instance, "WPFSampleDock", new SD.Size (100, 100));
            }

        /*------------------------------------------------------------------------------------**/
        /// <summary>React to the Window closed</summary>
        /*--------------+---------------+---------------+---------------+---------------+------*/
        protected override void OnClosed (EventArgs e)
            {
            base.OnClosed (e);

            Detach ();
            s_window = null;
            }

        /*------------------------------------------------------------------------------------**/
        /// <summary>Creates and opens the Window</summary>
        /*--------------+---------------+---------------+---------------+---------------+------*/
        public static void OpenWindow (string unparsed)
            {
            if (null == s_window)
                {
                s_window = new DockWindow ();
                s_window.Show ();
                }
            }

        /*------------------------------------------------------------------------------------**/
        /// <summary>Closes the Window</summary>
        /*--------------+---------------+---------------+---------------+---------------+------*/
        public static void CloseWindow (string unparsed)
            {
            if (null != s_window)
                {
                s_window.Close ();
                }
            }
        }
    }
