using Game.Scripts.Addons;
using Game.Scripts.SpaceShip.Parts;
using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

public partial class PlaceHolder : HBoxContainer
{

	private const string ItemHolderPath = "res://Prefabs/UI/itemHolder.tscn";
	private Panel Cursor { get { if (_cursor == null) _cursor = GetParent().GetParent().FindChild(nameof(Cursor)) as Panel; return _cursor; } }
	private Panel _cursor;

    private bool HoldingLeftMouseButton { get; set; }
    private bool HoldingRightMouseButton { get; set; }
    public int ItemHolderCount { get { return _itemHolders.Count; } }
	public float Width { get { return this.CustomMinimumSize.X; } }
    public float Height { get { return this.CustomMinimumSize.Y; } }

    private int _selectedIndex = 0;
	public int SelectedIndex 
	{ 
		get { return _selectedIndex; }
		set
		{
			_selectedIndex = 
				value <=              -1 ? value + ItemHolderCount :
				value >= ItemHolderCount ? value - ItemHolderCount :
				value;
			Cursor.GlobalPosition = _itemHolders[_selectedIndex].GlobalPosition + Vector2.Up * 10f + Vector2.Left * 10f;
			Cursor.Size = _itemHolders[_selectedIndex].Size + Vector2.One * 20f;
		}
	}
	


	private List<ItemHolder> _itemHolders = new List<ItemHolder>();
    public ItemHolder this[int index] { get { return _itemHolders[index]; } }
	public void ManageRayCast(Vector3 position)
	{
		if (WorldUI.PartInfo.HoveringOver) return;
		
		//Calcultate position
		Vector3 realPostion = (position - WorldUI.BuilderInterface.SelectedShip.WorldPosition).Rotated(Vector3.Up, -WorldUI.BuilderInterface.SelectedShip.WorldRotation);
		Vector3I keyPosition = new Vector3I((int)MathF.Round(realPostion.X),0, (int)MathF.Round(realPostion.Z));
        
		//Adding
		if (Input.IsMouseButtonPressed(MouseButton.Left) && HoldingLeftMouseButton == false)
        {
			if(WorldUI.BuilderInterface.SelectedShip.GetPart(keyPosition) == null && this[SelectedIndex].ShipPart != null)
			{
                var type = this[SelectedIndex].ShipPart.GetType();
				_ = Activator.CreateInstance(type, new object[] { WorldUI.BuilderInterface.SelectedShip, keyPosition, 0f, Key.None, Key.None }) as ShipPart;
            }
            HoldingLeftMouseButton = true;
        }
		
		// Removing
        if (!Input.IsMouseButtonPressed(MouseButton.Left)) { HoldingLeftMouseButton = false; }
        if (Input.IsMouseButtonPressed(MouseButton.Right) && HoldingRightMouseButton == false)
        {
			WorldUI.BuilderInterface.SelectedShip.RemovePart(keyPosition);
            HoldingRightMouseButton = true;
        }
        if (!Input.IsMouseButtonPressed(MouseButton.Right)) { HoldingRightMouseButton = false; }
    }

    public override void _Ready()
	{
		for(int i = 0; i<10; i++)
		{
            ItemHolder itemHolder = ResourceLoader.Load<PackedScene>(ItemHolderPath).Instantiate() as ItemHolder;
            _itemHolders.Add(itemHolder);
			AddChild(itemHolder);
		}
		Resize();
		GetViewport().SizeChanged += () =>
		{
			Resize();
        };
		this.MinimumSizeChanged += () =>
		{
            SelectedIndex = _selectedIndex;
        };
		this.VisibilityChanged += () =>
		{
			SelectedIndex = _selectedIndex;
		};


		this[0].ShipPart = new Thruster();
		this[1].ShipPart = new Cannon();
		SelectedIndex = 0;
    }

	private void Resize()
	{
        float newWidth = GetViewportRect().Size.X * 0.8f;
		newWidth = newWidth > 1000f ? 1000f : newWidth;
        float newHeight = newWidth / ItemHolderCount;
        this.CustomMinimumSize = new Vector2(newWidth, newHeight);
        foreach (ItemHolder itemHolder in _itemHolders)
        {
            itemHolder.CustomMinimumSize = new Vector2(newHeight, newHeight);
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		SetUpSelection();
	}
	private void SetUpSelection()
	{
		SetUpKeySelection();
		SetUpScroolSelection();
	}
	private void SetUpScroolSelection()
	{
		int input = Aditions.PlaceHolderMouseScrool();
		if(input != 0)
		{
			SelectedIndex += input;
		}
	}
	private void SetUpKeySelection()
	{
		if (Aditions.IsKeyPressedInBuildTime(Key.Key1)) { SelectedIndex = 0; }
        if (Aditions.IsKeyPressedInBuildTime(Key.Key2)) { SelectedIndex = 1; }
        if (Aditions.IsKeyPressedInBuildTime(Key.Key3)) { SelectedIndex = 2; }
        if (Aditions.IsKeyPressedInBuildTime(Key.Key4)) { SelectedIndex = 3; }
        if (Aditions.IsKeyPressedInBuildTime(Key.Key5)) { SelectedIndex = 4; }
        if (Aditions.IsKeyPressedInBuildTime(Key.Key6)) { SelectedIndex = 5; }
        if (Aditions.IsKeyPressedInBuildTime(Key.Key7)) { SelectedIndex = 6; }
        if (Aditions.IsKeyPressedInBuildTime(Key.Key8)) { SelectedIndex = 7; }
        if (Aditions.IsKeyPressedInBuildTime(Key.Key9)) { SelectedIndex = 8; }
        if (Aditions.IsKeyPressedInBuildTime(Key.Key0)) { SelectedIndex = 9; }
    }


}
