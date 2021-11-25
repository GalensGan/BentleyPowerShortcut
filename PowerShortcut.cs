/*-------------------------------------------------------------------------------------+
|
|  $Copyright: (c) 2015 Bentley Systems, Incorporated. All rights reserved. $
|
+--------------------------------------------------------------------------------------*/

/*-------------------------------------------------------------------------------------+
| This example demonstrates the use of WPF and Window creation api and offers keyins to| 
| perform operations like opening and closing the floating window, opening and closing | 
| dockable window.                                                                     |
| This addin is an example and works to when loaded in the Microstation and can be     |
| accessed via the keyins to perform the following functionalities.                    |
| 1. Open floaing window.                                                              |
| 2. Close floating Window.                                                            |
| 3. Open dockable window.                                                             |
| 4. Close dockable window.                                                            |
|                                                                                      |
| It also demonstrates how to use various control on these windows like a Text edit    |
| box, model dialog, slider bar etc.                                                   |
+--------------------------------------------------------------------------------------*/

using System;
using System.Resources;
using System.Windows;

namespace WowuTool.PowerShortcut
{
    /*====================================================================================**/
    ///
    /// <summary>Singleton AddIn class</summary>
    ///
    /*==============+===============+===============+===============+===============+======*/
    [Bentley.MstnPlatformNET.AddInAttribute(MdlTaskID = "PowerShortcut")]
    public class PowerShortcutAddin : Bentley.MstnPlatformNET.AddIn
    {
        private static PowerShortcutAddin s_WPFSampleApp;
        private static ResourceManager s_ResourceManager;

        /*------------------------------------------------------------------------------------**/
        /// <summary>Constructor for the AddIn</summary>
        /*--------------+---------------+---------------+---------------+---------------+------*/
        private PowerShortcutAddin
        (
        IntPtr mdlDesc
        )
            : base(mdlDesc)
        {

        }

        /*------------------------------------------------------------------------------------**/
        /// <summary>The Run method</summary>
        /*--------------+---------------+---------------+---------------+---------------+------*/
        protected override int Run
        (
        string[] commandLine
        )
        {
            // save a reference to our addin to prevent it from being garbage collected.
            s_WPFSampleApp = this;

            // Get the localized resources
            s_ResourceManager = Properties.Resources.ResourceManager;

            return 0;
        }

        /*------------------------------------------------------------------------------------**/
        /// <summary>Static method to get the AddIn</summary>
        /*--------------+---------------+---------------+---------------+---------------+------*/
        internal static PowerShortcutAddin Instance
        {
            get { return s_WPFSampleApp; }
        }

        /*------------------------------------------------------------------------------------**/
        /// <summary>Static method to get the ResourceManager</summary>
        /*--------------+---------------+---------------+---------------+---------------+------*/
        internal static ResourceManager ResourceManager
        {
            get { return s_ResourceManager; }
        }
    } // WPFSampleApp
}   // namespace WPFSample
