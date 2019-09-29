using Mono.Cecil;

namespace Installer.Instrumentation
{
    public class UnlockAccess : AbstractInstrumentation
    {

        public UnlockAccess() : base(typeof(UnlockAccess).Name)
        {
            
        }

        public override void Instrument(AnankeContext ananke, AssemblyDefinition ass)
        {
            var main = ass.MainModule;
            foreach (var type in main.Types)
            {
                if (type.IsNotPublic)
                    type.IsPublic = true;
            }
        }
    }
}