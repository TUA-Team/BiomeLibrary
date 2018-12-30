using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BiomeLibrary
{
    public class BiomePlayer : ModPlayer
    {
        public override void Load(TagCompound tag)
        {
            
            BiomeLibs.player = Main.LocalPlayer.GetModPlayer<BiomePlayer>();
        }

        [Obsolete("Now use ModBiome.InBiome()", false)]
        public bool Inbiome(string biomeName)
        {
            return false;
        }

        public override void SendCustomBiomes(BinaryWriter writer)
        {
            for (var i = 0; i < BiomeLibs.biomes.Count; i++)
                BiomeLibs.biomes.Values.ToList()[i].SendCustomBiomes(writer);
        }

        public override void ReceiveCustomBiomes(BinaryReader reader)
        {
            for (var i = 0; i < BiomeLibs.biomes.Count; i++)
                BiomeLibs.biomes.Values.ToList()[i].ReceiveCustomBiomes(reader);
        }
    }
}
