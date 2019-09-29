using System;

namespace Ananke.Attachment.Core.Items
{
    public class ItemActivator
    {
        public ItemActivator(Func<SignsOfLife.Entities.Items.InventoryItem> activatorFunctor)
        {
            _activationFunctor = activatorFunctor;
        }

        public SignsOfLife.Entities.Items.InventoryItem Activate()
        {
            return _activationFunctor();
        }
        
        private Func<SignsOfLife.Entities.Items.InventoryItem> _activationFunctor;

    }
}