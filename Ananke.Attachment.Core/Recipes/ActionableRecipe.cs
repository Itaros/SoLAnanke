using System.Collections.Generic;
using SignsOfLife.Crafting;

namespace Ananke.Attachment.Core.Recipes
{
    /**
     * Interface to manipulate existing recipes in the game
     */
    public class ActionableRecipe
    {

        internal ActionableRecipe(Recipe recipe, List<Recipe> rootCollection)
        {
            Recipe = recipe;
            _internalRootCollection = rootCollection;
        }

        private List<Recipe> _internalRootCollection;
        public Recipe Recipe { get; private set; }

        public void Remove()
        {
            _internalRootCollection.RemoveAll(o => o == Recipe);
        }

        public void Reinstate()
        {
            if(!ProbeForPresence())
                _internalRootCollection.Add(Recipe);
        }

        public bool ProbeForPresence()
        {
            return _internalRootCollection.Exists(o=>o==Recipe);
        }
        
    }
}