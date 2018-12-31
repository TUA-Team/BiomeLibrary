using BiomeLibrary.Enums;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

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

        private BiomeAlternative _alt = BiomeAlternative.noAlt;
        private EvilSpecific _evilSpecific = EvilSpecific.both;

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

        public String BiomeName //Set biome name
        {
            get;
            internal set;
        }

        internal bool Valid => _tileCount >= MinimumTileRequirement && Condition();

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

        public virtual bool BiomeAltGeneration(ref String message)
        {
            message = "An hallow alt has been generated";
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
