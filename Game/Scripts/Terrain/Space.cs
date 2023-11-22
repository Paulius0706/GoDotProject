using Godot;
using System;

public partial class Space : Node3D
{
	public static Space Instance { get; private set; }
	// Called when the node enters the scene tree for the first time.
	public bool IsLoaded { get; private set; } = false;

	public Space()
	{
		Instance = this;

	}
	public override void _Ready()
	{

    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		IsLoaded = true;
	}
}
