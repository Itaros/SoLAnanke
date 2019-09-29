using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SignsOfLife.Entities.Items;

namespace Ananke.Attachment.Core.Items
{
    public class SweetBridgeDog
    {
        public IEnumerable<Tuple<string, int>> CollectBuiltinItemTypes()
        {
            var coreAss = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => 
                String.Equals(a.GetName().Name, "Signs of Life Modded", StringComparison.InvariantCultureIgnoreCase));
            
            List<Tuple<string, int>> extracted = new List<Tuple<string, int>>();
                
            var builtinTypesEnum = coreAss.GetType("SignsOfLife.Entities.Items.InventoryItemType");
            var enumFields = builtinTypesEnum.GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var field in enumFields)
            {
                extracted.Add(new Tuple<string, int>(field.Name, (int)field.GetValue(null)));
            }

            return extracted;
        }

        public IEnumerable<Tuple<int,Type>> ProbeTypesOfCreatedObjects(IEnumerable<int> indexes)
        {
            var coreAss = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => 
                String.Equals(a.GetName().Name, "Signs of Life Modded", StringComparison.InvariantCultureIgnoreCase));

            var builtinHandler = coreAss.GetType("SignsOfLife.Entities.Items.InventoryItemHandler");
            var methodCreator = builtinHandler.GetMethod("GetNewItemByItemTypeAnankeBackupCall",
                BindingFlags.Public | BindingFlags.Static);
            
            List<Tuple<int,Type>> extracted = new List<Tuple<int,Type>>();

            foreach (var id in indexes)
            {
                if (id == (int)InventoryItemType.NULL)
                {
                    extracted.Add(new Tuple<int, Type>(0, null));
                    continue;
                }

                object someObject = null;
                try
                {
                    someObject = methodCreator.Invoke(null, new object[] {id});
                }
                catch (TargetInvocationException e)
                {
                    Console.WriteLine($"Item {id} probing has exploded: \n{e}");
                }

                if(someObject == null)
                    continue;
                extracted.Add(new Tuple<int, Type>(id, someObject.GetType()));
            }

            return extracted;
        }
        
    }
}