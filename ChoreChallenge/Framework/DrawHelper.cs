using System;
using Microsoft.Xna.Framework;
using StardewValley;

namespace ChoreChallenge.Framework
{
	public static class DrawHelper
	{
		public static readonly Color AchievementColor = Color.Yellow;
        public static readonly Color InfoColor = Color.LightGray;
		public static readonly Color UnlockColor = Color.Green;
		public static readonly Color LockedColor = Color.Gray;
        public static readonly Color FinishedColor = Color.Red;
		public static readonly Color WarningColor = Color.Red;

		public static void DisplayAchievementUnlock(IAchievement achievement)
		{
            Game1.chatBox.addMessage($"Unlocked \"{achievement.Description}\" ({achievement.Score})", AchievementColor);
        }
		public static void DisplayAchievementStatus(IAchievement achievement)
		{
			if (achievement.HasSeen)
			{
				Game1.chatBox.addMessage($"    Unlocked: \"{achievement.Description}\" ({achievement.Score})", UnlockColor);
			}
			else
			{
                Game1.chatBox.addMessage($"    Locked: \"{achievement.Description}\" ({achievement.Score})", LockedColor);
            }
        }
		public static void DisplayInfo(string info)
		{
            Game1.chatBox.addMessage(info, InfoColor);
        }
		public static void DisplayScore(int score, TimeSpan duration)
		{
            Game1.chatBox.addMessage($"Ended with {score} points in {duration.ToString(@"mm\:ss\.fff")}", FinishedColor);
        }

		public static void DisplayWarning(string warning)
		{
            Game1.chatBox.addMessage(warning, WarningColor);
        }
	}
}

