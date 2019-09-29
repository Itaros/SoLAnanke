using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Installer.Instrumentation
{
    public class AddAttachmentAnankeCore : AbstractInstrumentation
    {

        public AddAttachmentAnankeCore() : base(typeof(AddAttachmentAnankeCore).Name)
        {
            
        }

        public override void Instrument(AnankeContext ananke, AssemblyDefinition ass)
        {
            var main = ass.MainModule;
            main.AssemblyReferences.Add(new AssemblyNameReference("Ananke.Attachment.Core", new Version(1,0,0,0)));
        }
    }
}