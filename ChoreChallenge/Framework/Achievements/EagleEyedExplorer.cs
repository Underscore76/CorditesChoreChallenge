using System;
using System.Collections.Generic;
using StardewModdingAPI;
using StardewValley.Objects;
using Microsoft.Xna.Framework;
using HarmonyLib;
using StardewValley.Locations;
using StardewValley;

namespace ChoreChallenge.Framework.Achievements
{
	public class EagleEyedExplorer : CumulativeAchievement
	{
        private static EagleEyedExplorer instance;
		private static readonly Color ChestColor = new Color(64, 64, 64, 255);

		private static Dictionary<string, HashSet<Vector2>> Chests;

		public EagleEyedExplorer()
			: base("Eagle Eyed Explorer", 25)
		{
			instance = this;
        }

        public override void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Chest), "ShowMenu"),
                prefix: new HarmonyMethod(typeof(EagleEyedExplorer), nameof(EagleEyedExplorer.Prefix_ShowMenu))
                );
        }

        public static bool Prefix_ShowMenu(Chest __instance)
		{
            if (__instance.playerChoiceColor.Value == ChestColor)
			{
				string loc = Game1.currentLocation.Name;
				Vector2 tile = __instance.TileLocation;
                if (Chests.ContainsKey(loc) && Chests[loc].Contains(tile))
				{
					Chests[loc].Remove(tile);
					instance.CurrentValue++;
					instance.Monitor.Log($"Found {instance.CurrentValue} of {instance.MaxValue} chests", LogLevel.Alert);
				}
			}
			return true;
		}

        public override void OnSaveLoaded()
        {
			base.OnSaveLoaded();

			MaxValue = 0;
            Chests = new Dictionary<string, HashSet<Vector2>>();
            foreach (var location in Game1.locations)
			{
				if (!Chests.ContainsKey(location.Name))
				{
					Chests.Add(location.Name, new HashSet<Vector2>());
				}
				foreach(var obj in location.Objects.Values)
				{
					if (obj is Chest chest && chest.DisplayName == "Stone Chest" && chest.playerChoiceColor.Value == ChestColor)
					{
						Chests[location.Name].Add(chest.TileLocation);
						MaxValue++;
						this.Monitor.Log($"{location.Name}: {chest.TileLocation}", LogLevel.Alert);
					}
				}
			}
        }
    }
}

