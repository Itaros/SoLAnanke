using System;
using System.Collections.Generic;
using System.Linq;

namespace Ananke.Attachment.Core.Entities
{
    public class LivingEntityRegistry
    {
        public LivingEntityRegistry()
        {
            
        }

        public void Add(LivingEntityDefinition definition)
        {
            if(definition == null)
                throw new ArgumentException();
            var conflict = _definitions.FirstOrDefault(d => d.Id == definition.Id);
            if(conflict != null)
                throw new InvalidOperationException($"Attempting to add conflicting definition: {definition.Mnemonic}->{conflict.Mnemonic} at {conflict.Id}");
            _definitions.Add(definition);
        }

        public LivingEntityDefinition GetById(long id)
        {
            //TODO: OPTIMIZE
            return _definitions.FirstOrDefault(d => d.Id == id);
        }

        public LivingEntityDefinition GetByMnemonic(string mnemonic)
        {
            //TODO: Optimize
            return _definitions.FirstOrDefault(d => d.Mnemonic == mnemonic);
        }
        
        private List<LivingEntityDefinition> _definitions = new List<LivingEntityDefinition>();

        internal void Reset()
        {
            _definitions.Clear();
        }
    }
}