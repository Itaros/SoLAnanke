using System.IO;

namespace Ananke.Attachment.Core.Mod
{
    public interface IModContextV1
    {
        
        DirectoryInfo ModDirectory { get; }
        
        ISoLModV1 ModV1 { get; }
        
    }
}