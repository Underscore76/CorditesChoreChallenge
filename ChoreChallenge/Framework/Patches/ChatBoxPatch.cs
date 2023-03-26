using System;
using System.Collections.Generic;
using ChoreChallenge.Framework.Achievements;
using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;

namespace ChoreChallenge.Framework.Patches
{
	public static class ChatBoxPatch
	{
		private static IReflectionHelper Reflection;
		public static List<ChatMessage> ChatMessages = new List<ChatMessage>();

		static ChatBoxPatch()
		{
			Game1.chatBox.maxMessages = 15;
		}

		public static void Register(IModHelper helper, Harmony harmony)
		{
			Reflection = helper.Reflection;
            harmony.Patch(
                original: AccessTools.Method(typeof(ChatBox), "addMessage"),
                postfix: new HarmonyMethod(typeof(ChatBoxPatch), nameof(ChatBoxPatch.Postfix_addMessage))
                );
            harmony.Patch(
                original: AccessTools.Method(typeof(ChatBox), "draw"),
                prefix: new HarmonyMethod(typeof(ChatBoxPatch), nameof(ChatBoxPatch.Prefix_draw)),
                postfix: new HarmonyMethod(typeof(ChatBoxPatch), nameof(ChatBoxPatch.Postfix_draw))
                );
            harmony.Patch(
                original: AccessTools.Method(typeof(ChatBox), "update"),
                prefix: new HarmonyMethod(typeof(ChatBoxPatch), nameof(ChatBoxPatch.Prefix_draw)),
                postfix: new HarmonyMethod(typeof(ChatBoxPatch), nameof(ChatBoxPatch.Postfix_draw))
                );
        }

		public static IReflectedField<List<ChatMessage>> GetChatMessages()
		{
			return Reflection.GetField<List<ChatMessage>>(Game1.chatBox, "messages");
		}
		public static void Postfix_addMessage()
		{
			if (Reflection == null) return;

			var chatMessages = GetChatMessages().GetValue();
			if (chatMessages.Count == 0) return;

			ChatMessages.Add(chatMessages[chatMessages.Count - 1]);
        }

		public static bool Prefix_draw()
		{
			return false;
		}

        public static void Postfix_draw()
        {
        }

        public static bool Prefix_update()
        {
            return false;
        }

        public static void Postfix_update()
        {
        }
    }
}

