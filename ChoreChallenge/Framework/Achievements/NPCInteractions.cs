﻿using System;
using System.Collections.Generic;
using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;

namespace ChoreChallenge.Framework.Achievements
{
	public class MusicProducer : IAchievement
	{
        private HashSet<string> ValidResponses;
        private static MusicProducer instance;
        public MusicProducer()
            : base("Music Producer", 5)
        {
            instance = this;
            ValidResponses = new HashSet<string>
            {
                "Event_band1",
                "Event_band2",
                "Event_band3",
                "Event_band4",
            };
        }

        public override void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Dialogue), "chooseResponse"),
                prefix: new HarmonyMethod(typeof(MusicProducer), nameof(MusicProducer.Prefix_chooseResponse))
                );
        }

        public static bool Prefix_chooseResponse(Response response)
        {
            if (instance.ValidResponses.Contains(response.responseKey))
            {
                instance.HasSeen = true;
            }
            return true;
        }
    }

    public class PlayAPrank : IAchievement
    {
        private static PlayAPrank instance;
        public PlayAPrank()
            : base("Play a Prank", 10)
        {
            instance = this;
        }

        public override void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(DialogueBox), "update"),
                prefix: new HarmonyMethod(typeof(PlayAPrank), nameof(PlayAPrank.Prefix_update))
                );
        }

        public static bool Prefix_update(DialogueBox __instance)
        {
            if (
                __instance.characterDialogue != null &&
                __instance.characterDialogue.speaker != null &&
                __instance.characterDialogue.speaker.Name == "Sebastian" &&
                Game1.player.hat.Value != null &&
                Game1.player.hat.Value.which.Value == 8 // skeleton mask
                )
            {
                instance.HasSeen = true;
            }
            return true;
        }
    }
}

