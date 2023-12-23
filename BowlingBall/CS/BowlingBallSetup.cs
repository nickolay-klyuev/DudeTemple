using Godot;
using System;

public partial class BowlingBallSetup : Node
{
    [Signal]
    public delegate void BallSetupIsDirtyEventHandler(int dirtyProperty);

    private Color _ballColor;
    private bool _bIsGlowing = true;
    private float _glowingStrength;

    public Color BallColor
    {
        get { return _ballColor; }
        set 
        {
            _ballColor = value;
            MarkDirty(EBallSetupProperty.Color);
        }
    }

    public bool IsGlowing
    {
        get { return _bIsGlowing; }
        set
        {
            _bIsGlowing = value;
            MarkDirty(EBallSetupProperty.IsGlowing);
        }
    }

    public float GlowingStrength
    {
        get { return _glowingStrength; }
        set
        {
            _glowingStrength = value;
            MarkDirty(EBallSetupProperty.GlowingStrength);
        }
    }

    private void MarkDirty(EBallSetupProperty property)
    {
        EmitSignal(SignalName.BallSetupIsDirty, (int)property);
    }
}
