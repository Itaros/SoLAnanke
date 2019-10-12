using Ananke.Attachment.Core.Graphics;
using Ananke.Attachment.Core.Items;
using Ananke.Attachment.Core.StaticPrefabs;

namespace Ananke.Attachment.Core
{
    public class AnankeContext
    {
        public static AnankeContext Current { get; } = new AnankeContext();
        
        public long Version => 10000;

        public ItemsRegistry ItemsRegistry { get; } = new ItemsRegistry();
        
        public StaticPrefabRegistry StaticPrefabRegistry { get; } = new StaticPrefabRegistry();

        public DumbGraphicsRegistry DumbGraphicsRegistry { get; } = new DumbGraphicsRegistry();

    }
}