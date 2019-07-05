using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;

namespace BiomeLibrary
{
    public static class BiomeLoader
    {
        internal static readonly Dictionary<string, ModBiome> loadedBiomes = new Dictionary<string, ModBiome>();

        internal static void AutoloadBiomes(Mod mod)
        {
            if (mod.Code == null) return;

            foreach (Type type in mod.Code.GetTypes().Where(t => t.IsSubclassOf(typeof(ModBiome))))
                AutoloadBiome(mod, type);
        }

        private static void AutoloadBiome(Mod mod, Type type)
        {
            ModBiome biome = (ModBiome)Activator.CreateInstance(type);
            string internalName = GetInternalName(mod, type);

            biome.mod = mod;
            biome.BiomeInternalName = internalName;
            biome.SetDefault();

            loadedBiomes.Add(internalName, biome);
        }

        internal static void ActionOnAllBiomes(Action<ModBiome> action)
        {
            foreach (ModBiome biome in loadedBiomes.Values)
                action?.Invoke(biome);
        }

        public static ModBiome GetBiome<T>(this Mod mod) => GetBiome(mod, typeof(T).Name);
        
        public static ModBiome GetBiome(this Mod mod, string biomeName)
        {
            string internalName = GetInternalName(mod, biomeName);

            return !loadedBiomes.ContainsKey(internalName) ? null : loadedBiomes[internalName];
        }

        public static int BiomeType<T>(this Mod mod) => GetBiome<T>(mod).Biome.type;
        public static int BiomeType(this Mod mod, string biomeName) => GetBiome(mod, biomeName).Biome.type;

        internal static void Unload()
        {
            loadedBiomes.Clear();
        }


        internal static string GetInternalName(Mod mod, Type type) => mod.Name + ':' + type.Name;

        internal static string GetInternalName(Mod mod, string typeName) => mod.Name + ':' + typeName;
    }
}