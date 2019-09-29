using System.IO;
using System.Linq;

namespace Installer
{
    public class DetectSoL
    {

        public DetectSoL()
        {
            
        }
        
        public FileInfo GetSoLMainExecutable()
        {
            //"Signs of Life.exe"
            DirectoryInfo pwd = new DirectoryInfo(Directory.GetCurrentDirectory());
            FileInfo candidate = pwd.EnumerateFiles().FirstOrDefault(f => f.Name == "Signs of Life.exe");
            return candidate;
        }
        
    }
}