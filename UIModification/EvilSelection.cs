using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BiomeLibrary.API;
using BiomeLibrary.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace BiomeLibrary.UIModification
{
    [Obsolete("Remaking this entirely")]
    class EvilSelection : UIState
    {
        private UIList allEvilAvailable;
        private UIScrollbar scrollbar;
        private int prevScreenWidth, prevScreenHeight;

        public override void OnInitialize()
        {
            allEvilAvailable = new UIList();
            List<ModBiome> allEvil = BiomeLibs.Biomes.Values.Where(i => i.BiomeAlt == BiomeAlternative.evilAlt).ToList();

            scrollbar = new UIScrollbar();
            scrollbar.Top.Set(-5, 0f);
            scrollbar.HAlign = 1f;

            Add(GenerateButton("Corruption"));
            Add(GenerateButton("Crimson"));
            foreach (var biome in allEvil)
            {
                Add(GenerateButton(biome));
            }
            
            Add(GenerateButton("Random"));
            allEvilAvailable.Width.Set(800, 0f);
            allEvilAvailable.Height.Set(400, 0f);
            allEvilAvailable.Left.Set(Main.screenWidth / 2 - 400, 0f);
            allEvilAvailable.Top.Set(Main.screenHeight / 2 - 200, 0f);
            allEvilAvailable.SetScrollbar(scrollbar);
            Width.Set(Main.screenWidth, 0f);
            Height.Set(Main.screenHeight, 0f);
            Left.Set(0, 0f);
            Top.Set(0, 0f);
            Append(allEvilAvailable);
            Append(scrollbar);
        }

        public UIMenuButton GenerateButton(ModBiome biome)
        {
            UIMenuButton button = new UIMenuButton(biome.BiomeName, 5, 5);
            button.SetChangingSize(0.6f, 0.8f);
            button.OnClick += (evt, element) =>
            {
                BiomeWorld.currentEvil = biome.BiomeName;
                BiomeWorld.PendingEvil = biome.BiomeName;
                Main.menuMode = 7;
            };
            return button;
        }

        public UIMenuButton GenerateButton(String biomeName)
        {
            UIMenuButton button = new UIMenuButton(biomeName, 5, 5);
            button.SetChangingSize(0.6f, 0.8f);
            button.OnClick += (evt, element) =>
            {
                BiomeWorld.currentEvil = biomeName;
                BiomeWorld.PendingEvil = biomeName;
                Main.menuMode = 7;
            };
            return button;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Main.screenWidth != prevScreenWidth || Main.screenHeight != prevScreenHeight)
            {
                allEvilAvailable.Left.Set(Main.screenWidth / 2 - 400, 0f);
                allEvilAvailable.Top.Set(Main.screenHeight / 2 - 200, 0f);
                Width.Set(Main.screenWidth, 0f);
                Height.Set(Main.screenHeight, 0f);
                Recalculate();
            }

            prevScreenWidth = Main.screenWidth;
            prevScreenHeight = Main.screenHeight;
        }

        public void Add(UIElement item)
        {
            allEvilAvailable._items.Add(item);
            UIElement _innerList = (UIElement) typeof(UIList).GetField("_innerList", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(allEvilAvailable);
            _innerList.Append(item);
            _innerList.Recalculate();
            typeof(UIList).GetField("_innerList", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(allEvilAvailable, _innerList);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            String text = "Select world evil";
            CalculatedStyle style = GetInnerDimensions();
            Vector2 vector = Main.fontDeathText.MeasureString(text);
            float x = style.Width / 2 - vector.X / 2;
            Utils.DrawBorderStringFourWay(spriteBatch, Main.fontDeathText, text, x, 180, Color.Gray, Color.Black, default(Vector2), 0.8f);
        }

        protected override void DrawChildren(SpriteBatch spriteBatch)
        {
            foreach (UIElement element in this.Elements)
            {
                if (element is UIScrollbar && allEvilAvailable.Count <= 7)
                {
                    continue;
                }
                element.Draw(spriteBatch);
            }

            
        }
    }

}
