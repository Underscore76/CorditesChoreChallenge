using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using StardewModdingAPI;
using StardewValley.Menus;
using StardewValley;
using Microsoft.Xna.Framework;

namespace ChoreChallenge.Framework.Achievements
{
	public class Geologist : ItemProcessing
    {
        private static Geologist instance;
        public Geologist()
            : base(
                  "Geologist", 5,
                  new List<string>
                    {
                        "Geode",
                        "Frozen Geode",
                        "Magma Geode",
                        "Omni Geode",
                        "Artifact Trove"
                    }
                  )
        {
            AddMachineToWatch("Geode Crusher");
            instance = this;
        }

        public override void Patch(Harmony harmony)
        {
            // clint drops in
            harmony.Patch(
                original: AccessTools.Method(typeof(GeodeMenu), "startGeodeCrack"),
                postfix: new HarmonyMethod(typeof(Geologist), nameof(Geologist.Postfix_startGeodeCrack))
                );

            // handle processing machines (and anti-spam logic)
            harmony.Patch(
                    original: AccessTools.Method(typeof(StardewValley.Object), "onReadyForHarvest"),
                    postfix: new HarmonyMethod(typeof(Geologist), nameof(Geologist.Postfix_onReadyForHarvest))
                );
            harmony.Patch(
                    original: AccessTools.Method(typeof(StardewValley.Object), "performToolAction"),
                    postfix: new HarmonyMethod(typeof(Geologist), nameof(Geologist.Postfix_performToolAction))
                );
            harmony.Patch(
                original: AccessTools.Method(typeof(StardewValley.Object), "performObjectDropInAction"),
                prefix: new HarmonyMethod(typeof(Geologist), nameof(Geologist.Prefix_performObjectDropInAction))
                );

            // scan and ensure all machines still exist/add results
            harmony.Patch(
                    original: AccessTools.Method(typeof(Farmer), "performPassOut"),
                    prefix: new HarmonyMethod(typeof(Geologist), nameof(Geologist.Prefix_performPassOut))
                );
            harmony.Patch(
                    original: AccessTools.Method(typeof(GameLocation), "startSleep"),
                    prefix: new HarmonyMethod(typeof(Geologist), nameof(Geologist.Prefix_startSleep))
                );
        }

        public static void Postfix_startGeodeCrack(GeodeMenu __instance)
        {
            if (__instance.geodeSpot.item != null &&
                instance.NeededItems.Contains(__instance.geodeSpot.item.Name)
                )
            {
                instance.CompletedItems.Add(__instance.geodeSpot.item.Name);
            }
            instance.CurrentValue = instance.CompletedItems.Count;
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

