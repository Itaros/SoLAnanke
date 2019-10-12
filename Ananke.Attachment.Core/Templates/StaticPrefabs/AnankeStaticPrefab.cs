using SignsOfLife.Prefabs.StaticPrefabs;

namespace Ananke.Attachment.Core.Templates.StaticPrefabs
{
    public class AnankeStaticPrefab : StaticPrefab
    {
        public AnankeStaticPrefab(string prefabName, Tilemap tilemap, long id) 
            : base(prefabName, tilemap.ConvertToSoLTileData(), (StaticPrefabType)id)
        {
            
        }
    }
}