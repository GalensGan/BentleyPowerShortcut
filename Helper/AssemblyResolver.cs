using Bentley.DgnPlatformNET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WowuTool.PowerShortcut.Helper
{
    /// <summary>
    /// 程序集解析失败时进行调用
    /// </summary>
    internal class AssemblyResolver
    {
        public AssemblyResolver()
        {
            // 注册事件
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private List<Assembly> _allAssemblies = null;
        private System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (_allAssemblies == null)
            {
                _allAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            }

            var assemblyName = new AssemblyName(args.Name);

            // 从已经加载的程序集中查找替换
            var target = _allAssemblies.Find(x => x.GetName().Name == assemblyName.Name);

            // 如果没有找到，从安装目录中去查找
            if (target == null)
            {
                string assembliesDir = Path.Combine(ConfigurationManager.GetVariable("_ROOTDIR"), "Assemblies");
                var files = new DirectoryInfo(assembliesDir).GetFiles("*.dll", SearchOption.AllDirectories).ToList();
                // 找到文件
                var nameWithExtension = $"{assemblyName.Name}.dll";
                var fileInfo = files.Find(x => x.Name == nameWithExtension);
                if (fileInfo != null)
                {
                    target = Assembly.LoadFile(fileInfo.FullName);
                    _allAssemblies.Add(target);
                }
            }

            return target;
        }
    }
}
