using BiomeLibrary.API;
using BiomeLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace BiomeLibrary
{
    public partial class BiomeWorld : ModWorld
    {
        public void DecideEvil(GenerationProgress progress)
        {
            if (BiomeWorld.PendingEvil == "Random")
            {
                List<ModBiome> allEvil = BiomeLibs.Biomes.Values.Where(i => i.BiomeAlt == BiomeAlternative.evilAlt).ToList();
                List<String> evilName = new List<string>();
                evilName.Add("Corruption");
                evilName.Add("Crimson");
                foreach (ModBiome biome in allEvil)
                {
                    evilName.Add(biome.BiomeName);
                }
                BiomeWorld.PendingEvil = evilName[WorldGen.genRand.Next(evilName.Count)];               
            }
            if (BiomeWorld.PendingEvil == "Crimson")
                WorldGen.crimson = true;
        }

        public void GenerateEvil(GenerationProgress progress, PassLegacy pass)
        {
            if (BiomeWorld.PendingEvil == "Corruption" || BiomeWorld.PendingEvil == "Crimson")
                GenerateVanillaEvil(progress, pass);
            else
                BiomeLibs.Biomes.Values.Single(i => i.BiomeName == BiomeWorld.PendingEvil)
                    .BiomeAltWorldGeneration(progress, pass);
        }

        public void GenerateVanillaEvil(GenerationProgress progress, PassLegacy evilPass)
        {
            vanillaEvilPass.Apply(progress);
        }
    }
}
