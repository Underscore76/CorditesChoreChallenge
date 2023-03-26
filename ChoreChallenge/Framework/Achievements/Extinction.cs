using HarmonyLib;
using StardewModdingAPI;
using StardewValley;

namespace ChoreChallenge.Framework.Achievements
{
    public class Extinction : IAchievement
    {
        private const string PepperRexName = "Pepper Rex";
        private const int KillGoal = 50;

        private static Extinction instance;
        public Extinction()
            : base("Extinction", 5)
        {
            instance = this;
        }

        public override void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Stats), nameof(Stats.monsterKilled)),
                postfix: new HarmonyMethod(typeof(Extinction), nameof(Extinction.Postfix_monsterKilled))
                );
        }

        public static void Postfix_monsterKilled(string name)
        {
            if (name == PepperRexName)
            {
                int dinosKilled = Game1.stats.getMonstersKilled(PepperRexName);
                instance.Monitor.Log($"{nameof(Extinction)} - Pepper Rex killed: {dinosKilled}", LogLevel.Info);
                if (dinosKilled >= KillGoal)
                {
                    instance.HasSeen = true;
                }
            }
        }
    }
}