﻿using System.IO;
using System.Linq;
using Ananke.Attachment.Core;
using Ananke.Attachment.Core.Entities;
using Ananke.Attachment.Core.Graphics;
using Ananke.Attachment.Core.Items;
using Ananke.Attachment.Core.Mod;
using Ananke.Attachment.Core.Phases;
using Ananke.Attachment.Core.Recipes;
using Ananke.Attachment.Core.StaticPrefabs;
using Ananke.Attachment.Core.Tools;
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
        public string Name => "Example Mod";

        public void Init(AnankeContext context, IModContextV1 mod)
        {
            PathHelper path = new PathHelper(mod as ModContext);

                var spriteGoldenChicken = AnankeContext.Current.DumbGraphicsRegistry.DefineResource(
                path.GetResource(@"assets\goldenchicken.png"),
                new Rectangle(17, 10, 32, 44));
            
            var spriteGoldenChickenMagnument = AnankeContext.Current.DumbGraphicsRegistry.DefineResource(
                path.GetResource(@"assets\goldenchicken_magnument.png"),
                new Rectangle(0, 0, 64, 64));            

            var itemDefinitionGoldenChicken = new ItemDefinition(
                50001,
                "examplemod_goldenchicken",
                new ItemActivator(() => new GoldenChickenItem(50001, spriteGoldenChicken)));
            context.ItemsRegistry.Add(itemDefinitionGoldenChicken);

            //TODO: MY GOD!!! IT IS SO UGLY!
            long sptMagnumentId = 2000;
            var staticPrefabDefintionMagnument = new StaticPrefabDefinition(
                sptMagnumentId,
                "golden_chincken_magnument",
                new StaticPrefabActivator(
                    () => new GoldenChickenMagnument(sptMagnumentId,
                        spriteGoldenChickenMagnument)
                )
            );
            context.StaticPrefabRegistry.Add(staticPrefabDefintionMagnument);
            
            //Living entites
            var myAliveGoldenChicken = new LivingEntityDefinition(
                10001,
                "GCHICK", new LivingEntityActivator(
                    () => new AliveGoldenChicken(10001)
                )
            );
            context.LivingEntityRegistry.Add(myAliveGoldenChicken);
            
            context.Compatibility.V1Compat.AddActionForRecipeLoadingPhase(
                GetType(), ctx =>
                {
                    var category = ctx.RecipeRegistry.GetRecipeCategory("Swag");

                    ctx.RecipeRegistry.CreateRecipe(
                        "Golden Chicken Statue",
                        category, RecipeRegistry.SoLCraftingCategory.FURNITURE,
                        new[]
                        {
                            new RequiredRecipeMaterial(
                                InventoryItemType.GOLD_INGOT,
                                1),
                        },
                        new []{new RecipeRegistry.ItemRecipeOutcome(itemDefinitionGoldenChicken.Id, 1)}
                    );

                    ctx.RecipeRegistry.CreateRecipe(
                        "Golden Chicken Magnument",
                        category, RecipeRegistry.SoLCraftingCategory.PROJECTS,
                        new[]
                        {
                            new RequiredRecipeMaterial(
                                InventoryItemType.GOLD_INGOT,
                                5),
                        },
                        new []{new RecipeRegistry.StaticPrefabRecipeOutcome(staticPrefabDefintionMagnument.Id)}
                    );

                    //Vanilla recipe deletion
                    var fancyRecipe = ctx.RecipeRegistry.Discover(r => r.Name == "Wooden Chair");
                    foreach (var actionableRecipe in fancyRecipe)
                    {
                        actionableRecipe.Remove();
                    }
                }
            );
        }
    }
}