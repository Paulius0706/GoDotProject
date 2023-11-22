using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PartInfo;

namespace Game.Scripts.SpaceShip.Parts
{
    public partial class Cannon : ShipPart
    {
        public override List<PartInfoFields> HideList { get { return new List<PartInfoFields>() { PartInfoFields.NegativeKey, PartInfoFields.Power }; } }
        public override string MeshPath { get { return "res://Meshes/ShipParts/Cannon.res"; } }
        public override string InventoryImagePath { get { return "res://Textures/Images/ThrusterImage.png"; } }
        public Cannon()
        {
        }

        public Cannon(Ship ship, Vector3I localPos, float rotation = 0, Key pKey = Key.None, Key nKey = Key.None) : base(ship, localPos, rotation, pKey, nKey)
        {
            meshInstance.Scale *= 0.5f;
        }
    }
}
