using Bentley.DgnPlatformNET;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwTools.PowerShortcut.Models
{
    class ShortcutConfig
    {
        private readonly string shortcutFieldName = "shortcuts";
        private static ShortcutConfig _instance;

        private JObject _config = null;
        private ShortcutConfig()
        {
            string configPath = ConfigurationManager.GetVariable("_USTN_HOMEROOT")+"shortcutsConfig.json";

            if (!File.Exists(configPath)) {
                // 请添加
                System.Windows.Forms.MessageBox.Show($"请添加快捷键配置文件:{configPath}");
                return;
            }

            string configStr = File.ReadAllText(configPath);
            _config = JsonConvert.DeserializeObject<JObject>(configStr);
        }

        public static ShortcutConfig Instance
        {
            get
            {
                if (_instance == null) _instance = new ShortcutConfig();

                return _instance;
            }
        }

        public void Reload()
        {
            _instance = new ShortcutConfig();
        }

        public Shortcut GetShortcut(string name)
        {
            Shortcut definition = new Shortcut();
            if (_config == null) return definition;

            JArray jArray = _config.Value<JArray>(shortcutFieldName);
            // 从里面找到name相等的值            
            foreach (JObject jobj in jArray)
            {
                JArray jNames = jobj.Value<JArray>("names");
                if (jNames == null || jNames.Count<1) continue;

               List<string> namesTemp = jNames.ToList().ConvertAll(item => item.Value<string>().ToLower());

                if (!namesTemp.Contains(name.ToLower())) continue;

                // 获取数据
                definition.Name = jobj.Value<string>("name");
                definition.Keyin = jobj.Value<string>("keyin");
                definition.Description = jobj.Value<string>("description");

                break;
            }

            return definition;
        }

        public void UpdateShortcut(Shortcut shortcut) {
            Shortcut temp = GetShortcut(shortcut.Name);
            shortcut.Keyin = temp.Keyin;
            shortcut.Description = temp.Description;
        }

        public string TabFullName(string name)
        {
            if (_config == null) return string.Empty;

            JArray jArray = _config.Value<JArray>(shortcutFieldName);
            string lowerName = name.ToLower();
            string shortcut = name;

            // 从里面找到name相等的值            
            foreach (JObject jobj in jArray)
            {
                JArray jNames = jobj.Value<JArray>("names");
                if (jNames == null || jNames.Count < 1) continue;

                List<string> namesTemp = jNames.ToList().ConvertAll(item => item.Value<string>().ToLower());

                string shortcutTemp = namesTemp.Find(n => n.Contains(lowerName));
                if (string.IsNullOrEmpty(shortcutTemp)) continue;

                shortcut = shortcutTemp;
                break;
            }

            return shortcut;
        }
    }
}
