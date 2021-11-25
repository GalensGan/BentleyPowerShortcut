using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WowuTool.PowerShortcut.Models
{
    /// <summary>
    /// 单个快捷键
    /// </summary>
    class Shortcut
    {
        /// <summary>
        /// 快捷键名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// keyin 命令
        /// </summary>
        public string Keyin { get; set; }

        /// <summary>
        /// 执行一系列命令
        /// </summary>
        public List<string> Keyins { get; set; } = new List<string>();

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; } = "无";

        /// <summary>
        /// 使用频率
        /// </summary>
        public double Frequency { get; set; } = 0;

        /// <summary>
        /// 运行 keyin
        /// </summary>
        public bool RunKeyin()
        {
            if (string.IsNullOrEmpty(Keyin) && Keyins.Count < 1) return false;

            if (!string.IsNullOrEmpty(Keyin)) Bentley.MstnPlatformNET.Session.Instance.Keyin(Keyin);

            // 执行keyins数组
            foreach (var keyin in Keyins)
            {
                Bentley.MstnPlatformNET.Session.Instance.Keyin(keyin);
            }

            // 保存使用次数
            Frequency++;

            // 保存使用情况到本地
            ShortcutConfig.Instance.SaveFrequency(Name, Frequency);

            return true;
        }

        public static Shortcut GetNullShortcut()
        {
            return new Shortcut()
            {
                Name = "未匹配到快捷键",
                Description = "可输入空格或回车打开上一次命令"
            };
        }
    }
}
