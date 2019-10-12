namespace Ananke.Attachment.Core.Templates.StaticPrefabs
{
    public class Tilemap
    {

        public class TileVariant
        {
            public TileVariant(TileSpecialType special)
            {
                SpecialType = special;
                ConcreteTypeId = null;
            }

            public TileVariant(long blockid)
            {
                SpecialType = null;
                ConcreteTypeId = blockid;
            }

            public TileVariant()
            {
                SpecialType = null;
                ConcreteTypeId = null;
            }
            
            public enum TileSpecialType
            {
                BLOCKS_LIGHT,
                DISALLOWS_FRIDGE,
                PLATFORM_SLOPE_LTR,
                PLATFORM_SLOPE_RTL,
                PLATFORM_HORIZONTAL,
                FULLCOLLISION
            }
            
            public TileSpecialType? SpecialType { get; }
            public long? ConcreteTypeId { get; }

            public int ToSoLTileDataSuperID()
            {
                if (SpecialType.HasValue)
                {
                    switch (SpecialType)
                    {
                        case TileSpecialType.BLOCKS_LIGHT:
                            return 994;
                        case TileSpecialType.DISALLOWS_FRIDGE:
                            return 995;
                        case TileSpecialType.PLATFORM_SLOPE_LTR:
                            return 996;
                        case TileSpecialType.PLATFORM_SLOPE_RTL:
                            return 997;
                        case TileSpecialType.PLATFORM_HORIZONTAL:
                            return 998;
                        case TileSpecialType.FULLCOLLISION:
                            return 999;
                    }
                }else if (ConcreteTypeId.HasValue)
                {
                    return (int)ConcreteTypeId.Value;
                }

                return 0;
            }
            
        }
        
        public Tilemap(int width, int height)
        {
            _tileArray = new int[width * height];
            Width = width;
            Height = height;
        }

        public Tilemap(TileVariant[,] variants)
        :this(variants.GetLength(0), variants.GetLength(1))
        {
            for (var x = 0; x < variants.GetLength(0); x++)
            for (var y = 0; y < variants.GetLength(1); y++)
            {
                SetTileAt(x,y, variants[x,y]);
            }
        }

        public int Width { get; }
        public int Height { get; }
        
        private int[] _tileArray;

        public void SetTileAt(int x, int y, TileVariant variant)
        {
            int position = y * Width + x;
            _tileArray[position] = variant.ToSoLTileDataSuperID();
        }
        
        public int[,] ConvertToSoLTileData()
        {
            int[,] solFormat = new int[Width,Height];
            int x = 0;
            int y = 0;
            for (int i = 0; i < _tileArray.Length; i++)
            {
                solFormat[x, y] = _tileArray[i];
                x++;
                if (x >= Width)
                {
                    x = 0;
                    y++;
                }
            }

            return solFormat;
        }
    }
}