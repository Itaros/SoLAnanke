using System;

namespace Ananke.Attachment.Core.Entities
{
    public class LivingEntityActivator
    {
        public LivingEntityActivator(Func<SignsOfLife.Entities.LivingEntity> activatorFunctor)
        {
            _activationFunctor = activatorFunctor;
        }

        public SignsOfLife.Entities.LivingEntity Activate()
        {
            return _activationFunctor();
        }
        
        private Func<SignsOfLife.Entities.LivingEntity> _activationFunctor;
    }
}