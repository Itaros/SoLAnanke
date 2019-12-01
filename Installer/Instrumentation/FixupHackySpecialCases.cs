using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Installer.Instrumentation
{
    public class FixupHackySpecialCases : AbstractInstrumentation
    {
        
        //inventoryItem = (InventoryItem) new CampfireSatchel((StaticPrefab) null);

        public FixupHackySpecialCases() : base(typeof(FixupHackySpecialCases).Name)
        {
        }

        public override void Instrument(AnankeContext ananke, AssemblyDefinition ass)
        {

            //Fix campfire hacksatchel
            var campfireSatchelType = ass.MainModule.Types.FirstOrDefault(t => t.Name == "CampfireSatchel");

            MethodDefinition hackyCtor = new MethodDefinition(
                ".ctor", 
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, 
                ass.MainModule.TypeSystem.Void);

            MethodReference myShittyCtor = campfireSatchelType.Methods.First(m => m.Name == ".ctor");
            
            hackyCtor.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            hackyCtor.Body.Instructions.Add(Instruction.Create(OpCodes.Ldnull));//Pass null
            hackyCtor.Body.Instructions.Add(Instruction.Create(OpCodes.Call, myShittyCtor));
            hackyCtor.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
            campfireSatchelType.Methods.Add(hackyCtor);
            
        }
    }
}