/*--------------------------------------------------------------------------------------+
|
|  $Copyright: (c) 2015 Bentley Systems, Incorporated. All rights reserved. $
|
+--------------------------------------------------------------------------------------*/
using SwTools.PowerShortcut.Models;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml;

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
            ShortcutConfig.Instance.Reload();
        }

        public static void Install(string unparsed)
        {
            // 安装
            // 判断是否有配置文件
            if (!File.Exists(ShortcutConfig.ConfigPath))
            {
                OpenFileDialog fileDialog = new OpenFileDialog()
                {
                    FileName = "shortcutsConfig.json",
                    Filter = "json配置文件|*.json"
                };
                if (fileDialog.ShowDialog() != DialogResult.OK) return;

                // 将文件读取保存到指定目录
                StreamWriter streamWriter = File.CreateText(ShortcutConfig.ConfigPath);
                streamWriter.Write(new StreamReader(fileDialog.OpenFile()).ReadToEnd());
                streamWriter.Close();
            }

            // 修改空格快捷键
            string prefPath = Path.GetDirectoryName(ShortcutConfig.ConfigPath) + "\\prefs\\OpenRoadsDesigner_Metric.KeyboardShortcuts.xml";

            // 判断该文件是否存在，如果不存在，提示手动修改
            if (!File.Exists(prefPath))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(prefPath);
                var root = xmlDoc.SelectSingleNode("KeyboardShortcuts/KeyboardShortcut[@ScanCode= '0x39']");
                XmlNode keyinNode = root.SelectSingleNode("Keyin");
                keyinNode.InnerText = "Power shortcut";
                xmlDoc.Save(prefPath);

                MessageBox.Show("未找到系统快捷键配置，请手动修改激活的快捷键");
            }

            // 修改自动加载
            string personalConfPath = Path.GetDirectoryName(ShortcutConfig.ConfigPath) + "\\prefs\\Personal.ucf";
            // 读取值并修改
            string configContent = File.ReadAllText(personalConfPath);
            string autoloadSentence = "MS_DGNAPPS > PowerShortcut";
            if (!configContent.Contains(autoloadSentence))
            {
                // 添加语句使其自动加载
                StreamWriter configWriter = new StreamWriter(personalConfPath, true);
                configWriter.WriteLine(autoloadSentence);
                configWriter.Close();
            }

            MessageBox.Show("安装成功，重启后生效！");
        }

        public static void OpenConfig(string unparsed)
        {
            // 打开配置文件
            if (!File.Exists(ShortcutConfig.ConfigPath))
            {
                StreamWriter streamWriter = File.CreateText(ShortcutConfig.ConfigPath);
                streamWriter.Close();
            }

            Process.Start("notepad.exe", ShortcutConfig.ConfigPath);
        }
    }
}
