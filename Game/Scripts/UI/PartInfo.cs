using Game.Scripts.SpaceShip;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

public partial class PartInfo : HBoxContainer
{

    public enum PartInfoFields
    {
        Name,
        Position,
        Power,
        PositiveKey,
        NegativeKey,
        Tag
    }
    
    // Called when the node enters the scene tree for the first time.
    private readonly List<Key> ValidInputKeys = new List<Key>()
    {
        Key.Up,Key.Down, Key.Left, Key.Right,
        Key.Kp0, Key.Kp1, Key.Kp2, Key.Kp3, Key.Kp4, Key.Kp5, Key.Kp6, Key.Kp7, Key.Kp8, Key.Kp9,
        Key.Key0, Key.Key1, Key.Key2, Key.Key3, Key.Key4, Key.Key5, Key.Key6, Key.Key7, Key.Key8, Key.Key9,
        Key.A, Key.B, Key.C, Key.D, Key.E, Key.F, Key.G, Key.H, Key.I, Key.J, Key.K, Key.L, Key.M, Key.N, Key.O, Key.P, Key.Q, Key.R, Key.S, Key.T, Key.U, Key.V, Key.W, Key.X, Key.Y, Key.Z,
        Key.Space, Key.Shift,
        Key.Plus, Key.Minus
    };
    
    private bool HodlingRightButton { get; set; }
    private bool HodlingLeftButton { get; set; }


    private IShipPart _selectedShipPart;

    public IShipPart SelectedShipPart 
	{
		get { return _selectedShipPart; } 
		set { _selectedShipPart = value; SetValues(); HideUnhide(); WorldUI.BuilderInterface.SelectedShip = value.ParentPart; } 
	}
    public bool Editing { get { return PositiveKey.ButtonPressed || NegativeKey.ButtonPressed || Tag.HasFocus() || Power.HasFocus(); } }
    public bool HoveringOver { get; private set; }

	private Button Name;
    private Button NameLabel;

    private Button Position;
    private Button PositionLabel;

    private HSlider Power;
    private Button  PowerString;
    private Button  PowerLabel;

    private Button PositiveKey;
    private Button PositiveKeyLabel;

    private Button NegativeKey;
    private Button NegativeKeyLabel;

    private LineEdit Tag;
    private Button	TagLabel;

    private PanelContainer labelsPanelContainer;
    private PanelContainer valuesPanelContainer;

    private PanelContainer labelsContentContainer;
    private PanelContainer valuesContentContainer;

	private VBoxContainer labelsContainer;
    private VBoxContainer valuesContainer;

    public PartInfo() : base()
    {

    }

    public override void _Ready()
	{
		GetContainers();
		GetValueComponents();
        GetLabelComponents();
        SetUpMouseVisibility();
		SetUpEvents();
        HideUnhide();
    }
	private void GetContainers()
	{
        this.labelsPanelContainer   = this.FindChild("LabelsContainer") as PanelContainer;
        this.labelsContentContainer = this.labelsPanelContainer.FindChild("ContentContainer") as PanelContainer;
        this.labelsContainer        = this.labelsContentContainer.FindChild("Labels") as VBoxContainer;

        this.valuesPanelContainer   = this.FindChild("ValuesContainer") as PanelContainer;
        this.valuesContentContainer = this.valuesPanelContainer.FindChild("ContentContainer") as PanelContainer;
        this.valuesContainer        = this.valuesContentContainer.FindChild("Values") as VBoxContainer;
    }
    private void GetLabelComponents()
    {
        NameLabel           = this.labelsContainer.FindChild(nameof(Name)) as Button;
        PositionLabel       = this.labelsContainer.FindChild(nameof(Position)) as Button;
        PowerLabel          = this.labelsContainer.FindChild(nameof(Power)) as Button;
        PositiveKeyLabel    = this.labelsContainer.FindChild(nameof(PositiveKey)) as Button;
        NegativeKeyLabel    = this.labelsContainer.FindChild(nameof(NegativeKey)) as Button;
        TagLabel            = this.labelsContainer.FindChild(nameof(Tag)) as Button;
    }
	private void GetValueComponents()
	{
		Name        = this.valuesContainer.FindChild(nameof(Name)) as Button;
        Position    = this.valuesContainer.FindChild(nameof(Position)) as Button;
        Power       = this.valuesContainer.FindChild(nameof(Power)) as HSlider;
        PowerString = this.Power.FindChild("Value") as Button;
        PositiveKey = this.valuesContainer.FindChild(nameof(PositiveKey)) as Button;
        NegativeKey = this.valuesContainer.FindChild(nameof(NegativeKey)) as Button;
        Tag         = this.valuesContainer.FindChild(nameof(Tag)) as LineEdit;
    }
    private void SetUpMouseVisibility()
    {
        this.labelsPanelContainer  .MouseFilter = MouseFilterEnum.Pass;
        this.labelsContentContainer.MouseFilter = MouseFilterEnum.Pass;
        this.labelsContainer       .MouseFilter = MouseFilterEnum.Pass;

        this.valuesPanelContainer  .MouseFilter = MouseFilterEnum.Pass;
        this.valuesContentContainer.MouseFilter = MouseFilterEnum.Pass;
        this.valuesContainer       .MouseFilter = MouseFilterEnum.Pass;

        this.NameLabel       .MouseFilter = MouseFilterEnum.Pass;
        this.PositionLabel   .MouseFilter = MouseFilterEnum.Pass;
        this.PowerLabel      .MouseFilter = MouseFilterEnum.Pass;
        this.PositiveKeyLabel.MouseFilter = MouseFilterEnum.Pass;
        this.NegativeKeyLabel.MouseFilter = MouseFilterEnum.Pass;
        this.TagLabel        .MouseFilter = MouseFilterEnum.Pass;

        this.Name       .MouseFilter = MouseFilterEnum.Pass;
        this.Position   .MouseFilter = MouseFilterEnum.Pass;
        this.Power      .MouseFilter = MouseFilterEnum.Pass;
        this.PowerString.MouseFilter = MouseFilterEnum.Pass;
        this.PositiveKey.MouseFilter = MouseFilterEnum.Pass;
        this.NegativeKey.MouseFilter = MouseFilterEnum.Pass;
        this.Tag        .MouseFilter = MouseFilterEnum.Pass; 

    }
	private void SetUpEvents()
	{
		
        Power.ValueChanged      += (value) => { PowerString.Text = (int)(value * 100f) + "%"; SelectedShipPart.PowerMultiplier = (float)value; };
		PositiveKey.ButtonDown  += () => { PositiveKey.Text = "<_>"; };
        NegativeKey.ButtonDown  += () => { NegativeKey.Text = "<_>"; };
        Power.MouseExited       += () => { Power.ReleaseFocus(); };
        Tag.MouseExited         += () => { Tag.ReleaseFocus(); };

        this.MouseEntered += () => { this.HoveringOver = true; };
        this.MouseExited += () => { this.HoveringOver = false; };
    }
    

	private void SetValues()
	{
		Name.Text = SelectedShipPart.GetType().Name;
        Position.Text = string.Format("{0,3}:{1,3}", SelectedShipPart.LocalPosition.X, SelectedShipPart.LocalPosition.Z);
        Power.Value = SelectedShipPart.PowerMultiplier;
        PositiveKey.Text = SelectedShipPart.PKey.ToString();
		NegativeKey.Text = SelectedShipPart.NKey.ToString();
		Tag.Text = "";
    }
    private void HideUnhide()
    {
        if (SelectedShipPart is null) { this.Visible = false; return; }
        this.Visible = true;
        SetVisibility(PartInfoFields.Name       , NameLabel       , Name);
        SetVisibility(PartInfoFields.Position   , PositionLabel   , Position);
        SetVisibility(PartInfoFields.Power      , PowerLabel      , Power, PowerString);
        SetVisibility(PartInfoFields.PositiveKey, PositiveKeyLabel, PositiveKey);
        SetVisibility(PartInfoFields.NegativeKey, NegativeKeyLabel, NegativeKey);
        SetVisibility(PartInfoFields.Tag        , TagLabel        , Tag);

        PositiveKeyLabel.Text = NegativeKey.Visible ? "PositiveKey" : "Key";
    }
    private void SetVisibility(PartInfoFields infoFields, Control label, Control value, params Control[] additions)
    {
        if (!SelectedShipPart.HideList.Any(o => o.ToString() == label.Name))
        {
            label.Visible = true;
            value.Visible = true;
            foreach (var control in additions) control.Visible = true;
        }
        else
        {
            label.Visible = false;
            value.Visible = false;
            foreach (var control in additions) control.Visible = false;
        }
    }



    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
        if (SelectedShipPart is null) return;
        if (PositiveKey.Text == "<>") SetTriggerKey(PositiveKey, (Key key) => SelectedShipPart.PKey = key);
        if (NegativeKey.Text == "<>") SetTriggerKey(NegativeKey, (Key key) => SelectedShipPart.NKey = key);
        if (PositiveKey.Text == "<_>" && Input.IsAnythingPressed()) { PositiveKey.Text = "<>"; }
        if (NegativeKey.Text == "<_>" && Input.IsAnythingPressed()) { NegativeKey.Text = "<>"; }b

        if (Input.IsKeyPressed(Key.E) && !HodlingRightButton)
        {
            HodlingRightButton = true;
            if(WorldUI.BuilderInterface.Building) SelectedShipPart.RotateRight();
        }
        if (!Input.IsKeyPressed(Key.E)) { HodlingRightButton = false; }

        if (Input.IsKeyPressed(Key.Q) && HodlingLeftButton)
        {
            HodlingLeftButton = true;
            if (WorldUI.BuilderInterface.Building) SelectedShipPart.RotateLeft();
        }
        if (!Input.IsKeyPressed(Key.Q)) { HodlingLeftButton = false; }


    }

    private void SetTriggerKey(Button triggrtButton, Action<Key> action)
    {
        if (!Input.IsAnythingPressed()) return;
        foreach (Key key in ValidInputKeys)
        {
            if (Input.IsKeyPressed(key))
            {
                triggrtButton.Text = key.ToString();
                action.Invoke(key);
                break;
            }
        }
        triggrtButton.ReleaseFocus();
        triggrtButton.ButtonPressed = false;
    }
}
