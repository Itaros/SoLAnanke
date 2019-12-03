using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SignsOfLife.Entities;
using SignsOfLife.Entities.Items;
using SignsOfLife.Prefabs.StaticPrefabs;

namespace Ananke.Attachment.Core
{
    public class SweetBridgeDog
    {

        public IEnumerable<Tuple<string, int>> CollectBuiltingEnumedEntities()
        {
            var coreAss = GetCurrentAss();

            List<Tuple<string, int>> extracted = new List<Tuple<string, int>>();

            var builtinTypesEnum = coreAss.GetType("SignsOfLife.Entities.LivingEntityType");
            var enumFields = builtinTypesEnum.GetFields(BindingFlags.Public | BindingFlags.Static);
            
            foreach (var field in enumFields)
            {
                extracted.Add(new Tuple<string, int>(field.Name, (int)field.GetValue(null)));
            }

            return extracted;
        }
        
        public IEnumerable<Tuple<string, int>> CollectBuiltinEnummedStaticPrefabs()
        {
            var coreAss = GetCurrentAss();

            List<Tuple<string, int>> extracted = new List<Tuple<string, int>>();

            var builtinTypesEnum = coreAss.GetType("SignsOfLife.Prefabs.StaticPrefabs.StaticPrefabType");
            var enumFields = builtinTypesEnum.GetFields(BindingFlags.Public | BindingFlags.Static);
            
            foreach (var field in enumFields)
            {
                extracted.Add(new Tuple<string, int>(field.Name, (int)field.GetValue(null)));
            }

            return extracted;

        }
        
        public IEnumerable<Tuple<string, int>> CollectBuiltinItemTypes()
        {
            var coreAss = GetCurrentAss();
            
            List<Tuple<string, int>> extracted = new List<Tuple<string, int>>();
                
            var builtinTypesEnum = coreAss.GetType("SignsOfLife.Entities.Items.InventoryItemType");
            var enumFields = builtinTypesEnum.GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var field in enumFields)
            {
                extracted.Add(new Tuple<string, int>(field.Name, (int)field.GetValue(null)));
            }

            return extracted;
        }

        private static Assembly GetCurrentAss()
        {
            var coreAss = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a =>
                String.Equals(a.GetName().Name, "Signs of Life Modded", StringComparison.InvariantCultureIgnoreCase));
            return coreAss;
        }

        public IEnumerable<Tuple<int,Type>> ProbeTypesOfCreatedObjects(IEnumerable<int> indexes)
        {
            var coreAss = GetCurrentAss();

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

        public IEnumerable<Tuple<Tuple<string, int>, Type>> ProbeTypesOfCreatedEntites(
            IEnumerable<Tuple<string, int>> listOfVanillaEntities)
        {
            var coreAss = GetCurrentAss();
            var builtinHandler = coreAss.GetType("SignsOfLife.Entities.LivingEntity");
            var methodCreator = builtinHandler.GetMethod("CreateNewBackupCall", 
                BindingFlags.Public | BindingFlags.Static,
                System.Type.DefaultBinder, 
                CallingConventions.Any, 
                new []{typeof(LivingEntityType)}, 
                null);
            
             List<Tuple<Tuple<string, int>, Type>> tuple = new List<Tuple<Tuple<string, int>, Type>>();

             foreach (var vanillaEntity in listOfVanillaEntities)
             {
                 
                 LivingEntity prefabPrototype = null;
                 try
                 {
                     var prototype = methodCreator.Invoke(null, new object[] {(LivingEntityType) vanillaEntity.Item2});
                     prefabPrototype = (LivingEntity) prototype;
                 }
                 catch (Exception e)
                 {
                     Console.WriteLine($"LivingEntity {vanillaEntity.Item2} aka {vanillaEntity.Item1} probing has exploded: \n{e}");
                 }

                 Type type = null;

                 if (prefabPrototype != null)
                     type = prefabPrototype.GetType();
                 tuple.Add(new Tuple<Tuple<string, int>, Type>(vanillaEntity, type));
                 
             }

             return tuple;
        }
        
        public IEnumerable<Tuple<Tuple<string, int>, Type>> ProbeTypesOfCreatedStaticPrefabs(IEnumerable<Tuple<string,int>> listOfVanillaPrefabs)
        {
            var coreAss = GetCurrentAss();
            var builtinHandler = coreAss.GetType("SignsOfLife.Prefabs.StaticPrefabs.StaticPrefab");
            var methodCreator = builtinHandler.GetMethod("GetNewStaticPrefabByStaticPrefabTypeAnankeBackupCall",
                BindingFlags.Public | BindingFlags.Static);
            
            List<Tuple<Tuple<string, int>, Type>> tuple = new List<Tuple<Tuple<string, int>, Type>>();
            
            foreach (var vanillaPrefab in listOfVanillaPrefabs)
            {
                StaticPrefab prefabPrototype = null;
                try
                {
                    var prototype = methodCreator.Invoke(null, new object[] {(StaticPrefabType) vanillaPrefab.Item2});
                    prefabPrototype = (StaticPrefab) prototype;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"StaticPrefab {vanillaPrefab.Item2} aka {vanillaPrefab.Item1} probing has exploded: \n{e}");
                }

                Type type = null;

                if (prefabPrototype != null)
                    type = prefabPrototype.GetType();
                tuple.Add(new Tuple<Tuple<string, int>, Type>(vanillaPrefab, type));
            }

            return tuple;
        }
    }
}