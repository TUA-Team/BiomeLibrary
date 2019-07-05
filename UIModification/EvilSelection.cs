using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BiomeLibrary.API;
using BiomeLibrary.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace BiomeLibrary.UIModification
{
    public class EvilSelection : UIState
    {
        private UIList _allEvilAvailable;
        private UIScrollbar _scrollbar;
        private int _prevScreenWidth, _prevScreenHeight;

        public override void OnInitialize()
        {
            _allEvilAvailable = new UIList();
            List<ModBiome> allEvil = BiomeLibs.Biomes.Values.Where(i => i.BiomeAlt == BiomeAlternative.evilAlt).ToList();

            _scrollbar = new UIScrollbar();
            _scrollbar.Top.Set(-5, 0f);
            _scrollbar.HAlign = 1f;

            Add(GenerateButton("Corruption"));
            Add(GenerateButton("Crimson"));

            foreach (ModBiome biome in allEvil)
                Add(GenerateButton(biome));

            Add(GenerateButton("Random"));
            _allEvilAvailable.Width.Set(800, 0f);
            _allEvilAvailable.Height.Set(400, 0f);
            _allEvilAvailable.Left.Set(Main.screenWidth / 2 - 400, 0f);
            _allEvilAvailable.Top.Set(Main.screenHeight / 2 - 200, 0f);
            _allEvilAvailable.SetScrollbar(_scrollbar);
            Width.Set(Main.screenWidth, 0f);
            Height.Set(Main.screenHeight, 0f);
            Left.Set(0, 0f);
            Top.Set(0, 0f);
            Append(_allEvilAvailable);
            Append(_scrollbar);
        }

        public UIMenuButton GenerateButton(ModBiome biome)
        {
            UIMenuButton button = new UIMenuButton(biome.BiomeName, 5, 5);
            button.SetChangingSize(0.6f, 0.8f);
            button.OnClick += (evt, element) =>
            {
                BiomeWorld.currentEvil = biome.BiomeName;
                BiomeWorld.pendingEvil = biome.BiomeName;
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
                BiomeWorld.pendingEvil = biomeName;
                Main.menuMode = 7;
            };
            return button;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Main.screenWidth != _prevScreenWidth || Main.screenHeight != _prevScreenHeight)
            {
                _allEvilAvailable.Left.Set(Main.screenWidth / 2 - 400, 0f);
                _allEvilAvailable.Top.Set(Main.screenHeight / 2 - 200, 0f);
                Width.Set(Main.screenWidth, 0f);
                Height.Set(Main.screenHeight, 0f);
                Recalculate();
            }

            _prevScreenWidth = Main.screenWidth;
            _prevScreenHeight = Main.screenHeight;
        }

        public void Add(UIElement item)
        {
            _allEvilAvailable._items.Add(item);
            UIElement _innerList = (UIElement) typeof(UIList).GetField("_innerList", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(_allEvilAvailable);
            _innerList.Append(item);
            _innerList.Recalculate();
            typeof(UIList).GetField("_innerList", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(_allEvilAvailable, _innerList);
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
                if (element is UIScrollbar && _allEvilAvailable.Count <= 7)
                {
                    continue;
                }
                element.Draw(spriteBatch);
            }

            
        }
    }

}
