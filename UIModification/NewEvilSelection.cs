﻿using System;
using System.Collections.Generic;
using System.Linq;
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
    class NewEvilSelection : UIState
    {
        public UIPanel mainPanelContent;
        private UITextPanel<string> evilText = new UITextPanel<string>("");
        private UITextPanel<string> leftArrow = new UITextPanel<string>("<", 1f, false);
        private UITextPanel<string> rightArrow = new UITextPanel<string>(">", 1f, false);
        private UITextPanel<string> worldEvilPickText = new UITextPanel<string>("Pick world evil", 1f, true);
        private UITextPanel<string> selectButton = new UITextPanel<string>("select");
        private UITextPanel<string> backButton = new UITextPanel<string>("back");

        private List<ModBiome> allEvil;

        private List<String> evilName;

        private int listIndex = 0;

        //Picture must be 464x124

        public override void OnInitialize()
        {
            allEvil = BiomeLibs.Biomes.Values.Where(i => i.BiomeAlt == BiomeAlternative.evilAlt).ToList();

            evilName = new List<string>();
            evilName.Add("Corruption");
            evilName.Add("Crimson");
            foreach (ModBiome biome in allEvil)
            {
                evilName.Add(biome.BiomeName);
            }
            evilName.Add("Random");

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

            worldEvilPickText.BackgroundColor = Color.Transparent;
            worldEvilPickText.BorderColor = Color.Transparent;
            worldEvilPickText.HAlign = 0.5f;
            worldEvilPickText.VAlign = 0.6f;
            worldEvilPickText.Top.Set(-50f, 0);


            selectButton.OnClick += Select;
            backButton.OnClick += Back;

            mainPanelContent.Append(rightArrow);
            mainPanelContent.Append(leftArrow);
            mainPanelContent.Append(evilText);
            Append(selectButton);
            Append(backButton);

            Append(mainPanelContent);
            Append(worldEvilPickText);            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            evilText.SetText(evilName[listIndex]);
            Vector2 textPanelBarOffset = new Vector2(5, 124 + 5);
            Vector2 mainPanelPosition = mainPanelContent.GetInnerDimensions().Position();
            Vector2 mainPanelDimension = new Vector2(mainPanelContent.GetInnerDimensions().Width, mainPanelContent.GetInnerDimensions().Height);

            leftArrow.Left.Set(textPanelBarOffset.X, 0);
            leftArrow.Top.Set(textPanelBarOffset.Y, 0);

            rightArrow.Left.Set(textPanelBarOffset.X + 40 + 380 + 4, 0);
            rightArrow.Top.Set(textPanelBarOffset.Y, 0);

            evilText.Left.Set(textPanelBarOffset.X + 40 + 2, 0);
            evilText.Top.Set(textPanelBarOffset.Y, 0);

            worldEvilPickText.Top.Set(-130f, 0);

            selectButton.Top.Set(mainPanelPosition.Y + mainPanelDimension.Y + selectButton.Height.Pixels + 3, 0);
            selectButton.Left.Set(mainPanelPosition.X + mainPanelDimension.X - selectButton.Width.Pixels, 0);

            backButton.Top.Set(mainPanelPosition.Y + mainPanelDimension.Y + backButton.Height.Pixels + 3, 0);
            backButton.Left.Set(mainPanelPosition.X, 0);
        }

        public void LeftArrow(UIMouseEvent mouseEvent, UIElement targetElement)
        {
            listIndex--;
            if (listIndex < 0)
            {
                listIndex = evilName.Count - 1;
            }
        }

        public void RightArrow(UIMouseEvent mouseEvent, UIElement targetElement)
        {
            listIndex++;
            if (listIndex >= evilName.Count)
            {
                listIndex = 0;
            }
        }

        public void Select(UIMouseEvent mouseEvent, UIElement targetElement)
        {
            BiomeWorld.currentEvil = evilName[listIndex];
            BiomeWorld.PendingEvil = evilName[listIndex];
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
            Texture2D evilPreview = null;
            if (evilName[listIndex] == "Corruption")
            {
                evilPreview = BiomeLibs.Instance.GetTexture("Texture/Evil/Corruption");
            }
            else if (evilName[listIndex] == "Crimson")
            {
                evilPreview = BiomeLibs.Instance.GetTexture("Texture/Evil/Crimson");
            }
            else if (evilName[listIndex] == "Random")
            {
                evilPreview = BiomeLibs.Instance.GetTexture("Texture/Random");
            }

            return evilPreview;
        }
    }
}