/*--------------------------------------------------------------------------------------+
|
|  $Copyright: (c) 2015 Bentley Systems, Incorporated. All rights reserved. $
|
+--------------------------------------------------------------------------------------*/
using System.Runtime.InteropServices;

namespace SwTools.PowerShortcut
{
    class Keyins
    {
        /*------------------------------------------------------------------------------------**/
        /// <summary>Open method for the keyin</summary>
        /*--------------+---------------+---------------+---------------+---------------+------*/
        public static void PowerShortcutWindow(string unparsed)
        {
            Views.FloatWindow.OpenWindow(unparsed);
        }

        public static void ReloadConfig(string unparsed)
        {
            Models.ShortcutConfig.Instance.Reload();
        }
    }
}
