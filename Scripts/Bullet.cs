using Godot;
using System;

public class Bullet : KinematicBody2D
{
  [Export]
  public int Speed = 750;

  private Vector2 _Velocity = new Vector2();

  private String _Mouse = "LMB";

  public void Start(Vector2 pos, float dir, String mouse)
  {
	_Mouse = mouse;
	Rotation = dir;
	Position = pos;
	_Velocity = new Vector2(Speed, 0).Rotated(Rotation);
  }

  public override void _PhysicsProcess(float delta)
  {
	var collision = MoveAndCollide(_Velocity * delta);
	if (collision != null)
	{
	  if (collision.Collider.HasMethod("Hit"))
	  {
		collision.Collider.Call("Hit", collision, _Mouse);
		QueueFree();
	  }

	  if (collision.Collider.GetType().Name == "Cube")
	  {
		QueueFree();
	  }

	  if (collision.Collider.GetType().Name == "CloseDoor")
	  {
		QueueFree();
	  }

	}
  }
}
