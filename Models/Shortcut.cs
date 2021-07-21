using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwTools.PowerShortcut.Models
{
    class Shortcut
    {
        public string Name { get; set; }

        public string Keyin { get; set; }

        public string Description { get; set; } = "无";

        public string Update()
        {
            // 通过 name 来更新 descriotion
            // 读取配置文件
            ShortcutConfig.Instance.UpdateShortcut(this);

            return null;
        }
    }
}
