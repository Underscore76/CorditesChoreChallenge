using System;
using HarmonyLib;
using StardewModdingAPI;
using StardewValley;

namespace ChoreChallenge.Framework.Achievements
{
	public class Capitalist : IAchievement
	{
        protected const int MoneyThreshold = 65000;
        private static Capitalist instance;
        public Capitalist()
            : base("Capitalist", 10)
        {
            instance = this;
        }

        public override void Patch(Harmony harmony)
        {
            harmony.Patch(
                    original: AccessTools.Method(typeof(Farmer), "performPassOut"),
                    prefix: new HarmonyMethod(typeof(Capitalist), nameof(Capitalist.Prefix_performPassOut))
                );
            harmony.Patch(
                    original: AccessTools.Method(typeof(GameLocation), "startSleep"),
                    prefix: new HarmonyMethod(typeof(Capitalist), nameof(Capitalist.Prefix_startSleep))
                );
        }
        protected void RunEndOfDay()
        {
            HasSeen = Game1.player.Money >= MoneyThreshold;
        }
        public static bool Prefix_performPassOut()
        {
            instance.RunEndOfDay();
            return true;
        }

        public static bool Prefix_startSleep()
        {
            instance.RunEndOfDay();
            return true;
        }
        public override void OnEnd()
        {
            RunEndOfDay();
            base.OnEnd();
        }
    }
}

