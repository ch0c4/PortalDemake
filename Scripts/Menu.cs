using Godot;
using System;

public class Menu : Control
{
  private Resource _CursorMenu = GD.Load("res://Assets/cursor_menu.png");

  private Vector2 _PositionNG = new Vector2(76, 65);

  private Vector2 _PositionOptions = new Vector2(76, 90);

  private Sprite _Arrow;

  private bool _OnNG = true;

  private AudioStreamPlayer2D _MenuSelectSound;

  private Global _Global;

  public override void _Ready()
  {
	Input.SetCustomMouseCursor(_CursorMenu);

	_Arrow = GetNode<Sprite>("Menu/CenterRow/Arrows/Arrow");

	_MenuSelectSound = GetNode<AudioStreamPlayer2D>("MenuSelect");

	_Global = (Global)GetNode("/root/Global");
	_Global.StopMainThemeMusic();
  }

  public override void _Process(float delta)
  {
	if (Input.IsActionJustPressed("ui_down"))
	{
	  _MenuSelectSound.Play();
	  _Arrow.Position = _PositionOptions;
	  _OnNG = false;
	}

	if (Input.IsActionJustPressed("ui_up"))
	{
	  _MenuSelectSound.Play();
	  _Arrow.Position = _PositionNG;
	  _OnNG = true;
	}

	if (Input.IsActionJustPressed("ui_select"))
	{

	  if (_OnNG)
	  {
		_Global.GoToTutorial();
	  }

	  if (!_OnNG)
	  {
		_Global.GoToCredits();
	  }
	}
  }


  private void _on_NewGame_mouse_entered()
  {
	_Arrow.Position = _PositionNG;
	_OnNG = true;
	_MenuSelectSound.Play();
  }


  private void _on_OptionsButton_mouse_entered()
  {
	_Arrow.Position = _PositionOptions;
	_OnNG = false;
	_MenuSelectSound.Play();
  }


  private void _on_NewGame_pressed()
  {
	_Global.GoToTutorial();
  }


  private void _on_OptionsButton_pressed()
  {
	_Global.GoToCredits();
  }



}
