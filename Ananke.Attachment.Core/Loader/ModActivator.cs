using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Ananke.Attachment.Core.Mod;

namespace Ananke.Attachment.Core.Loader
{
    public class ModActivator
    {

        private DirectoryInfo _modDirectory;
        public ModActivator(DirectoryInfo modDirectory)
        {
            _modDirectory = modDirectory;
        }

        public IEnumerable<ISoLModV1> Activate()
        {
            List<ISoLModV1> mods = new List<ISoLModV1>();
            
            var runtime = _modDirectory.EnumerateFiles().First(
                f=>String.Equals(f.Name,"Runtime.dll", StringComparison.InvariantCultureIgnoreCase));
            var modAss = Assembly.LoadFile(runtime.FullName);

            var primaryModule = modAss.Modules.First();
            var modEntries = primaryModule.GetTypes().Where(t=>typeof(ISoLModV1).IsAssignableFrom(t));
            foreach (var modEntry in modEntries)
            {
                ISoLModV1 mod = (ISoLModV1) Activator.CreateInstance(modEntry);
                mod.Init(AnankeContext.Current);
                mods.Add(mod);
            }

            return mods;
        }
    }
}