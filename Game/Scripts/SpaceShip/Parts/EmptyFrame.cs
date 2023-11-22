using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PartInfo;

namespace Game.Scripts.SpaceShip.Parts
{
    public partial class EmptyFrame : ShipPart
    {
        public override List<PartInfoFields> HideList { get { return new List<PartInfoFields>() { PartInfoFields.NegativeKey, PartInfoFields.Power, PartInfoFields.PositiveKey }; } }
        public override string MeshPath { get { return "res://Meshes/ShipParts/EmptyFrame.tres"; } }
        public float Power = 10f;

        public EmptyFrame() : base() { }
        public EmptyFrame(Ship ship, Vector3I localPos, float rotation = 0f, Key pKey = Key.None, Key nKey = Key.None) : base(ship, localPos, rotation, pKey, nKey) { }
        public override void _Ready()
        {
            base._Ready();
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
            base._Process(delta);
        }
    }
}
