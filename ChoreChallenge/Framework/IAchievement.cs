using System;
using StardewModdingAPI;
using StardewValley;
using HarmonyLib;

namespace ChoreChallenge
{
	public abstract class IAchievement
	{
        protected IMonitor Monitor;
        protected IReflectionHelper Reflection;

        protected string Description;
		public virtual int GetScore()
		{
			if (hasSeen) return Score;
			return 0;
		}
		protected int Score;

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
					DisplayAchievement(Description);
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

		public void DisplayAchievement(string description)
		{
            Game1.addHUDMessage(new HUDMessage(description, achievement: true));
        }

        public void DisplayInfo(string info)
        {
            Game1.addHUDMessage(new HUDMessage(info, HUDMessage.newQuest_type));
        }

		public virtual void Patch(Harmony harmony) { }

		public virtual void OnSaveLoaded() { hasSeen = false; }
		public virtual void OnUpdate() { }
	}
}

