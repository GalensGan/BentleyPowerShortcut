using Bentley.DgnPlatformNET;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WowuTool.PowerShortcut.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WowuTool.PowerShortcut.Models
{
    class ShortcutConfig
    {
        private static ShortcutConfig _instance;

        private List<Shortcut> _shortcuts = new List<Shortcut>();

        private JObject _primaryObj;

        public static string ConfigPath { get; private set; } = ConfigurationManager.GetVariable("_USTN_HOMEROOT") + "shortcutsConfig.json";

        private ShortcutConfig()
        {
            LoadConfig();
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
        /// 加载配置文件
        /// </summary>
        private bool LoadConfig()
        {
            if (!File.Exists(ConfigPath))
            {
                // 请添加
                System.Windows.Forms.MessageBox.Show($"请添加快捷键配置文件:{ConfigPath}");
                return false;
            }
            try
            {

                string configStr = File.ReadAllText(ConfigPath);
                _primaryObj = JsonConvert.DeserializeObject<JObject>(configStr);

                // 添加系统配置
                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("WowuTool.PowerShortcut.Models.systemShortcuts.json");
                StreamReader sr = new StreamReader(stream, Encoding.UTF8);
                var systemShortcutsConfig = sr.ReadToEnd();
                sr.Close();
                stream.Close();

                JObject sysShortcuts = JObject.Parse(systemShortcutsConfig);
                JObject configObj = _primaryObj.DeepClone() as JObject;
                configObj.Merge(sysShortcuts);

                // 获取最小频率，对所有频率进行缩小，避免太多不好看
                bool isNarrowFrequency = configObj.SelectValueOrDefault("settings.narrowFrequency", false);
                if (isNarrowFrequency)
                {
                    // 获取所有的最小值
                    JObject frequencyObj = configObj.SelectValueOrDefault("frequency", new JObject());
                    var keys = frequencyObj.Properties();
                    if (keys.Count() > 1)
                    {
                        Dictionary<string, double> frequencyDic = new Dictionary<string, double>();
                        foreach (var key in keys)
                        {
                            frequencyDic.Add(key.Name, key.Value.Value<double>());
                        }

                        // 开始精简
                        List<double> frequencyLs = frequencyDic.Values.Distinct().OrderBy(item => item).ToList();
                        // 重新生成频率
                        foreach (var key in keys)
                        {
                            var value = frequencyDic[key.Name];
                            var index = frequencyLs.FindIndex(item => item == value);
                            configObj["frequency"][key.Name] = index + 1;
                        }
                    }
                }


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
                            Keyins = jt.SelectValueOrDefault("keyins", new JArray()).ToObject<List<string>>(),
                            Description = jt.Value<string>("description"),
                            Frequency = jt.Value<double>("frequency")
                        };

                        // 从频率表中读取频率数据
                        var sysF = configObj.SelectValueOrDefault($"frequency.{shortcut.Name}", 0);
                        shortcut.Frequency += sysF;

                        // 保存
                        _shortcuts.Add(shortcut);
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
                return false;
            }
        }

        /// <summary>
        /// 重新加载配置文件
        /// </summary>
        public void Reload()
        {
            // 先备份原来的 shortcuts
            List<Shortcut> temp = new List<Shortcut>(_shortcuts);
            _shortcuts.Clear();

            // 加载失败后恢复原来的快捷键
            if (!LoadConfig())
            {
                _shortcuts.AddRange(temp);
            }
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

            Regex regex = new Regex(pattern,RegexOptions.IgnoreCase);

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

        /// <summary>
        /// 保存使用频率
        /// </summary>
        /// <param name="shortcutName"></param>
        /// <param name="frequency"></param>
        public void SaveFrequency(string shortcutName, double frequency)
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
