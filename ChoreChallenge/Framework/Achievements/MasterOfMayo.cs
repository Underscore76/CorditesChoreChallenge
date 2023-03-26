using System;
using System.Collections.Generic;
using HarmonyLib;
using StardewValley;

namespace ChoreChallenge.Framework.Achievements
{
	public class MasterOfMayo : ItemProcessing
	{
        private static MasterOfMayo instance;
        public MasterOfMayo()
            : base(
                  "Master of Mayo", 25,
                  new List<string>
                    {
                        "Mayonnaise",
                        "Duck Mayonnaise",
                        "Void Mayonnaise",
                        "Dinosaur Mayonnaise"
                    }
                  )
        {
            AddMachineToWatch("Mayonnaise Machine");
            instance = this;
        }

        public override void Patch(Harmony harmony)
        {
            // handle processing machines (and anti-spam logic)
            harmony.Patch(
                    original: AccessTools.Method(typeof(StardewValley.Object), "onReadyForHarvest"),
                    postfix: new HarmonyMethod(typeof(MasterOfMayo), nameof(MasterOfMayo.Postfix_onReadyForHarvest))
                );
            harmony.Patch(
                    original: AccessTools.Method(typeof(StardewValley.Object), "performToolAction"),
                    postfix: new HarmonyMethod(typeof(MasterOfMayo), nameof(MasterOfMayo.Postfix_performToolAction))
                );
            harmony.Patch(
                original: AccessTools.Method(typeof(StardewValley.Object), "performObjectDropInAction"),
                prefix: new HarmonyMethod(typeof(MasterOfMayo), nameof(MasterOfMayo.Prefix_performObjectDropInAction))
                );

            // scan and ensure all machines still exist/add results
            harmony.Patch(
                    original: AccessTools.Method(typeof(Farmer), "performPassOut"),
                    prefix: new HarmonyMethod(typeof(MasterOfMayo), nameof(MasterOfMayo.Prefix_performPassOut))
                );
            harmony.Patch(
                    original: AccessTools.Method(typeof(GameLocation), "startSleep"),
                    prefix: new HarmonyMethod(typeof(MasterOfMayo), nameof(MasterOfMayo.Prefix_startSleep))
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
            if (probe) return true;
            switch (dropInItem.Name)
            {
                case "Egg":
                case "Large Egg":
                    instance.performObjectDropInAction(__instance, "Mayonnaise", probe);
                    break;
                case "Duck Egg":
                    instance.performObjectDropInAction(__instance, "Duck Mayonnaise", probe);
                    break;
                case "Void Egg":
                    instance.performObjectDropInAction(__instance, "Void Mayonnaise", probe);
                    break;
                case "Dinosaur Egg":
                    instance.performObjectDropInAction(__instance, "Dinosaur Mayonnaise", probe);
                    break;
            }
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

