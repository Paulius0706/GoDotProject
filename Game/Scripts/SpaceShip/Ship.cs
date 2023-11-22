using Game.Scripts.Addons;
using Game.Scripts.SpaceShip;
using Game.Scripts.SpaceShip.Parts;
using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;

public partial class Ship : RigidBody3D, IShip
{
    public const float deg2rad = 0.0174532925f;
    private static bool HoldingMouseButton { get; set; } = false;
    public static IShip Player { get; private set; }
    public virtual List<PartInfo.PartInfoFields> HideList { get { return new List<PartInfo.PartInfoFields>() { PartInfo.PartInfoFields.Power,PartInfo.PartInfoFields.PositiveKey,PartInfo.PartInfoFields.NegativeKey }; } }
    public Key PKey { get { return Key.None; } set { } }
    public Key NKey { get { return Key.None; } set { } }
    public Vector3I LocalPosition
    {
        get
        {
            return new Vector3I((int)Math.Round(this.Position.X), (int)Math.Round(this.Position.Y), (int)Math.Round(this.Position.Z));
        }
        set
        {
            this.Position = value;
        }
    }
    public Vector3 WorldPosition { get { return this.GlobalPosition; } }
    public virtual string InventoryImagePath { get { return "res://Textures/Images/icon.svg"; } }
    public float PowerMultiplier { get { return 0; } set { } }
    public IShip ParentPart { get { return this.GetParent() == null ? this : this.GetParent<Ship>(); } }

    public float LocalRotation { get { return this.Rotation.Y; } }
    public float WorldRotation { get { return this.GlobalRotation.Y; } }

    public IShipPart GetPart(Vector3I position) { return this.GetChildren().FirstOrDefault(o => GetRoundedPosition(((Node3D)o).Position) == position) as ShipPart; }
    public Vector3I GetRoundedPosition(Vector3 position) { return new Vector3I((int)MathF.Round(position.X), (int)MathF.Round(position.Y), (int)MathF.Round(position.Z)); }
    
    public void RemovePart(Vector3I position)
    {
        ShipPart shipPart = GetPart(position) as ShipPart;
        if (shipPart == null) return;
        shipPart.QueueFree();
    }


    public override void _Ready()
	{
        if(Player == null)
        {
            Player = this;
        }
        if(Player == this)
        {
            Terrain.player = this;
            CameraController.player = this;
            WorldUI.BuilderInterface.SelectedShip = this;
        }
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public bool firstFrame = true;
	public override void _Process(double delta)
	{
        if (firstFrame)
        {
            firstFrame = false;
            _ = new Thruster(this, new Vector3I(1, 0, 1), -90f, Key.D);
            _ = new Thruster(this, new Vector3I(-1, 0, 1), 90f, Key.A);
            _ = new Thruster(this, new Vector3I(-1, 0, -1), 90f, Key.D);
            _ = new Thruster(this, new Vector3I(1, 0, -1), -90f, Key.A);
            _ = new Thruster(this, new Vector3I(0, 0, -1), 0f, Key.W);
            _ = new EmptyFrame(this, new Vector3I(1, 0, 0));
            _ = new EmptyFrame(this, new Vector3I(-1, 0, 0));
        }
    }
    public override void _InputEvent(Camera3D camera, InputEvent @event, Vector3 position, Vector3 normal, int shapeIdx)
    {
        if (Input.IsMouseButtonPressed(MouseButton.Left) && HoldingMouseButton == false)
        {
            WorldUI.PartInfo.SelectedShipPart = this;
            HoldingMouseButton = true;
        }
        if (!Input.IsMouseButtonPressed(MouseButton.Left))
        {
            HoldingMouseButton = false;
        }
    }

    public void RotateLeft()
    {
    
    }

    public void RotateRight()
    { 

    }

    public void AddChild(Node node)
    {
        base.AddChild(node);
    }

    string IShip.GetPath()
    {
        return base.GetPath();
    }
}

