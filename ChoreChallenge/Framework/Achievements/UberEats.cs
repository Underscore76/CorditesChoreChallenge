using System;
using System.Collections.Generic;
using HarmonyLib;
using StardewModdingAPI;
using StardewValley;

namespace ChoreChallenge.Framework.Achievements
{
    public class UberEatsDriverI : IAchievement
    {
        private static UberEatsDriverI instance;

        private readonly string NPCName = "Jodi";
        private readonly string ObjectName = "Pancakes";
        private readonly int TimeStart = 900;
        private readonly int TimeEnd = 1000;
        public UberEatsDriverI()
            : base("Uber Eats Driver I", 5)
        {
            instance = this;
        }

        public override void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(NPC), "receiveGift"),
                prefix: new HarmonyMethod(typeof(UberEatsDriverI), nameof(UberEatsDriverI.Prefix_receiveGift))
                );
        }

        public static bool Prefix_receiveGift(NPC __instance, StardewValley.Object o)
        {
            instance.Monitor.Log($"{__instance.Name} {o.name} {Game1.timeOfDay}", LogLevel.Alert);
            if (
                __instance.Name == instance.NPCName &&
                o.name == instance.ObjectName &&
                instance.TimeStart <= Game1.timeOfDay &&
                Game1.timeOfDay <= instance.TimeEnd
                )
            {
                instance.HasSeen = true;
            }
            return true;
        }
    }
    public class UberEatsDriverII : IAchievement
    {
        private static UberEatsDriverII instance;

        private readonly string NPCName = "Caroline";
        private readonly string ObjectName = "Fish Taco";
        private readonly int TimeStart = 1200;
        private readonly int TimeEnd = 1300;
        public UberEatsDriverII()
            : base("Uber Eats Driver II", 5)
        {
            instance = this;
        }

        public override void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(NPC), "receiveGift"),
                prefix: new HarmonyMethod(typeof(UberEatsDriverII), nameof(UberEatsDriverII.Prefix_receiveGift))
                );
        }

        public static bool Prefix_receiveGift(NPC __instance, StardewValley.Object o)
        {
            if (
                __instance.Name == instance.NPCName &&
                o.name == instance.ObjectName &&
                instance.TimeStart <= Game1.timeOfDay &&
                Game1.timeOfDay <= instance.TimeEnd
                )
            {
                instance.HasSeen = true;
            }
            return true;
        }
    }
    public class UberEatsDriverIII : IAchievement
    {
        private static UberEatsDriverIII instance;

        private readonly string NPCName = "Emily";
        private readonly string ObjectName = "Survival Burger";
        private readonly int TimeStart = 1800;
        private readonly int TimeEnd = 1900;
        public UberEatsDriverIII()
            : base("Uber Eats Driver III", 5)
        {
            instance = this;
        }

        public override void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(NPC), "receiveGift"),
                prefix: new HarmonyMethod(typeof(UberEatsDriverIII), nameof(UberEatsDriverIII.Prefix_receiveGift))
                );
        }

        public static bool Prefix_receiveGift(NPC __instance, StardewValley.Object o)
        {
            if (
                __instance.Name == instance.NPCName &&
                o.name == instance.ObjectName &&
                instance.TimeStart <= Game1.timeOfDay &&
                Game1.timeOfDay <= instance.TimeEnd
                )
            {
                instance.HasSeen = true;
            }
            return true;
        }
    }
    public class UberEatsDriverIV : IAchievement
    {
        private static UberEatsDriverIV instance;

        private readonly string NPCName = "Demetrius";
        private readonly string ObjectName = "Ice Cream";
        private readonly int TimeStart = 2000;
        private readonly int TimeEnd = 2100;
        public UberEatsDriverIV()
            : base("Uber Eats Driver IV", 5)
        {
            instance = this;
        }

        public override void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(NPC), "receiveGift"),
                prefix: new HarmonyMethod(typeof(UberEatsDriverIV), nameof(UberEatsDriverIV.Prefix_receiveGift))
                );
        }

        public static bool Prefix_receiveGift(NPC __instance, StardewValley.Object o)
        {
            if (
                __instance.Name == instance.NPCName &&
                o.name == instance.ObjectName &&
                instance.TimeStart <= Game1.timeOfDay &&
                Game1.timeOfDay <= instance.TimeEnd
                )
            {
                instance.HasSeen = true;
            }
            return true;
        }
    }
    public class UberEatsDriverV : IAchievement
    {
        private static UberEatsDriverV instance;

        private readonly string NPCName = "Shane";
        private readonly string ObjectName = "Pepper Poppers";
        private readonly int TimeStart = 2200;
        private readonly int TimeEnd = 2300;
        public UberEatsDriverV()
            : base("Uber Eats Driver V", 5)
        {
            instance = this;
        }

        public override void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(NPC), "receiveGift"),
                prefix: new HarmonyMethod(typeof(UberEatsDriverV), nameof(UberEatsDriverV.Prefix_receiveGift))
                );
        }

        public static bool Prefix_receiveGift(NPC __instance, StardewValley.Object o)
        {
            if (
                __instance.Name == instance.NPCName &&
                o.name == instance.ObjectName &&
                instance.TimeStart <= Game1.timeOfDay &&
                Game1.timeOfDay <= instance.TimeEnd
                )
            {
                instance.HasSeen = true;
            }
            return true;
        }
    }

    public class UberEatsDriverOfTheDay : AchievementCollection
    {
        public UberEatsDriverOfTheDay() :
            base(
                "Uber Eats Driver of the Day", 10,
                new List<IAchievement>
                {
                    new UberEatsDriverI(),
                    new UberEatsDriverII(),
                    new UberEatsDriverIII(),
                    new UberEatsDriverIV(),
                    new UberEatsDriverV()
                })
        {
        }
    }
}

