using BiomeLibrary.API;
using BiomeLibrary.Enums;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;

namespace BiomeLibrary
{
    public partial class BiomeWorld : ModWorld
    {
        public static bool infinite = false;
        public static bool chunked = false;
        public static readonly bool x64Terraria = IntPtr.Size == 8;
        public static string currentEvil = "Corruption";
        internal static string PendingEvil = "";

        public override void Load(TagCompound tag)
        {
            BiomeLibs.World = mod.GetModWorld<BiomeWorld>();

            currentEvil = (Main.ActiveWorldFileData.HasCorruption) ? "Corruption" : "Crimson";
            if (tag.ContainsKey("evil"))
            {
                currentEvil = tag.GetString("evil");
            }

            base.Load(tag);
        }

        public override TagCompound Save()
        {
            TagCompound tag = new TagCompound();
            tag.Add("evil", currentEvil);
            return tag;
        }

        public override void TileCountsAvailable(int[] tileCounts)
        {
            for (int i = 0; i < BiomeLibs.Biomes.Count; i++)
                BiomeLibs.Biomes.Values.ToList()[i].InternalTileCount(tileCounts);
        }

        public override void ResetNearbyTileEffects()
        {
            for (int i = 0; i < BiomeLibs.Biomes.Count; i++)
                BiomeLibs.Biomes.Values.ToList()[i].ResetTileCount();
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int resetIndex = tasks.FindIndex(i => i.Name == "Reset");
            int terrainIndex = tasks.FindIndex(i => i.Name == "Terrain");
            
            if (terrainIndex != -1)
            {
                tasks.Insert(resetIndex + 1, new PassLegacy("Deciding World Evil", (progress) => DecideEvil(progress)));
            }

            int evilIndex = tasks.FindIndex(i => i.Name == "Corruption");
            if (evilIndex != -1)
            {
                tasks[evilIndex] = new PassLegacy("Corruption", (progress) => GenerateEvil(progress, tasks[resetIndex] as PassLegacy));
            }
        }

        public override void ModifyHardmodeTasks(List<GenPass> list)
        {
            int goodIndex = list.FindIndex(i => i.Name == "Hardmode Good");
            int badIndex = list.FindIndex(i => i.Name == "Hardmode Evil");
            Main.hardMode = true;
            list[goodIndex] = (new PassLegacy("Hardmode Good", (progress) => GenerateGoodBiome(progress)));
            list[badIndex] = new PassLegacy("Hardmode Evil", (progress) => GenerateBadBiome(progress));
        }

        
    }
}
