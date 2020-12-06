using Godot;
using System;

public class WinZone : Position2D
{

  [Export]
  public bool IsClose = true;

  private PackedScene _CloseDoor = (PackedScene)GD.Load("res://Prefabs/CloseDoor.tscn");

  private RigidBody2D _ClosedDoor;

  public override void _Ready()
  {
	if (IsClose)
	{
	  _ClosedDoor = (RigidBody2D)_CloseDoor.Instance();
	  AddChild(_ClosedDoor);
	}
  }

  public void OpenDoor()
  {
	if (!IsClose) return;

	IsClose = false;

	_ClosedDoor.QueueFree();
  }

  public void CloseTheDoor()
  {
	if (IsClose) return;

	IsClose = true;
	_ClosedDoor = (RigidBody2D)_CloseDoor.Instance();
	CallDeferred("add_child", _ClosedDoor);
  }
}
