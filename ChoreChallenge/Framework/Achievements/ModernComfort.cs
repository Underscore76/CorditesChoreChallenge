using System;
using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Objects;
using StardewValley.Locations;

namespace ChoreChallenge.Framework.Achievements
{
	public class ModernComfort : IAchievement
    {
        private const string BedName = "Modern Double Bed";
        private static ModernComfort instance;
        public ModernComfort()
            : base("Life of Modern Comfort", 5)
        {
            instance = this;
        }

        public override void Patch(Harmony harmony)
        {
            harmony.Patch(
                    original: AccessTools.Method(typeof(GameLocation), "doSleep"),
                    postfix: new HarmonyMethod(typeof(ModernComfort), nameof(ModernComfort.Postfix_doSleep))
                );
        }

        public static void Postfix_doSleep()
        {
            var location = Game1.getLocationFromName(Game1.player.lastSleepLocation.Value);
            var point = Game1.player.lastSleepPoint.Value;
            if (location is FarmHouse farmHouse)
            {
                var bed = BedFurniture.GetBedAtTile(farmHouse, point.X, point.Y);
                if (bed.Name == BedName)
                {
                    instance.HasSeen = true;
                }
            }
        }
    }
}

