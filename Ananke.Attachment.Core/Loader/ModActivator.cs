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

        public IEnumerable<ModContext> Activate()
        {
            List<ModContext> mods = new List<ModContext>();

            var runtime = _modDirectory.EnumerateFiles().First(
                f => f.Extension == ".dll");
            var modAss = Assembly.LoadFile(runtime.FullName);

            var primaryModule = modAss.Modules.First();
            var modEntries = primaryModule.GetTypes().Where(t=>typeof(ISoLModV1).IsAssignableFrom(t));
            foreach (var modEntry in modEntries)
            {
                ISoLModV1 mod = (ISoLModV1) Activator.CreateInstance(modEntry);
                ModContext ctx = new ModContext(mod, _modDirectory);
                mod.Init(AnankeContext.Current, ctx);
                mods.Add(ctx);
            }

            return mods;
        }
    }
}