using Game.Scripts.SpaceShip;
using Godot;
using System;

public partial class BuilderInterface : Control
{
    public const string GridMeshPath = "res://Meshes/UI/Building/BuildGridMesh.tres";

    public IShip SelectedShip { get { return _selectedShip; } set { _selectedShip = value; } }
    private IShip _selectedShip;
    public PlaceHolder PlaceHolder { get { if (_placeHolder == null) _placeHolder = FindChild("PlaceHolderBacground").FindChild(nameof(PlaceHolder)) as PlaceHolder; return _placeHolder; } }
    private PlaceHolder _placeHolder;
    public bool Building { get { return this.Visible; } private set { this.Visible = value;} }
    private MultiMeshInstance3D multiMeshInstance { get; set; }
    private MultiMesh multiMesh { get; set; }
    private Mesh mesh { get; set; }
    private bool HoldingBuildButton { get; set; } = false;

	
	// Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        this.VisibilityChanged += () =>
        {
            if(multiMeshInstance != null)
            {
                multiMeshInstance.Visible = this.Visible;
            }
        };
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

        if (SelectedShip == null) { SelectedShip = Ship.Player; }
        ToggleBuidMode();
        if (Building)
        {
            AddBuidGrid();
            MoveBuildGrid();
        }
    }

    public void ManageRayCast(Vector3 position)
    {
        if (!Building) return;
        PlaceHolder.ManageRayCast(position);
        
    }

    
    private void ToggleBuidMode()
    {
        if (Input.IsKeyPressed(Key.B) && HoldingBuildButton == false)
        {
            Building = Building ? false : true;
            HoldingBuildButton = true;
        }
        if (!Input.IsKeyPressed(Key.B))
        {
            HoldingBuildButton = false;
        }
    }
    private void AddBuidGrid()
    {
        if (multiMeshInstance != null) return;

        mesh = GD.Load<Mesh>(GridMeshPath);
        multiMesh = new MultiMesh();
        multiMesh.UseColors = true;
        multiMesh.Mesh = mesh;

        multiMesh.TransformFormat = MultiMesh.TransformFormatEnum.Transform3D;
        multiMesh.InstanceCount = 2000;
        multiMeshInstance = new MultiMeshInstance3D()
        {
            Multimesh = multiMesh,
            Position = Vector3.Zero,

        };
        Space.Instance.AddChild(multiMeshInstance);
        for (int x = -16; x <= 16; x++)
        {
            for (int y = -16; y <= 16; y++)
            {
                Transform3D transform3D = new Transform3D(Basis.Identity, new Vector3(x, 0, y));
                multiMesh.SetInstanceTransform((x + 16) * 32 + (y + 16), transform3D);
            }
        }
    }
    private void MoveBuildGrid()
    {
        if (multiMeshInstance == null) return;
        if (SelectedShip == null) return;
        multiMeshInstance.GlobalPosition = SelectedShip.WorldPosition + Vector3.Up * 0.5f;
        multiMeshInstance.GlobalRotation = new Vector3(0,SelectedShip.WorldRotation,0);
    }

}
