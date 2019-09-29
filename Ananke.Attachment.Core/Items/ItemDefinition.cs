namespace Ananke.Attachment.Core.Items
{
    public class ItemDefinition
    {
        public ItemDefinition(long id, string mnemonic, ItemActivator activator)
        {
            Id = id;
            Mnemonic = mnemonic;
            Activator = activator;
        }
        
        public long Id { get; }
        public string Mnemonic { get; }
        
        public ItemActivator Activator { get; }
        
    }
}