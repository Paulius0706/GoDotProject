using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Scripts.SpaceShip
{
    public partial class ShipPartRigBody : RigidBody3D
    {
        // Called when the node enters the scene tree for the first time.
        private bool mouseOver = false;
        private bool holding = false;
        public ShipPart ShipPart { get; private set; }
        public ShipPartRigBody(ShipPart shipPart)
        {
            ShipPart = shipPart;
            
        }
        public override void _Ready()
        {
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
            ManageInput();
        }

        private void ManageInput()
        {
            if (!mouseOver) return;
            GD.Print("detected");
            if (Input.IsMouseButtonPressed(MouseButton.Left) && holding == false)
            {
                WorldUI.PartInfo.SelectedShipPart = ShipPart;
                holding = true;
            }
            if(!Input.IsMouseButtonPressed(MouseButton.Left))
            { 
                holding = false; 
            }


        }
        public override void _InputEvent(Camera3D camera, InputEvent @event, Vector3 position, Vector3 normal, int shapeIdx)
        {
            if (Input.IsMouseButtonPressed(MouseButton.Left) && holding == false)
            {
                WorldUI.PartInfo.SelectedShipPart = ShipPart;
                holding = true;
            }
            if (!Input.IsMouseButtonPressed(MouseButton.Left))
            {
                holding = false;
            }
        }

        public override void _MouseEnter() { mouseOver = true; GD.Print("Entered"); }
        
        public override void _MouseExit() { mouseOver = false; GD.Print("Exited"); }




        //public override void _Input(InputEvent @event)
        //{
        //    this.SetProcessUnhandledInput(false);
        //    if(@event is InputEventMouseButton)
        //    {
        //        var buttonEvent = (@event as InputEventMouseButton);
        //        if(buttonEvent.ButtonIndex == MouseButton.Left)
        //        {
        //            GD.Print("detected");
        //            PartInfo.Instance.SelectedShipPart = ShipPart;
        //        }
        //    }
        //}
    }
}
