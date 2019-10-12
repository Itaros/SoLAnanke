namespace Ananke.Attachment.Core
{
    public static class SoLPhaseDelegator
    {

        public static void OnAfterInitialRecipesWereLoaded()
        {
            var phase = AnankeContext.Current.PhaseController.AcquirePhase(AfterRecipesConfigLoadedPhaseName);
            AnankeContext.Current.PhaseController.RunPhase(phase, AnankeContext.Current);
        }
        
        public static readonly string AfterRecipesConfigLoadedPhaseName = "onRecipeLoad";
        
    }
}