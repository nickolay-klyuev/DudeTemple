using Godot;
using Godot.Collections;
using System;

public partial class DudeController : CharacterBody3D
{
	[Export]
	private float _lookAroundSensitivity = 1.0f;

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

	public const float JumpVelocity = 4.5f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	private bool _bIsDisabled = false;

	private CollisionShape3D _dudeCollision;

	private bool _bIsHoldingThing = false;
	private Node3D _holdingThing;

	public override void _Ready()
	{
		#if DEBUG
		CheckHelper.Check(this, _dudeFace, _dudeHand, _aimRaycast, _grabSocket, _interactLabel);
		#endif

		_dudeCollision = GetChild<CollisionShape3D>(0);
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
			var TurnModifier = 0.01f * _lookAroundSensitivity;

			RotateY(-mouseMotion.Relative.X * TurnModifier);
			_dudeFace.RotateX(-mouseMotion.Relative.Y * TurnModifier);
		}
	}

	private void InteractProcess() // TODO: Try to get rid of this process. 
	{
		if (_dudeHand.IsColliding())
		{
			if (!_interactLabel.Visible)
			{
				_interactLabel.Visible = true;
			}

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

		thing.GlobalPosition = _grabSocket.GlobalPosition;
		_holdingThing = thing;
		_bIsHoldingThing = true;
	}

	private void HoldingThingProcess() // TODO: maybe it's possible to rid off this process?
	{
		if (_bIsHoldingThing && _holdingThing != null)
		{
			_holdingThing.GlobalPosition = _grabSocket.GlobalPosition;

			if (Input.IsActionJustPressed("MainAction"))
			{
				ReleaseThing();
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

		((IGrabbable)_holdingThing).ThrowToDirection(throwDirection, 90.0f);// TODO: Make force dynamic.
		_holdingThing = null;
	}
}
