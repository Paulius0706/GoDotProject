
using Godot;
using System;

public partial class WorldUI : Control
{
	private static WorldUI Instance { get; set; }
	private static PartInfo _partInfo;
	public static PartInfo PartInfo 
	{ 
		get 
		{  
			if(_partInfo == null) _partInfo = Instance.FindChild(nameof(PartInfo)) as PartInfo;
			return _partInfo;
        } 
	}
	private static BuilderInterface _builderInterface;
    public static BuilderInterface BuilderInterface 
	{ 
		get
		{
            if (_builderInterface == null) _builderInterface = Instance.FindChild(nameof(BuilderInterface)) as BuilderInterface;
            return _builderInterface;
        } 
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

    }
}
