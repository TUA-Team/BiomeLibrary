using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiomeLibrary.API;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace BiomeLibrary.IntegratedBiome
{
    class Hell : ModBiome
    {
        public override void SetDefault()
        {
            IsHallowAlt = false;
            MinimumTileRequirement = 0;
            biomeBlock.Add(TileID.Ash);
        }

        public override bool condition()
        {
            Vector2 playerPos = Main.LocalPlayer.Center / 16;
            return playerPos.Y < Main.maxTilesY - 200;
        }

        
    }
}
