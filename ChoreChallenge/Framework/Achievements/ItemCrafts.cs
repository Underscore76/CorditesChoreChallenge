using System;
using StardewModdingAPI;
using StardewValley;
using HarmonyLib;

namespace ChoreChallenge.Achievements
{
    public class WickedDecorations : IAchievement
    {
        private static WickedDecorations instance;
        private bool HasCrafted;
        public WickedDecorations()
            : base("Wicked Decorations", 5)
        {
            instance = this;
        }

        public override void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(CraftingRecipe), "consumeIngredients"),
                prefix: new HarmonyMethod(typeof(WickedDecorations), nameof(WickedDecorations.Prefix_consumeIngredients))
                );
            harmony.Patch(
                original: AccessTools.Method(typeof(Utility), "tryToPlaceItem"),
                postfix: new HarmonyMethod(typeof(WickedDecorations), nameof(WickedDecorations.Postfix_tryToPlaceItem))
                );
        }

        public static bool Prefix_consumeIngredients(CraftingRecipe __instance)
        {
            if (__instance.name == "Wicked Statue")
            {
                instance.HasCrafted = true;
            }
            return true;
        }

        public static void Postfix_tryToPlaceItem(bool __result, GameLocation location, Item item)
        {
            if (__result && location.Name == "FarmHouse" && item.Name == "Wicked Statue" && instance.HasCrafted)
            {
                instance.HasSeen = true;
            }
            //instance.Monitor.Log($"{__result} {location.Name} {item.Name}", LogLevel.Alert);
        }

        public override void OnSaveLoaded()
        {
            HasCrafted = false;
            base.OnSaveLoaded();
        }
    }

    public class LavishDecorations : IAchievement
    {
        private static LavishDecorations instance;
        private bool HasCrafted;
        public LavishDecorations()
            : base("Lavish Decorations", 5)
        {
            instance = this;
        }

        public override void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(CraftingRecipe), "consumeIngredients"),
                prefix: new HarmonyMethod(typeof(LavishDecorations), nameof(LavishDecorations.Prefix_consumeIngredients))
                );
            harmony.Patch(
                original: AccessTools.Method(typeof(Utility), "tryToPlaceItem"),
                postfix: new HarmonyMethod(typeof(LavishDecorations), nameof(LavishDecorations.Postfix_tryToPlaceItem))
                );
        }

        public static bool Prefix_consumeIngredients(CraftingRecipe __instance)
        {
            if (__instance.name == "Gold Brazier")
            {
                instance.HasCrafted = true;
            }
            return true;
        }

        public static void Postfix_tryToPlaceItem(bool __result, GameLocation location, Item item)
        {
            if (__result && location.Name == "FarmHouse" && item.Name == "Gold Brazier" && instance.HasCrafted)
            {
                instance.HasSeen = true;
            }
            //instance.Monitor.Log($"{__result} {location.Name} {item.Name}", LogLevel.Alert);
        }

        public override void OnSaveLoaded()
        {
            HasCrafted = false;
            base.OnSaveLoaded();
        }
    }
}

