using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Installer.Utilities
{
    public class CallHelper
    {
        public static void ReplaceCall(MethodReference old, MethodReference replacement, AssemblyDefinition asm)
        {
            foreach (ModuleDefinition mod in asm.Modules)
            {
                foreach (TypeDefinition td in mod.Types)
                {
                    IterateType(td, old, replacement);
                }
            }
        }
        public static void IterateType(TypeDefinition td, MethodReference old, MethodReference replacement)
        {
            foreach (TypeDefinition ntd in td.NestedTypes)
            {
                IterateType(ntd, old, replacement);
            }

            foreach (MethodDefinition md in td.Methods)
            {
                if (md.HasBody)
                {
                    for (int i = 0; i < md.Body.Instructions.Count - 1; i++)
                    {
                        Instruction inst = md.Body.Instructions[i];
                        if (inst.OpCode == OpCodes.Call)
                        {
                            if (inst.Operand == old)
                            {
                                inst.Operand = replacement;
                            }
                        }
                    }
                }
            }
        }
    }
}