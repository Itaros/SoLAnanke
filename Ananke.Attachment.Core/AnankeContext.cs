using Ananke.Attachment.Core.Entities;
using Ananke.Attachment.Core.Graphics;
using Ananke.Attachment.Core.Items;
using Ananke.Attachment.Core.Mod;
using Ananke.Attachment.Core.Phases;
using Ananke.Attachment.Core.Recipes;
using Ananke.Attachment.Core.StaticPrefabs;

namespace Ananke.Attachment.Core
{
    public class AnankeContext
    {

        public AnankeContext()
        {
            Compatibility = new CompatibilityProviders(this);
        }
        
        public static AnankeContext Current { get; } = new AnankeContext();
        
        public long Version => 10000;

        public ItemsRegistry ItemsRegistry { get; } = new ItemsRegistry();

        public LivingEntityRegistry LivingEntityRegistry { get; } = new LivingEntityRegistry();
        
        public StaticPrefabRegistry StaticPrefabRegistry { get; } = new StaticPrefabRegistry();

        public PhaseController PhaseController { get; } = new PhaseController();
        
        public CompatibilityProviders Compatibility { get; }

        public DumbGraphicsRegistry DumbGraphicsRegistry { get; } = new DumbGraphicsRegistry();

        public RecipeRegistry RecipeRegistry { get; } = new RecipeRegistry();

    }
}