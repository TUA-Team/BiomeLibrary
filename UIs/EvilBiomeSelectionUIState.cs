using System;
using System.Collections.Generic;
using System.Linq;
using BiomeLibrary.Enums;
using BiomeLibrary.Worlds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace BiomeLibrary.UIs
{
    public class EvilBiomeSelectionUIState : UIState
    {
        public UIPanel mainPanelContent;
        private UITextPanel<string> 
            evilText = new UITextPanel<string>(""),
            leftArrow = new UITextPanel<string>("<", 1f, false),
            rightArrow = new UITextPanel<string>(">", 1f, false),
            _worldEvilPickText = new UITextPanel<string>("Pick world evil", 1f, true),
            _selectButton = new UITextPanel<string>("Select"),
            _backButton = new UITextPanel<string>("Back");

        private List<ModBiome> _allEvilBiomes;
        private List<Tuple<string, string>> _evilBiomeNames;

        private int _listIndex = 0;

        //Picture must be 464x124

        public override void OnInitialize()
        {
            _allEvilBiomes = BiomeLoader.loadedBiomes.Values.Where(i => i.BiomeAlternative == BiomeAlternative.Evil).ToList();

            _evilBiomeNames = new List<Tuple<string, string>>();
            _evilBiomeNames.Add(new Tuple<string, string>("Vanilla:Corruption", "Corruption"));
            _evilBiomeNames.Add(new Tuple<string, string>("Vanilla:Crimson", "Crimson"));

            foreach (ModBiome biome in _allEvilBiomes)
                _evilBiomeNames.Add(new Tuple<string, string>(biome.BiomeInternalName, biome.BiomeName));

            _evilBiomeNames.Add(new Tuple<string, string>("BiomeLibs:Random", "Random"));

            this.Width.Set(Main.screenWidth, 0);
            this.Height.Set(Main.screenHeight, 0);
            this.Top.Set(0, 0);
            this.Left.Set(0, 0);

            mainPanelContent = new UIPanel();
            mainPanelContent.HAlign = 0.5f;
            mainPanelContent.VAlign = 0.6f;
            mainPanelContent.Width.Set(500, 0);
            mainPanelContent.Height.Set(190, 0);

            leftArrow = new UITextPanel<string>("<", 1f, false);
            leftArrow.Width.Set(40, 0);
            leftArrow.Height.Set(40, 0);
            leftArrow.OnClick += LeftArrow;

            rightArrow = new UITextPanel<string>(">", 1f, false);
            rightArrow.Width.Set(40, 0);
            rightArrow.Height.Set(40, 0);
            rightArrow.OnClick += RightArrow;

            evilText = new UITextPanel<string>("");
            evilText.Width.Set(380, 0);
            evilText.Height.Set(40, 0);

            _worldEvilPickText.BackgroundColor = Color.Transparent;
            _worldEvilPickText.BorderColor = Color.Transparent;
            _worldEvilPickText.HAlign = 0.5f;
            _worldEvilPickText.VAlign = 0.6f;
            _worldEvilPickText.Top.Set(-50f, 0);


            _selectButton.OnClick += Select;
            _backButton.OnClick += Back;

            mainPanelContent.Append(rightArrow);
            mainPanelContent.Append(leftArrow);
            mainPanelContent.Append(evilText);

            Append(_selectButton);
            Append(_backButton);

            Append(mainPanelContent);
            Append(_worldEvilPickText);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            evilText.SetText(_evilBiomeNames[_listIndex].Item2);
            Vector2 textPanelBarOffset = new Vector2(5, 124 + 5);
            Vector2 mainPanelPosition = mainPanelContent.GetInnerDimensions().Position();
            Vector2 mainPanelDimension = new Vector2(mainPanelContent.GetInnerDimensions().Width, mainPanelContent.GetInnerDimensions().Height);

            leftArrow.Left.Set(textPanelBarOffset.X, 0);
            leftArrow.Top.Set(textPanelBarOffset.Y, 0);

            rightArrow.Left.Set(textPanelBarOffset.X + 40 + 380 + 4, 0);
            rightArrow.Top.Set(textPanelBarOffset.Y, 0);

            evilText.Left.Set(textPanelBarOffset.X + 40 + 2, 0);
            evilText.Top.Set(textPanelBarOffset.Y, 0);

            _worldEvilPickText.Top.Set(-130f, 0);

            _selectButton.Top.Set(mainPanelPosition.Y + mainPanelDimension.Y + 15, 0);
            _selectButton.Left.Set(mainPanelPosition.X + mainPanelDimension.X - _selectButton.GetInnerDimensions().Width - 12, 0);

            _backButton.Top.Set(mainPanelPosition.Y + mainPanelDimension.Y + 15, 0);
            _backButton.Left.Set(mainPanelPosition.X - 12, 0);

            Recalculate();
        }

        public void LeftArrow(UIMouseEvent mouseEvent, UIElement targetElement)
        {
            _listIndex--;

            if (_listIndex < 0)
                _listIndex = _evilBiomeNames.Count - 1;
        }

        public void RightArrow(UIMouseEvent mouseEvent, UIElement targetElement)
        {
            _listIndex++;

            if (_listIndex >= _evilBiomeNames.Count)
                _listIndex = 0;
        }

        public void Select(UIMouseEvent mouseEvent, UIElement targetElement)
        {
            BiomeLibraryWorld.CurrentEvil = _evilBiomeNames[_listIndex].Item2;
            BiomeLibraryWorld.PendingEvil = _evilBiomeNames[_listIndex].Item2;
            Main.menuMode = 7;
        }

        public void Back(UIMouseEvent mouseEvent, UIElement targetElement)
        {
            Main.menuMode = -7;
        }

        protected override void DrawChildren(SpriteBatch spriteBatch)
        {
            base.DrawChildren(spriteBatch);
            CalculatedStyle innerDimension = mainPanelContent.GetInnerDimensions();


            var evilPreview = EvilPreview();

            float textureScaleX = 1f;
            float textureScaleY = 1f;

            if (evilPreview.Width > 464)
            {
                textureScaleX = 464 / evilPreview.Width;
            }

            if (evilPreview.Height > 124)
            {
                textureScaleY = 124 / evilPreview.Height;
            }

            Vector2 texturePostion = new Vector2(464 / 2 - evilPreview.Width * textureScaleX / 2,
                124 / 2 - EvilPreview().Height * textureScaleY / 2);

            spriteBatch.Draw(evilPreview, new Vector2(innerDimension.X + 5f, innerDimension.Y) + texturePostion, null, Color.White, 0f, Vector2.Zero, new Vector2(textureScaleX, textureScaleY), SpriteEffects.None, 1f);

        }

        private Texture2D EvilPreview()
        {
            switch (_evilBiomeNames[_listIndex].Item2)
            {
                case "Corruption":
                    return BiomeLibrary.Instance.GetTexture("Textures/Evil/Corruption");
                case "Crimson":
                    return BiomeLibrary.Instance.GetTexture("Textures/Evil/Crimson");
                case "Random":
                    return BiomeLibrary.Instance.GetTexture("Textures/Random");
                default:
                    return BiomeLoader.loadedBiomes[_evilBiomeNames[_listIndex].Item1].BiomePreview;
            }
        }
    }
}