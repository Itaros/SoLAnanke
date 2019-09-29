using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SignsOfLife.Entities.Items;

namespace Runtime.ExampleMod
{
    public class GoldenChickenItem : InventoryItem
    {
        public GoldenChickenItem(long id, Texture2D texture) : base(0f, 0f, texture, new Rectangle(17,10, 32, 44))
        {
            Category = "Example Item";
            Stackable = true;
            Name = "Golden Chicken Statue";
            Description = "Fancy Example Item";
            NewInventoryItemType = (InventoryItemType)id;
        }
    }
}