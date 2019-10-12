using SignsOfLife.Prefabs.StaticPrefabs;

namespace Ananke.Attachment.Core
{
    public static class SoLStaticPrefabInlineRegistryBypass
    {
        public static StaticPrefab GetNewStaticPrefabByName(string name)
        {
            var definition = AnankeContext.Current.StaticPrefabRegistry.GetByNetworkAndSchematicName(name);
            return definition?.Activator.Activate();
        }

        public static StaticPrefab GetNewStaticPrefabByStaticPrefabType(StaticPrefabType staticPrefabType)
        {
            long unwrapExtended = (long) staticPrefabType;
            var definition = AnankeContext.Current.StaticPrefabRegistry.GetById(unwrapExtended);
            return definition?.Activator.Activate();
        }
        
    }
}