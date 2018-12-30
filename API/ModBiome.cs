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

        private bool _isHallowAlt;

        public Mod mod { get; internal set; }

        public int MinimumTileRequirement
        {
            get => _minimumTileRequirement;
            set => _minimumTileRequirement = value;
        }

        public bool IsHallowAlt
        {
            get => _isHallowAlt;
            set => _isHallowAlt = value;
        }

        public String BiomeName //Set biome name
        {
            get => _biomeName;
            set => _biomeName = value;
        }

        internal bool Valid => _tileCount >= MinimumTileRequirement && condition();

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



        public virtual bool condition()
        {
            return true;
        }

        public virtual bool HallowAltGeneration(ref String message)
        {
            message = "An hallow alt has been generated";
            return false;
        }

        public virtual void SetMobSpawning(IDictionary<int, int> pool) //To be implemented yet
        {

        }

        internal void resetTileCount()
        {
            _tileCount = 0;
        }

        public virtual int tileCount(int[] tileCounts)
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
