namespace Ananke.Attachment.Core.Mod
{
    public interface ISoLModV1 : ISoLMod
    {
        void Init(AnankeContext ananke, IModContextV1 ctx);
    }
}