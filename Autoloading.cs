using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;

namespace BiomeLibrary
{
    internal static class Autoloading
    {
        private static readonly List<Mod> _foundMods = new List<Mod>();

        internal static void Initialize()
        {
            for (int i = 0; i < ModLoader.Mods.Length; i++)
                _foundMods.Add(ModLoader.Mods[i]);
        }

        internal static void ActionOnAllMods(Action<Mod> action)
        {
            for (int i = 0; i < _foundMods.Count; i++)
                try
                {
                    action?.Invoke(ModLoader.Mods[i]);
                }
                catch (Exception)
                {
                    // Ignored
                }
        }
    }
}