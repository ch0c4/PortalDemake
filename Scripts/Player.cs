using Godot;
using System;

public class Player : KinematicBody2D
{

  [Export]
  public int Speed = 100;

  [Signal]
  public delegate void LiftCube();

  [Signal]
  public delegate void DropCube(Vector2 position);

  private bool _IsTouchingCube = false;

  private bool _IsLiftingCube = false;

  private PackedScene _Bullet = (PackedScene)GD.Load("res://Prefabs/Bullet.tscn");

  private Vector2 _Velocity = new Vector2();

  private PackedScene _Cube = (PackedScene)GD.Load("res://Prefabs/Cube.tscn");

  private AudioStreamPlayer2D _AttackSound;

  private Global _Global;

  public override void _Ready()
  {
	Rotate(-(Mathf.Pi / 2));
	_Global = (Global)GetNode("/root/Global");
	_AttackSound = GetNode<AudioStreamPlayer2D>("AttackSound");
  }

  public override void _Process(float delta)
  {
	LookAt(GetGlobalMousePosition());
  }

  public override void _PhysicsProcess(float delta)
  {
	_GetInput();
	var dir = GetGlobalMousePosition() - GlobalPosition;
	if (dir.Length() > 5)
	{
	  Rotation = dir.Angle();
	}
	_Velocity = MoveAndSlide(_Velocity * Speed);

	for (int i = 0; i < GetSlideCount(); i++)
	{
	  var collision = GetSlideCollision(i);
	  if (collision.Collider.HasMethod("HitPlayer"))
	  {
		collision.Collider.Call("HitPlayer", collision);
	  }
	}
  }

  private void _on_CubeContainer_body_entered(object body)
  {
	_IsTouchingCube = true;
  }

  private void _on_CubeContainer_body_exited(object body)
  {
	_IsTouchingCube = false;
  }

  private void _GetInput()
  {
	_Velocity = new Vector2();

	if (Input.IsActionPressed("ui_up"))
	{
	  _Velocity.y -= 1;
	}

	if (Input.IsActionPressed("ui_down"))
	{
	  _Velocity.y += 1;
	}

	if (Input.IsActionPressed("ui_left"))
	{
	  _Velocity.x -= 1;
	}

	if (Input.IsActionPressed("ui_right"))
	{
	  _Velocity.x += 1;
	}

	if (Input.IsActionJustPressed("LMB") && !_IsTouchingCube && !_IsLiftingCube)
	{
	  _Shoot("LMB");
	  _AttackSound.Play();
	}

	if (Input.IsActionJustPressed("RMB") && !_IsTouchingCube && !_IsLiftingCube)
	{
	  _Shoot("RMB");
	  _AttackSound.Play();
	}

	if (Input.IsActionJustPressed("LMB") && _IsTouchingCube && !_IsLiftingCube)
	{
	  _LiftCube();
	}
	else if (Input.IsActionJustPressed("RMB") && _IsTouchingCube && !_IsLiftingCube)
	{
	  _LiftCube();
	}
	else if (Input.IsActionJustPressed("LMB") && _IsLiftingCube)
	{
	  _DropCube();
	}
	else if (Input.IsActionJustPressed("RMB") && _IsLiftingCube)
	{
	  _DropCube();
	}
  }

  private void _Shoot(String mouse)
  {
	var pew = (Bullet)_Bullet.Instance();
	pew.Start(GetNode<Position2D>("Muzzle").GlobalPosition, Rotation, mouse);
	GetParent().AddChild(pew);
  }

  private void _LiftCube()
  {
	EmitSignal(nameof(LiftCube));
	RigidBody2D cube = (RigidBody2D)_Cube.Instance();
	GetNode<Position2D>("CubeContainer").AddChild(cube);
	_IsLiftingCube = true;
  }

  private void _DropCube()
  {
	_IsLiftingCube = false;
	Position2D cubeContainer = GetNode<Position2D>("CubeContainer");
	Vector2 cubePosition = cubeContainer.GlobalPosition;
	cubePosition.x -= 5;
	cubePosition.y -= 5;
	EmitSignal(nameof(DropCube), cubePosition);
	cubeContainer.GetNode<RigidBody2D>("Cube").QueueFree();
  }
}
