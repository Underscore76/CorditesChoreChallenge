using System;
using System.Collections.Generic;
using ChoreChallenge.Framework.Achievements;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;

namespace ChoreChallenge.Framework
{
    // Allows Scrolling History
	public class CustomChatBox : ChatBox
	{
        private readonly int MaxMessageToDisplay = 10;
        private int HistoryTail;
        private static IReflectionHelper Reflection;
        // a reference to the underlying chat boxes private messages
        private List<ChatMessage> Messages;

        public CustomChatBox()
            : base()
		{
            Messages = Reflection.GetField<List<ChatMessage>>(this, "messages").GetValue();
            maxMessages = 10000;
        }

        public static void Register(IModHelper helper)
        {
            Reflection = helper.Reflection;
        }

        public override void receiveScrollWheelAction(int direction)
        {
            if (choosingEmoji)
            {
                base.receiveScrollWheelAction(direction);
            }
            else
            {
                HistoryTail += Math.Sign(direction);
                HistoryTail = Math.Min(Messages.Count-1, Math.Max(HistoryTail, MaxMessageToDisplay));
            }
        }

        public override void addMessage(string message, Color color)
        {
            base.addMessage(message, color);
            HistoryTail = Messages.Count-1;
        }

        public override void draw(SpriteBatch b)
        {
            int heightSoFar = 0;
            bool drawBG = false;
            if (Messages.Count > 0)
            {
                for (int j = HistoryTail; j >= Math.Max(0, HistoryTail - MaxMessageToDisplay); j--)
                {
                    ChatMessage message = Messages[j];
                    if (chatBox.Selected || message.alpha > 0.01f)
                    {
                        heightSoFar += message.verticalSize;
                        drawBG = true;
                    }
                }
                if (drawBG)
                {
                    IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(301, 288, 15, 15), xPositionOnScreen, yPositionOnScreen - heightSoFar - 20 + ((!chatBox.Selected) ? chatBox.Height : 0), chatBox.Width, heightSoFar + 20, Color.White, 4f, drawShadow: false);
                }
                heightSoFar = 0;
                for (int i = HistoryTail; i >= Math.Max(0, HistoryTail - MaxMessageToDisplay); i--)
                {
                    ChatMessage message2 = Messages[i];
                    heightSoFar += message2.verticalSize;
                    message2.draw(b, xPositionOnScreen + 12, yPositionOnScreen - heightSoFar - 8 + ((!chatBox.Selected) ? chatBox.Height : 0));
                }
            }
            if (chatBox.Selected)
            {
                chatBox.Draw(b, drawShadow: false);
                emojiMenuIcon.draw(b, Color.White, 0.99f);
                if (choosingEmoji)
                {
                    emojiMenu.draw(b);
                }
                if (isWithinBounds(Game1.getMouseX(), Game1.getMouseY()) && !Game1.options.hardwareCursor)
                {
                    Game1.mouseCursor = (Game1.options.gamepadControls ? 44 : 0);
                }
            }
        }
    }
}

