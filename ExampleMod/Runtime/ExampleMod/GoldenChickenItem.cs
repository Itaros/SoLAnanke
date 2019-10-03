using Ananke.Attachment.Core.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SignsOfLife.Entities.Items;

namespace Runtime.ExampleMod
{
    public class GoldenChickenItem : InventoryItem
    {
        public GoldenChickenItem(long id, DumbGraphicsRegistry.ResourceDefinition texture) 
            : base(0f, 0f, texture.GetRenderable().Item1, texture.GetRenderable().Item2)
        {
            Category = "Example Item";
            Stackable = true;
            Name = "Golden Chicken Statue";
            Description = "Fancy Example Item";
            NewInventoryItemType = (InventoryItemType)id;
            texture.OnGraphicsReload += OnGraphicsReload;
        }

        private void OnGraphicsReload(DumbGraphicsRegistry.ResourceDefinition definition)
        {
            this.Texture = definition.GetRenderable().Item1;
            this.SpriteBounds = definition.GetRenderable().Item2;
        }
    }
}