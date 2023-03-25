using System;
using System.Collections.Generic;
using HarmonyLib;
using StardewModdingAPI;
using StardewValley.Locations;

namespace ChoreChallenge.Framework.Achievements
{

	public class Pantry : IAchievement
	{
        private static Pantry instance;
        public Pantry()
            : base("Fully Stocked", 5)
        {
            instance = this;
        }

        public override void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(StardewValley.Locations.CommunityCenter), "markAreaAsComplete"),
                prefix: new HarmonyMethod(typeof(Pantry), nameof(Pantry.Prefix_markAreaAsComplete))
                );
        }

        public static bool Prefix_markAreaAsComplete(int area)
        {
            if (area == StardewValley.Locations.CommunityCenter.AREA_Pantry)
            {
                instance.HasSeen = true;
            }
            return true;
        }
    }

    public class BulletinBoard : IAchievement
    {
        private static BulletinBoard instance;
        public BulletinBoard()
            : base("Bulliten Board", 10)
        {
            instance = this;
        }

        public override void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(StardewValley.Locations.CommunityCenter), "markAreaAsComplete"),
                prefix: new HarmonyMethod(typeof(BulletinBoard), nameof(BulletinBoard.Prefix_markAreaAsComplete))
                );
        }

        public static bool Prefix_markAreaAsComplete(int area)
        {
            if (area == StardewValley.Locations.CommunityCenter.AREA_Bulletin)
            {
                instance.HasSeen = true;
            }
            return true;
        }
    }

    public class FishTank : IAchievement
    {
        private static FishTank instance;
        public FishTank()
            : base("Tanks for the Fish", 25)
        {
            instance = this;
        }

        public override void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(StardewValley.Locations.CommunityCenter), "markAreaAsComplete"),
                prefix: new HarmonyMethod(typeof(FishTank), nameof(FishTank.Prefix_markAreaAsComplete))
                );
        }

        public static bool Prefix_markAreaAsComplete(int area)
        {
            if (area == StardewValley.Locations.CommunityCenter.AREA_FishTank)
            {
                instance.HasSeen = true;
            }
            return true;
        }
    }

    public class CommunityCenter : AchievementCollection
	{
        public CommunityCenter() :
            base(
                "Happy Apples", 20,
                new List<IAchievement>
                {
                    new Pantry(),
                    new BulletinBoard(),
                    new FishTank()
                })
        {
        }
    }
}

