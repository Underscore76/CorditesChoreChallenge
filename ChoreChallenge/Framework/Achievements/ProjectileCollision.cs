using System;
using HarmonyLib;
using Netcode;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Monsters;
using StardewValley.Projectiles;

namespace ChoreChallenge.Framework.Achievements
{
	public class PopTheBalloon : IAchievement
	{
        private static PopTheBalloon instance;
        public PopTheBalloon()
            : base("Pop the Balloon", 5)
        {
            instance = this;
        }

        public override void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(BasicProjectile), "behaviorOnCollisionWithMonster"),
                prefix: new HarmonyMethod(typeof(PopTheBalloon), nameof(PopTheBalloon.Prefix_behaviorOnCollisionWithMonster))
                );
        }

        protected bool ProjectileIsWood(BasicProjectile projectile)
        {
            var spriteFromObjectSheet = Reflection.GetField<NetBool>(projectile, "spriteFromObjectSheet", required: false);
            var currentTileSheetIndex = Reflection.GetField<NetInt>(projectile, "currentTileSheetIndex", required:false);
            //instance.Monitor.Log($"{spriteFromObjectSheet.GetValue()}||{currentTileSheetIndex.GetValue()}", LogLevel.Alert);
            return (
                currentTileSheetIndex != null &&
                spriteFromObjectSheet != null &&
                spriteFromObjectSheet.GetValue().Value &&
                (currentTileSheetIndex.GetValue().Value == 388 || // sprite sheet wood index
                currentTileSheetIndex.GetValue().Value == 389) // sprite sheet wood projectile index
            );
        }

        public static bool Prefix_behaviorOnCollisionWithMonster(BasicProjectile __instance, NPC n, GameLocation location)
        {
            if (n is SquidKid squidKid && instance.ProjectileIsWood(__instance))
            {
                instance.HasSeen = true;
            }
            return true;
        }
    }
}

