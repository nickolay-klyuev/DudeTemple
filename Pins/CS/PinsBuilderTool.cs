using Godot;
using Godot.NativeInterop;
using System;

[Tool]
public partial class PinsBuilderTool : Node3D
{
	private PackedScene _pinScene;

	[Export]
	PackedScene PinScene
	{ 
		get
		{
			return _pinScene;
		}
		set
		{
			Clear();

			_pinScene = value;

			Build();
		}
		
	}

	private Vector3[] _pinsLocations = new Vector3[10]
	{ 
		new Vector3(0.389f, 0.0f, -0.486f),
		new Vector3(0.383f, 0.0f, -0.165f),
		new Vector3(0.389f, 0.0f, 0.142f),
		new Vector3(0.387f, 0.0f, 0.483f),
		new Vector3(0.225f, 0.0f, -0.355f),
		new Vector3(0.225f, 0.0f, 0.0f),
		new Vector3(0.243f, 0.0f, 0.324f),
		new Vector3(0.111f, 0.0f, -0.189f),
		new Vector3(0.113f, 0.0f, 0.196f),
		new Vector3(0.0f, 0.0f, 0.0f)
	};

	private void Clear()
	{
		foreach (Node3D child in GetChildren())
		{
			child.QueueFree();
		}
	}

	private void Build()
	{
		if (_pinScene == null)
		{
			return;
		}

		foreach (Vector3 loc in _pinsLocations)
		{
			Node3D pin = _pinScene.Instantiate<Node3D>();
			AddChild(pin);
			pin.Position = loc;
		}
	}
}
