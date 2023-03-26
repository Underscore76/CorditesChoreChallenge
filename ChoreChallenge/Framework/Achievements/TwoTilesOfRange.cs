using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using System.Collections.Generic;

namespace ChoreChallenge.Framework.Achievements
{
    public class TwoTilesOfRange : IAchievement
    {
        private static TwoTilesOfRange instance;
        public TwoTilesOfRange()
            : base("2 Tiles of Range", 5)
        {
            instance = this;
        }

        public override void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Farmer), nameof(Farmer.addItemToInventory), new[] { typeof(Item), typeof(List<Item>) }),
                postfix: new HarmonyMethod(typeof(TwoTilesOfRange), nameof(TwoTilesOfRange.Postfix_addItemToInventory))
                );
        }

        public static void Postfix_addItemToInventory(Item __result, Item item)
        {
            if (item?.Name == "Golden Scythe")
            {
                instance.HasSeen = true;
                instance.Monitor.Log($"{nameof(TwoTilesOfRange)} - Added item to inventory: {item?.Name}", LogLevel.Info);
            }
        }
    }
}