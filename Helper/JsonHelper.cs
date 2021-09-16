using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwTools.PowerShortcut.Helper
{
   static  class JsonHelper
    {
        public static T SelectValueOrDefault<T>(this JToken token, string jsonPath, T default_)
        {
            if (string.IsNullOrEmpty(jsonPath)) return default_;

            var value = token.SelectToken(jsonPath);
            if (value == null) return default_;

            return value.ToObject<T>();
        }
    }
}
