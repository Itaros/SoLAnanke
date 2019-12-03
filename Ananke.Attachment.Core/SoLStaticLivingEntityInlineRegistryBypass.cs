using SignsOfLife.Entities;

namespace Ananke.Attachment.Core
{
    public class SoLStaticLivingEntityInlineRegistryBypass
    {
        public static LivingEntityType GetNewLivingEntityTypeByName(string name)
        {
            var definition = AnankeContext.Current.LivingEntityRegistry.GetByMnemonic(name);
            return  (LivingEntityType)(definition?.Id ?? (long)LivingEntityType.NULL);
        }

        public static LivingEntity GetNewLivingEntityByStaticPrefabType(LivingEntityType staticPrefabType)
        {
            long unwrapExtended = (long) staticPrefabType;
            var definition = AnankeContext.Current.LivingEntityRegistry.GetById(unwrapExtended);
            return definition?.Activator.Activate();
        }
    }
}