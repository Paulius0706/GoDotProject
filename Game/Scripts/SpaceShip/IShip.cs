using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Scripts.SpaceShip
{
    public interface IShip : IShipPart
    {
        void AddChild(Node node);
        string GetPath();
        void RemovePart(Vector3I position);
        IShipPart GetPart(Vector3I position);
    }
}
