using BiomeLibrary.Enums;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace BiomeLibrary.API
{
    public class ModBiome
    {

        private readonly int _runtimeID;

        public readonly IList<int> biomeBlock = new List<int>(); //List of block ID, this list cannot overriden
        public readonly IList<int> npcList = new List<int>();

        private int _tileCount;
        private int _minimumTileRequirement;

        private string _biomeName = "";
        internal string _biomeInternalName = "";

        private BiomeAlternative _alt = BiomeAlternative.noAlt;
        private EvilSpecific _evilSpecific = EvilSpecific.both;
        private String _evilSpecificBoundName = "Corruption";

        public Texture2D biomePreview => mod.TextureExists(this.GetType().FullName.Replace(".", "/"))
            ? mod.GetTexture(this.GetType().FullName.Replace(".", "/"))
            : BiomeLibs.Instance.GetTexture("Texture/Random");

        public Mod mod
        {
            get;
            internal set;
        }

        public int MinimumTileRequirement
        {
            get => _minimumTileRequirement;
            set => _minimumTileRequirement = value;
        }

        public BiomeAlternative BiomeAlt
        {
            get => _alt;
            set => _alt = value;
        }

        public EvilSpecific EvilSpecific
        {
            get => _evilSpecific;
            set => _evilSpecific = value;
        }

        public String EvilSpecificBoundName
        {
            get => _evilSpecificBoundName;
            protected set => _evilSpecificBoundName = value;
        }

        public String BiomeName //Set biome name
        {
            get;
            set;
        }

        internal bool Valid => !WorldGen.gen && _tileCount >= MinimumTileRequirement && Condition();

        internal int TileCount
        {
            get { return _tileCount; }
            set { _tileCount = value; }
        }

        public int RuntimeID
        {
            get { return _runtimeID; }
        }

        public virtual void SetDefault()
        {

        }

        public virtual bool Condition()
        {
            return true;
        }

        [Obsolete("This is deprecated, pls use BiomeAltHardmodeGeneration")]
        public virtual bool BiomeAltGeneration(ref String message)
        {
            return BiomeAltHardmodeGeneration(ref message);
        }

        /// <summary>
        /// This is specific to hardmode generation
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public virtual bool BiomeAltHardmodeGeneration(ref String message)
        {
            message = "An hallow alt has been generated";
            return false;
        }

        public virtual void BiomeAltWorldGeneration(GenerationProgress progress, PassLegacy pass)
        {
        }

        /// <summary>
        /// Those next hook are W.I.P
        /// </summary>
        public virtual void OnEnter()
        {
        }

        public virtual void OnExit()
        {
        }

        public virtual bool OnNearby(float percent)
        {
            return false;
        }

        internal void ResetTileCount()
        {
            _tileCount = 0;
        }

        internal int InternalTileCount(int[] tileCounts)
        {
            foreach (int i in biomeBlock)
            {
                _tileCount += tileCounts[i];
            }
            return _tileCount;
        }

        public bool InBiome()
        {
            return Main.LocalPlayer != null && Valid;
        }
    }
}
