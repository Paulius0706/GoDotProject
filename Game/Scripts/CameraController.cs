using Game.Scripts.Addons;
using Godot;
using System;
using System.Linq;
using System.Reflection.Metadata;

public partial class CameraController : Camera3D
{
	public static CameraController Instance { get; private set; }
	public static Ship player;
	public const float ZoomSpeed = 4f;
	public float Zoom = 5;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
		SetPhysicsProcess(true);

    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		BindToPlayer(delta);
		RayCast();
    }
    private void BindToPlayer(double delta)
    {
        if (player == null) return;

        if (WorldUI.BuilderInterface.Building)
        {
            Position = WorldUI.BuilderInterface.SelectedShip.WorldPosition + Vector3.Up * Zoom;
            //Rotation = new Vector3(WorldUI.BuilderInterface.SelectedShip.WorldRotation, 0, 0);
            Rotation = Vector3.Zero;
            RotateX(((float)Math.PI / 180f) * -90f);
            RotateY(WorldUI.BuilderInterface.SelectedShip.WorldRotation);
        }
        else
        {
            Position = player.Position + Vector3.Up * Zoom;
            Rotation = Vector3.Zero;
            RotateX(((float)Math.PI / 180f) * -90f);
        }
        Zoom += Aditions.CameraMouseScrool() * Zoom * ZoomSpeed * (float)delta;
        
    }
    private void RayCast()
	{
        Vector2 mouse = GetViewport().GetMousePosition();
        Vector3 from = this.ProjectRayOrigin(mouse);
        Vector3 to = from + this.ProjectRayNormal(mouse) * 100;

        var spaceState = GetWorld3D().DirectSpaceState;
        // use global coordinates, not local to node
        var query = PhysicsRayQueryParameters3D.Create(from, to);
        var result = spaceState.IntersectRay(query);
		if(result.Count > 0)
		{
            Variant colliderKey = result.Keys.First(o => o.Obj.ToString() == "collider");
            var obj = result[colliderKey].Obj;
            SelectPartInfo(obj);
		}
        if (WorldUI.BuilderInterface.Building)
        {
            SelectBuildPlace(to, from);
        }
    }
    private void SelectPartInfo(object obj)
    {
        if (obj == null) return;
        if(obj is RigidBody3D)
        {
            RigidBody3D rigidBody = obj as RigidBody3D;
            rigidBody._InputEvent(this, null, Vector3.Zero, Vector3.Zero, 0);
        }
    }
    private void SelectBuildPlace(Vector3 to, Vector3 from)
    {
        Vector3 direction = (to - from).Normalized();
        float heigth = from.Y - to.Y;
        float lenght = (from.Y - (WorldUI.BuilderInterface.SelectedShip.WorldPosition.Y)) / heigth * (to - from).Length();
        Vector3 position = lenght * direction + from;
        WorldUI.BuilderInterface.ManageRayCast(position);
    }

}
