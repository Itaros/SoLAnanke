namespace Ananke.Attachment.Core.Mod
{
    public class CompatibilityProviders
    {
        public CompatibilityProviders(AnankeContext context)
        {
            V1Compat = new SoLModV1CompatibilityProvider(context);
        }

        public SoLModV1CompatibilityProvider V1Compat { get; }
    }
}