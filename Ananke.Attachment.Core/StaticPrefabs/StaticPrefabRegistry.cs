using System;
using System.Collections.Generic;
using System.Linq;

namespace Ananke.Attachment.Core.StaticPrefabs
{
    public class StaticPrefabRegistry
    {

        internal void Reset()
        {
            _definitions.Clear();
        }
        
        public void Add(StaticPrefabDefinition definition)
        {
            if(definition == null)
                throw new ArgumentException();
            var conflict = _definitions.FirstOrDefault(
                d => d.Id == definition.Id ||
                     d.SchematicAndNetworkName == definition.SchematicAndNetworkName
            );
            if (conflict != null)
                throw new InvalidOperationException(
                    $"Attempting to add conflicting definition: {definition.SchematicAndNetworkName}->{conflict.SchematicAndNetworkName} at {conflict.Id}");
            _definitions.Add(definition);
        }

        public void Add(params StaticPrefabDefinition[] definitions)
        {
            foreach (var definition in definitions)
            {
                Add(definition);
            }
        }
        
        public StaticPrefabDefinition GetById(long id)
        {
            return _definitions.FirstOrDefault(d=>d.Id==id);
        }

        public StaticPrefabDefinition GetByNetworkAndSchematicName(string name)
        {
            return _definitions.FirstOrDefault(d => d.SchematicAndNetworkName == name);
        }
        
        private List<StaticPrefabDefinition> _definitions = new List<StaticPrefabDefinition>();
        
    }
}