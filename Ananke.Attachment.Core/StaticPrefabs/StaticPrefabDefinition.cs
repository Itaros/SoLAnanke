namespace Ananke.Attachment.Core.StaticPrefabs
{
    public class StaticPrefabDefinition
    {

        public StaticPrefabDefinition(long id, string schematicAndNetworkName, StaticPrefabActivator activator)
        {
            Id = id;
            SchematicAndNetworkName = schematicAndNetworkName;
            Activator = activator;
        }
        
        public long Id { get; }
        public string SchematicAndNetworkName { get; }
        public StaticPrefabActivator Activator { get; }
        
    }
}