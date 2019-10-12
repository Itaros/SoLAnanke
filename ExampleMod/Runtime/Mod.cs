using System.IO;
using System.Linq;
using Ananke.Attachment.Core;
using Ananke.Attachment.Core.Graphics;
using Ananke.Attachment.Core.Items;
using Ananke.Attachment.Core.Mod;
using Ananke.Attachment.Core.Phases;
using Ananke.Attachment.Core.StaticPrefabs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Runtime.ExampleMod;
using SignsOfLife.Crafting;
using SignsOfLife.Entities.Items;
using SignsOfLife.Prefabs.StaticPrefabs;

namespace Runtime
{
    // ReSharper disable once UnusedMember.Global
    public class Mod : ISoLModV1
    {
        public void Init(AnankeContext context)
        {
            var spriteGoldenChicken = AnankeContext.Current.DumbGraphicsRegistry.DefineResource(
                new FileInfo(@"C:\MODDING\Signs of Life\mods\example\assets\goldenchicken.png"),
                new Rectangle(17, 10, 32, 44));
            
            var spriteGoldenChickenMagnument = AnankeContext.Current.DumbGraphicsRegistry.DefineResource(
                new FileInfo(@"C:\MODDING\Signs of Life\mods\example\assets\goldenchicken_magnument.png"),
                new Rectangle(0, 0, 64, 64));            

            var itemDefinitionGoldenChicken = new ItemDefinition(
                50001,
                "examplemod_goldenchicken",
                new ItemActivator(() => new GoldenChickenItem(50001, spriteGoldenChicken)));
            context.ItemsRegistry.Add(itemDefinitionGoldenChicken);

            //TODO: MY GOD!!! IT IS SO UGLY!
            StaticPrefabType sptMagnument = (StaticPrefabType) 2000;
            var staticPrefabDefintionMagnument = new StaticPrefabDefinition(
                (long) sptMagnument,
                "golden_chincken_magnument",
                new StaticPrefabActivator(
                    (() => new GoldenChickenMagnument(sptMagnument,
                        spriteGoldenChickenMagnument)
                    )
                )
            );
            context.StaticPrefabRegistry.Add(staticPrefabDefintionMagnument);
            
            context.Compatibility.V1Compat.AddActionForRecipeLoadingPhase(
                GetType(), ctx =>
                {
                    SignsOfLife.UI.Gumps.CraftingWindow.AllCategories.Add("Swag");
                    //TODO: Too ugly
                    SignsOfLife.UI.Hud._allRecipes.Add(new Recipe
                    {
                        Name = "Golden Chicken Statue",
                        AutoGrant = true,
                        Category = "Swag",
                        CraftingCategory = "Furniture",
                        HideUnlessDebug = false,
                        RequiredMaterials = new[]
                        {
                            new RequiredRecipeMaterial(InventoryItemType.GOLD_INGOT, 1),
                        }.ToList(),
                        ResultItem = new ResultRecipeItem((InventoryItemType) itemDefinitionGoldenChicken.Id, 1)
                    });
                    //TODO: Too ugly
                    SignsOfLife.UI.Hud._allRecipes.Add(new Recipe
                    {
                        Name = "Golden Chicken Magnument",
                        AutoGrant = true,
                        Category = "Swag",
                        CraftingCategory = "Projects",
                        HideUnlessDebug = false,
                        RequiredMaterials = new[]
                        {
                            new RequiredRecipeMaterial(InventoryItemType.GOLD_INGOT, 5),
                        }.ToList(),
                        ResultItem = new ResultRecipeItem(InventoryItemType.NULL, 1)
                            {StaticPrefabType = (StaticPrefabType) staticPrefabDefintionMagnument.Id}
                    });
                }
            );
        }
    }
}