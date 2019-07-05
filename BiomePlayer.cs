using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BiomeLibrary
{
    public class BiomePlayer : ModPlayer
    {
        public override void Load(TagCompound tag)
        {
            
            BiomeLibs.Player = Main.LocalPlayer.GetModPlayer<BiomePlayer>();
        }  

        public override void SendCustomBiomes(BinaryWriter writer)
        {
            /*for (var i = 0; i < BiomeLibs.biomes.Count; i++)
                BiomeLibs.biomes.Values.ToList()[i].SendCustomBiomes(writer);*/
        }

        public override void ReceiveCustomBiomes(BinaryReader reader)
        {
            /*for (var i = 0; i < BiomeLibs.biomes.Count; i++)
                BiomeLibs.biomes.Values.ToList()[i].ReceiveCustomBiomes(reader);*/
        }
    }
}
