using Mono.Cecil;

namespace Installer.Instrumentation
{
    public class ChangeCoreModuleName : AbstractInstrumentation
    {

        public ChangeCoreModuleName() : base(typeof(ChangeCoreModuleName).Name)
        {
            
        }

        public override void Instrument(AnankeContext ananke, AssemblyDefinition ass)
        {
            ass.Name.Name = $"{ass.Name.Name} Modded";
        }
    }
}