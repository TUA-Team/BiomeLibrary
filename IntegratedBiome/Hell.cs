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
            MinimumTileRequirement = 0;
            biomeBlock.Add(TileID.Ash);
        }

        public override bool Condition()
        {
            Vector2 playerPos = Main.LocalPlayer.Center / 16;
            return playerPos.Y < Main.maxTilesY - 200;
        }

        
    }
}
