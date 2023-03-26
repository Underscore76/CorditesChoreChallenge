using System;
using HarmonyLib;
using StardewModdingAPI;
using StardewValley;

namespace ChoreChallenge.Framework.Achievements
{
	public class ClintDeservesLoveToo : IAchievement
	{
        private static ClintDeservesLoveToo instance;
        public ClintDeservesLoveToo()
            : base("Clint Deserves Love Too", 10)
        {
            instance = this;
        }

        public override void OnUpdate()
        {
            if (
                Game1.player.toolBeingUpgraded.Value != null &&
                Game1.player.toolBeingUpgraded.Value.UpgradeLevel == Tool.iridium)
            {
                HasSeen = true;
            }
            base.OnUpdate();
        }
    }
}

