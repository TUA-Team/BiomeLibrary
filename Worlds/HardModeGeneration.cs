using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BiomeLibrary.Enums;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace BiomeLibrary.Worlds
{
    public sealed partial class BiomeLibraryWorld : ModWorld
    {
        public void GenerateGoodHardmodeBiome(GenerationProgress progress)
        {
            float num = (float)WorldGen.genRand.Next(300, 400) * 0.001f;
            float num2 = (float)WorldGen.genRand.Next(200, 300) * 0.001f;

            int num3 = (int)((float)Main.maxTilesX * num);
            int num5 = 1;

            if (WorldGen.genRand.Next(2) == 0)
            {
                num5 = -1;
            }
            int num6 = 1;
            if (WorldGen.dungeonX < Main.maxTilesX / 2)
            {
                num6 = -1;
            }
            if (num6 < 0)
            {

                num3 = (int)((float)Main.maxTilesX * num2);

            }
            else
            {
                num3 = (int)((float)Main.maxTilesX * (1f - num2));
            }
            DetermineHallowAlt(num3, num5);
        }

        public void GenerateBadHardmodeBiome(GenerationProgress progress)
        {
            float num = (float)WorldGen.genRand.Next(300, 400) * 0.001f;
            float num2 = (float)WorldGen.genRand.Next(200, 300) * 0.001f;

            int num4 = (int)((float)Main.maxTilesX * (1f - num));
            int num5 = 1;

            if (WorldGen.genRand.Next(2) == 0)
            {
                num4 = (int)((float)Main.maxTilesX * num);
                num5 = -1;
            }

            int num6 = 1;

            if (WorldGen.dungeonX < Main.maxTilesX / 2)
                num6 = -1;

            if (num6 < 0)
                num4 = (int)((float)Main.maxTilesX * num2);

            GenerateEvilAlt(num4, num5, BiomeLoader.loadedBiomes.Values.ToList());
        }

        private void DetermineHallowAlt(int num3, int num5)
        {
            VanillaBiome currentEvil = Main.ActiveWorldFileData.HasCorruption ? VanillaBiome.Corruption : VanillaBiome.Crimson;

            List<ModBiome> allAltToGen = BiomeLoader.loadedBiomes.Values.Where(biome =>
                biome.BiomeAlternative == BiomeAlternative.Hallow && 
                biome.BiomeSpecific == VanillaBiome.BothEvilBiomes.ToString()).ToList();

            if (!BiomeLoader.loadedBiomes.Values.Any(biome =>
                    biome.BiomeSpecific == VanillaBiome.Crimson.ToString() || biome.BiomeAlternative == BiomeAlternative.Hallow)
                && currentEvil == VanillaBiome.Crimson)
            {
                Main.NewText("The fantasy creature has arrived!", Color.LightCyan);
                WorldGen.GERunner(num3, 0, 3f * (float)3 * num5, 5f, true);
                return;
            }

            ExtractAllSpecificAlt(currentEvil, allAltToGen);
            GenerateHallowAlt(num3, num5, currentEvil, allAltToGen);

            MethodInfo draw = typeof(Main).GetMethod("Draw", BindingFlags.Instance | BindingFlags.NonPublic);
            Object[] obj = { Main._drawInterfaceGameTime };
            draw.Invoke(Main.instance, obj);
        }

        private void GenerateHallowAlt(int num3, int num5, VanillaBiome currentEvil, List<ModBiome> allAltToGen)
        {
            ModBiome biome;

            int rng = (currentEvil == VanillaBiome.Corruption ? WorldGen.genRand.Next(allAltToGen.Count) : WorldGen.genRand.Next(allAltToGen.Count - 1));

            if (rng == allAltToGen.Count)
            {
                Main.NewText("The fantasy creature has arrived!", Color.LightCyan);
                WorldGen.GERunner(num3, 0, 3f * (float)3 * num5, 5f, true);
            }

            biome = allAltToGen[rng];


            string message = "";
            if (!biome.BiomeAlternativeGeneration(ref message))
            {
                Main.NewText(message);
                BWRunner(num3, 0, blockFinder(biome.biomeBlocks), (float)(3 * num5), 5f);
            }
        }

        private void GenerateEvilAlt(int num4, int num5, List<ModBiome> allBiome)
        {
            if (CurrentEvil == VanillaBiome.Corruption.ToString() || CurrentEvil == VanillaBiome.Crimson.ToString())
            {
                WorldGen.GERunner(num4, 0, (float)(3 * -(float)num5), 5f, false);
                return;
            }

            ModBiome biome = allBiome.Single(i => i.BiomeName == CurrentEvil);
        }

        private static void ExtractAllSpecificAlt(VanillaBiome currentEvil, List<ModBiome> allAltToGen)
        {
            switch (currentEvil)
            {
                case VanillaBiome.Corruption:
                    allAltToGen.AddRange(BiomeLoader.loadedBiomes.Values.Where(biome =>
                        biome.BiomeSpecific == VanillaBiome.Corruption.ToString() &&
                        biome.BiomeAlternative == BiomeAlternative.Hallow).ToList());
                    break;

                case VanillaBiome.Crimson:
                    allAltToGen.AddRange(BiomeLoader.loadedBiomes.Values.Where(biome =>
                        biome.BiomeSpecific == VanillaBiome.Crimson.ToString() &&
                        biome.BiomeAlternative == BiomeAlternative.Hallow).ToList());
                    break;

                case VanillaBiome.Modded:
                    allAltToGen.AddRange(BiomeLoader.loadedBiomes.Values.Where(biome =>
                        biome.BiomeSpecific == VanillaBiome.Modded.ToString() &&
                        biome.BiomeAlternative == BiomeAlternative.Hallow && biome.BiomeSpecific == CurrentEvil).ToList());
                    break;
            }
        }


        private int[] blockFinder(IList<int> biomeBlockList)
        {
            int[] blockList = { TileID.Grass, TileID.Dirt, TileID.Stone, TileID.Sand, TileID.Sandstone, TileID.HardenedSand, TileID.IceBlock };

            blockList[0] = Utilities.FindModdedTileIDInArray(biomeBlockList, "Grass");
            blockList[1] = Utilities.FindModdedTileIDInArray(biomeBlockList, "Dirt");
            blockList[2] = Utilities.FindModdedTileIDInArray(biomeBlockList, "Stone", "Sand");
            blockList[3] = Utilities.FindModdedTileIDInArray(biomeBlockList, "Sand", "Hardened");
            blockList[4] = Utilities.FindModdedTileIDInArray(biomeBlockList, "Sandstone");
            blockList[5] = Utilities.FindModdedTileIDInArray(biomeBlockList, "HardenedSand");
            blockList[6] = Utilities.FindModdedTileIDInArray(biomeBlockList, "Ice");

            return blockList;
        }

        private void BWRunner(int i, int j, int[] blockList, float speedX = 0f, float speedY = 0f)
        {
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
                        if ((double)(Math.Abs((float)k - value.X) + Math.Abs((float)l - value.Y)) < (double)num * 0.5 * (1.0 + (double)WorldGen.genRand.Next(-10, 11) * 0.015))
                        {
                            /*if (Main.tile[k, l].wall == 63 || Main.tile[k, l].wall == 65 || Main.tile[k, l].wall == 66 || Main.tile[k, l].wall == 68 || Main.tile[k, l].wall == 69 || Main.tile[k, l].wall == 81)
                            {
                                
                            }
                            if (Main.tile[k, l].wall == 3 || Main.tile[k, l].wall == 83)
                            {
                                Main.tile[k, l].wall = 28;
                            }*/

                            int type = Main.tile[k, l].type;
                            if (Main.tile[k, l].type == 0)
                            {
                                Main.tile[k, l].type = (ushort)blockList[1];
                                WorldGen.SquareTileFrame(k, l, true);
                            }
                            if (TileID.Sets.Conversion.Stone[type])
                            {
                                Main.tile[k, l].type = (ushort)blockList[2];
                                WorldGen.SquareTileFrame(k, l, true);
                            }
                            if (TileID.Sets.Conversion.Grass[type])
                            {
                                Main.tile[k, l].type = (ushort)blockList[0];
                                WorldGen.SquareTileFrame(k, l, true);
                            }
                            if (TileID.Sets.Conversion.Sand[type])
                            {
                                Main.tile[k, l].type = (ushort)blockList[3];
                                WorldGen.SquareTileFrame(k, l, true);
                            }
                            if (TileID.Sets.Conversion.Ice[type])
                            {
                                Main.tile[k, l].type = (ushort)blockList[6];
                                WorldGen.SquareTileFrame(k, l, true);
                            }
                            if (TileID.Sets.Conversion.HardenedSand[type])
                            {
                                Main.tile[k, l].type = (ushort)blockList[5];
                                WorldGen.SquareTileFrame(k, l, true);
                            }
                            if (TileID.Sets.Conversion.Sandstone[type])
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