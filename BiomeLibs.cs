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
		public static BiomePlayer Player;
		public static BiomeWorld World;
		public static Mod Instance;


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

		public override void Unload()
		{
			Biomes.Clear();
			Biomes = null;
		}

		public override void Load()
		{
			Biomes = new Dictionary<string, ModBiome>();
			Instance = this;
			LoadModContent(Autoload);
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

		
	}
}
