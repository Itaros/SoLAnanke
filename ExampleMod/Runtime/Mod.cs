using System.IO;
using Ananke.Attachment.Core;
using Ananke.Attachment.Core.Graphics;
using Ananke.Attachment.Core.Items;
using Ananke.Attachment.Core.Mod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Runtime.ExampleMod;
using SignsOfLife;

namespace Runtime
{
    public class Mod : ISoLModV1
    {
        public void Init(AnankeContext context)
        {

            var spriteGoldenChicken = AnankeContext.Current.DumbGraphicsRegistry.DefineResource(
                new FileInfo(@"C:\MODDING\Signs of Life\mods\example\assets\goldenchicken.png"),
                new Rectangle(17, 10, 32, 44));

            context.ItemsRegistry.Add(
                new ItemDefinition(
                    50001, 
                    "examplemod_goldenchicken", 
                    new ItemActivator(()=>new GoldenChickenItem(50001, spriteGoldenChicken))));
        }
    }
}