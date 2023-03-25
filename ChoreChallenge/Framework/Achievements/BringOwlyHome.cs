
using System;
using StardewModdingAPI;
using StardewValley;
using HarmonyLib;

namespace ChoreChallenge.Achievements
{
    public class BringOwlyHome : IAchievement
    {
        private static BringOwlyHome instance;
        public BringOwlyHome()
            : base("Bring Owly Home", 10)
        {
            instance = this;
        }

        public override void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Utility), "tryToPlaceItem"),
                postfix: new HarmonyMethod(typeof(BringOwlyHome), nameof(BringOwlyHome.Postfix_tryToPlaceItem))
                );
        }

        public static void Postfix_tryToPlaceItem(bool __result, GameLocation location, Item item)
        {
            if (__result && location.Name == "FarmHouse" && item.Name == "Stone Owl")
            {
                instance.HasSeen = true;
            }
            instance.Monitor.Log($"{__result} {location.Name} {item.Name}", LogLevel.Alert);
        }
    }
}

