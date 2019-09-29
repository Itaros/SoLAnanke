using System.IO;
using Ananke.Attachment.Core;
using Ananke.Attachment.Core.Items;
using Ananke.Attachment.Core.Mod;
using Microsoft.Xna.Framework.Graphics;
using Runtime.ExampleMod;
using SignsOfLife;

namespace Runtime
{
    public class Mod : ISoLModV1
    {
        public void Init(AnankeContext context)
        {

            Texture2D sprite;
            using (FileStream fileStream =
                new FileStream(@"C:\MODDING\Signs of Life\mods\example\assets\goldenchicken.png", FileMode.Open))
            {
                sprite = Texture2D.FromStream(SpaceGame.GetGraphicsDevice(), fileStream);
            }

            context.ItemsRegistry.Add(
                new ItemDefinition(
                    50001, 
                    "examplemod_goldenchicken", 
                    new ItemActivator(()=>new GoldenChickenItem(50001, sprite))));
        }
    }
}