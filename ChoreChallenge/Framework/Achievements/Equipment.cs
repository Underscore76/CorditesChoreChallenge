using System;
using System.Collections.Generic;
using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Objects;

namespace ChoreChallenge.Framework.Achievements
{
	public class FiveFingers : CumulativeAchievement
	{
        private HashSet<string> Rings;
        private static FiveFingers instance;
        public FiveFingers()
            : base("You Actually Have 5 Fingers", 10)
        {
            instance = this;
            Rings = new HashSet<string>();
            MaxValue = 5;
        }

        public override void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Ring), "onEquip"),
                prefix: new HarmonyMethod(typeof(FiveFingers), nameof(FiveFingers.Prefix_onEquip))
                );
        }

        public static bool Prefix_onEquip(Ring __instance)
        {
            instance.Rings.Add(__instance.Name);
            instance.CurrentValue = instance.Rings.Count;
            return true;
        }

        public override void OnSaveLoaded()
        {
            Rings.Clear();
            base.OnSaveLoaded();
        }
    }

    public class MadHatter : CumulativeAchievement
    {
        private HashSet<int> Hats;
        private static MadHatter instance;
        public MadHatter()
            : base("The Mad Hatter", 10)
        {
            instance = this;
            Hats = new HashSet<int>();
            MaxValue = 9;
        }


        public static void Postfix_changeHat(int newHat)
        {
            if (newHat > 0)
            {
                instance.Hats.Add(newHat);
            }
            instance.CurrentValue = instance.Hats.Count;
        }

        public override void OnSaveLoaded()
        {
            Hats.Clear();
            base.OnSaveLoaded();
        }

        public override void OnUpdate()
        {
            if (Game1.player.hat.Value != null)
            {
                int hat = Game1.player.hat.Value.which.Value;
                instance.Hats.Add(hat);
            }
            instance.CurrentValue = instance.Hats.Count;
            base.OnUpdate();
        }
    }
}

