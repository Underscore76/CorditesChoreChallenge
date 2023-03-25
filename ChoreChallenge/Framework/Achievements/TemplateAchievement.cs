using System;
using HarmonyLib;
using StardewModdingAPI;
using StardewValley;

namespace ChoreChallenge.Framework.Achievements
{
    // use this template to copy paste in a new class
    public class TemplateAchievement : IAchievement
    {
        private static TemplateAchievement instance;
        public TemplateAchievement()
            : base("TemplateAchievement", 0)
        {
            instance = this;
        }

        public override void Patch(Harmony harmony)
        {
            throw new NotImplementedException();
            harmony.Patch(
                original: AccessTools.Method(typeof(StardewValley.Object), "methodName"),
                prefix: new HarmonyMethod(typeof(TemplateAchievement), nameof(TemplateAchievement.Prefix_methodName))
                );
        }

        public static bool Prefix_methodName(object __instance)
        {
            instance.Monitor.Log($"{__instance}", LogLevel.Alert);
            return true;
        }
    }
}

