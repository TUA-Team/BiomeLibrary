using System.Collections.Generic;
using BiomeLibrary.Enums;
using Terraria;
using Terraria.GameContent.Biomes;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;

namespace BiomeLibrary.Worlds
{
    public sealed partial class BiomeLibraryWorld : ModWorld
    {
        private const string
            HARDMODE_GOOD = "Hardmode Good",
            HARDMODE_BAD = "Hardmode Evil";

        public override void Load(TagCompound tag)
        {
            LastLoadedWorld = this;

            CurrentEvil = Main.ActiveWorldFileData.HasCorruption ? VanillaBiome.Corruption.ToString() : VanillaBiome.Crimson.ToString();

            if (tag.ContainsKey(nameof(CurrentEvil)))
                CurrentEvil = tag.GetString(nameof(CurrentEvil));

            base.Load(tag);
        }

        public override TagCompound Save()
        {
            TagCompound tag = new TagCompound();

            tag.Add(nameof(CurrentEvil), CurrentEvil);

            return tag;
        }

        public override void TileCountsAvailable(int[] tileCounts)
        {
            BiomeLoader.ActionOnAllBiomes(b => b.CountBiomeTiles(tileCounts));
        }

        public override void ResetNearbyTileEffects() => BiomeLoader.ActionOnAllBiomes(b => b.TileCount = 0);

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int
                resetIndex = tasks.FindIndex(t => t.Name.Equals("Reset")),
                terrainIndex = tasks.FindIndex(t => t.Name.Equals("Terrain")),
                evilIndex = tasks.FindIndex(t => t.Name.Equals("Corruption"));

            if (terrainIndex != -1)
                tasks.Insert(resetIndex + 1, new PassLegacy("Deciding World Evil Biome", p => DecideEvilBiome(p)));

            if (evilIndex != -1)
            {
                VanillaEvilPass = tasks[evilIndex];
                tasks[evilIndex] = new PassLegacy("Corruption", p => GenerateEvilBiome(p, tasks[resetIndex] as PassLegacy));
            }
        }

        public override void ModifyHardmodeTasks(List<GenPass> list)
        {
            int
                hardmodeGood = list.FindIndex(g => g.Name.Equals(HARDMODE_GOOD)),
                hardmodeBad = list.FindIndex(g => g.Name.Equals(HARDMODE_BAD));

            list[hardmodeGood] = new PassLegacy(HARDMODE_GOOD, p => GenerateGoodHardmodeBiome(p));
            list[hardmodeBad] = new PassLegacy(HARDMODE_BAD, p => GenerateBadHardmodeBiome(p));
        }


        /// <summary>The last <see cref="ModWorld"/> that has gone through its <see cref="ModWorld.Load"/> method. Does not necessarily mean a world is currently loaded.</summary>
        public static BiomeLibraryWorld LastLoadedWorld { get; private set; }

        public static bool Infinite { get; private set; }
        public static bool Chunked { get; private set; }

        public static string PendingEvil { get; internal set; }
        public static string CurrentEvil { get; internal set; }

        internal static GenPass VanillaEvilPass { get; private set; }
    }
}