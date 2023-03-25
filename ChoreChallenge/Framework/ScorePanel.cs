using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Tools;

namespace ChoreChallenge.Framework
{
    public class ScorePanel
    {
        private DateTime StartTime;
        public ScorePanel()
        {
            Start();
        }
        public void Start()
        {
            StartTime = DateTime.Now;
        }

        public void Draw(SpriteBatch spriteBatch, int score)
        {
            float tempZoom = SpriteText.fontPixelZoom;
            SpriteText.fontPixelZoom = 0.5f;
            Rectangle tsarea = Game1.game1.GraphicsDevice.Viewport.GetTitleSafeArea();
            tsarea.Width = Math.Max(
                SpriteText.getWidthOfString("mm:ss.fff"),
                SpriteText.getWidthOfString("Score: 999")
                );
            tsarea.X += SpriteText.getWidthOfString("          ") + 16;
            int fontHeight = (int)(SpriteText.getHeightOfString(" ") * 1.5);
            DrawHelper.StaticDraw(spriteBatch, DateTime.Now - StartTime, tsarea);
            tsarea.Y += fontHeight;
            DrawHelper.StaticDraw(spriteBatch, $"Score: {score}", tsarea);
            SpriteText.fontPixelZoom = tempZoom;
        }
    }

    public class DrawHelper
    {
        public static void StaticDraw(SpriteBatch spriteBatch, TimeSpan span, Rectangle rect)
        {
            SpriteText.drawString(
                spriteBatch,
                span.ToString(Format(span)),
                rect.Left + 16, rect.Top + 16,
                999999, rect.Width, 999999, 1f, 1f, false, 2, "", 4
            );
        }

        public static void StaticDraw(SpriteBatch spriteBatch, string text, Rectangle rect)
        {
            SpriteText.drawString(
                spriteBatch,
                text,
                rect.Left + 16, rect.Top + 16,
                999999, rect.Width, 999999, 1f, 1f, false, 2, "", 4
            );
        }

        public static string Format(TimeSpan span)
        {
            return (span.Hours > 0) ?
                @"hh\:mm\:ss\.fff" :
                @"mm\:ss\.fff";
        }
    }
}

