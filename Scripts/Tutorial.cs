using Godot;
using System;

public class Tutorial : Control
{

  private Resource _CursorMenu = GD.Load("res://Assets/cursor_menu.png");

  private Timer _Timer;

  public override void _Ready()
  {

	Input.SetCustomMouseCursor(_CursorMenu);

	_Timer = GetNode<Timer>("Timer");
  }

  private void _on_Timer_timeout()
  {
	var global = (Global)GetNode("/root/Global");
	global.GotoScene(global.ScenePathList[0]);
  }

  public override void _Process(float delta)
  {
	if (Input.IsActionJustPressed("LMB") || Input.IsActionJustPressed("RMB") || Input.IsActionJustPressed("ui_select"))
	{
	  _Timer.Stop();
	  var global = (Global)GetNode("/root/Global");
	  global.GotoScene(global.ScenePathList[0]);
	}
  }

}


