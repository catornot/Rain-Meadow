﻿using Menu;
using Menu.Remix;
using Menu.Remix.MixedUI;
using UnityEngine;
namespace RainMeadow
{
    public class CustomInputDialogueBox : MenuDialogBox
    {
        protected MenuTabWrapper tabWrapper;
        public SymbolButton cancelButton;
        public SimpleButton continueButton;
        public OpTextBox textBox;
        public UIelementWrapper textBoxWrapper;

        public CustomInputDialogueBox(Menu.Menu menu, MenuObject owner, string text, string signalText, Vector2 pos, Vector2 size, bool forceWrapping = false)
            : base(menu, owner, text, pos, size, forceWrapping)
        {
            this.tabWrapper = new MenuTabWrapper(menu, this);
            subObjects.Add(tabWrapper);

            Vector2 center = size / 2f;
            textBox = new OpTextBox(new Configurable<string>(""), center + new Vector2(-80f, -15f), 160f);
            textBox.accept = OpTextBox.Accept.StringASCII;
            textBoxWrapper = new UIelementWrapper(this.tabWrapper, textBox);

            Vector2 where = new Vector2((center.x - 55f), 20f);

            continueButton = new SimpleButton(menu, this, menu.Translate("CONFIRM"), signalText, where, new Vector2(110f, 30f));
            subObjects.Add(continueButton);

            cancelButton = new SymbolButton(menu, this, "Menu_Symbol_Clear_All", "CLOSE_DIALOG", size - new Vector2(40f, 40f));
            subObjects.Add(cancelButton);
        }
        public override void RemoveSprites()
        {
            base.RemoveSprites();
            tabWrapper.RemoveSprites();
            continueButton.RemoveSprites();
        }
    }
}
