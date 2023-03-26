using System;
using System.Collections.Generic;
using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using StardewValley.TerrainFeatures;
using Microsoft.Xna.Framework;
using StardewValley.Tools;

namespace ChoreChallenge.Framework.Achievements
{
	public class DutifulFarmer : IAchievement
    {
        private HashSet<Vector2> CropLocations;
        private HashSet<long> HasMilk;
        private bool FailedCrops;
        private bool FinishedCrops;
        private bool FinishedAnimals;
        private static DutifulFarmer instance;
        public DutifulFarmer()
            : base("Dutiful Farmer", 5)
        {
            instance = this;
            CropLocations = new HashSet<Vector2>();
            HasMilk = new HashSet<long>();
        }

        public override void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(HoeDirt), "performToolAction"),
                prefix: new HarmonyMethod(typeof(DutifulFarmer), nameof(DutifulFarmer.Prefix_performToolAction))
                );
        }
        //performToolAction(Tool t, int damage, Vector2 tileLocation, GameLocation location)
        public static bool Prefix_performToolAction(HoeDirt __instance, Tool t, int damage, Vector2 tileLocation, GameLocation location)
        {
            if (!instance.FailedCrops && location.IsFarm)
            {
                if (__instance.crop is Crop crop)
                {
                    if (t == null && damage > 0)
                    {
                        instance.FailedCrops = true;
                    }
                    else if (t.isHeavyHitter() && !(t is Hoe) && !(t is MeleeWeapon))
                    {
                        instance.FailedCrops = true;
                    }
                }
                if (instance.FailedCrops)
                {
                    DrawHelper.DisplayWarning($"Failed: {instance.Description}");
                }
            }
            return true;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (FailedCrops) return;

            if (!FailedCrops && !FinishedCrops)
            {
                ScanCrops();
            }
            if (!FinishedAnimals)
            {
                ScanAnimals();
            }
            if (!FailedCrops && FinishedCrops && FinishedAnimals)
            {
                HasSeen = true;
            }
        }

        private void ScanAnimals()
        {
            bool allHandled = true;
            foreach (var animal in Game1.getFarm().getAllFarmAnimals())
            {
                if (!allHandled) return; // early exit
                if (HasMilk.Contains(animal.myID.Value))
                {
                    allHandled &= (animal.wasPet.Value || animal.currentProduce.Value == -1);
                }
                else
                {
                    allHandled &= animal.wasPet.Value;
                }
            }
            FinishedAnimals = allHandled;
        }

        private void ScanCrops()
        {
            if (CropLocations.Count > 0)
            {
                var Farm = Game1.getFarm();
                HashSet<Vector2> toRemove = new HashSet<Vector2>();
                foreach (var tile in CropLocations)
                {
                    if (Farm.terrainFeatures.TryGetValue(tile, out var tf))
                    {
                        if (tf is HoeDirt hoeDirt)
                        {
                            if (hoeDirt.crop is Crop crop)
                            {
                                // multi-crop that has been harvested
                                if (!CropReadyToHarvest(crop))
                                {
                                    toRemove.Add(tile);
                                }
                            }
                            else
                            {
                                toRemove.Add(tile);
                            }
                        }
                    }
                    else
                    {
                        toRemove.Add(tile);
                    }
                }
                CropLocations.ExceptWith(toRemove);
                if (toRemove.Count > 0)
                {
                    Monitor.Log($"Crops Remaining: {CropLocations.Count}", LogLevel.Info);
                }
            }

            if (CropLocations.Count == 0)
            {
                FinishedCrops = true;
            }
        }

        protected bool CropReadyToHarvest(Crop crop)
        {
            return crop != null && (crop.currentPhase.Value >= crop.phaseDays.Count - 1 && (!crop.fullyGrown.Value || crop.dayOfCurrentPhase.Value <= 0));
        }

        public override void OnSaveLoaded()
        {
            base.OnSaveLoaded();
            FailedCrops = false;
            FinishedCrops = false;
            FinishedAnimals = false;
            CropLocations.Clear();
            HasMilk.Clear();

            var Farm = Game1.getFarm();
            foreach (var terrainFeature in Farm.terrainFeatures.Values)
            {
                if (terrainFeature is HoeDirt hoeDirt)
                {
                    if (hoeDirt.crop is Crop crop)
                    {
                        // ready to harvest
                        if (CropReadyToHarvest(crop))
                        {
                            CropLocations.Add(hoeDirt.currentTileLocation);
                        }
                    }
                }
            }
            foreach (var animal in Farm.getAllFarmAnimals())
            {
                if (animal.type.Contains("Cow") || animal.type.Contains("Goat"))
                {
                    if (animal.currentProduce.Value > 0 && animal.age.Value >= animal.ageWhenMature.Value)
                    {
                        HasMilk.Add(animal.myID.Value);
                    }
                }
            }
        }
    }
}

