/*--------------------------------------------------------------------------------------+
|
|  $Copyright: (c) 2015 Bentley Systems, Incorporated. All rights reserved. $
|
+--------------------------------------------------------------------------------------*/
using WowuTool.PowerShortcut.Models;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Bentley.DgnPlatformNET;

namespace WowuTool.PowerShortcut
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
                // 生成一个示例配置
                JObject sample = new JObject()
                {
                    new JProperty("shortcuts",new JArray()
                    {
                        new JObject()
                        {
                            new JProperty("names",new JArray()),
                            new JProperty("keyin",""),
                            new JProperty("keyins",new JArray()),
                            new JProperty("description","这是一个快捷键配置示例，在实际中，keyin 与 keyins 可以任选其一使用，也可以一起使用")
                        }
                    }),
                };

                // 将文件读取保存到指定目录
                StreamWriter streamWriter = File.CreateText(ShortcutConfig.ConfigPath);
                streamWriter.Write(JsonConvert.SerializeObject(sample, Newtonsoft.Json.Formatting.Indented));
                streamWriter.Close();
            }

            // 修改空格快捷键
            string keyboardShorcutsFullName = ShortcutConfig.KeyboardShortcuts;

            // 修改自动加载
            string personalConfPath = Path.GetDirectoryName(ShortcutConfig.ConfigPath) + "\\prefs\\Personal.ucf";
            // 读取值并修改
            string configContent = File.ReadAllText(personalConfPath);
            string autoloadSentence = "\r\n%level Organization\r\nMS_DGNAPPS > PowerShortcut";
            if (!configContent.Contains(autoloadSentence))
            {
                // 添加语句使其自动加载
                StreamWriter configWriter = new StreamWriter(personalConfPath, true);
                configWriter.Write(autoloadSentence);
                configWriter.Close();
            }

            // 判断该文件是否存在，如果不存在，提示手动修改
            if (File.Exists(keyboardShorcutsFullName))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(keyboardShorcutsFullName);
                var root = xmlDoc.SelectSingleNode("KeyboardShortcuts/KeyboardShortcut[@ScanCode= '0x39']");
                XmlNode keyinNode = root.SelectSingleNode("Keyin");
                keyinNode.InnerText = "Power shortcut";
                xmlDoc.Save(keyboardShorcutsFullName);
            }
            else
            {
                MessageBox.Show("未找到系统快捷键配置，需要手动修改激活的快捷键");
                return;
            }

            MessageBox.Show("安装成功！");
        }

        public static void OpenConfig(string unparsed)
        {
            // 打开配置文件
            if (!File.Exists(ShortcutConfig.ConfigPath))
            {
                StreamWriter streamWriter = File.CreateText(ShortcutConfig.ConfigPath);
                streamWriter.Close();
            }

            // 监听文件变化，当文件改变时，自动加载快捷键
            //FileSystemWatcher watcher = new FileSystemWatcher(Path.GetDirectoryName(ShortcutConfig.ConfigPath))
            //{
            //    Filter = "*.json",
            //    NotifyFilter = NotifyFilters.LastWrite,
            //    IncludeSubdirectories = false,
            //    EnableRaisingEvents = true
            //};
            //watcher.Changed += (sender, e) =>
            //{
            //    if (e.FullPath == ShortcutConfig.ConfigPath)
            //    {
            //        // 重新加载
            //        ShortcutConfig.Instance.Reload();
            //    }
            //};
            // 在新进程中打开编辑器
            Task.Factory.StartNew(() =>
            {
                Process process = Process.Start("notepad.exe", ShortcutConfig.ConfigPath);
                process.WaitForExit();
                // 重新加载
                ShortcutConfig.Instance.Reload();
            });
        }

        public static void Setting(string unparsed)
        {
            // 修改设置

        }
    }
}
