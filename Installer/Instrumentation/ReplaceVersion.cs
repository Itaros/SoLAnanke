using System.Collections;
using System.IO;
using System.Linq;
using Installer.Utilities;
using Mono.Cecil;
using Mono.Cecil.Cil;

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
            FieldDefinition versionServerField = null;
            foreach (var clazz in main.Types)
            {
                versionField = clazz.Fields.FirstOrDefault(f => f.Name == "VERSION_STRING");
                if (versionField != null)
                    break;
            }
            foreach (var clazz in main.Types)
            {
                versionServerField = clazz.Fields.FirstOrDefault(f => f.Name == "SERVER_VERSION_STRING");
                if (versionServerField != null)
                    break;
            }
            if(versionField == null)
                throw new IOException("VERSION_STRING field is not found!");
            if(versionServerField == null)
                throw new IOException("SERVER_VERSION_STRING field is not found!");
            PrefixVersionStaticReadonly(ananke, versionField);
            PrefixVersionStaticReadonly(ananke, versionServerField);
        }

        private static void PrefixVersionStaticReadonly(AnankeContext ananke, FieldDefinition field)
        {
            var versionSettingStaticCtor = field.DeclaringType.Methods.FirstOrDefault(m => m.Name == ".cctor");
            var versionFieldSettingInstruction = versionSettingStaticCtor.Body.Instructions.FirstOrDefault(i =>
                i.OpCode == OpCodes.Stsfld && i.Operand == field);

            versionFieldSettingInstruction.Previous.Operand =
                $"[MODDED-{ananke.Version}]{versionFieldSettingInstruction.Previous.Operand as string}";

            string versionFieldConstantValue = (string) field.Constant;
        }
    }
}