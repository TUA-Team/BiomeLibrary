using BiomeLibrary.API;
using BiomeLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace BiomeLibrary
{
    public partial class BiomeWorld : ModWorld
    {
        public void DecideEvil(GenerationProgress progress)
        {
            if (BiomeWorld.PendingEvil == "Random")
            {
                List<ModBiome> allEvil = BiomeLibs.Biomes.Values.Where(i => i.BiomeAlt == BiomeAlternative.evilAlt).ToList();
                List<String> evilName = new List<string>();
                evilName.Add("Corruption");
                evilName.Add("Crimson");
                foreach (ModBiome biome in allEvil)
                {
                    evilName.Add(biome.BiomeName);
                }
                BiomeWorld.PendingEvil = evilName[WorldGen.genRand.Next(evilName.Count)];               
            }
            if (BiomeWorld.PendingEvil == "Crimson")
                WorldGen.crimson = true;
        }

        public void GenerateEvil(GenerationProgress progress, PassLegacy pass)
        {
            switch (BiomeWorld.PendingEvil)
            {
                case "Corruption":
                    GenerateCorruption(progress, pass);
                    break;
                case "Crimson":
                    GenerateCrimson(progress, pass);
                    break;
                default:
                    BiomeLibs.Biomes.Values.Single(i=>i.BiomeName == BiomeWorld.PendingEvil).BiomeAltWorldGeneration(progress, pass);
                    break;
            }
        }

        public void GenerateCorruption(GenerationProgress progress, PassLegacy evilPass)
        {
            FieldInfo info = typeof(PassLegacy).GetField("_method", BindingFlags.Instance | BindingFlags.NonPublic);
            WorldGenLegacyMethod method = (WorldGenLegacyMethod)info.GetValue(evilPass);
            var dungeonSideInfo = method.Method.DeclaringType?.GetFields
                (
                    BindingFlags.NonPublic |
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.Static
                )
                .Single(x => x.Name == "dungeonSide");
            int dungeonSide = (int)dungeonSideInfo.GetValue(method.Target);
            int i2;
            progress.Message = Lang.gen[20].Value;
            int num19 = 0;
            while ((double)num19 < (double)Main.maxTilesX * 0.00045)
            {
                float value2 = (float)((double)num19 / ((double)Main.maxTilesX * 0.00045));
                progress.Set(value2);
                bool flag5 = false;
                int num20 = 0;
                int num21 = 0;
                int num22 = 0;
                while (!flag5)
                {
                    int num23 = 0;
                    flag5 = true;
                    int num24 = Main.maxTilesX / 2;
                    int num25 = 200;
                    num20 = WorldGen.genRand.Next(320, Main.maxTilesX - 320);
                    num21 = num20 - WorldGen.genRand.Next(200) - 100;
                    num22 = num20 + WorldGen.genRand.Next(200) + 100;
                    if (num21 < 285)
                    {
                        num21 = 285;
                    }
                    if (num22 > Main.maxTilesX - 285)
                    {
                        num22 = Main.maxTilesX - 285;
                    }
                    if (num20 > num24 - num25 && num20 < num24 + num25)
                    {
                        flag5 = false;
                    }
                    if (num21 > num24 - num25 && num21 < num24 + num25)
                    {
                        flag5 = false;
                    }
                    if (num22 > num24 - num25 && num22 < num24 + num25)
                    {
                        flag5 = false;
                    }
                    if (num20 > WorldGen.UndergroundDesertLocation.X && num20 < WorldGen.UndergroundDesertLocation.X + WorldGen.UndergroundDesertLocation.Width)
                    {
                        flag5 = false;
                    }
                    if (num21 > WorldGen.UndergroundDesertLocation.X && num21 < WorldGen.UndergroundDesertLocation.X + WorldGen.UndergroundDesertLocation.Width)
                    {
                        flag5 = false;
                    }
                    if (num22 > WorldGen.UndergroundDesertLocation.X && num22 < WorldGen.UndergroundDesertLocation.X + WorldGen.UndergroundDesertLocation.Width)
                    {
                        flag5 = false;
                    }
                    for (int num26 = num21; num26 < num22; num26++)
                    {
                        for (int num27 = 0; num27 < (int)Main.worldSurface; num27 += 5)
                        {
                            if (Main.tile[num26, num27].active() && Main.tileDungeon[(int)Main.tile[num26, num27].type])
                            {
                                flag5 = false;
                                break;
                            }
                            if (!flag5)
                            {
                                break;
                            }
                        }
                    }
                    if (num23 < 200 && (int)typeof(WorldGen).GetField("JungleX", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) > num21 && (int)typeof(WorldGen).GetField("JungleX", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) < num22)
                    {
                        num23++;
                        flag5 = false;
                    }
                }
                int num28 = 0;
                for (int num29 = num21; num29 < num22; num29++)
                {
                    if (num28 > 0)
                    {
                        num28--;
                    }
                    if (num29 == num20 || num28 == 0)
                    {
                        int num30 = (int)WorldGen.worldSurfaceLow;
                        while ((double)num30 < Main.worldSurface - 1.0)
                        {
                            if (Main.tile[num29, num30].active() || Main.tile[num29, num30].wall > 0)
                            {
                                if (num29 == num20)
                                {
                                    num28 = 20;
                                    WorldGen.ChasmRunner(num29, num30, WorldGen.genRand.Next(150) + 150, true);
                                    break;
                                }
                                if (WorldGen.genRand.Next(35) == 0 && num28 == 0)
                                {
                                    num28 = 30;
                                    bool makeOrb = true;
                                    WorldGen.ChasmRunner(num29, num30, WorldGen.genRand.Next(50) + 50, makeOrb);
                                    break;
                                }
                                break;
                            }
                            else
                            {
                                num30++;
                            }
                        }
                    }
                    int num31 = (int)WorldGen.worldSurfaceLow;
                    while ((double)num31 < Main.worldSurface - 1.0)
                    {
                        if (Main.tile[num29, num31].active())
                        {
                            int num32 = num31 + WorldGen.genRand.Next(10, 14);
                            for (int num33 = num31; num33 < num32; num33++)
                            {
                                if ((Main.tile[num29, num33].type == 59 || Main.tile[num29, num33].type == 60) && num29 >= num21 + WorldGen.genRand.Next(5) && num29 < num22 - WorldGen.genRand.Next(5))
                                {
                                    Main.tile[num29, num33].type = 0;
                                }
                            }
                            break;
                        }
                        num31++;
                    }
                }
                double num34 = Main.worldSurface + 40.0;
                for (int num35 = num21; num35 < num22; num35++)
                {
                    num34 += (double)WorldGen.genRand.Next(-2, 3);
                    if (num34 < Main.worldSurface + 30.0)
                    {
                        num34 = Main.worldSurface + 30.0;
                    }
                    if (num34 > Main.worldSurface + 50.0)
                    {
                        num34 = Main.worldSurface + 50.0;
                    }
                    i2 = num35;
                    bool flag6 = false;
                    int num36 = (int)WorldGen.worldSurfaceLow;
                    while ((double)num36 < num34)
                    {
                        if (Main.tile[i2, num36].active())
                        {
                            if (Main.tile[i2, num36].type == 53 && i2 >= num21 + WorldGen.genRand.Next(5) && i2 <= num22 - WorldGen.genRand.Next(5))
                            {
                                Main.tile[i2, num36].type = 112;
                            }
                            if (Main.tile[i2, num36].type == 0 && (double)num36 < Main.worldSurface - 1.0 && !flag6)
                            {
                                typeof(WorldGen).GetField("grassSpread", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, 0);
                                WorldGen.SpreadGrass(i2, num36, 0, 23, true, 0);
                            }
                            flag6 = true;
                            if (Main.tile[i2, num36].type == 1 && i2 >= num21 + WorldGen.genRand.Next(5) && i2 <= num22 - WorldGen.genRand.Next(5))
                            {
                                Main.tile[i2, num36].type = 25;
                            }
                            if (Main.tile[i2, num36].wall == 216)
                            {
                                Main.tile[i2, num36].wall = 217;
                            }
                            else if (Main.tile[i2, num36].wall == 187)
                            {
                                Main.tile[i2, num36].wall = 220;
                            }
                            if (Main.tile[i2, num36].type == 2)
                            {
                                Main.tile[i2, num36].type = 23;
                            }
                            if (Main.tile[i2, num36].type == 161)
                            {
                                Main.tile[i2, num36].type = 163;
                            }
                            else if (Main.tile[i2, num36].type == 396)
                            {
                                Main.tile[i2, num36].type = 400;
                            }
                            else if (Main.tile[i2, num36].type == 397)
                            {
                                Main.tile[i2, num36].type = 398;
                            }
                        }
                        num36++;
                    }
                }
                for (int num37 = num21; num37 < num22; num37++)
                {
                    for (int num38 = 0; num38 < Main.maxTilesY - 50; num38++)
                    {
                        if (Main.tile[num37, num38].active() && Main.tile[num37, num38].type == 31)
                        {
                            int num39 = num37 - 13;
                            int num40 = num37 + 13;
                            int num41 = num38 - 13;
                            int num42 = num38 + 13;
                            for (int num43 = num39; num43 < num40; num43++)
                            {
                                if (num43 > 10 && num43 < Main.maxTilesX - 10)
                                {
                                    for (int num44 = num41; num44 < num42; num44++)
                                    {
                                        if (Math.Abs(num43 - num37) + Math.Abs(num44 - num38) < 9 + WorldGen.genRand.Next(11) && WorldGen.genRand.Next(3) != 0 && Main.tile[num43, num44].type != 31)
                                        {
                                            Main.tile[num43, num44].active(true);
                                            Main.tile[num43, num44].type = 25;
                                            if (Math.Abs(num43 - num37) <= 1 && Math.Abs(num44 - num38) <= 1)
                                            {
                                                Main.tile[num43, num44].active(false);
                                            }
                                        }
                                        if (Main.tile[num43, num44].type != 31 && Math.Abs(num43 - num37) <= 2 + WorldGen.genRand.Next(3) && Math.Abs(num44 - num38) <= 2 + WorldGen.genRand.Next(3))
                                        {
                                            Main.tile[num43, num44].active(false);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                num19++;
            }
        }

        public void GenerateCrimson(GenerationProgress progress, PassLegacy evilPass)
        {
            FieldInfo info = typeof(PassLegacy).GetField("_method", BindingFlags.Instance | BindingFlags.NonPublic);
            WorldGenLegacyMethod method = (WorldGenLegacyMethod)info.GetValue(evilPass);
            var dungeonSideInfo = method.Method.DeclaringType?.GetFields
                (
                    BindingFlags.NonPublic |
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.Static
                )
                .Single(x => x.Name == "dungeonSide");
            int dungeonSide = (int)dungeonSideInfo.GetValue(method.Target);


            progress.Message = Lang.gen[72].Value;
            int num = 0;
            int i2;
            while ((double)num < (double)Main.maxTilesX * 0.00045)
            {
                float value = (float)((double)num / ((double)Main.maxTilesX * 0.00045));
                progress.Set(value);
                bool flag2 = false;
                int num2 = 0;
                int num3 = 0;
                int num4 = 0;
                while (!flag2)
                {
                    int num5 = 0;
                    flag2 = true;
                    int num6 = Main.maxTilesX / 2;
                    int num7 = 200;
                    if (dungeonSide < 0)
                    {
                        num2 = WorldGen.genRand.Next(600, Main.maxTilesX - 320);
                    }
                    else
                    {
                        num2 = WorldGen.genRand.Next(320, Main.maxTilesX - 600);
                    }
                    num3 = num2 - WorldGen.genRand.Next(200) - 100;
                    num4 = num2 + WorldGen.genRand.Next(200) + 100;
                    if (num3 < 285)
                    {
                        num3 = 285;
                    }
                    if (num4 > Main.maxTilesX - 285)
                    {
                        num4 = Main.maxTilesX - 285;
                    }
                    if (dungeonSide < 0 && num3 < 400)
                    {
                        num3 = 400;
                    }
                    else if (dungeonSide > 0 && num3 > Main.maxTilesX - 400)
                    {
                        num3 = Main.maxTilesX - 400;
                    }
                    if (num2 > num6 - num7 && num2 < num6 + num7)
                    {
                        flag2 = false;
                    }
                    if (num3 > num6 - num7 && num3 < num6 + num7)
                    {
                        flag2 = false;
                    }
                    if (num4 > num6 - num7 && num4 < num6 + num7)
                    {
                        flag2 = false;
                    }
                    if (num2 > WorldGen.UndergroundDesertLocation.X && num2 < WorldGen.UndergroundDesertLocation.X + WorldGen.UndergroundDesertLocation.Width)
                    {
                        flag2 = false;
                    }
                    if (num3 > WorldGen.UndergroundDesertLocation.X && num3 < WorldGen.UndergroundDesertLocation.X + WorldGen.UndergroundDesertLocation.Width)
                    {
                        flag2 = false;
                    }
                    if (num4 > WorldGen.UndergroundDesertLocation.X && num4 < WorldGen.UndergroundDesertLocation.X + WorldGen.UndergroundDesertLocation.Width)
                    {
                        flag2 = false;
                    }
                    for (int k = num3; k < num4; k++)
                    {
                        for (int l = 0; l < (int)Main.worldSurface; l += 5)
                        {
                            if (Main.tile[k, l].active() && Main.tileDungeon[(int)Main.tile[k, l].type])
                            {
                                flag2 = false;
                                break;
                            }
                            if (!flag2)
                            {
                                break;
                            }
                        }
                    }
                    if (num5 < 200 && (int)typeof(WorldGen).GetField("JungleX", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) > num3 && (int)typeof(WorldGen).GetField("JungleX", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) < num4)
                    {
                        num5++;
                        flag2 = false;
                    }
                }
                WorldGen.CrimStart(num2, (int)WorldGen.worldSurfaceLow - 10);
                for (int m = num3; m < num4; m++)
                {
                    int num8 = (int)WorldGen.worldSurfaceLow;
                    while ((double)num8 < Main.worldSurface - 1.0)
                    {
                        if (Main.tile[m, num8].active())
                        {
                            int num9 = num8 + WorldGen.genRand.Next(10, 14);
                            for (int n = num8; n < num9; n++)
                            {
                                if ((Main.tile[m, n].type == 59 || Main.tile[m, n].type == 60) && m >= num3 + WorldGen.genRand.Next(5) && m < num4 - WorldGen.genRand.Next(5))
                                {
                                    Main.tile[m, n].type = 0;
                                }
                            }
                            break;
                        }
                        num8++;
                    }
                }
                double num10 = Main.worldSurface + 40.0;
                for (int num11 = num3; num11 < num4; num11++)
                {
                    num10 += (double)WorldGen.genRand.Next(-2, 3);
                    if (num10 < Main.worldSurface + 30.0)
                    {
                        num10 = Main.worldSurface + 30.0;
                    }
                    if (num10 > Main.worldSurface + 50.0)
                    {
                        num10 = Main.worldSurface + 50.0;
                    }
                    i2 = num11;
                    bool flag3 = false;
                    int num12 = (int)WorldGen.worldSurfaceLow;
                    while ((double)num12 < num10)
                    {
                        if (Main.tile[i2, num12].active())
                        {
                            if (Main.tile[i2, num12].type == 53 && i2 >= num3 + WorldGen.genRand.Next(5) && i2 <= num4 - WorldGen.genRand.Next(5))
                            {
                                Main.tile[i2, num12].type = 234;
                            }
                            if (Main.tile[i2, num12].type == 0 && (double)num12 < Main.worldSurface - 1.0 && !flag3)
                            {
                                typeof(WorldGen).GetField("grassSpread", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, 0);
                                WorldGen.SpreadGrass(i2, num12, 0, 199, true, 0);
                            }
                            flag3 = true;
                            if (Main.tile[i2, num12].wall == 216)
                            {
                                Main.tile[i2, num12].wall = 218;
                            }
                            else if (Main.tile[i2, num12].wall == 187)
                            {
                                Main.tile[i2, num12].wall = 221;
                            }
                            if (Main.tile[i2, num12].type == 1)
                            {
                                if (i2 >= num3 + WorldGen.genRand.Next(5) && i2 <= num4 - WorldGen.genRand.Next(5))
                                {
                                    Main.tile[i2, num12].type = 203;
                                }
                            }
                            else if (Main.tile[i2, num12].type == 2)
                            {
                                Main.tile[i2, num12].type = 199;
                            }
                            else if (Main.tile[i2, num12].type == 161)
                            {
                                Main.tile[i2, num12].type = 200;
                            }
                            else if (Main.tile[i2, num12].type == 396)
                            {
                                Main.tile[i2, num12].type = 401;
                            }
                            else if (Main.tile[i2, num12].type == 397)
                            {
                                Main.tile[i2, num12].type = 399;
                            }
                        }
                        num12++;
                    }
                }
                int num13 = WorldGen.genRand.Next(10, 15);
                for (int num14 = 0; num14 < num13; num14++)
                {
                    int num15 = 0;
                    bool flag4 = false;
                    int num16 = 0;
                    while (!flag4)
                    {
                        num15++;
                        int num17 = WorldGen.genRand.Next(num3 - num16, num4 + num16);
                        int num18 = WorldGen.genRand.Next((int)(Main.worldSurface - (double)(num16 / 2)), (int)(Main.worldSurface + 100.0 + (double)num16));
                        if (num15 > 100)
                        {
                            num16++;
                            num15 = 0;
                        }
                        if (!Main.tile[num17, num18].active())
                        {
                            while (!Main.tile[num17, num18].active())
                            {
                                num18++;
                            }
                            num18--;
                        }
                        else
                        {
                            while (Main.tile[num17, num18].active() && (double)num18 > Main.worldSurface)
                            {
                                num18--;
                            }
                        }
                        if (num16 > 10 || (Main.tile[num17, num18 + 1].active() && Main.tile[num17, num18 + 1].type == 203))
                        {
                            WorldGen.Place3x2(num17, num18, 26, 1);
                            if (Main.tile[num17, num18].type == 26)
                            {
                                flag4 = true;
                            }
                        }
                        if (num16 > 100)
                        {
                            flag4 = true;
                        }
                    }
                }
                num++;
            }


        }
    }
}
