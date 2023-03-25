using System;
using System.Collections.Generic;
using StardewModdingAPI;

namespace ChoreChallenge.Framework
{
    public abstract class UniqueItems : IAchievement
    {
        protected HashSet<string> InitialItems;
        protected HashSet<string> Items;
        protected UniqueItems(string description, int score, IEnumerable<string> items) : base(description, score)
        {
            InitialItems = new HashSet<string>(items);
            Items = new HashSet<string>();
        }

        protected bool AddItem(string name)
        {
            if (InitialItems.Contains(name))
            {
                return Items.Add(name);
            }
            return false;
        }

        public override void OnSaveLoaded()
        {
            Items.Clear();
            base.OnSaveLoaded();
        }

        public override void OnUpdate()
        {
            HasSeen = InitialItems.SetEquals(Items);
            base.OnUpdate();
        }
    }
}

