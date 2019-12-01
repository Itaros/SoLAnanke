using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ananke.Attachment.Core.Mod;

namespace Ananke.Attachment.Core.Loader
{
    public class ModLoader
    {

        private DirectoryInfo _modsDir;
        
        private List<ModContext> _loadedMods = new List<ModContext>();
        
        public ModLoader(DirectoryInfo modsDir)
        {
            _modsDir = modsDir;
        }

        public void LoadMods()
        {
            var modDirectories = _modsDir.EnumerateDirectories();
            Console.WriteLine($"Found {modDirectories.Count()} mods. Loading...");
            foreach (var modDirectory in modDirectories)
            {
                Console.WriteLine($"Loading {modDirectory.Name}...");
                ModActivator activator = new ModActivator(modDirectory);
                _loadedMods.AddRange(activator.Activate());
            }
            Console.WriteLine("...Done!");
        }
        
    }
}