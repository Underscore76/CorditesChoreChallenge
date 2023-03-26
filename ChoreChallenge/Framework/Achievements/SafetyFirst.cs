using System;
using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Objects;


namespace ChoreChallenge.Framework.Achievements
{
	public class SafetyFirst : IAchievement
    {
        private const string HardHatName = "Hard Hat";
        private static SafetyFirst instance;
        public SafetyFirst()
            : base("Safety First", 10)
        {
            instance = this;
        }

        public override void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(FishTankFurniture), "checkForAction"),
                prefix: new HarmonyMethod(typeof(SafetyFirst), nameof(SafetyFirst.Prefix_checkForAction)),
                postfix: new HarmonyMethod(typeof(SafetyFirst), nameof(SafetyFirst.Postfix_checkForAction))
                );
        }

        public static Hat CurrentItemIsHat(Farmer who)
        {
            return who.CurrentItem as Hat;
        }

        public static bool Prefix_checkForAction(Farmer who, out bool __state)
        {
            Hat hat = CurrentItemIsHat(who);
            __state = hat != null && hat.Name == HardHatName;
            return true;
        }

        public static void Postfix_checkForAction(Farmer who, bool __state)
        {
            if (__state && CurrentItemIsHat(who) == null)
            {
                instance.HasSeen = true;
            }
        }
    }
}

