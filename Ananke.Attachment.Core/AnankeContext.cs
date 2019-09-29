﻿using Ananke.Attachment.Core.Items;

namespace Ananke.Attachment.Core
{
    public class AnankeContext
    {
        public static AnankeContext Current { get; } = new AnankeContext();
        
        public long Version => 10000;

        public ItemsRegistry ItemsRegistry { get; } = new ItemsRegistry();

    }
}