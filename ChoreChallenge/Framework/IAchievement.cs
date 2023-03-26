using System;
using StardewModdingAPI;
using StardewValley;
using HarmonyLib;
using Microsoft.Xna.Framework;

namespace ChoreChallenge.Framework
{
	public abstract class IAchievement
	{
        protected IMonitor Monitor;
        protected IReflectionHelper Reflection;

        public string Description { get; protected set; }
		public int Score { get; protected set; }
		public virtual int GetScore()
		{
			if (hasSeen) return Score;
			return 0;
		}

		private bool hasSeen;
		public bool HasSeen {
			get
			{
				return hasSeen;
			}
			protected set
			{
				if (hasSeen) return;
				if (value)
				{
					DisplayAchievement();
                }
				hasSeen = value;
			}
		}

		public IAchievement(string description, int score)
		{
			Description = description;
			Score = score;
		}
		public virtual void Initialize(IMonitor monitor, IReflectionHelper reflection)
		{
			Monitor = monitor;
			Reflection = reflection;
		}

		public void DisplayAchievement()
		{
			DrawHelper.DisplayAchievementUnlock(this);
        }

        public void DisplayInfo(string info)
        {
            DrawHelper.DisplayInfo(info);
        }

		public virtual void Patch(Harmony harmony) { }

		public virtual void OnSaveLoaded() { hasSeen = false; }
		public virtual void OnUpdate() { }
		public virtual void OnEnd() { OnUpdate(); }
	}
}

