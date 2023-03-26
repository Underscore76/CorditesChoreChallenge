using System;
using HarmonyLib;
using StardewModdingAPI;
using StardewValley;

namespace ChoreChallenge.Framework.Achievements
{
	public class RareAndValuable : IAchievement
	{
        private uint InitialShards;
        private static RareAndValuable instance;
        public RareAndValuable()
            : base("Rare and Valuable", 10)
        {
            instance = this;
        }

        public override void OnUpdate()
        {
            instance.Monitor.Log($"{InitialShards}/{Game1.stats.PrismaticShardsFound}", LogLevel.Alert);
            if (Game1.stats.PrismaticShardsFound >= InitialShards + 2)
            {
                instance.HasSeen = true;
            }
            base.OnUpdate();
        }

        public override void OnSaveLoaded()
        {
            InitialShards = Game1.stats.PrismaticShardsFound;
            base.OnSaveLoaded();
        }
    }
}

