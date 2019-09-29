namespace Ananke.Attachment.Core
{
    public static class SoLInventoryItemHandlerBypass
    {
        public static SignsOfLife.Entities.Items.InventoryItem GetNewItemByItemType(
            SignsOfLife.Entities.Items.InventoryItemType id)
        {
            var items = AnankeContext.Current.ItemsRegistry;
            var definition = items.GetById((long)id);
            if (definition == null)
                return null;
            return definition.Activator.Activate();
        }
    }
}