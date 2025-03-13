using Godot;
using Godot.Collections;
using System;

public partial class DudeController : CharacterBody3D
{
	private const string INTERACT_TEXT = "press E to";

	[Export]
	private Camera3D _dudeFace;

	[Export]
	private RayCast3D _dudeHand;

	[Export]
	private RayCast3D _aimRaycast;

	[Export]
	private Node3D _grabSocket;

	[Export]
	private float _speed = 5.0f;

	[Export]
	private float _sprintSpeed = 7.0f;

	[Export]
	private Label _interactLabel;

	private bool _bInteractLabelVisibilityCache;

	[Export]
	private float _addingForceMod = 100.0f;

	[Export]
	private AudioStreamPlayer3D _stepsPlayer;

	[Export] private float _stepLength = 200.0f;
	private float _currentStepLength = 0.0f;

	[ExportCategory("Throwing")]
	[Export]
	private Vector2 _throwForce = new Vector2(25.0f, 150.0f); // X - Min, Y - Max

	[Export] private Node3D _throwMaxForceLoc;

	[Export]
	private ThrowForce _throwForceBar;

	private bool _bThrowForceBarVisibilityCache;

	[Export] private Control _crossHair;

	private bool _bIsAddingForce = false;

	private float _currentForce = 0.0f;

	public const float JumpVelocity = 4.5f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	private bool _bIsDisabled = false;

	private CollisionShape3D _dudeCollision;

	private bool _bIsHoldingThing = false;
	private Node3D _holdingThing;
	private float _currentForcePercent = 0.0f;

	private DudeSettings _settings;

	private float _cameraSpeed = 1.0f;

	public override void _Ready()
	{
		_crossHair ??= GetNode<Control>("CenterPoint");
		_throwMaxForceLoc ??= GetNode<Node3D>("ThrowMaxForceLoc");
		
		_settings = GetTree().Root.GetNode<DudeSettings>("DudeSettings");

		UpdateCameraSpeed();
		_settings.SettingsUpdated += UpdateCameraSpeed;

		_stepsPlayer ??= GetNode<AudioStreamPlayer3D>("StepsAudioPlayer");

#if DEBUG
		CheckHelper.Check(this, _dudeFace, _dudeHand, _aimRaycast, _grabSocket, _interactLabel, 
			_throwForceBar, _crossHair, _throwMaxForceLoc, _stepsPlayer);
#endif

		_throwForceBar.Visible = false;
		_currentForce = _throwForce.X;
		
		_dudeCollision = GetChild<CollisionShape3D>(0);

		_bInteractLabelVisibilityCache = _interactLabel.Visible;
		_bThrowForceBarVisibilityCache = _throwForceBar.Visible;
	}

    public override void _Process(double delta)
    {
        base._Process(delta);
		
        // Adding force to throw
		if (_bIsAddingForce && _currentForce < _throwForce.Y)
		{
			_currentForce += (float)delta * _addingForceMod;

			_currentForcePercent = (_currentForce - _throwForce.X) / (_throwForce.Y - _throwForce.X);
			_throwForceBar.SetForcePercentage(_currentForcePercent);
		}
    }

    public override void _PhysicsProcess(double delta)
	{
		if (_bIsDisabled)
		{
			return;
		}

		// Sprint check
		float currentSpeed;
		if (Input.IsActionPressed("Sprint"))
		{
			currentSpeed = _sprintSpeed;
		}
		else
		{
			currentSpeed = _speed;
		}

		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity.Y -= gravity * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		Vector2 inputDir = Input.GetVector("MoveLeft", "MoveRight", "MoveForward", "MoveBackward");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * currentSpeed;
			velocity.Z = direction.Z * currentSpeed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, currentSpeed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, currentSpeed);
		}

		if (IsOnFloor())
		{
			_currentStepLength += velocity.Length();
			
			if (_currentStepLength >= _stepLength)
			{
				_currentStepLength = 0.0f;
				_stepsPlayer.Stop();
				_stepsPlayer.Play();
			}
		}

		Velocity = velocity;
		MoveAndSlide();

		InteractProcess();
		HoldingThingProcess();
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (_bIsDisabled)
		{
			return;
		}

		// Turn dude around by mouse X axis
		if (@event is InputEventMouseMotion mouseMotion)
		{
			RotateCamera(mouseMotion.Relative.X, mouseMotion.Relative.Y);
		}
	}

	public void RotateCamera(float x, float y)
	{
		var TurnModifier = 0.01f * _cameraSpeed;

		RotateY(-x * TurnModifier);
		_dudeFace.RotateX(-y * TurnModifier);
	}

	private void InteractProcess()
	{
		if (!_bIsHoldingThing && _dudeHand.IsColliding()) // Interact only when hands are empty (_bIsHoldingThing == false)
		{
			IInteractable interactableObject = _dudeHand.GetCollider() as IInteractable;
			if (interactableObject != null && Input.IsActionJustPressed("Interact"))
			{
				interactableObject.Interact();
			}

			IGrabbable grabbableThing = _dudeHand.GetCollider() as IGrabbable;
			if (grabbableThing != null && Input.IsActionJustPressed("Interact"))
			{
				grabbableThing.Grab();
				HoldThing(grabbableThing as Node3D);
			}

			if (!_interactLabel.Visible)
			{
				if (interactableObject != null)
				{
					_interactLabel.Text = $"{INTERACT_TEXT} {interactableObject.GetInteractLabel()}";
				}
				else
				{
					_interactLabel.Text = $"{INTERACT_TEXT} grab";
				}

				_interactLabel.Visible = true;
			}
		}
		else
		{
			if (_interactLabel.Visible)
			{
				_interactLabel.Visible = false;
			}
		}
	}

	private void HoldThing(Node3D thing)
	{
		if (thing == null)
		{
			#if DEBUG
			GD.PrintErr($"{Name}: you are trying to hold a thing that doesn't exist!!!");
			#endif

			return;
		}

		Tween grabTween = CreateTween();
		grabTween.TweenProperty(thing, "position", _grabSocket.GlobalPosition, 0.1f);
		grabTween.Finished += () =>
		{
			_holdingThing = thing;
			_bIsHoldingThing = true;
		};
	}

	private void HoldingThingProcess() // TODO: maybe it's possible to rid off this process?
	{
		if (_bIsHoldingThing && _holdingThing != null)
		{
			// Update holding ball location
			_holdingThing.GlobalPosition = _grabSocket.GlobalPosition.Lerp(_throwMaxForceLoc.GlobalPosition, _currentForcePercent);
			
			if (Input.IsActionJustPressed("MainAction"))
			{
				_bIsAddingForce = true;
				_throwForceBar.Visible = true;
			}
			else if (Input.IsActionJustReleased("MainAction"))
			{
				ReleaseThing();
				_currentForcePercent = 0.0f;
			}
		}
	}

	private void ReleaseThing()
	{
		_bIsHoldingThing = false;
		
		Vector3 throwDirection;
		if (_aimRaycast.IsColliding())
		{

			throwDirection = _aimRaycast.GetCollisionPoint() - _grabSocket.GlobalPosition;
		}
		else
		{
			throwDirection = _aimRaycast.ToGlobal(_aimRaycast.TargetPosition) - _grabSocket.GlobalPosition;
		}

		((IGrabbable)_holdingThing).ThrowToDirection(throwDirection, _currentForce);

		_throwForceBar.Visible = false;
		_holdingThing = null;
		_bIsAddingForce = false;
		_currentForce = _throwForce.X;
		_throwForceBar.Value = 0.0f;
	}

	private void UpdateCameraSpeed()
	{
		_cameraSpeed = _settings.GetCameraSpeed();
	}

	private void OnPauseMenuOpened()
	{
		_bInteractLabelVisibilityCache = _interactLabel.Visible;
		_bThrowForceBarVisibilityCache = _throwForceBar.Visible;
		
		_throwForceBar.Visible = false;
		_interactLabel.Visible = false;
		_crossHair.Visible = false;
	}

	private void OnPauseMenuClosed()
	{
		_throwForceBar.Visible = _bThrowForceBarVisibilityCache;
		_interactLabel.Visible = _bInteractLabelVisibilityCache;
		
		_crossHair.Visible = true;
	}
}
