using Godot;
using Godot.Collections;
using System;

public partial class Chunk : Node3D
{
	public const int Size = 16;
    public const string BaseBlock = "res://Meshes/Terrain/Base.tres";
    public const string BaseChunk = "res://Meshes/Terrain/ChunkMultiMesh.tres";

    public Vector2I ChunkPosition { get; private set; }
	public Dictionary<Vector4I, Node3D> blocks = new Dictionary<Vector4I, Node3D>();
    public Mesh mesh;
    public MultiMesh multiMesh;
    public MultiMeshInstance3D multiMeshInstance;

    public Chunk(Vector2I ChunkPosition) : base()
	{
		Position = new Vector3I(ChunkPosition.X, 0, ChunkPosition.Y) * Size;
		this.ChunkPosition = ChunkPosition;
    }
    private void Debug()
    {
        mesh = GD.Load<Mesh>(BaseBlock);
        multiMesh = new MultiMesh();
        multiMesh.UseColors = true;
        multiMesh.Mesh = mesh;

        multiMesh.TransformFormat = MultiMesh.TransformFormatEnum.Transform3D;
        multiMesh.InstanceCount = 1000;
        

        multiMeshInstance = new MultiMeshInstance3D()
        {
            Multimesh = multiMesh,
            Position = Vector3.Zero,

        };
        
        AddChild(multiMeshInstance);

        Random random = new Random(ChunkPosition.X * 100000 + ChunkPosition.Y);
        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 16; y++)
            {
                Vector3 temp = (Position + Vector3.Back * y + Vector3.Right * x) * 0.25f;
                float rnd = Terrain.noise.GetNoise2D(temp.X, temp.Z);
                if (rnd < 0f) continue;
                Transform3D transform3D = new Transform3D(Basis.Identity, new Vector3(x, 0, y));
                multiMesh.SetInstanceTransform(x * 16 + y, transform3D);
                multiMesh.SetInstanceColor(x * 16 + y, new Color(0, 0, 0, 1));
            }
        }
    }


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        Debug();
        
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    protected override void Dispose(bool disposing)
    {
        multiMeshInstance.Dispose();
        base.Dispose(disposing);
    }
}
