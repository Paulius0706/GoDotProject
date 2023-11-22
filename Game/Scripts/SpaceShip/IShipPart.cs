using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PartInfo;

namespace Game.Scripts.SpaceShip
{
    public interface IShipPart
    {

        Key PKey { get; set; }
        Key NKey { get; set; }
        Vector3 WorldPosition { get; }
        Vector3I LocalPosition { get; set; }
        float LocalRotation { get; }
        float WorldRotation { get; }
        float Mass { get; set; }
        float PowerMultiplier { get; set; }
        List<PartInfoFields> HideList { get; }
        IShip ParentPart { get; }
        string InventoryImagePath { get; }
        void RotateLeft();
        void RotateRight();

    }
}
