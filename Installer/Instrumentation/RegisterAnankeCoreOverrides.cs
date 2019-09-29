using System.Linq;
using Installer.Utilities;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Installer.Instrumentation
{
    public class RegisterAnankeCoreOverrides : AbstractInstrumentation
    {
        public RegisterAnankeCoreOverrides() : base(typeof(RegisterAnankeCoreOverrides).Name)
        {
            
        }

        public override void Instrument(AnankeContext ananke, AssemblyDefinition ass)
        {
            var main = ass.MainModule;
            //SoLInventoryItemHandlerBypass

            AssemblyDefinition attachment = AssemblyDefinition.ReadAssembly("Ananke.Attachment.Core.dll");
            var bootstrapMethod = attachment.MainModule.Types.First(t => t.Name == "SoLInventoryItemHandlerBypass").Methods
                .First(m => m.IsStatic && m.Name == "GetNewItemByItemType");
            
            var inventoryItemHandlerTypedef = main.Types.First(t=>t.Name == "InventoryItemHandler");

            var getNewItemByItemTypeMethoddef = inventoryItemHandlerTypedef.Methods.First(m => m.Name == "GetNewItemByItemType");
            getNewItemByItemTypeMethoddef.Name = "GetNewItemByItemTypeAnankeBackupCall";
                
            var getNewItemByItemTypeMethoddefExternationProbe = new MethodDefinition(
                "GetNewItemByItemType", 
                MethodAttributes.Public| MethodAttributes.Static, 
                getNewItemByItemTypeMethoddef.ReturnType);
            getNewItemByItemTypeMethoddefExternationProbe.Parameters.Add(getNewItemByItemTypeMethoddef.Parameters[0]);
            inventoryItemHandlerTypedef.Methods.Add(getNewItemByItemTypeMethoddefExternationProbe);

            {
                var processor = getNewItemByItemTypeMethoddefExternationProbe.Body.GetILProcessor();

                processor.Body.Instructions.Clear();
                processor.Emit(OpCodes.Ldarg_0);
                processor.Emit(OpCodes.Call,main.ImportReference(bootstrapMethod));
                processor.Emit(OpCodes.Ret);
            }
            
            CallHelper.ReplaceCall(getNewItemByItemTypeMethoddef, getNewItemByItemTypeMethoddefExternationProbe, ass);
            
        }
    }
}