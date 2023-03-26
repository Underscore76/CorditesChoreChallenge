using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using StardewModdingAPI;
using StardewModdingAPI.Events;

using ChoreChallenge.Framework;
using ChoreChallenge.Framework.Achievements;

namespace ChoreChallenge
{
    class ModEntry : Mod
    {
        public int Score;
        public ScorePanel ScorePanel;
        public List<IAchievement> Achievements;
        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.SaveLoaded += GameLoop_SaveLoaded;
            helper.Events.GameLoop.UpdateTicked += GameLoop_UpdateTicked;
            helper.Events.Display.RenderedHud += Display_RenderedHud;
            ScorePanel = new ScorePanel();

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
            Achievements.Add(new TwoTilesOfRange());


            var harmony = new Harmony(this.ModManifest.UniqueID);
            foreach (var ach in Achievements)
            {
                ach.Initialize(this.Monitor, helper.Reflection);
                ach.Patch(harmony);
            }
        }

        private void Display_RenderedHud(object sender, RenderedHudEventArgs e)
        {
            ScorePanel.Draw(e.SpriteBatch, Score);
        }

        private void GameLoop_UpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;
            Score = 0;
            foreach (var ach in Achievements)
            {
                ach.OnUpdate();
                Score += ach.GetScore();
            }
        }

        private void GameLoop_SaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            ScorePanel.Start();
            foreach (var ach in Achievements)
            {
                ach.OnSaveLoaded();
            }
        }
    }
}
