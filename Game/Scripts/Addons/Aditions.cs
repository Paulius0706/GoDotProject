using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Scripts.Addons
{
    public static class Aditions
    {
        public static void Add(this Dictionary<Vector3I, ShipPart> parts, ShipPart shipPart)
        {
            parts.Add(shipPart.LocalPosition, shipPart);
        }

        public static bool IsKeyPressedInGameTime(Key key)
        {
            if (WorldUI.PartInfo.Editing) return false;
            if (WorldUI.BuilderInterface.Building) return false;
            return Input.IsKeyPressed(key);
        }
        public static bool IsKeyPressedInBuildTime(Key key)
        {
            if (WorldUI.PartInfo.Editing) return false;
            if (!WorldUI.BuilderInterface.Building) return false;
            return Input.IsKeyPressed(key);
        }
        public static int CameraMouseScrool()
        {
            if (Input.IsKeyPressed(Key.Shift)) return 0;
            if (Input.IsActionJustReleased("Mouse_Scroll_Down")) { return 1; }
            if (Input.IsActionJustReleased("Mouse_Scroll_Up")) { return -1; }
            return 0;
        }
        public static int PlaceHolderMouseScrool()
        {
            if (!Input.IsKeyPressed(Key.Shift)) return 0;
            if (!WorldUI.BuilderInterface.Building) return 0;
            if (Input.IsActionJustReleased("Mouse_Scroll_Down")) { return 1; }
            if (Input.IsActionJustReleased("Mouse_Scroll_Up")) { return -1; }
            return 0;
        }


    }
}
