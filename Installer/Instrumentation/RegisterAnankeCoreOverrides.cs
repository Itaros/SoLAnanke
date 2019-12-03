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
            AssemblyDefinition attachment = AssemblyDefinition.ReadAssembly("Ananke.Attachment.Core.dll");          
            
            //TODO: Deduplicate code
            //SoLInventoryItemHandlerBypass
            {
                var bootstrapMethod = attachment.MainModule.Types.First(t => t.Name == "SoLInventoryItemHandlerBypass")
                    .Methods
                    .First(m => m.IsStatic && m.Name == "GetNewItemByItemType");

                var inventoryItemHandlerTypedef = main.Types.First(t => t.Name == "InventoryItemHandler");

                var getNewItemByItemTypeMethoddef =
                    inventoryItemHandlerTypedef.Methods.First(m => m.Name == "GetNewItemByItemType");
                getNewItemByItemTypeMethoddef.Name = "GetNewItemByItemTypeAnankeBackupCall";

                var getNewItemByItemTypeMethoddefExternationProbe = new MethodDefinition(
                    "GetNewItemByItemType",
                    MethodAttributes.Public | MethodAttributes.Static,
                    getNewItemByItemTypeMethoddef.ReturnType);
                getNewItemByItemTypeMethoddefExternationProbe.Parameters.Add(
                    getNewItemByItemTypeMethoddef.Parameters[0]);
                inventoryItemHandlerTypedef.Methods.Add(getNewItemByItemTypeMethoddefExternationProbe);

                {
                    var processor = getNewItemByItemTypeMethoddefExternationProbe.Body.GetILProcessor();

                    processor.Body.Instructions.Clear();
                    processor.Emit(OpCodes.Ldarg_0);
                    processor.Emit(OpCodes.Call, main.ImportReference(bootstrapMethod));
                    processor.Emit(OpCodes.Ret);
                }

                CallHelper.ReplaceCall(getNewItemByItemTypeMethoddef, getNewItemByItemTypeMethoddefExternationProbe,
                    ass);
            }
            
            //SoLStaticLivingEntityInlineRegistryBypass
            {
                var bootstrapMethod = attachment.MainModule.Types
                    .First(t => t.Name == "SoLStaticLivingEntityInlineRegistryBypass").Methods.First(m =>
                        m.IsStatic && m.Name == "GetNewLivingEntityByStaticPrefabType");

                var definitionTypeReturn = main.Types.First(t => t.Name == "LivingEntity");
                var definitionTypeEnumerator = main.Types.First(t => t.Name == "LivingEntityType");
                
                var definitionOldMethod =
                    definitionTypeReturn.Methods.First(m => m.Name == "CreateNew" && m.Parameters[0].ParameterType == definitionTypeEnumerator);
                
                BootstrapMethodTo(ass, definitionOldMethod, definitionTypeReturn, bootstrapMethod);

                var bootstrapNameToTypeMethod = attachment.MainModule.Types
                    .First(t => t.Name == "SoLStaticLivingEntityInlineRegistryBypass").Methods.First(m =>
                        m.IsStatic && m.Name == "GetNewLivingEntityTypeByName");
                
                var definitionNameToTypeOldMethod =
                    definitionTypeReturn.Methods.First(m => m.Name == "GetLivingEntityTypeByName");
                BootstrapMethodTo(ass, definitionNameToTypeOldMethod, definitionTypeReturn, bootstrapNameToTypeMethod);
            }
            
            /*
             TODO PRODUCTION CRITICAL:
             OVERRIDE GetNewStaticPrefabByName as well otherwise 
             Workshop(yes, it is at least partially is in the game) support 
             and multiplayer will break.
             */
            //SoLStaticPrefabBypass
            {
                var bootstrapMethod = attachment.MainModule.Types.First(t => t.Name == "SoLStaticPrefabInlineRegistryBypass")
                    .Methods
                    .First(m => m.IsStatic && m.Name == "GetNewStaticPrefabByStaticPrefabType");

                var inventoryItemHandlerTypedef = main.Types.First(t => t.Name == "StaticPrefab");

                var getNewItemByItemTypeMethoddef =
                    inventoryItemHandlerTypedef.Methods.First(m => m.Name == "GetNewStaticPrefabByStaticPrefabType");
                getNewItemByItemTypeMethoddef.Name = "GetNewStaticPrefabByStaticPrefabTypeAnankeBackupCall";

                var getNewItemByItemTypeMethoddefExternationProbe = new MethodDefinition(
                    "GetNewStaticPrefabByStaticPrefabType",
                    MethodAttributes.Public | MethodAttributes.Static,
                    getNewItemByItemTypeMethoddef.ReturnType);
                getNewItemByItemTypeMethoddefExternationProbe.Parameters.Add(
                    getNewItemByItemTypeMethoddef.Parameters[0]);
                inventoryItemHandlerTypedef.Methods.Add(getNewItemByItemTypeMethoddefExternationProbe);

                {
                    var processor = getNewItemByItemTypeMethoddefExternationProbe.Body.GetILProcessor();

                    processor.Body.Instructions.Clear();
                    processor.Emit(OpCodes.Ldarg_0);
                    processor.Emit(OpCodes.Call, main.ImportReference(bootstrapMethod));
                    processor.Emit(OpCodes.Ret);
                }

                CallHelper.ReplaceCall(getNewItemByItemTypeMethoddef, getNewItemByItemTypeMethoddefExternationProbe,
                    ass);
            }

            //Phase hook OnAfterInitialRecipesWereLoaded
            {
                var delegatorMethod = attachment.MainModule.Types.First(t => t.Name == "SoLPhaseDelegator").Methods
                    .First(m => m.IsStatic && m.Name == "OnAfterInitialRecipesWereLoaded");

                var solClass = main.Types.FirstOrDefault(t => t.Name == "SpaceGame");
                var solClassLoadContentMethod = solClass.Methods.FirstOrDefault(m => m.Name == "LoadContent");
                var solClassLoadContentMethodIL = solClassLoadContentMethod.Body.GetILProcessor();

                var ilcodeCall =
                    solClassLoadContentMethodIL.Create(OpCodes.Call, main.ImportReference(delegatorMethod));
                solClassLoadContentMethodIL.InsertAfter(
                    solClassLoadContentMethodIL.Body.Instructions.First(i =>
                        i.OpCode == OpCodes.Call && ((MethodReference) i.Operand).Name.Contains("GetRecipes")).Next,//We pick a call and step after result assignment
                    ilcodeCall
                );
            }
            
        }

        private static MethodDefinition BootstrapMethodTo(AssemblyDefinition whichAssToReplaceIn,
            MethodDefinition definitionOldMethod, TypeDefinition definitionTypeReturn, MethodDefinition delegateToMethod)
        {
            ModuleDefinition main = whichAssToReplaceIn.MainModule;
            definitionOldMethod.Name = $"{definitionOldMethod.Name}BackupCall";

            //Create new method and use it instead
            var definitionNewMethod = new MethodDefinition(
                "CreateNew",
                MethodAttributes.Public | MethodAttributes.Static,
                definitionOldMethod.ReturnType);
            definitionNewMethod.Parameters.Add(
                definitionOldMethod.Parameters[0]);
            definitionTypeReturn.Methods.Add(definitionNewMethod);

            {
                var processor = definitionNewMethod.Body.GetILProcessor();

                processor.Body.Instructions.Clear();
                processor.Emit(OpCodes.Ldarg_0);
                processor.Emit(OpCodes.Call, main.ImportReference(delegateToMethod));
                processor.Emit(OpCodes.Ret);
            }

            CallHelper.ReplaceCall(definitionOldMethod, definitionNewMethod,
                whichAssToReplaceIn);
            return definitionNewMethod;
        }
    }
}