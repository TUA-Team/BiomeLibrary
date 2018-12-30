using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiomeLibrary.API;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;

namespace BiomeLibrary
{
    public class BiomeWorld : ModWorld
    {
        public static bool infinite = false;
        public static bool chunked = false;
        public static readonly bool x64Terraria = IntPtr.Size == 8;

        public override void PostUpdate()
        {
        }

        public override void Load(TagCompound tag)
        {
            BiomeLibs.world = mod.GetModWorld<BiomeWorld>();
            resetList();
            base.Load(tag);
        }

        public override TagCompound Save()
        {
            TagCompound tag = new TagCompound();
            tag.Add("64bit", x64Terraria);
            tag.Add("chunked", chunked);
            tag.Add("infinite", infinite);
            return tag;
        }


        public void resetList() {
            BiomeLibs.reset();
        }

        [Obsolete("Now integrated into ModBiome", true)]
        internal void addBlock(String biomeName, String[] block)
        {
        }

        [Obsolete("Now integrated into ModBiome", true)]
        internal void addBlock(String biomeName, int[] blockID)
        {
        }

        public override void TileCountsAvailable(int[] tileCounts)
        {
            for (int i = 0; i < BiomeLibs.biomes.Count; i++)
                BiomeLibs.biomes.Values.ToList()[i].tileCount(tileCounts);
        }

        public override void ResetNearbyTileEffects()
        {
            for (int i = 0; i < BiomeLibs.biomes.Count; i++)
                BiomeLibs.biomes.Values.ToList()[i].TileCount = 0;
        }

        public override void ModifyHardmodeTasks(List<GenPass> list)
        {
            Main.hardMode = true;
            list[0] = (new PassLegacy("Hardmode Good", delegate
            {
                
                float num = (float)WorldGen.genRand.Next(300, 400) * 0.001f;
                float num2 = (float)WorldGen.genRand.Next(200, 300) * 0.001f;
                int num3 = (int)((float)Main.maxTilesX * num);
                int num4 = (int)((float)Main.maxTilesX * (1f - num));
                int num5 = 1;
                if (WorldGen.genRand.Next(2) == 0)
                {
                    num4 = (int)((float)Main.maxTilesX * num);
                    num3 = (int)((float)Main.maxTilesX * (1f - num));
                    num5 = -1;
                }
                int num6 = 1;
                if (WorldGen.dungeonX < Main.maxTilesX / 2)
                {
                    num6 = -1;
                }
                if (num6 < 0)
                {
                    if (num4 < num3)
                    {
                        num4 = (int)((float)Main.maxTilesX * num2);
                    }
                    else
                    {
                        num3 = (int)((float)Main.maxTilesX * num2);
                    }
                }
                else if (num4 > num3)
                {
                    num4 = (int)((float)Main.maxTilesX * (1f - num2));
                }
                else
                {
                    num3 = (int)((float)Main.maxTilesX * (1f - num2));
                }

                bool rand = Main.rand.NextBool();
                
                if (rand)
                {
                    WorldGen.GERunner(num3, 0, 3f * (float)3*num5, 5f, true);
                }
                else
                {
                    ModBiome biome;
                    while (true)
                    {

                        biome =
                            BiomeLibs.biomes.Values.ToList()[
                                Main.rand.Next(BiomeLibs.biomes.Values.ToList().Count)];
                        if (biome.IsHallowAlt)
                        {
                            break;
                        }
                    }

                    String message = "";
                    if (!biome.HallowAltGeneration(ref message))
                    {
                        BWRunner(num3, 0, blockFinder(biome.biomeBlock), (float)(3 * num5), 5f);
                    }  
                }
            }));
        }

        private int[] blockFinder(IList<int> biomeBlockList)
        {
            int[] blockList = { TileID.Grass, TileID.Dirt, TileID.Stone, TileID.Sand, TileID.Sandstone};            blockList[0] = ModExtension.FindTileIDInArray("Grass", biomeBlockList);
            blockList[1] = ModExtension.FindTileIDInArray("Dirt", biomeBlockList);
            blockList[2] = ModExtension.FindTileIDInArray("Stone", "Sand", biomeBlockList);
            blockList[3] = ModExtension.FindTileIDInArray("Sand", "Stone", biomeBlockList);
            blockList[4] = ModExtension.FindTileIDInArray("Ice", biomeBlockList);
            return blockList;
        }

        private void BWRunner(int i, int j, int[] blockList, float speedX = 0f, float speedY = 0f)
        {
            String text = "";
            text += mod.Name + "           ";
            for (int x = 0; x < blockList.Length; x++)
            {
                text += blockList[x] + "  ";
            }

            int num = WorldGen.genRand.Next(200, 250);
            float num2 = (float)(Main.maxTilesX / 4200);
            num = (int)((float)num * num2);
            double num3 = (double)num;
            Vector2 value;
            value.X = (float)i;
            value.Y = (float)j;
            Vector2 value2;
            value2.X = (float)WorldGen.genRand.Next(-10, 11) * 0.1f;
            value2.Y = (float)WorldGen.genRand.Next(-10, 11) * 0.1f;
            if (speedX != 0f || speedY != 0f)
            {
                value2.X = speedX;
                value2.Y = speedY;
            }
            bool flag = true;
            while (flag)
            {
                int num4 = (int)((double)value.X - num3 * 0.5);
                int num5 = (int)((double)value.X + num3 * 0.5);
                int num6 = (int)((double)value.Y - num3 * 0.5);
                int num7 = (int)((double)value.Y + num3 * 0.5);
                if (num4 < 0)
                {
                    num4 = 0;
                }
                if (num5 > Main.maxTilesX)
                {
                    num5 = Main.maxTilesX;
                }
                if (num6 < 0)
                {
                    num6 = 0;
                }
                if (num7 > Main.maxTilesY)
                {
                    num7 = Main.maxTilesY;
                }
                for (int k = num4; k < num5; k++)
                {
                    for (int l = num6; l < num7; l++)
                    {
                        if ((double)(System.Math.Abs((float)k - value.X) + System.Math.Abs((float)l - value.Y)) < (double)num * 0.5 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.015))
                        {
                            /*if (Main.tile[k, l].wall == 63 || Main.tile[k, l].wall == 65 || Main.tile[k, l].wall == 66 || Main.tile[k, l].wall == 68 || Main.tile[k, l].wall == 69 || Main.tile[k, l].wall == 81)
                            {
                                
                            }
                            if (Main.tile[k, l].wall == 3 || Main.tile[k, l].wall == 83)
                            {
                                Main.tile[k, l].wall = 28;
                            }*/
                            if (Main.tile[k, l].type == 0 || Main.tile[k, l].type == TileID.Mud)
                            {
                                Main.tile[k, l].type = (ushort) blockList[3];
                                WorldGen.SquareTileFrame(k, l, true);
                            }
                            if (Main.tile[k, l].type == 1 || Main.tile[k, l].type == 25 || Main.tile[k, l].type == 203)
                            {
                                Main.tile[k, l].type = (ushort)blockList[1];
                                WorldGen.SquareTileFrame(k, l, true);
                            }
                            if (Main.tile[k, l].type == 2 || Main.tile[k, l].type == 23 || Main.tile[k, l].type == 199)
                            {
                                Main.tile[k, l].type = (ushort)blockList[0];
                                WorldGen.SquareTileFrame(k, l, true);
                            }
                            if (Main.tile[k, l].type == 53 || Main.tile[k, l].type == 123 || Main.tile[k, l].type == 112 || Main.tile[k, l].type == 234)
                            {
                                Main.tile[k, l].type = (ushort)blockList[2];
                                WorldGen.SquareTileFrame(k, l, true);
                            }
                            if (Main.tile[k, l].type == 161 || Main.tile[k, l].type == 163 || Main.tile[k, l].type == 200)
                            {
                                Main.tile[k, l].type = (ushort)blockList[4];
                                WorldGen.SquareTileFrame(k, l, true);     
                            }
                        }
                    }
                }
                value += value2;
                value2.X += (float)WorldGen.genRand.Next(-10, 11) * 0.05f;
                if (value2.X > speedX + 1f)
                {
                    value2.X = speedX + 1f;
                }
                if (value2.X < speedX - 1f)
                {
                    value2.X = speedX - 1f;
                }
                if (value.X < -(float)num || value.Y < -(float)num || value.X > (float)(Main.maxTilesX + num) || value.Y > (float)(Main.maxTilesX + num))
                {
                    flag = false;
                }
            }

        }
    }
}
