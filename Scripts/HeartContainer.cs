using Godot;
using System;

public class HeartContainer : Node2D
{
  public Stat _Stat;

  public Sprite _Heart1;

  public Sprite _Heart2;

  public Sprite _Heart3;

  public Texture _HeartEmpty = (Texture)GD.Load("res://Assets/heart_empty.png");

  public Texture _HeartFull = (Texture)GD.Load("res://Assets/heart_full.png");

  public override void _Ready()
  {
	_Stat = (Stat)GetNode("/root/Stat");
	_Heart1 = GetNode<Sprite>("Heart1");
	_Heart2 = GetNode<Sprite>("Heart2");
	_Heart3 = GetNode<Sprite>("Heart3");
	_CheckLife();

  }

  public override void _Process(float delta)
  {
	_CheckLife();

  }

  private void _CheckLife()
  {
	if (_Stat.Life == 3)
	{
	  _Heart1.Texture = _HeartFull;
	  _Heart2.Texture = _HeartFull;
	  _Heart3.Texture = _HeartFull;
	}

	if (_Stat.Life == 2)
	{
	  _Heart1.Texture = _HeartFull;
	  _Heart2.Texture = _HeartFull;
	  _Heart3.Texture = _HeartEmpty;
	}

	if (_Stat.Life == 1)
	{
	  _Heart1.Texture = _HeartFull;
	  _Heart2.Texture = _HeartEmpty;
	  _Heart3.Texture = _HeartEmpty;
	}
  }

}
