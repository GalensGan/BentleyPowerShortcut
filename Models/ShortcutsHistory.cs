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
        public List<string> History { get; set; } = new List<string>();

        public void Push(string shortcutName) {
            if (string.IsNullOrEmpty(shortcutName)) return;

            if (History.Count == _limit)
            {
                History.RemoveAt(0);
            }
            History.Add(shortcutName);
            _currentIndex = History.Count - 1;
        }

        private int _currentIndex = 0;

        public string Previous()
        {
            if (History.Count < 1) return string.Empty;

            _currentIndex -= 1;
            if (_currentIndex < 0)
            {
                _currentIndex += History.Count;
            }

            return History[_currentIndex];
        }

        public string Next()
        {
            if (History.Count < 1) return string.Empty;

            _currentIndex += 1;
            if (_currentIndex >= History.Count)
            {
                _currentIndex -= History.Count;
            }

            return History[_currentIndex];
        }
    }
}
