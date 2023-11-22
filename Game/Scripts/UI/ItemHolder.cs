using Game.Scripts.SpaceShip;
using Godot;
using System;

public partial class ItemHolder : PanelContainer
{
	// Called when the node enters the scene tree for the first time.
    private TextureRect Image { get { if (_image == null) _image = this.FindChild(nameof(Image)) as TextureRect; return _image; } }
	private TextureRect _image;
    private Label Count { get { if (_count == null) _count = this.FindChild(nameof(Count)) as Label; return _count; } }
    private Label _count;

    private IShipPart _shipPart;
    public IShipPart ShipPart 
    { 
        get 
        { 
            return _shipPart; 
        } 
        set 
        {
            _shipPart = value;
            if (_shipPart == null) return;
            Image.Texture = GD.Load<Texture2D>(_shipPart.InventoryImagePath);
        } 
    }

    public override void _Ready()
	{
        this.MinimumSizeChanged += () =>
        {
            Image.CustomMinimumSize = new Vector2(this.CustomMinimumSize.X - 10f, this.CustomMinimumSize.Y - 10f);
        };
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
