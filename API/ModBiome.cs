using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
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
        public int minimumTileRequirement;

        private string _biomeName = "";

        private bool _isValid;
        private bool _isHallowAlt;

        protected Mod _mod;

        public Mod mod
        {
            get => _mod;
            internal set => _mod = value;
        }

        public bool IsHallowAlt
        {
            get => IsHallowAlt;
            set => IsHallowAlt = value;
        }

        public String biomeName //Set biome name
        {
            get => _biomeName;
            set => _biomeName = value;
        } 

        internal bool Valid
        {
            get => IsValid();
            set => _isValid = value;
        }

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

        private bool IsValid()
        {
            _isValid = _tileCount >= minimumTileRequirement && condition();
            return _isValid;
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

        public virtual void SetMobSpawning(IDictionary<int, int> pool)
        {

        }

        private void resetTileCount()
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
            return Valid;
        }

        public void SendCustomBiomes(BinaryWriter writer)
        {
            var flags = new BitsByte();
            flags[0] = Valid;
            writer.Write(flags);
        }


        public void ReceiveCustomBiomes(BinaryReader reader)
        {
            BitsByte flag = reader.ReadByte();
            _isValid = flag[0];
        }
    }
}
