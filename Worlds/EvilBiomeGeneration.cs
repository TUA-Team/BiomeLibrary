using System;
using System.Collections.Generic;
using System.Linq;
using BiomeLibrary.Enums;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace BiomeLibrary.Worlds
{
    public sealed partial class BiomeLibraryWorld : ModWorld
    {
        public void DecideEvilBiome(GenerationProgress progress)
        {
            if (PendingEvil.Equals("Random", StringComparison.InvariantCultureIgnoreCase))
            {
                List<ModBiome> allModdedEvilBiomes = BiomeLoader.loadedBiomes.Values.Where(b => b.BiomeAlternative == BiomeAlternative.Evil).ToList();

                List<string> evilBiomeNames = new List<string>()
                {
                    "Corruption",
                    "Crimson",
                };

                for (int i = 0; i < allModdedEvilBiomes.Count; i++)
                    evilBiomeNames.Add(allModdedEvilBiomes[i].BiomeName);

                PendingEvil = evilBiomeNames[WorldGen.genRand.Next(evilBiomeNames.Count)];
            }

            WorldGen.crimson = PendingEvil.Equals("Crimson", StringComparison.InvariantCultureIgnoreCase);
        }

        public void GenerateEvilBiome(GenerationProgress progress, PassLegacy pass)
        {
            if (PendingEvil.Equals("Corruption", StringComparison.InvariantCultureIgnoreCase) ||
                PendingEvil.Equals("Crimson", StringComparison.InvariantCultureIgnoreCase))
                GenerateVanillaEvilBiome(progress);
            else
                BiomeLoader.loadedBiomes.Values.Single(b => b.BiomeName.Equals(PendingEvil, StringComparison.InvariantCultureIgnoreCase)).BiomeAlternativeWorldGeneration(progress, pass);
        }

        public void GenerateVanillaEvilBiome(GenerationProgress progress) => VanillaEvilPass.Apply(progress);
    }
}