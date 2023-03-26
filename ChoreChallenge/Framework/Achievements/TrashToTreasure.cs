using System;
using HarmonyLib;
using StardewValley;
using StardewValley.Menus;
using System.Collections.Generic;
using StardewModdingAPI;

namespace ChoreChallenge.Framework.Achievements
{
    public class TrashToTreasure : ItemProcessing
    {
        private static TrashToTreasure instance;
        public TrashToTreasure()
            : base(
                  "Trash to ... Treasure?", 5,
                  new List<string> {
                        "Trash",
                        "Driftwood",
                        "Soggy Newspaper",
                        "Broken CD",
                        "Broken Glasses"
                    }
                  )
        {
            AddMachineToWatch("Recycling Machine");
            instance = this;
        }

        public override void Patch(Harmony harmony)
        {
            // handle processing machines (and anti-spam logic)
            harmony.Patch(
                    original: AccessTools.Method(typeof(StardewValley.Object), "onReadyForHarvest"),
                    postfix: new HarmonyMethod(typeof(TrashToTreasure), nameof(TrashToTreasure.Postfix_onReadyForHarvest))
                );
            harmony.Patch(
                    original: AccessTools.Method(typeof(StardewValley.Object), "performToolAction"),
                    postfix: new HarmonyMethod(typeof(TrashToTreasure), nameof(TrashToTreasure.Postfix_performToolAction))
                );
            harmony.Patch(
                original: AccessTools.Method(typeof(StardewValley.Object), "performObjectDropInAction"),
                prefix: new HarmonyMethod(typeof(TrashToTreasure), nameof(TrashToTreasure.Prefix_performObjectDropInAction))
                );

            // scan and ensure all machines still exist/add results
            harmony.Patch(
                    original: AccessTools.Method(typeof(Farmer), "performPassOut"),
                    prefix: new HarmonyMethod(typeof(TrashToTreasure), nameof(TrashToTreasure.Prefix_performPassOut))
                );
            harmony.Patch(
                    original: AccessTools.Method(typeof(GameLocation), "startSleep"),
                    prefix: new HarmonyMethod(typeof(TrashToTreasure), nameof(TrashToTreasure.Prefix_startSleep))
                );
        }

        public static void Postfix_onReadyForHarvest(StardewValley.Object __instance, GameLocation environment)
        {
            instance.onReadyForHarvest(__instance, environment);
        }

        public static void Postfix_performToolAction(StardewValley.Object __instance, bool __result, GameLocation location)
        {
            instance.performToolAction(__instance, location, __result);
        }

        public static bool Prefix_performObjectDropInAction(StardewValley.Object __instance, StardewValley.Item dropInItem, bool probe)
        {
            instance.performObjectDropInAction(__instance, dropInItem, probe);
            return true;
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
    }
}

