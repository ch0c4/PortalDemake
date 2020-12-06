using Godot;
using System;

public class Enemy : KinematicBody2D
{

  private PackedScene _Bullet = (PackedScene)GD.Load("res://Prefabs/Bullet_Enemy.tscn");

  private void _Shoot()
  {
	var pew = (Bullet_Enemy)_Bullet.Instance();
	GetTree().Root.AddChild(pew);
	var PositionMuzzle = GetNode<Position2D>("Muzzle").GlobalPosition;

	pew.Start(PositionMuzzle, Rotation);
  }

  private void _on_DetectZone_body_entered(object body)
  {
	if (body.GetType().Name == "Player")
	{
	  _Shoot();

	}
  }

}



