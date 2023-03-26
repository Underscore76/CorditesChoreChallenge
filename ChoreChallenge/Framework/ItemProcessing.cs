using System;
using System.Collections.Generic;
using StardewValley;
using Object = StardewValley.Object;

namespace ChoreChallenge.Framework
{
    public abstract class ItemProcessing : CumulativeAchievement
    {
        protected HashSet<string> NeededItems;
        protected HashSet<string> CompletedItems;
        private HashSet<MachineItem> ActiveMachines;
        private HashSet<string> ValidMachineNames;

        protected ItemProcessing(string description, int score, List<string> items)
            : base(description, score)
        {
            NeededItems = new HashSet<string>(items);
            MaxValue = NeededItems.Count;
            CompletedItems = new HashSet<string>();
            ActiveMachines = new HashSet<MachineItem>();
            ValidMachineNames = new HashSet<string>();
        }

        public void AddMachineToWatch(string name)
        {
            ValidMachineNames.Add(name);
        }

        protected void onReadyForHarvest(Object obj, GameLocation location)
        {
            MachineItem toRemove = null;
            foreach (var machine in ActiveMachines)
            {
                if (
                    machine.LocationName == location.Name &&
                    machine.TileLocation == obj.TileLocation
                    )
                {
                    toRemove = machine;
                    break;
                }
            }
            if (toRemove != null)
            {
                ActiveMachines.Remove(toRemove);
                CompletedItems.Add(toRemove.DropInItem);
            }
            CurrentValue = CompletedItems.Count;
        }

        protected void performToolAction(Object obj, GameLocation location, bool success)
        {
            if (!success) return;

            MachineItem toRemove = null;
            foreach (var machine in ActiveMachines)
            {
                if (
                    machine.LocationName == location.Name &&
                    machine.TileLocation == obj.TileLocation
                    )
                {
                    toRemove = machine;
                    break;
                }
            }
            if (toRemove != null)
            {
                ActiveMachines.Remove(toRemove);
            }
            CurrentValue = CompletedItems.Count;
        }

        protected void performObjectDropInAction(Object obj, Item dropInItem, bool probe)
        {
            if (probe) return; // just checking if it can drop in
            if (!ValidMachineNames.Contains(obj.Name)) return; // the correct type of machine
            if (NeededItems.Contains(dropInItem.Name) && obj.heldObject.Value == null)
            {
                ActiveMachines.Add(new MachineItem(Game1.currentLocation.Name, obj.TileLocation, dropInItem.Name));
            }
        }

        protected void performObjectDropInAction(Object obj, string dropInItem, bool probe)
        {
            if (probe) return; // just checking if it can drop in
            if (!ValidMachineNames.Contains(obj.Name)) return; // the correct type of machine
            if (NeededItems.Contains(dropInItem) && obj.heldObject.Value == null)
            {
                ActiveMachines.Add(new MachineItem(Game1.currentLocation.Name, obj.TileLocation, dropInItem));
            }
        }

        protected void RunEndOfDay()
        {
            foreach (var machine in ActiveMachines)
            {
                var loc = Game1.getLocationFromName(machine.LocationName);
                // ensure the machine actually exists at end of day and there is 
                if (loc != null && loc.Objects.TryGetValue(machine.TileLocation, out var obj))
                {
                    if (obj.heldObject.Value == null) continue;
                    if (NeededItems.Contains(machine.DropInItem))
                    {
                        CompletedItems.Add(machine.DropInItem);
                    }
                }
            }
            CurrentValue = CompletedItems.Count;
        }

        public override void OnSaveLoaded()
        {
            CompletedItems.Clear();
            ActiveMachines.Clear();
            base.OnSaveLoaded();
        }
    }
}