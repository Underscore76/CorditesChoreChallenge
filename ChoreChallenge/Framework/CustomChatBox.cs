using System;
using System.Collections.Generic;
using System.Reflection;
using ChoreChallenge.Framework.Achievements;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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
        private const int MinScrollBarHeight = 20;
        private const int ScrollBarWidth = 32;
        private const int Offset = 8;

        private Rectangle ScrollBar;
        private Rectangle BackgroundRect;
        private Rectangle ScrollBarRegion;
        private bool IsScrolling;
        private bool DrawBG;

        // a reference to the underlying chat boxes private messages
        private List<ChatMessage> Messages;
        private ModEntry Mod;

        public CustomChatBox(ModEntry mod)
            : base()
		{
            Messages = Reflection.GetField<List<ChatMessage>>(this, "messages").GetValue();
            maxMessages = 10000;
            Mod = mod;

            ScrollBar = new Rectangle(0, 0, ScrollBarWidth, MinScrollBarHeight);
            UpdateBackgroundRect();
        }

        public static void Register(IModHelper helper)
        {
            Reflection = helper.Reflection;
        }

        #region controls
        private void SetScrollBarToTail()
        {
            if (Messages.Count > MaxMessageToDisplay)
            {
                ScrollBar.X = ScrollBarRegion.X;
                // what fraction of the bar should the pill take up
                ScrollBar.Height = Math.Max(MinScrollBarHeight, ScrollBarRegion.Height * MaxMessageToDisplay / Messages.Count);
                // fraction of messages above the tail
                float range = (float)(HistoryTail - MaxMessageToDisplay) / (Messages.Count - MaxMessageToDisplay);
                // place bar at clamped position
                ScrollBar.Y = (int)((ScrollBarRegion.Height-ScrollBar.Height) * range) + ScrollBarRegion.Y;
                if (ScrollBar.Bottom > ScrollBarRegion.Bottom)
                {
                    ScrollBar.Y = ScrollBarRegion.Bottom - ScrollBar.Height;
                }
            }
        }

        public Rectangle ClickableRegion
        {
            get
            {
                Rectangle rect = ScrollBarRegion;
                rect.Inflate(ScrollBarWidth, 0);
                rect.X -= ScrollBarWidth/2;
                return rect;
            }
        }

        public override void leftClickHeld(int x, int y)
        {
            if (IsScrolling)
            {
                float percentage = (float)(y - ScrollBarRegion.Y + ScrollBar.Height/2) / ScrollBarRegion.Height;
                int index = (int)(percentage * Messages.Count);
                HistoryTail = Math.Min(Messages.Count - 1, Math.Max(index, MaxMessageToDisplay));
                SetScrollBarToTail();
            }
            else
            {
                base.leftClickHeld(x, y);
            }
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            if (ClickableRegion.Contains(x, y))
            {
                // NOTE: THIS IS SOME JANK LOGIC I PULLED OUT OF THE GAME
                // For some reason, leftClickHeld isn't being called? so instead the game just does a
                // receiveLeftClick and force call leftClickHeld/release. It's so bizarre..
                IsScrolling = true;
                leftClickHeld(x, y);
                releaseLeftClick(x, y);
            }
            else
            {
                base.receiveLeftClick(x, y, playSound);
            }
        }

        public override void releaseLeftClick(int x, int y)
        {
            IsScrolling = false;
            base.releaseLeftClick(x, y);
        }

        public override void receiveScrollWheelAction(int direction)
        {
            if (choosingEmoji)
            {
                base.receiveScrollWheelAction(direction);
            }
            else
            {
                HistoryTail -= Math.Sign(direction);
                HistoryTail = Math.Min(Messages.Count-1, Math.Max(HistoryTail, MaxMessageToDisplay));
            }
        }
        #endregion

        #region input
        protected override void runCommand(string command)
        {
            if (command == "chores")
            {
                DrawHelper.DisplayInfo("Current Chore Status");
                Mod.PrintStatus();
            }
            else
            {
                base.runCommand(command);
                HistoryTail = Messages.Count - 1;
            }
        }
        public override void addErrorMessage(string message)
        {
            base.addErrorMessage(message);
            HistoryTail = Messages.Count - 1;
        }
        public override void addInfoMessage(string message)
        {
            base.addInfoMessage(message);
            HistoryTail = Messages.Count - 1;
        }
        public override void addMessage(string message, Color color)
        {
            base.addMessage(message, color);
            HistoryTail = Messages.Count-1;
        }
        #endregion

        public void UpdateBackgroundRect()
        {
            int heightSoFar = 0;
            DrawBG = false;
            for (int j = HistoryTail; j >= Math.Max(0, HistoryTail - MaxMessageToDisplay) && j < Messages.Count; j--)
            {
                ChatMessage message = Messages[j];
                if (chatBox.Selected || message.alpha > 0.01f)
                {
                    heightSoFar += message.verticalSize;
                    DrawBG = true;
                }
            }
            BackgroundRect = new Rectangle(
                xPositionOnScreen,
                yPositionOnScreen - heightSoFar - 20 + ((!chatBox.Selected) ? chatBox.Height : 0),
                chatBox.Width,
                heightSoFar + 20
                );
            ScrollBarRegion = new Rectangle(
                BackgroundRect.Right - Offset - ScrollBarWidth,
                BackgroundRect.Top + Offset,
                ScrollBarWidth,
                BackgroundRect.Height - 2*Offset
                );
            SetScrollBarToTail();
        }

        public override void update(GameTime time)
        {
            UpdateBackgroundRect();
            if (!IsScrolling)
            {
                SetScrollBarToTail();
            }

            base.update(time);
        }

        public override void draw(SpriteBatch b)
        {
            if (DrawBG)
            {
                IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(301, 288, 15, 15),
                    BackgroundRect.X, BackgroundRect.Y, BackgroundRect.Width, BackgroundRect.Height,
                    Color.White, 4f, drawShadow: false);
            }
            int heightSoFar = 0;
            for (int i = HistoryTail; i >= Math.Max(0, HistoryTail - MaxMessageToDisplay) && i < Messages.Count; i--)
            {
                ChatMessage message2 = Messages[i];
                heightSoFar += message2.verticalSize;
                message2.draw(b, xPositionOnScreen + 12, yPositionOnScreen - heightSoFar - 8 + ((!chatBox.Selected) ? chatBox.Height : 0));
            }
            if (chatBox.Selected)
            {
                chatBox.Draw(b, drawShadow: false);
                if (Messages.Count > MaxMessageToDisplay)
                {
                    b.Draw(Game1.staminaRect, ScrollBarRegion, new Color(16,16,16));
                    Rectangle rect = ScrollBar;
                    rect.Height += Offset/2;
                    b.Draw(Game1.staminaRect, rect, Color.White);
                }
                emojiMenuIcon.draw(b, Color.White, 0.99f);
                if (choosingEmoji)
                {
                    emojiMenu.draw(b);
                }

                if (!Game1.options.hardwareCursor)
                {
                    if (ClickableRegion.Contains(Game1.getMouseX(), Game1.getMouseY()))
                    {
                        Game1.mouseCursor = 44;
                    }
                    else if (isWithinBounds(Game1.getMouseX(), Game1.getMouseY()))
                    {
                        Game1.mouseCursor = (Game1.options.gamepadControls ? 44 : 0);
                    }
                }
            }
        }
    }
}

