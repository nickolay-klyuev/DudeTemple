using Godot;
using System;

public partial class DudeController : CharacterBody3D
{
	[Signal]
	public delegate void OnInteractProcessChangedEventHandler(bool bIsInteracting);

	[Export]
	private float _lookAroundSensitivity = 1.0f;

	[Export]
	Camera3D DudeFace;

	[Export]
	RayCast3D DudeHand;

	[Export]
	private float _speed = 5.0f;

	[Export]
	private float _sprintSpeed = 7.0f;

	public const float JumpVelocity = 4.5f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	private bool _bIsDisabled = false;

	private CollisionShape3D _dudeCollision;

	public override void _Ready()
	{
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
			DudeFace.RotateX(-mouseMotion.Relative.Y * TurnModifier);
		}
	}

	private void InteractProcess() // TODO: Try to get rid of this process. 
	{
		if (DudeHand.IsColliding())
		{
			EmitSignal(SignalName.OnInteractProcessChanged, true);

			IInteractable interactableObject = DudeHand.GetCollider() as IInteractable;
			if (interactableObject != null && Input.IsActionJustPressed("Interact"))
			{
				interactableObject.Interact();
			}
		}
		else
		{
			EmitSignal(SignalName.OnInteractProcessChanged, false);
		}
	}

	// Change Dude's behave for different game modes
	public void OnGameModeChanged(TempleState.GameMode gameMode)
	{
		if (gameMode == TempleState.GameMode.Walking)
		{
			SetDisabled(false);
		}
		else if (gameMode == TempleState.GameMode.RollingBalls)
		{
			EmitSignal(SignalName.OnInteractProcessChanged, false); // TODO: need to refactor this one
			SetDisabled(true);
		}
	}

	private void SetDisabled(bool bIsDisabled)
	{
		_bIsDisabled = bIsDisabled;
		_dudeCollision.Disabled = _bIsDisabled;
	}
}
