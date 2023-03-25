using System;
using HarmonyLib;
using StardewValley;

namespace ChoreChallenge.Framework.Achievements
{
    public class MoneyMaker : IAchievement
    {
        private static MoneyMaker instance;
        public MoneyMaker()
            : base("Money Maker", 5)
        {
            instance = this;
        }

        public override void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(StardewValley.Object), "performObjectDropInAction"),
                prefix: new HarmonyMethod(typeof(MoneyMaker), nameof(MoneyMaker.Prefix_performObjectDropInAction))
                );
        }

        public static bool Prefix_performObjectDropInAction(StardewValley.Object __instance, StardewValley.Item dropInItem, bool probe)
        {
            if (!probe && __instance.Name == "Keg" && dropInItem.ParentSheetIndex == 258 && __instance.heldObject.Value == null)
            {
                instance.HasSeen = true;
            }
            return true;
        }
    }
}

