using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using System.Linq;

namespace ChoreChallenge.Framework.Achievements
{
    public class RestAndRelaxation : IAchievement
    {
        private static RestAndRelaxation instance;

        private float energyRegained = 0;
        private float? startingEnergy = null;

        public RestAndRelaxation()
            : base("Rest and Relaxation", 5)
        {
            instance = this;
        }

        public override void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Farmer), "updateCommon"),
                postfix: new HarmonyMethod(typeof(RestAndRelaxation), nameof(RestAndRelaxation.Postfix_updateCommon))
                );
        }

        public static void Postfix_updateCommon(GameTime time, GameLocation location)
        {
            var farmer = location.farmers.FirstOrDefault();
            if (farmer?.swimming.Value == true)
            {
                if (instance.startingEnergy == null)
                {
                    instance.startingEnergy = farmer.Stamina;
                }
                instance.energyRegained = farmer.Stamina - instance.startingEnergy.Value;
                if (instance.energyRegained >= 100)
                {
                    instance.HasSeen = true;
                }
                instance.Monitor.Log($"{nameof(RestAndRelaxation)} - swimming, current stamina {farmer.Stamina}, starting {instance.startingEnergy}, regained {instance.energyRegained}", LogLevel.Debug);
            }
            else
            {
                instance.energyRegained = 0;
                instance.startingEnergy = null;
                instance.Monitor.Log($"{nameof(RestAndRelaxation)} - stopped swimming", LogLevel.Debug);
            }
        }
    }
}