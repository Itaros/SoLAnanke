using System.IO;
using Ananke.Attachment.Core.Mod;

namespace Ananke.Attachment.Core.Tools
{
    public class PathHelper
    {
        public PathHelper(ModContext context)
        {
            localDir = context.ModDirectory;
        }

        private DirectoryInfo localDir;

        public FileInfo GetResource(string relativePath)
        {
            return new FileInfo(Path.Combine(localDir.FullName, relativePath));
        }
        
    }
}