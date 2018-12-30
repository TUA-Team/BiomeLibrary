using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BiomeLibrary.API;
using Terraria;
using Terraria.ModLoader;

namespace BiomeLibrary
{
    public class BiomeLibs : Mod
	{
	    public static BiomePlayer player;
	    public static BiomeWorld world;
	    public static Mod instance;


        public static Dictionary<String, ModBiome> biomes = new Dictionary<string, ModBiome>();

        public BiomeLibs()
		{
            
		    world = this.GetModWorld<BiomeWorld>();
		    instance = this;
		    Properties = new ModProperties()
		    {
		        Autoload = true,
		        AutoloadGores = true,
		        AutoloadSounds = true
		    };
            
        }

	    public override void Unload()
	    {
            biomes = new Dictionary<string, ModBiome>();
	    }

	    public override void Load()
	    {
	        instance = this;
            reset();
	        LoadModContent(mod =>
	        {
	            Autoload(mod);
	        });
        }

        internal static void reset() {
            biomes.Clear();
        }

	    [Obsolete("Now automaticaly loaded on Mod Loading", true)]
        public static void RegisterNewBiome(String biomeName, int minTileRequired, Mod mod)
	    {
	    }

	    [Obsolete("Now integrated into ModBiome", true)]
        public static void addHallowAltBiome(String biomeName, string message = null)
	    {
	    }

	    [Obsolete("Now integrated into ModBiome", true)]
        public static bool InBiome(String biomeName)
	    {
	        return false;
	    }

	    [Obsolete("Now integrated into ModBiome", true)]
        public static void AddBlockInBiome(String biomeName, String[] blockName)
	    {
	        if (world == null)
	        {
	            world = instance.GetModWorld<BiomeWorld>();
	        }
	        world.addBlock(biomeName, blockName);
	    }

	    [Obsolete("Now integrated into ModBiome", true)]
        public static void AddBlockInBiomeByID(String biomeName, int[] blockID)
        {
            if (world == null)
            {
                world = instance.GetModWorld<BiomeWorld>();
            }
            world.addBlock(biomeName, blockID);
        }

	    [Obsolete("Now integrated into ModBiome", true)]
        public static void setBlockMin(String biomeName, int limit) {
        }

        public static void SetCondition(String biomeName, Func<bool> condition)
        {
        }

	    internal void Autoload(Mod mod)
	    {

	        if (mod.Code == null)
	            return;

	        foreach (Type type in mod.Code.GetTypes().OrderBy(type => type.FullName, StringComparer.InvariantCulture))
	        {
	            if (type.IsSubclassOf(typeof(ModBiome)))
	            {
                    
	                AutoloadBiome(mod.Name, type);
	            }

	        }
	    }

	    private static void LoadModContent(Action<Mod> loadAction)
	    {
	        //Object o = new OverworldHandler();
	        int num = 0;
	        foreach (var mod in ModLoader.LoadedMods)
	        {
	            try
	            {
	                loadAction(mod);
	            }
	            catch (Exception e)
	            {
	            }
	        }
	    }

	    private void AutoloadBiome(String modName, Type type)
	    {
	        ModBiome biome = (ModBiome)Activator.CreateInstance(type);
            biomes.Add(modName + ":" + type.Name, biome);
	    }
    }
}
