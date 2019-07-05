using System.Collections.Generic;
using Terraria.ModLoader;

namespace BiomeLibrary
{
    public sealed class Utilities
    {
        public static ushort FindModdedTileIDInArray(IList<int> tiles, string endsWith = "", string contains = "")
        {
            foreach (int tileID in tiles)
            {
                ModTile modTile = TileLoader.GetTile(tileID);

                if (modTile != null &&
                    (!string.IsNullOrWhiteSpace(endsWith) && modTile.Name.EndsWith(endsWith)) &&
                    (!string.IsNullOrWhiteSpace(contains) && modTile.Name.Contains(contains)))
                    return modTile.Type;
            }

            return ushort.MinValue;
        }
    }
}