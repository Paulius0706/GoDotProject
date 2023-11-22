using Game.Scripts.SpaceShip;
using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static PartInfo;

public partial class ShipPart : Node3D, IShipPart
{

    public const float deg2rad = 0.0174532925f;
    public virtual List<PartInfoFields> HideList { get { return new List<PartInfoFields>(); } }
	public virtual string MeshPath { get { return "res://Meshes/ShipParts/Base.tres"; } } 
    public virtual string MaterialPath { get { return "res://Textures/Materials/ShipPartsBaseMaterial.tres"; } }
    public virtual string InventoryImagePath { get { return "res://Textures/Images/icon.svg"; } }
    private static bool HoldingBuildButton { get; set; } = false;


    public Key PKey { get; set; } = Key.None;
    public Key NKey { get; set; } = Key.None;
    public Vector3I LocalPosition 
    { 
        get { return new Vector3I((int)Math.Round(this.Position.X), (int)Math.Round(this.Position.Y), (int)Math.Round(this.Position.Z)); }
        set 
        {
            if (this.ParentPart == null) return;
            Vector3I oldPos = this.LocalPosition;
            this.Position = value;
            rigidBody.GlobalPosition = this.GlobalPosition;
            //rigidBody.GlobalRotation = this.GlobalRotation;
            RefreshJoint();
        }
    }
    public Vector3 WorldPosition { get { return this.GlobalPosition; } }
    public float LocalRotation
    {
        get { return RotationDegrees.Y; }
        set 
        {
            if (this.ParentPart == null) return;
            this.Rotation = new Vector3(0,value * deg2rad,0);
            //rigidBody.GlobalPosition = this.GlobalPosition;
            rigidBody.GlobalRotation = this.GlobalRotation;
            RefreshJoint();
        }
    }
    public float WorldRotation { get { return this.GlobalRotation.Y; } }
    public float Mass { get; set; } = 1f;
    public float PowerMultiplier { get; set; } = 1f;
    public IShip ParentPart { get; private set; }



    public MeshInstance3D meshInstance;
    public CollisionShape3D collisionShape;
    public ShipPartRigBody rigidBody;
    public Generic6DofJoint3D joint;

    private bool isLoaded = false;
    public ShipPart() { }
    public ShipPart(Ship ship, Vector3I localPos, float rotation = 0f, Key pKey = Key.None, Key nKey = Key.None) 
    {
        this.PKey = pKey;
        this.NKey = nKey;
        this.ParentPart = ship;
        this.ParentPart.AddChild(this);
        this.AddRigBody();
        this.LocalPosition = localPos;
        this.LocalRotation = rotation;
        this.TreeExited += () =>
        {
            if (joint != null) joint.QueueFree();
            if (this.rigidBody != null) rigidBody.QueueFree();
        };
    }
    private void AddRigBody()
    {
        this.rigidBody = new ShipPartRigBody(this);
        this.rigidBody.TopLevel = true;

        Space.Instance.AddChild(rigidBody);
        this.rigidBody.MaxContactsReported = 3;
        this.rigidBody.ContactMonitor = true;
        this.rigidBody.ContinuousCd = true;
        this.rigidBody.AxisLockAngularX = true;
        this.rigidBody.AxisLockAngularZ = true;
        this.rigidBody.AxisLockLinearY = true;
        this.rigidBody.GravityScale = 0;

        AddMesh();
        AddCollider();
        RefreshJoint();
    }
    private void AddCollider()
    {
        this.collisionShape = new CollisionShape3D();
        this.collisionShape.Shape = new BoxShape3D();
        this.collisionShape.Scale = Vector3.One * 0.95f;
        this.rigidBody.AddChild(this.collisionShape);
    }
    private void AddMesh()
    {
        Mesh mesh = GD.Load<Mesh>(MeshPath);
        this.meshInstance = new MeshInstance3D()
        {
            Mesh = mesh,
            Name = "Mesh",
            Position = new Vector3(0, 0, 0)
        };
        this.meshInstance.MaterialOverride = GD.Load<ShaderMaterial>(MaterialPath);
        this.rigidBody.AddChild(this.meshInstance);
    }
    private void RefreshJoint()
    {
        if(joint != null) joint.QueueFree();
        joint = new Generic6DofJoint3D();
        Space.Instance.AddChild(joint);
        this.joint.GlobalPosition = this.GlobalPosition;
        joint.NodeA = this.ParentPart.GetPath();
        joint.NodeB = this.rigidBody.GetPath();
    }


    public override void _Ready()
    {
        
    }

    public override void _Process(double delta)
	{
        //Vector3 positionDiffirence = rigidBody.GlobalPosition - this.GlobalPosition;
        this.rigidBody.GlobalPosition = this.GlobalPosition;
        this.rigidBody.GlobalRotation = this.GlobalRotation;
        //this.rigidBody.ApplyImpulse(positionDiffirence * this.rigidBody.Mass * 0.5f);
        //rigidBody rigidBody.Mass * (-positionDiffirence);
    }

    public void RotateLeft()
    {
        this.RotationDegrees = this.RotationDegrees + new Vector3(0, -90, 0);
        this.rigidBody.GlobalRotation = this.GlobalRotation;
        RefreshJoint();
    }

    public void RotateRight()
    {
        this.RotationDegrees = this.RotationDegrees + new Vector3(0, 90, 0);
        this.rigidBody.GlobalRotation = this.GlobalRotation; 
        RefreshJoint();
    }
}
