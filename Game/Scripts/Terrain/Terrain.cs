using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class Terrain : Node3D
{
	//private static Resource ChunkResource;
	public const string ChunkResourcePath = "res://Prefabs/chunk.tscn";
	public static FastNoiseLite noise = new FastNoiseLite();
	public static int ChunkDistance = 3;
	public static Ship player;
	public static Dictionary<Vector2I, Chunk> Chunks = new Dictionary<Vector2I, Chunk>();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		noise.Seed = 1000;
		noise.NoiseType = FastNoiseLite.NoiseTypeEnum.SimplexSmooth;
		noise.FractalType = FastNoiseLite.FractalTypeEnum.Fbm;
		noise.FractalOctaves = 3;
		noise.FractalLacunarity = 2f;
		noise.FractalGain = 0.5f;
		noise.Frequency = 0.1f;
		GD.Print(noise.Frequency);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (player == null) return;
		Vector2I playerChunkPos = GetTargetChunk(player.Position);
		
		for(int x = playerChunkPos.X - Terrain.ChunkDistance; x <= playerChunkPos.X + Terrain.ChunkDistance; x++)
		{
            for (int y = playerChunkPos.Y - Terrain.ChunkDistance; y <= playerChunkPos.Y + Terrain.ChunkDistance; y++)
            {
				Vector2I key = new Vector2I(x, y);
				if (Terrain.Chunks.ContainsKey(key)) continue;
				Chunk chunk = new Chunk(key);
                Terrain.Chunks.Add(key, chunk);
				this.AddChild(chunk);
			}
        }
		var keys = Terrain.Chunks.Keys.ToList();
        foreach (var key in keys)
		{
			//chunk.Value.ChunkPosition;
			var chunk = Terrain.Chunks[key];
			if(Math.Abs(chunk.ChunkPosition.X - playerChunkPos.X)> ChunkDistance
				|| Math.Abs(chunk.ChunkPosition.Y - playerChunkPos.Y) > ChunkDistance)
			{
				//GD.Print(key);
				this.RemoveChild(chunk);
				chunk.Dispose();
				Terrain.Chunks.Remove(key);
			}
		}
	}
    public static Vector2I GetTargetChunk(Vector3 objectPosition)
    {
        return new Vector2I((int)MathF.Floor(objectPosition.X / Chunk.Size), (int)MathF.Floor(objectPosition.Z / Chunk.Size));
    }
}
