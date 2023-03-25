using System;
using System.Collections.Generic;
using StardewModdingAPI;
using HarmonyLib;
using StardewValley;

namespace ChoreChallenge.Framework.Achievements
{
    public class ArtConnoisseurI : IAchievement
    {
        private static ArtConnoisseurI instance;
        public ArtConnoisseurI()
            : base("Art Connoisseur I", 5)
        {
            instance = this;
        }

        public override void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Utility), "tryToPlaceItem"),
                postfix: new HarmonyMethod(typeof(ArtConnoisseurI), nameof(ArtConnoisseurI.Postfix_tryToPlaceItem))
                );
        }

        public static void Postfix_tryToPlaceItem(bool __result, GameLocation location, Item item)
        {
            if (__result && location.Name == "FarmHouse" && item.Name == "'Highway 89'")
            {
                instance.HasSeen = true;
            }
        }
    }
    public class ArtConnoisseurII : IAchievement
    {
        private static ArtConnoisseurII instance;
        public ArtConnoisseurII()
            : base("Art Connoisseur II", 5)
        {
            instance = this;
        }

        public override void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Utility), "tryToPlaceItem"),
                postfix: new HarmonyMethod(typeof(ArtConnoisseurII), nameof(ArtConnoisseurII.Postfix_tryToPlaceItem))
                );
        }

        public static void Postfix_tryToPlaceItem(bool __result, GameLocation location, Item item)
        {
            if (__result && location.Name == "FarmHouse" && item.Name == "'Vista'")
            {
                instance.HasSeen = true;
            }
        }
    }
    public class ArtConnoisseurIII : IAchievement
    {
        private static ArtConnoisseurIII instance;
        public ArtConnoisseurIII()
            : base("Art Connoisseur III", 10)
        {
            instance = this;
        }

        public override void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Utility), "tryToPlaceItem"),
                postfix: new HarmonyMethod(typeof(ArtConnoisseurIII), nameof(ArtConnoisseurIII.Postfix_tryToPlaceItem))
                );
        }

        public static void Postfix_tryToPlaceItem(bool __result, GameLocation location, Item item)
        {
            if (__result && location.Name == "FarmHouse" && item.Name == "'A Night On Eco-Hill'")
            {
                instance.HasSeen = true;
            }
        }
    }

    public class ArtCollector : AchievementCollection
    {
        public ArtCollector()
            : base(
                  "Art Collector", 5,
                  new List<IAchievement>
                {
                      new ArtConnoisseurI(),
                      new ArtConnoisseurII(),
                      new ArtConnoisseurIII(),
                }
            )
        {
        }
    }
}

