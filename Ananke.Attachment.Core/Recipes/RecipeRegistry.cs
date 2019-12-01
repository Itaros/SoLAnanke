using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SignsOfLife.Crafting;
using SignsOfLife.Entities.Items;
using SignsOfLife.Prefabs.StaticPrefabs;

namespace Ananke.Attachment.Core.Recipes
{
    public class RecipeRegistry
    {

        /// <summary>
        /// Retrieves Recipe Category.
        /// Recipe Categories are subgroups of recipes like Tables, Chairs etc.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public RecipeCategory GetRecipeCategory(string name)
        {
            var known = _knownCategories.FirstOrDefault(c => c.SoLName == name);
            if (known != null)
                return known;
            var existingCategory = SignsOfLife.UI.Gumps.CraftingWindow.AllCategories.FirstOrDefault(c => c == name);
            RecipeCategory cat = new RecipeCategory(name);
            _knownCategories.Add(cat);
            if (existingCategory == null)
                SignsOfLife.UI.Gumps.CraftingWindow.AllCategories.Add(name);
            return cat;
        }
        
        private readonly List<RecipeCategory> _knownCategories = new List<RecipeCategory>();

        public enum SoLCraftingCategory
        {
            MELEE,
            RANGED,
            EQUIPMENT,
            GADGETS,
            TOOLS,
            MATERIALS,
            CLOTHING,
            CONSUMABLES,
            FURNITURE,
            TILES,
            PROJECTS
        }

        public abstract class AbstractRecipeOutcome
        {

            public AbstractRecipeOutcome(int amount)
            {
                Amount = amount;
            }
            
            public int Amount { get; }
            
        }

        public class ItemRecipeOutcome : AbstractRecipeOutcome
        {
            public ItemRecipeOutcome(long itemId, int amount) : base(amount)
            {
                ItemId = itemId;
            }
            
            public long ItemId { get; }
            
        }

        public class StaticPrefabRecipeOutcome : AbstractRecipeOutcome
        {
            public StaticPrefabRecipeOutcome(long staticPrefabId) : base(1)
            {
                PrefabId = staticPrefabId;
            }
            
            public long PrefabId { get; }
        }

        public class BlockRecipeOutcome : AbstractRecipeOutcome
        {
            public BlockRecipeOutcome(int blockid, int amount) : base(amount)
            {
                BlockId = blockid;
            }

            public int BlockId { get; }

        }
        
        public Recipe CreateRecipe(
            string recipeName, 
            RecipeCategory category, 
            SoLCraftingCategory craftingCategory,
            IEnumerable<RequiredRecipeMaterial> requiredMaterials,
            AbstractRecipeOutcome outcome)
        {

            string craftingCategorySoL = craftingCategory.ToString().Substring(0, 1).ToUpperInvariant() +
                                         craftingCategory.ToString().Substring(1).ToLowerInvariant();

            ResultRecipeItem result;
            switch (outcome)
            {
                case ItemRecipeOutcome itemRecipeOutcome:
                    result = new ResultRecipeItem((InventoryItemType) itemRecipeOutcome.ItemId, itemRecipeOutcome.Amount);
                    break;
                case StaticPrefabRecipeOutcome staticPrefabRecipeOutcome:
                    result = new ResultRecipeItem(InventoryItemType.NULL, staticPrefabRecipeOutcome.Amount)
                    {
                        StaticPrefabType = (StaticPrefabType)staticPrefabRecipeOutcome.PrefabId
                    };
                    break;
                case BlockRecipeOutcome blockRecipeOutcome:
                    result = new ResultRecipeItem(InventoryItemType.NULL, blockRecipeOutcome.Amount)
                    {
                        _blockID = blockRecipeOutcome.BlockId
                    };
                    break;
                default:
                    throw new ArgumentException("Unable to determine outcome policy!");
            }

            var recipe = new Recipe
            {
                Name = recipeName,
                AutoGrant = false,
                Category = category.SoLName,
                CraftingCategory = craftingCategorySoL,
                HideUnlessDebug = false,
                RequiredMaterials = requiredMaterials.ToList(),
                ResultItem = result
            };
            
            SignsOfLife.UI.Hud._allRecipes.Add(recipe);

            return recipe;
        }


        public IEnumerable<ActionableRecipe> Discover(Func<Recipe, bool> predicate)
        {
            var source = SignsOfLife.UI.Hud._allRecipes;
            List<ActionableRecipe> candidates = new List<ActionableRecipe>();
            foreach (var recipe in source)
            {
                if (predicate(recipe))
                {
                    candidates.Add(new ActionableRecipe(recipe, source));
                }
            }

            return candidates;
        }
    }
}