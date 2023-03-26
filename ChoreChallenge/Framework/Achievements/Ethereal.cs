using System;
using System.Collections.Generic;
using HarmonyLib;
using StardewModdingAPI;
using Object = StardewValley.Object;

namespace ChoreChallenge.Framework.Achievements
{
	public class Ethereal : CumulativeAchievement
    {
        public const int NumTotems = 4;
        protected HashSet<string> UsedTotems;

        private static Ethereal instance;
        public Ethereal()
            : base("Ethereal", 10)
        {
            instance = this;
            UsedTotems = new HashSet<string>();
            MaxValue = NumTotems;
        }

        public override void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Object), "totemWarp"),
                postfix: new HarmonyMethod(typeof(Ethereal), nameof(Ethereal.Postfix_totemWarp))
                );
        }

        public static void Postfix_totemWarp(Object __instance)
        {
            instance.UsedTotems.Add(__instance.Name);
            instance.CurrentValue = instance.UsedTotems.Count;
        }

        public override void OnSaveLoaded()
        {
            UsedTotems.Clear();
            base.OnSaveLoaded();
        }
    }
}

