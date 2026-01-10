using System.Runtime.Serialization;
using Godot;

public partial class RebindKeyButton : Button
{
    [Signal]
    public delegate void RebindKeyButtonPressedEventHandler(string actionName, Button buttonPressed);

    [Export]
    private string _actionName = "";

    public override void _Ready()
    {
        Pressed += EmitRebindButtonPressedEvent;
    }

    private void EmitRebindButtonPressedEvent()
    {
        EmitSignalRebindKeyButtonPressed(_actionName, this);
    }
}
