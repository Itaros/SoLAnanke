using System.IO;

namespace Ananke.Attachment.Core.Mod
{
    public class ModContext : IModContextV1
    {

        public ModContext(ISoLMod mod,  DirectoryInfo modDirectory)
        {
            ModDirectory = modDirectory;
        }
        
        public DirectoryInfo ModDirectory { get; }

        public ISoLMod Mod { get; }

        public ISoLModV1 ModV1 => Mod as ISoLModV1;
        
    }
}