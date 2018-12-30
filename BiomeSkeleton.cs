using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace BiomeLibrary
{
    [Obsolete("This class is now replaced with Mod Biome")]
    public class BiomeSkeleton
    {
        public List<String> blockList;
        public List<int> blockListID;
        public int biomeTileCount = 0;
        public byte biomeID;
        private int minTileRequired;
        private bool valid;
        private bool isSpreading;
        private string customHallowAltGenerationMessage;
        

        private Func<bool> condition;

        public Mod mod;

        public BiomeSkeleton(int minTileRequired, Mod mod)
        {
            this.minTileRequired = minTileRequired;
            blockList = new List<string>();
            blockListID = new List<int>();
            this.mod = mod;
        }

        public void resetTileCount()
        {
            biomeTileCount = 0;
        }

        public int tileCount(int[] tileCounts)
        {
            biomeTileCount = 0;
            foreach(int i in blockListID) {
                biomeTileCount += tileCounts[i];
            }
            return biomeTileCount;
        }

        public void registerTile(String[] blockName)
        {
            String text = "";
            for (int i = 0; i < blockName.Length; i++)
            {
                blockList.Add(blockName[i]);
                blockListID.Add(mod.TileType(blockName[i]));
                text += blockList[i] + " ";
            }
        }

        public void registerTile(int[] blockID)
        {
            for (int i = 0; i < blockID.Length; i++)
            {
                blockListID.Add(blockID[i]);
            }
        }

        public bool isValid()
        {
            bool c = true;
            if (condition != null) {
                c = condition.Invoke();
            }
            return biomeTileCount >= minTileRequired && c;
        }
		
		public bool checkYCoord(int j) {
            return j > 0 && j < Main.ActiveWorldFileData.WorldSizeY;
		}

        public bool checkXCoord(int i)
        {
            return i > 0 && i < Main.ActiveWorldFileData.WorldSizeX;
        }

        public void SetCondition(Func<bool> flag)
        {
             condition = flag;
        }

        public void SetMinTile(int minTile) {
            minTileRequired = minTile;
        }

        public void setMessage(string message) {
            customHallowAltGenerationMessage = message;
        }

        public string getMessage() {
            return customHallowAltGenerationMessage;
        }
    }
}
