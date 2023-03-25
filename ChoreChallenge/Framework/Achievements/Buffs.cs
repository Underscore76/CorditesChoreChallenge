using System;
using HarmonyLib;
using StardewModdingAPI;
using StardewValley;

namespace ChoreChallenge.Framework.Achievements
{
	public class Stinky : IAchievement
	{
		private static Stinky instance;
		public Stinky()
			: base("Stinky", 5)
		{
			instance = this;
		}

        public override void Patch(Harmony harmony)
        {
			harmony.Patch(
				original: AccessTools.Method(typeof(Buff), "addBuff"),
				prefix: new HarmonyMethod(typeof(Stinky), nameof(Stinky.Prefix_addBuff))
				);
        }

		public static bool Prefix_addBuff(Buff __instance)
		{
            if (__instance.sheetIndex == 23)
            {
                instance.HasSeen = true;
            }
            return true;
		}
    }

	public class SuperStinky : IAchievement
	{
		private static SuperStinky instance;
		public SuperStinky()
			: base("Super Stinky", 5)
		{
			instance = this;
		}

        public override void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Buff), "addBuff"),
                prefix: new HarmonyMethod(typeof(SuperStinky), nameof(SuperStinky.Prefix_addBuff))
                );
        }

        public static bool Prefix_addBuff(Buff __instance)
        {
            if (
                (Game1.player.hasBuff(23) && __instance.sheetIndex == 24) ||
                (Game1.player.hasBuff(24) && __instance.sheetIndex == 23)
            )
            {
                instance.HasSeen = true;
            }
            return true;
        }
    }

    public class FasterThanASpeedingHorse : IAchievement
    {
        private static FasterThanASpeedingHorse instance;
        public FasterThanASpeedingHorse()
            : base("Faster Than A Speeding Horse", 5)
        {
            instance = this;
        }

        public override void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Buff), "addBuff"),
                postfix: new HarmonyMethod(typeof(FasterThanASpeedingHorse), nameof(FasterThanASpeedingHorse.Postfix_addBuff))
                );
        }

        public static void Postfix_addBuff(Buff __instance)
        {
            if (Game1.player.addedSpeed >= 2)
            {
                instance.HasSeen = true;
            }
        }
    }

    public class Mach41 : IAchievement
    {
        private static Mach41 instance;
        public Mach41()
            : base("Mach 4.1", 25)
        {
            instance = this;
        }


        public override void OnUpdate()
        {
            //float v = Game1.player.temporarySpeedBuff + Game1.player.addedSpeed;
            //instance.Monitor.Log($"{v}\t{v > 4} ({instance.HasSeen})", LogLevel.Alert);
            // NOTE: I was running into issues with float precision comparison for some reason?
            if (Game1.player.temporarySpeedBuff + Game1.player.addedSpeed > 4.0f)
            {
                instance.HasSeen = true;
            }
        }
    }
}

