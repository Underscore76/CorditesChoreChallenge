using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using Microsoft.Xna.Framework;

using ChoreChallenge.Framework;
using ChoreChallenge.Framework.Achievements;
using ChoreChallenge.Framework.UI;
using System.Reflection;

namespace ChoreChallenge
{
    class ModEntry : Mod
    {
        public readonly TimeSpan ChallengeDuration = new TimeSpan(0, 30, 0);
        public bool HasFinished;
        public bool IsCheating;
        public int Score;
        public RunTimerMenu TimerMenu;
        public List<IAchievement> Achievements;

        // https://github.com/tylergibbs2/StardewValleyMods/blob/2fcff682a5121984a5b910c65a7d34b6dbea71db/BattleRoyale/ModEntry.cs#L80
        public static bool IsOnlyMod()
        {
            foreach (Assembly assm in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assm == Assembly.GetExecutingAssembly())
                    continue;

                Type[] assmTypes = Array.Empty<Type>();
                try
                {
                    assmTypes = assm.GetTypes();
                }
                catch (ReflectionTypeLoadException) { }

                foreach (Type type in assmTypes)
                {
                    if (type.IsSubclassOf(typeof(Mod)))
                        return false;
                }
            }

            return true;
        }

        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.SaveLoaded += GameLoop_SaveLoaded;
            helper.Events.GameLoop.UpdateTicked += GameLoop_UpdateTicked;
            helper.Events.GameLoop.DayEnding += GameLoop_DayEnding;

            IsCheating = !IsOnlyMod();
            CustomChatBox.Register(helper);

            TimerMenu = new RunTimerMenu(ChallengeDuration);

            Achievements = new List<IAchievement>();
            Achievements.Add(new EagleEyedExplorer());
            Achievements.Add(new Stinky());
            Achievements.Add(new SuperStinky());
            Achievements.Add(new MoneyMaker());
            Achievements.Add(new WickedDecorations());
            Achievements.Add(new LavishDecorations());
            Achievements.Add(new BringOwlyHome());
            Achievements.Add(new ArtCollector());
            Achievements.Add(new UberEatsDriverOfTheDay());
            Achievements.Add(new FasterThanASpeedingHorse());
            Achievements.Add(new Mach41());
            Achievements.Add(new CommunityCenter());
            Achievements.Add(new FiveFingers());
            Achievements.Add(new MadHatter());
            Achievements.Add(new MusicProducer());
            Achievements.Add(new PlayAPrank());
            Achievements.Add(new Geologist());
            Achievements.Add(new TrashToTreasure());
            Achievements.Add(new MasterOfMayo());
            Achievements.Add(new PopTheBalloon());
            Achievements.Add(new RareAndValuable());
            Achievements.Add(new TwoTilesOfRange());
            Achievements.Add(new RestAndRelaxation());
            Achievements.Add(new Brute());
            Achievements.Add(new Extinction());
            Achievements.Add(new KweeLittleChallenge());
            Achievements.Add(new DutifulFarmer());
            Achievements.Add(new ModernComfort());
            Achievements.Add(new Capitalist());
            Achievements.Add(new ClintDeservesLoveToo());
            Achievements.Add(new Ethereal());
            Achievements.Add(new SafetyFirst());


            var harmony = new Harmony(this.ModManifest.UniqueID);
            foreach (var ach in Achievements)
            {
                ach.Initialize(this.Monitor, helper.Reflection);
                ach.Patch(harmony);
            }
        }

        private void GameLoop_DayEnding(object sender, DayEndingEventArgs e)
        {
            TimerMenu.End();
        }

        private void GameLoop_UpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            TimerMenu.Update(Score);
            if (!HasFinished && TimerMenu.HasEnded)
            {
                LogEndScore(TimerMenu.RunDuration);
                HasFinished = true;
            }
            else
            {
                Score = 0;
                foreach (var ach in Achievements)
                {
                    ach.OnUpdate();
                    Score += ach.GetScore();
                }
            }
        }

        private void GameLoop_SaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            HasFinished = false;
            TimerMenu.Start();
            foreach (var ach in Achievements)
            {
                ach.OnSaveLoaded();
            }

            Game1.onScreenMenus.Remove(Game1.chatBox);
            Game1.onScreenMenus.Add(Game1.chatBox = new CustomChatBox());
            Game1.onScreenMenus.Add(TimerMenu);
            if (IsCheating)
            {
                DrawHelper.DisplayWarning("****OTHER MODS DETECTED: Have Fun Practicing****");
            }
        }

        private void LogEndScore(TimeSpan duration)
        {
            Score = 0;
            foreach (var ach in Achievements)
            {
                ach.OnEnd();
                Score += ach.GetScore();
            }
            foreach (var ach in Achievements)
            {
                if (ach is AchievementCollection collection)
                {
                    foreach (var subach in collection.Achievements)
                    {
                        DrawHelper.DisplayAchievementStatus(subach);
                    }
                }
                DrawHelper.DisplayAchievementStatus(ach);
            }
            DrawHelper.DisplayScore(Score, duration);
            if (IsCheating)
            {
                DrawHelper.DisplayWarning("****OTHER MODS DETECTED****");
            }
        }
    }
}
