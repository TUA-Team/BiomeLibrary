using System;
using System.Collections.Generic;
using System.Linq;
using BiomeLibrary.API;
using BiomeLibrary.UIModification;
using log4net;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace BiomeLibrary
{
	public class BiomeLibs : Mod
	{
		public static BiomePlayer Player;
		public static BiomeWorld World;
		public static Mod Instance;

	    public static int previousMenuMode = 0; 

		private readonly NewEvilSelection newEvilSelection = new NewEvilSelection();


		public static Dictionary<String, ModBiome> Biomes = new Dictionary<string, ModBiome>();

		public BiomeLibs()
		{
			
			World = this.GetModWorld<BiomeWorld>();
			Instance = this;
			Properties = new ModProperties()
			{
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
			
		}

		

		public override void Load()
		{
			Biomes = new Dictionary<string, ModBiome>();
			Instance = this;
		    Main.OnTick += UpdateTick;
		}

	    public override void Unload()
	    {
	        Biomes.Clear();
	        Biomes = null;
	        Instance = null;
	        Main.OnTick -= UpdateTick;
	    }

        public override void PostSetupContent()
	    {
	        LoadModContent(Autoload);
	    }

        internal void UpdateTick()
	    {
	        if (previousMenuMode != Main.menuMode)
	        {
	            LogManager.GetLogger("Menu mode").Info(Main.menuMode);
	        }

	        previousMenuMode = Main.menuMode;
	        if (Main.menuMode == -71)
	        {
	            SetMenuUIState(newEvilSelection);
	        }
        }

	    internal void Autoload(Mod mod)
	    {

	        if (mod.Code == null)
	            return;

	        foreach (Type type in mod.Code.GetTypes().OrderBy(type => type.FullName, StringComparer.InvariantCulture))
	        {
	            if (type.IsSubclassOf(typeof(ModBiome)))
	            {
	                mod.AutoloadBiome(type);
	            }
	        }
	    }

        internal static void SetMenuUIState(UIState state)
	    {
	        Main.menuMode = 888;
            Main.MenuUI.SetState(state);
	    }

        private static void LoadModContent(Action<Mod> loadAction)
		{
			//Object o = new OverworldHandler();
			int num = 0;
			foreach (var mod in ModLoader.Mods)
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
	}
}
