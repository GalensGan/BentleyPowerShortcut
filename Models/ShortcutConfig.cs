using Bentley.DgnPlatformNET;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SwTools.PowerShortcut.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SwTools.PowerShortcut.Models
{
    class ShortcutConfig
    {
        private static ShortcutConfig _instance;

        private List<Shortcut> _shortcuts = new List<Shortcut>();

        private JObject _primaryObj;

        public static string ConfigPath { get; private set; } = ConfigurationManager.GetVariable("_USTN_HOMEROOT") + "shortcutsConfig.json";

        private ShortcutConfig()
        {
            if (!File.Exists(ConfigPath))
            {
                // 请添加
                System.Windows.Forms.MessageBox.Show($"请添加快捷键配置文件:{ConfigPath}");
                return;
            }

            try
            {

                string configStr = File.ReadAllText(ConfigPath);
                _primaryObj = JsonConvert.DeserializeObject<JObject>(configStr);

                // 添加系统配置
                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SwTools.PowerShortcut.Models.systemShortcuts.json");
                StreamReader sr = new StreamReader(stream);
                var systemShortcutsConfig = sr.ReadToEnd();
                sr.Close();
                stream.Close();

                JObject sysShortcuts = JObject.Parse(systemShortcutsConfig);
                JObject configObj = _primaryObj.DeepClone() as JObject;
                configObj.Merge(sysShortcuts);

                // 将配置转成快捷键
                JArray arr = configObj.Value<JArray>("shortcuts");
                foreach (JToken jt in arr)
                {
                    // 遍历 name
                    JArray names = jt.Value<JArray>("names");
                    foreach (JToken name in names)
                    {
                        // 允许快捷键名称重复
                        Shortcut shortcut = new Shortcut()
                        {
                            // 获取数据
                            Name = name.ToObject<string>(),
                            Keyin = jt.Value<string>("keyin"),
                            Description = jt.Value<string>("description"),
                            Frequency = jt.Value<int>("frequency")
                        };

                        // 从频率表中读取频率数据
                        var sysF = configObj.SelectValueOrDefault($"frequency.{shortcut.Name}", 0);
                        shortcut.Frequency += sysF;

                        // 保存
                        _shortcuts.Add(shortcut);
                    }
                }

                // 获取最小频率，对所有频率缩小
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
        }

        public static ShortcutConfig Instance
        {
            get
            {
                if (_instance == null) _instance = new ShortcutConfig();

                return _instance;
            }
        }

        /// <summary>
        /// 重新加载配置文件
        /// </summary>
        public void Reload()
        {
            _instance = new ShortcutConfig();
        }

        /// <summary>
        /// 过滤符合条件的快捷键
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<Shortcut> GetShortcuts(string filter)
        {
            if (filter==null) return new List<Shortcut>();
            filter = filter.Trim(' ');

            if (string.IsNullOrEmpty(filter)) return new List<Shortcut>();

            // 进行正则匹配
            string pattern = "\\S*";
            for (int i = 0; i < filter.Length; i++)
            {
                pattern += filter[i] + "\\S*";
            }

            Regex regex = new Regex(pattern);

            // 排序
            var results = _shortcuts.FindAll(item => regex.IsMatch(item.Name));
            results = results.OrderByDescending(item => item.Frequency).ToList();
            // 找到全匹配的，移动到第一位
            var index = results.FindIndex(item => item.Name == filter);
            if (index > -1)
            {
                results.Insert(0, results[index]);
                results.RemoveAt(index + 1);
            }

            return results;
        }

        public void SaveFrequency(string shortcutName, int frequency)
        {
            var fObj = _primaryObj.SelectToken("frequency") as JObject;
            if (fObj == null)
            {
                fObj = new JObject();
                _primaryObj.Add(new JProperty("frequency", fObj));
            }

            // 获取特定的配置
            var fProp = fObj.SelectToken(shortcutName);
            if (fProp == null) fObj.Add(new JProperty(shortcutName, frequency));
            else fObj[shortcutName] = frequency;

            // 保存
            FileStream fileStream = new FileStream(ConfigPath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fileStream);
            sw.Write(JsonConvert.SerializeObject(_primaryObj, Formatting.Indented));
            sw.Close();
            fileStream.Close();
        }
    }
}
