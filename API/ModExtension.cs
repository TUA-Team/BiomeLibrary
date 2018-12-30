using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace BiomeLibrary.API
{
    public static class ModExtension
    {
        public static ModBiome GetBiome(this Mod self, string biomeName)
        {
            return BiomeLibs.biomes.ContainsKey(self.Name + ":" + biomeName) ? BiomeLibs.biomes[self.Name + ":" + biomeName] : null;
        }

        public static ModBiome GetBiome<T>(this Mod self)
        {
            return self.GetBiome(typeof(T).Name);
        }

        public static int BiomeType(this Mod self, string biomeName) => self.GetBiome(biomeName).RuntimeID;
        public static int BiomeType<T>(this Mod self) where T : ModBiome => self.GetBiome<T>().RuntimeID;

        public static ushort FindTileIDInArray(string tileName, IList<int> tile)
        {
            foreach (var tileID in tile)
            {
                ModTile modTile = TileLoader.GetTile(tileID);
                if (modTile.Name.Contains(tileName))
                {
                    return modTile.Type;
                }
            }
            return 1;
        }

        public static ushort FindTileIDInArray(string whatItNeedToContain, string whatItDoesntNeedToContain, IList<int> tile)
        {
            foreach (var tileID in tile)
            {
                ModTile modTile = TileLoader.GetTile(tileID);
                if (modTile.Name.Contains(whatItNeedToContain) && !modTile.Name.Contains(whatItDoesntNeedToContain))
                {
                    return modTile.Type;
                }
            }
            return 1;
        }
    }
}
