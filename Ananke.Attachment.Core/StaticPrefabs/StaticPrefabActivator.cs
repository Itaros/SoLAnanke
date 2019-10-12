using System;

namespace Ananke.Attachment.Core.StaticPrefabs
{
    public class StaticPrefabActivator
    {
        public StaticPrefabActivator(Func<SignsOfLife.Prefabs.StaticPrefabs.StaticPrefab> activatorFunctor)
        {
            _activationFunctor = activatorFunctor;
        }

        public SignsOfLife.Prefabs.StaticPrefabs.StaticPrefab Activate()
        {
            return _activationFunctor();
        }
        
        private Func<SignsOfLife.Prefabs.StaticPrefabs.StaticPrefab> _activationFunctor;

    }
}