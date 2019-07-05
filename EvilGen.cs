using BiomeLibrary.API;
using BiomeLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
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
            if (BiomeWorld.pendingEvil.Equals("Random", StringComparison.InvariantCultureIgnoreCase))
            {
                List<ModBiome> allEvil = BiomeLibs.Biomes.Values.Where(i => i.BiomeAlt == BiomeAlternative.evilAlt).ToList();
                List<string> evilName = new List<string>()
                {
                    "Corruption",
                    "Crimson"
                };

                foreach (ModBiome biome in allEvil)
                    evilName.Add(biome.BiomeName);

                BiomeWorld.pendingEvil = evilName[WorldGen.genRand.Next(evilName.Count)];               
            }

            if (BiomeWorld.pendingEvil.Equals("Crimson", StringComparison.InvariantCultureIgnoreCase))
                WorldGen.crimson = true;
        }

        public void GenerateEvil(GenerationProgress progress, PassLegacy pass)
        {
            if (BiomeWorld.pendingEvil.Equals("Corruption", StringComparison.InvariantCultureIgnoreCase) || BiomeWorld.pendingEvil.Equals("Crimson", StringComparison.InvariantCultureIgnoreCase))
                GenerateVanillaEvil(progress);
            else
                BiomeLibs.Biomes.Values.Single(i => i.BiomeName == BiomeWorld.pendingEvil)
                    .BiomeAltWorldGeneration(progress, pass);
        }

        public void GenerateVanillaEvil(GenerationProgress progress) => vanillaEvilPass.Apply(progress);
    }
}
