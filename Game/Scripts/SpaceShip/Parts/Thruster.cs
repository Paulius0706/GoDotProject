using Game.Scripts.Addons;
using Godot;
using System;
using System.Collections.Generic;
using static PartInfo;

public partial class Thruster : ShipPart
{
	public override List<PartInfoFields> HideList { get { return new List<PartInfoFields>() { PartInfoFields.NegativeKey }; } }
	public override string MeshPath { get { return "res://Meshes/ShipParts/Truster.tres"; } }
	public override string InventoryImagePath { get { return "res://Textures/Images/ThrusterImage.png"; } }
	public float Power = 10f;

	public Thruster() : base() {}
    public Thruster(Ship ship, Vector3I localPos, float rotation = 0f, Key pKey = Key.None, Key nKey = Key.None) : base(ship, localPos, rotation, pKey, nKey) { }
    public override void _Ready()
	{
		base._Ready();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) 
	{
		base._Process(delta);
		if (Aditions.IsKeyPressedInGameTime(PKey))
		{
			Vector3 foward = GlobalTransform.Basis.Z;
            rigidBody.ApplyImpulse(PowerMultiplier * Power * foward * (float)delta);
        }
	}
}
