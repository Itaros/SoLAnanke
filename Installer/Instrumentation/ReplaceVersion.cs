using System.IO;
using System.Linq;
using Installer.Utilities;
using Mono.Cecil;

namespace Installer.Instrumentation
{
    public class ReplaceVersion : AbstractInstrumentation
    {
        public ReplaceVersion() : base(typeof(ReplaceVersion).Name)
        {
            
        }
        public override void Instrument(AnankeContext ananke, AssemblyDefinition ass)
        {
            var main = ass.MainModule;
            FieldDefinition versionField = null;
            foreach (var clazz in main.Types)
            {
                versionField = clazz.Fields.FirstOrDefault(f => f.Name == "VERSION_STRING");
                if (versionField != null)
                    break;
            }
            if(versionField == null)
                throw new IOException("VERSION_STRING field is not found!");

            string versionFieldConstantValue = (string)versionField.Constant;
            
            StringHelper.ReplaceString(versionFieldConstantValue, $"[MODDED-{ananke.Version}]"+versionFieldConstantValue, ass);

        }
    }
}