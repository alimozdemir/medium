using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace hfdynamic
{
    public class DynamicTypeResolver
    {
        private Dictionary<string, Assembly> _assemblies;
        private DynamicLoad _loader;
        public DynamicTypeResolver()
        {
            _assemblies = new Dictionary<string, Assembly>();
            _loader = new DynamicLoad();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var asm in assemblies)
            {
                _assemblies.TryAdd(asm.GetName().Name, asm);
            }
        }

        public Assembly Load(string path)
        {
            var (asmName, asm) = _loader.Load(path);
            if (!_assemblies.ContainsKey(asmName.Name))
            {
                _assemblies.TryAdd(asmName.Name, asm);
            }

            return asm;
        }

        public Assembly AssemblyResolver (AssemblyName name)
        {
            _assemblies.TryGetValue(name.Name, out var asm);
            return asm;
        }

        public Type TypeResolver (Assembly asm, string title, bool caseSensetive)
        {
            return asm?.GetType(title);
        }

        public Type Find(string typeFull)
        {
            Type result = null;
            var t = typeFull.Split(',')[0];
            Console.WriteLine($"Looking for {t}");
            result = Type.GetType(
                    typeFull,
                    typeResolver: TypeResolver,
                    assemblyResolver: AssemblyResolver,
                    throwOnError: true);;

            return result;
        }

    }

    public class DynamicLoad
    {
        private const string pluginFolder = "plugins";
        public (AssemblyName, Assembly) Load(string pluginPath)
        {
            var path = Path.Combine(pluginFolder, pluginPath);

            var asmName = new AssemblyName(Path.GetFileNameWithoutExtension(path));

            PluginLoadContext loadContext = new PluginLoadContext(path);
            
            var asm = loadContext.LoadFromAssemblyName(asmName);

            return (asmName, asm);
        }
        public void Test ()
        {
            Console.WriteLine("hi");
        }
    }
}