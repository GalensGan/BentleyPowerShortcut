using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwTools.PowerShortcut.Models
{
    class ShortcutsHistory
    {
        private int _limit = 20;
        public List<Shortcut> History { get; set; } = new List<Shortcut>();

        public void Push(Shortcut shortcut)
        {
            // 判断是否是空命令
            if (shortcut == null || string.IsNullOrEmpty(shortcut.Keyin)) return;

            // 判断最后一个是否与当前相同
            var last = History.LastOrDefault();
            if (last != null && last.Name == shortcut.Name) return;

            if (History.Count == _limit)
            {
                History.RemoveAt(0);
            }
            History.Add(shortcut);
            _currentIndex = History.Count - 1;
        }

        private int _currentIndex = 0;

        public Shortcut Previous()
        {
            if (History.Count < 1) return null;

            var result = History[_currentIndex];

            _currentIndex -= 1;
            if (_currentIndex < 0)
            {
                _currentIndex += History.Count;
            }

            return result;
        }

        public Shortcut Next()
        {
            if (History.Count < 1) return null;

            var result = History[_currentIndex];

            _currentIndex += 1;
            if (_currentIndex >= History.Count)
            {
                _currentIndex -= History.Count;
            }

            return result;
        }
    }
}
