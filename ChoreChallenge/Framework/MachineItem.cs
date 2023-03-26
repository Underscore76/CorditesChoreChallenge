using System;
using Microsoft.Xna.Framework;

namespace ChoreChallenge.Framework
{
    public class MachineItem
    {
        public string LocationName;
        public Vector2 TileLocation;
        public string DropInItem;
        public MachineItem(string locName, Vector2 tile, string item)
        {
            LocationName = locName;
            TileLocation = tile;
            DropInItem = item;
        }
    }
}

