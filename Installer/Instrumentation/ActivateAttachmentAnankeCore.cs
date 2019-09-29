using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

namespace Installer.Instrumentation
{
    public class ActivateAttachmentAnankeCore : AbstractInstrumentation
    {
        public ActivateAttachmentAnankeCore() : base(typeof(ActivateAttachmentAnankeCore).Name)
        {
            
        }

        public override void Instrument(AnankeContext ananke, AssemblyDefinition ass)
        {
            var main = ass.MainModule;

            //List<Tuple<string, long>> items = new List<Tuple<string,long>>();
            
           // var enumWithBuiltinTypes = main.Types.FirstOrDefault(t => t.IsEnum && t.Name == "InventoryItemType");
            //foreach (var enumField in enumWithBuiltinTypes.Fields.Where(f => f.IsLiteral))
            //{
            //    string mnemonic = enumField.Name;
           //     long value = (int)enumField.Constant;
           //     items.Add(new Tuple<string, long>(mnemonic, value));
            //}

            AssemblyDefinition attachment = AssemblyDefinition.ReadAssembly("Ananke.Attachment.Core.dll");
            var bootstrapMethod = attachment.MainModule.Types.First(t => t.Name == "Bootstrap").Methods
                .First(m => m.IsStatic && m.Name == "Boot");
            
            //Finding SoL ctor...

            var sol = main.Types.FirstOrDefault(t => t.Name == "Config");
            //var solConstructor = sol.GetConstructors().First();
            var solInitialization = sol.GetMethods().First(m => m.Name== "LoadData");
            
            var ctorProcessor = solInitialization.Body.GetILProcessor();
            ctorProcessor.InsertAfter(
                ctorProcessor.Body.Instructions.First(i=>i.OpCode == OpCodes.Call && ((MethodReference)i.Operand).Name.Contains("LoadMusicData")),
                //ctorProcessor.Body.Instructions.Last(i=>i.OpCode==OpCodes.Ret),
                ctorProcessor.Create(OpCodes.Call, main.ImportReference(bootstrapMethod)));
            //Bootstrap.Boot
            
        }
    }
}