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
            helper.Events.Display.Rendered += Display_Rendered;

            IsCheating = !IsOnlyMod();
            CustomChatBox.Register(helper);

            TimerMenu = new RunTimerMenu(ChallengeDuration);

            Achievements = new List<IAchievement>();
            Achievements.Add(new Stinky());
            Achievements.Add(new SuperStinky());
            Achievements.Add(new MoneyMaker());
            Achievements.Add(new MusicProducer());
            Achievements.Add(new WickedDecorations());
            Achievements.Add(new LavishDecorations());
            Achievements.Add(new Brute());
            Achievements.Add(new PopTheBalloon());
            Achievements.Add(new Extinction());
            Achievements.Add(new FasterThanASpeedingHorse());
            Achievements.Add(new Geologist());
            Achievements.Add(new RestAndRelaxation());
            Achievements.Add(new TwoTilesOfRange());
            Achievements.Add(new ModernComfort());
            Achievements.Add(new TrashToTreasure());
            Achievements.Add(new DutifulFarmer());
            Achievements.Add(new FiveFingers());
            Achievements.Add(new MadHatter());
            Achievements.Add(new SafetyFirst());
            Achievements.Add(new PlayAPrank());
            Achievements.Add(new ClintDeservesLoveToo());
            Achievements.Add(new BringOwlyHome());
            Achievements.Add(new Ethereal());
            Achievements.Add(new Capitalist());
            Achievements.Add(new RareAndValuable());
            Achievements.Add(new MasterOfMayo());
            Achievements.Add(new KweeLittleChallenge());
            Achievements.Add(new Mach41());
            Achievements.Add(new EagleEyedExplorer());
            Achievements.Add(new ArtCollector());
            Achievements.Add(new UberEatsDriverOfTheDay());
            Achievements.Add(new CommunityCenter());


            var harmony = new Harmony(this.ModManifest.UniqueID);
            foreach (var ach in Achievements)
            {
                ach.Initialize(this.Monitor, helper.Reflection);
                ach.Patch(harmony);
            }
        }

        private void Display_Rendered(object sender, RenderedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            // this leads to a double draw but it forces it to be always on top
            if (TimerMenu != null)
                TimerMenu.draw(e.SpriteBatch);
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

        private static bool IsGameValid()
        {
            return Game1.uniqueIDForThisGame == 338344445 && Game1.player.farmName.Value == "Chore Day" && Game1.player.Name == "Cord" && Game1.stats.DaysPlayed == 46 && Game1.GetSaveGameName() == "Choreday";
        }

        private void GameLoop_SaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            TimerMenu.Start();
            Game1.onScreenMenus.Add(TimerMenu);
            if (IsGameValid())
            {
                HasFinished = false;
                foreach (var ach in Achievements)
                {
                    ach.OnSaveLoaded();
                }

                Game1.onScreenMenus.Remove(Game1.chatBox);
                Game1.onScreenMenus.Add(Game1.chatBox = new CustomChatBox());
                if (IsCheating)
                {
                    DrawHelper.DisplayWarning("****OTHER MODS DETECTED: Have Fun Practicing****");
                }
            }
            else
            {
                DrawHelper.DisplayWarning("****GAME NOT VALID FOR CHORE CHALLENGE****");
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
