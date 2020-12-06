using Godot;
using System;

public class Bullet_Enemy : KinematicBody2D
{

  [Export]
  public int Speed = 200;

  private Vector2 _Velocity = new Vector2();

  public void Start(Vector2 pos, float dir)
  {
	Position = pos;
	_Velocity = new Vector2(0, Speed).Rotated(Rotation);
  }

  public override void _PhysicsProcess(float delta)
  {
	var collision = MoveAndCollide(_Velocity * delta);
	if (collision != null)
	{
	  if (collision.Collider.GetType().Name == "Player")
	  {
		var stat = (Stat)GetNode("/root/Stat");
		stat.RemoveLife();
		if (stat.Life == 0)
		{
		  var global = (Global)GetNode("/root/Global");
		  global.GoToCredits();
		}
	  }
	  QueueFree();
	}
  }
}
