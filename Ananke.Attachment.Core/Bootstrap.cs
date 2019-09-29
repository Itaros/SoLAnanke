using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ananke.Attachment.Core.Items;
using Ananke.Attachment.Core.Loader;
using SignsOfLife.Entities.Items;
using SignsOfLife.Entities.Items.Materials;

namespace Ananke.Attachment.Core
{
    public static class Bootstrap
    {

        public static void Boot()
        {
            SweetBridgeDog bridge = new SweetBridgeDog();
            AnankeContext.Current.ItemsRegistry.Reset();
            IEnumerable<Tuple<string, int>> pairs = bridge.CollectBuiltinItemTypes();
            var enumerable = pairs as Tuple<string, int>[] ?? pairs.ToArray();
            IEnumerable<Tuple<int,Type>> typeProbe = bridge.ProbeTypesOfCreatedObjects(enumerable.Select(p=>p.Item2));
            foreach (var pair in enumerable)
            {
                var assoc = typeProbe.FirstOrDefault(p => p.Item1 == pair.Item2);
                Type type = null;
                if (assoc != null)
                    type = assoc.Item2;
                ItemActivator activator;
                if (type == null)
                    activator = new ItemActivator(() => null);
                else
                    activator = new ItemActivator(
                        () => (InventoryItem) Activator.CreateInstance(type)
                    );
                var definition = new VanillaItemDefinition(pair.Item2, pair.Item1, activator);
                AnankeContext.Current.ItemsRegistry.Add(definition);
            }
            
            ModLoader loader = new ModLoader(new DirectoryInfo("mods"));
            loader.LoadMods();
        }
        
    }
}