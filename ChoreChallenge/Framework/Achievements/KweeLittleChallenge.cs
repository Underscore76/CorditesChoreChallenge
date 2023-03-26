using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Locations;

namespace ChoreChallenge.Framework.Achievements
{
    public class KweeLittleChallenge : IAchievement
    {
        private static KweeLittleChallenge instance;
        public KweeLittleChallenge()
            : base("Kwee Little Challenge", 25)
        {
            instance = this;
        }

        public override void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Game1), nameof(Game1.warpFarmer), new[] { typeof(LocationRequest), typeof(int), typeof(int), typeof(int) }),
                postfix: new HarmonyMethod(typeof(KweeLittleChallenge), nameof(KweeLittleChallenge.Postfix_warpFarmer))
                );
        }

        public static void Postfix_warpFarmer(LocationRequest locationRequest)
        {
            if (locationRequest.Location is MineShaft mine)
            {
                instance.Monitor.Log($"{nameof(KweeLittleChallenge)} - entering mine floor: {mine.mineLevel}", LogLevel.Info);
                // skull caverns floors start at 120 so look for 25 or more floors after that
                // 77377 is the quarry mine so ignore that one
                if (mine.mineLevel >= 145 && mine.mineLevel != 77377)
                {
                    instance.HasSeen = true;
                }
            }
        }
    }
}