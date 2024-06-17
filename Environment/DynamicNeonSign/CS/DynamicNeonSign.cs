using Godot;
using System;

public partial class DynamicNeonSign : Node3D
{
	[Export] private bool _bEnableBlink = true;
	
	[Export] private Timer _blinkTimer;
	[Export] private MeshInstance3D _frontMesh;

	private RandomNumberGenerator _rand = new RandomNumberGenerator();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (!_bEnableBlink)
		{
			_blinkTimer.Stop();
			
			return;
		}
		
#if DEBUG
		CheckHelper.Check(this, _blinkTimer, _frontMesh);
#endif

		_blinkTimer.Timeout += Blink;
	}

	private void Blink()
	{
		_blinkTimer.WaitTime = _rand.RandfRange(3.0f, 10.0f);

		Material activeMat = _frontMesh.GetActiveMaterial(0);
		Variant cachedEmission = activeMat.Get("emission_energy_multiplier");
		activeMat.Set("emission_energy_multiplier", 0.0f);
		
		GetTree().CreateTimer(0.05f).Timeout += () => { activeMat.Set("emission_energy_multiplier", cachedEmission); };
	}
}
