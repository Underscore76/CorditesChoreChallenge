using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;
using System;

namespace ChoreChallenge.Framework.UI
{
    // modified from
    // https://github.com/tylergibbs2/StardewValleyMods/blob/master/Circuit/UI/RunTimerMenu.cs
    public class RunTimerMenu : IClickableMenu
    {
        private DateTime StartTime;
        private DateTime EndTime;
        private TimeSpan MaxDuration;
        private int LastScore;
        public bool HasEnded;

        public TimeSpan RunDuration
        {
            get
            {
                if (HasEnded)
                    return EndTime - StartTime;
                return Duration;
            }
        }
        public TimeSpan Duration
        {
            get
            {
                return DateTime.Now - StartTime;
            }
        }

        public int ScoreXPosition => xPositionOnScreen + width + 4;
        public int ScoreWidth => width + 8;


        public RunTimerMenu(TimeSpan duration) : base()
        {
            MaxDuration = duration;
            Start();

            CalculatePositions();
        }

        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            base.gameWindowSizeChanged(oldBounds, newBounds);

            CalculatePositions();
        }

        private void CalculatePositions()
        {
            xPositionOnScreen = 84;
            yPositionOnScreen = 7;

            if (Game1.dialogueFont != null)
            {
                Vector2 textSize = Game1.dialogueFont.MeasureString(GetFormattedTime(Duration));
                width = (int)textSize.X + 36;
                height = (int)textSize.Y + 12;
            }
            else
            {
                width = 174;
                height = 90;
            }
        }

        public void Start()
        {
            StartTime = DateTime.Now;
            HasEnded = false;
            CalculatePositions();
        }
        public void End()
        {
            EndTime = DateTime.Now;
            HasEnded = true;
        }
        public void Update(int score)
        {
            if (Duration >= MaxDuration)
            {
                End();
            }
            LastScore = score;
        }

        public static string GetFormattedTime(TimeSpan span)
        {
            return span.ToString(@"mm\:ss");
        }

        private void DrawTimerBox(SpriteBatch b)
        {
            drawTextureBox(
                b,
                Game1.menuTexture,
                new Rectangle(0, 256, 60, 60),
                xPositionOnScreen,
                yPositionOnScreen,
                width,
                height,
                Color.White,
                drawShadow: false
            );
        }

        private void DrawTimeRemaining(SpriteBatch b)
        {
            string text = GetFormattedTime(Duration);
            Vector2 textSize = Game1.dialogueFont.MeasureString(text);

            Utility.drawTextWithShadow(
                b,
                text,
                Game1.dialogueFont,
                new Vector2(
                    xPositionOnScreen + (width / 2) - ((int)textSize.X / 2),
                    yPositionOnScreen + (height / 2) - ((int)textSize.Y / 2) + 2
                ),
                Game1.textColor
            );
        }

        private void DrawScoreBox(SpriteBatch b)
        {
            drawTextureBox(
                b,
                Game1.menuTexture,
                new Rectangle(0, 256, 60, 60),
                ScoreXPosition,
                yPositionOnScreen,
                ScoreWidth,
                height,
                Color.White,
                drawShadow: false
            );
        }

        private void DrawScore(SpriteBatch b)
        {
            string text = $"={(300+LastScore).ToString().PadLeft(3)}";
            Vector2 textSize = Game1.dialogueFont.MeasureString(text);

            Utility.drawTextWithShadow(
                b,
                text,
                Game1.dialogueFont,
                new Vector2(
                    ScoreXPosition + (ScoreWidth / 2) - ((int)textSize.X / 2),
                    yPositionOnScreen + (height / 2) - ((int)textSize.Y / 2) + 2
                ),
                Game1.textColor
            );
        }

        public override void draw(SpriteBatch b)
        {
            base.draw(b);

            DrawTimerBox(b);
            DrawTimeRemaining(b);
            DrawScoreBox(b);
            DrawScore(b);

            drawMouse(b);
        }
    }
}