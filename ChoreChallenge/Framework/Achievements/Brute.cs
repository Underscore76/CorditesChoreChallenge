using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Monsters;
using static StardewValley.Debris;

namespace ChoreChallenge.Framework.Achievements
{
	public class Brute : IAchievement
	{
        private string LastCritTime;
        private static Brute instance;
        public Brute()
            : base("Brute", 5)
        {
            instance = this;

        }

        public override void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(GameLocation), "damageMonster",
                    new Type[] { typeof(Rectangle), typeof(int), typeof(int), typeof(bool), typeof(float), typeof(int), typeof(float), typeof(float), typeof(bool), typeof(Farmer) }),
                prefix: new HarmonyMethod(typeof(Brute), nameof(Brute.Prefix_damageMonster)),
                postfix: new HarmonyMethod(typeof(Brute), nameof(Brute.Postfix_damageMonster))
                );
        }

        public static bool Prefix_damageMonster(GameLocation __instance, out List<Tuple<Monster,int>> __state)
        {
            __state = new List<Tuple<Monster, int>>();
            if (instance.HasSeen) return true;

            foreach (var character in __instance.characters)
            {
                if (character is Monster monster)
                {
                    __state.Add(Tuple.Create(monster, monster.Health));
                }
            }
            return true;
        }

        public static void Postfix_damageMonster(GameLocation __instance, bool __result, List<Tuple<Monster, int>> __state)
        {
            if (!__result || instance.HasSeen) return;
            foreach (var tuple in __state)
            {
                // the monster has dropped this
                var monster = tuple.Item1;
                var originalHealth = tuple.Item2;
                int damageAmount = originalHealth - monster.Health;
                if (damageAmount >= 100)
                {
                    foreach (var debris in __instance.debris.Where(d => d.debrisType.Value == DebrisType.NUMBERS))
                    {
                        if (
                            debris.toHover == monster &&
                            debris.chunkType.Value == damageAmount &&
                            debris.nonSpriteChunkColor.Value != Color.Yellow
                            )
                        {
                            instance.HasSeen = true;
                            return;
                        }
                    }
                }
            }
        }
    }
}

/*
 * damageMonster
//public bool damageMonster(Microsoft.Xna.Framework.Rectangle areaOfEffect, int minDamage, int maxDamage, bool isBomb, float knockBackModifier, int addedPrecision, float critChance, float critMultiplier, bool triggerMonsterInvincibleTimer, Farmer who)
*/