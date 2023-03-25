using System;
using System.Collections.Generic;
using HarmonyLib;
using StardewModdingAPI;

namespace ChoreChallenge.Framework
{
    public abstract class AchievementCollection : IAchievement
    {
        private List<IAchievement> Achievements;
        public AchievementCollection(string description, int score, List<IAchievement> achievements) : base(description, score)
        {
            Achievements = new List<IAchievement>(achievements);
        }
        public override int GetScore()
        {
            int score = base.GetScore();
            foreach (var ach in Achievements)
            {
                score += ach.GetScore();
            }
            return score;
        }

        public override void Patch(Harmony harmony)
        {
            foreach (var ach in Achievements)
            {
                ach.Patch(harmony);
            }
        }

        public override void Initialize(IMonitor monitor, IReflectionHelper reflection)
        {
            foreach (var ach in Achievements)
            {
                ach.Initialize(monitor, reflection);
            }
            base.Initialize(monitor, reflection);
        }

        public override void OnSaveLoaded()
        {
            foreach (var ach in Achievements)
            {
                ach.OnSaveLoaded();
            }
            base.OnSaveLoaded();
        }

        public override void OnUpdate()
        {
            bool allDone = true;
            foreach (var ach in Achievements)
            {
                ach.OnUpdate();
                allDone &= ach.HasSeen;
            }
            if (allDone)
            {
                HasSeen = true;
            }
            base.OnUpdate();
        }
    }
}

