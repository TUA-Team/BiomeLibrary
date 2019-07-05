using BiomeLibrary.UIs;
using log4net;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace BiomeLibrary
{
	public sealed class BiomeLibrary : Mod
    {
        private const int EVIL_BIOME_SELECTION_MENU_ID = -71;
        private EvilBiomeSelectionUIState _evilBiomeSelectionUIState;

		public BiomeLibrary()
        {
            Instance = this;
        }

        public override void Load()
        {
            Autoloading.Initialize();
            Autoloading.ActionOnAllMods(BiomeLoader.AutoloadBiomes);

            _evilBiomeSelectionUIState = new EvilBiomeSelectionUIState();

            Main.OnTick += UpdateTick;
        }

        private void UpdateTick()
        {
            if (PreviousMenuMode != Main.menuMode)
                Logger.Info("Menu mode: " + Main.menuMode);

            PreviousMenuMode = Main.menuMode;

            if (Main.menuMode == EVIL_BIOME_SELECTION_MENU_ID) // UI ID for Evil Biome selection.
                SetMenuUIState(_evilBiomeSelectionUIState);
        }

        private static void SetMenuUIState(UIState uiState)
        {
            Main.menuMode = 888;
            Main.MenuUI.SetState(uiState);
        }

        public override void Unload()
        {
            Instance = null;
            BiomeLoader.Unload();

            _evilBiomeSelectionUIState = null;

            Main.OnTick -= UpdateTick;
        }
        
        public static BiomeLibrary Instance { get; private set; }

        public static int PreviousMenuMode { get; internal set; }
	}
}