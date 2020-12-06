using Godot;
using System;

public class Credits : Control
{

  private Resource _CursorMenu = GD.Load("res://Assets/cursor_menu.png");

  public override void _Ready()
  {
	Input.SetCustomMouseCursor(_CursorMenu);
	var global = (Global)GetNode("/root/Global");
	global.StopMainThemeMusic();
  }

  public override void _Process(float delta)
  {
	if (Input.IsActionJustPressed("ui_cancel"))
	{
	  var global = (Global)GetNode("/root/Global");
	  global.GoToMenu();
	}
  }
}
