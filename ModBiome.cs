using System.Collections.Generic;
using BiomeLibrary.Biomes;
using BiomeLibrary.Enums;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace BiomeLibrary
{
    public abstract class ModBiome : Biome
    {
        private const int STARTING_CUSTOM_BIOME_TYPE_ID = 100;
        internal readonly List<int> biomeBlocks = new List<int>();

        protected ModBiome()
        {
            type = STARTING_CUSTOM_BIOME_TYPE_ID + BiomeLoader.loadedBiomes.Count;
        }

        public virtual void SetDefault() { }

        public void SetBiomeSpecific(VanillaBiome biome) => SetBiomeSpecific(biome.ToString());

        public void SetBiomeSpecific(string biomeName) => BiomeSpecific = biomeName;

        /// <summary>Should the biome be allowed to exist in the current context.</summary>
        /// <example>
        /// The Hallow shouldn't exist in pre-hardmode.
        ///
        /// return Main.hardMode;
        /// </example>
        public virtual bool ShouldExist() => true;

        public void AddBiomeBlocks(params int[] types) => biomeBlocks.Add(type);


        /// <summary></summary>
        /// <param name="message"></param>
        /// <returns>Wether or not to use this method as the biome generator; false will use the default biome generator.</returns>
        public virtual bool BiomeAlternativeGeneration(ref string message) => BiomeAlternativeGenerationHardmode(ref message);

        /// <summary></summary>
        /// <param name="message"></param>
        /// <returns>Wether or not to use this method as the biome generator; false will use the default biome generator.</returns>
        public virtual bool BiomeAlternativeGenerationHardmode(ref string message)
        {
            message = "A " + BiomeName + " has been generated.";
            return false;
        }

        public virtual void BiomeAlternativeWorldGeneration(GenerationProgress progress, PassLegacy pass) { }

        public virtual void OnEnter(ModPlayer player) { }

        public virtual void OnExit(ModPlayer player) { }

        public virtual void OnNearby(ModPlayer player, float percent) { }

        public bool Valid() => !WorldGen.gen && ShouldExist() && TileCount >= MinimumTileRequirement;

        internal int CountBiomeTiles(int[] tiles)
        {
            for (int i = 0; i < biomeBlocks.Count; i++)
                TileCount += tiles[i];

            return TileCount;
        }

        public bool InBiome(Player player) => player != null && Valid();


        public Mod mod { get; internal set; }

        public Biome Biome { get; internal set; }

        public string BiomeName { get; }
        public string BiomeInternalName { get; internal set; }

        public virtual Texture2D BiomePreview
        {
            get
            {
                string texturePath = this.GetType().FullName.Replace('.', '/');

                if (mod.TextureExists(texturePath))
                    return mod.GetTexture(texturePath);
                else
                    return BiomeLibrary.Instance.GetTexture("Textures/Random");
            }
        }

        /// <summary>How many tiles are needed around the player for him to be considered inside the biome. Default is 100.</summary>
        public virtual int MinimumTileRequirement => 100;

        public virtual int TileCount { get; internal set; }

        public BiomeAlternative BiomeAlternative { get; protected set; }

        public string BiomeSpecific { get; private set; }

        public IReadOnlyList<int> BiomeBlockTypes => biomeBlocks.AsReadOnly();
    }
}