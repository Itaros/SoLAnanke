using System;
using System.Collections.Generic;
using System.Linq;

namespace Ananke.Attachment.Core.Items
{
    public class ItemsRegistry
    {

        public ItemsRegistry()
        {
            
        }

        public void Add(ItemDefinition definition)
        {
            if(definition == null)
                throw new ArgumentException();
            var conflict = _definitions.FirstOrDefault(d => d.Id == definition.Id);
            if(conflict != null)
                throw new InvalidOperationException($"Attempting to add conflicting definition: {definition.Mnemonic}->{conflict.Mnemonic} at {conflict.Id}");
            _definitions.Add(definition);
        }

        public ItemDefinition GetById(long id)
        {
            //TODO: OPTIMIZE
            return _definitions.FirstOrDefault(d => d.Id == id);
        }

        public ItemDefinition GetByMnemonic(string mnemonic)
        {
            //TODO: Optimize
            return _definitions.FirstOrDefault(d => d.Mnemonic == mnemonic);
        }
        
        private List<ItemDefinition> _definitions = new List<ItemDefinition>();

        public void Reset()
        {
            _definitions.Clear();
        }
    }
}