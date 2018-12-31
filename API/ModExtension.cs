using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace BiomeLibrary.API
{
    public static class ModExtension
    {
        public static ModBiome GetBiome(this Mod self, string biomeName)
        {
            return BiomeLibs.Biomes.ContainsKey(self.Name + ":" + biomeName) ? BiomeLibs.Biomes[self.Name + ":" + biomeName] : null;
        }

        public static ModBiome GetBiome<T>(this Mod self)
        {
            return self.GetBiome(typeof(T).Name);
        }

        internal static void AutoloadBiome(this Mod mod, Type type)
        {
            ModBiome biome = (ModBiome)Activator.CreateInstance(type);
            biome.mod = mod;
            biome.BiomeName = type.Name;
            biome.SetDefault();

            BiomeLibs.Biomes.Add(mod.Name + ":" + type.Name, biome);
        }

        public static int BiomeType(this Mod self, string biomeName) => self.GetBiome(biomeName).RuntimeID;
        public static int BiomeType<T>(this Mod self) where T : ModBiome => self.GetBiome<T>().RuntimeID;

        public static ushort FindTileIDInArray(string whatItNeedToEndWith, IList<int> tile)
        {
            foreach (var tileID in tile)
            {
                ModTile modTile = TileLoader.GetTile(tileID);
                if (modTile.Name.EndsWith(whatItNeedToEndWith))
                {
                    return modTile.Type;
                }
            }
            return 1;
        }

        public static ushort FindTileIDInArray(string whatItNeedToEndWith, string whatItDoesntNeedToContain, IList<int> tile)
        {
            foreach (var tileID in tile)
            {
                ModTile modTile = TileLoader.GetTile(tileID);
                if (modTile.Name.EndsWith(whatItNeedToEndWith) && !modTile.Name.Contains(whatItDoesntNeedToContain))
                {
                    return modTile.Type;
                }
            }
            return 1;
        }
    }
}
