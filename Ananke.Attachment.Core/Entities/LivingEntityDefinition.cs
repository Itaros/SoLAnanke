namespace Ananke.Attachment.Core.Entities
{
    public class LivingEntityDefinition
    {
        public LivingEntityDefinition(long id, string mnemonic, LivingEntityActivator activator)
        {
            Id = id;
            Mnemonic = mnemonic;
            Activator = activator;
        }
        
        public long Id { get; }
        
        public string Mnemonic { get; }
        
        public LivingEntityActivator Activator { get; }
    }
}