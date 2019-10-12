using System;
using Ananke.Attachment.Core.Phases;

namespace Ananke.Attachment.Core.Mod
{
    public class SoLModV1CompatibilityProvider
    {

        public SoLModV1CompatibilityProvider(AnankeContext context)
        {
            _context = context;
        }

        private AnankeContext _context;

        public void AddActionForRecipeLoadingPhase(Type mod, PhaseAction.OnPhaseStartedHandler action)
        {
            _context.PhaseController.RegisterPhaseAction(
                new PhaseAction(_context.PhaseController.AcquirePhase(SoLPhaseDelegator.AfterRecipesConfigLoadedPhaseName), mod, action));
        }
        
    }
}