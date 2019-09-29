using Mono.Cecil;

namespace Installer.Instrumentation
{
    public abstract class AbstractInstrumentation
    {

        protected AbstractInstrumentation(string name)
        {
            Name = name;
        }
        public abstract void Instrument(AnankeContext ananke, AssemblyDefinition ass);

        
        public string Name { get; }
    }
}