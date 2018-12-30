using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace BiomeLibrary.Commands
{
    class ChangeWorldSize : ModCommand
    {
        public override void Action(CommandCaller caller, string input, string[] args)
        {
            switch (args[0])
            {
                case "left":
                    if (!float.TryParse(args[1], out Main.leftWorld))
                    {
                        Main.leftWorld = 0;
                        Main.NewText("Error happened");
                        return;
                    }
                    Main.leftWorld *= 16;
                    BiomeWorld.chunked = true;
                    break;
                case "right":
                    if (!float.TryParse(args[1], out Main.rightWorld))
                    {
                        Main.rightWorld = Main.maxTilesX * 16;
                        Main.NewText("Error happened");
                        return;
                    }
                    Main.maxTilesX = (int)Main.rightWorld;
                    Main.rightWorld *= 16;
                    BiomeWorld.chunked = true;
                    break;
                case "top":
                    if (!float.TryParse(args[1], out Main.topWorld))
                    {
                        Main.topWorld = 0;
                        Main.NewText("Error happened");
                        return;
                    }
                    Main.topWorld *= 16;
                    BiomeWorld.chunked = true;
                    break;
                case "bottom":
                    if (!float.TryParse(args[1], out Main.bottomWorld))
                    {
                        Main.bottomWorld = Main.maxTilesY * 16;
                        Main.NewText("Error happened");
                        return;
                    }
                    Main.maxTilesY = (int)Main.bottomWorld;
                    Main.bottomWorld *= 16;
                    BiomeWorld.chunked = true;
                    break;
            }

            if (Main.maxTilesY > 2400 || Main.maxTilesX > 8400)
            {
                BiomeWorld.infinite = true;
            }
        }

        public override string Command
        {
            get { return "ChangeWorldSize"; }
        }
        public override CommandType Type
        {
            get { return CommandType.World; }
        }
    }
}
