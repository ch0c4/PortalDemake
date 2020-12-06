using Godot;
using System;

public class Options : Control
{

  public override void _Ready()
  {
    var global = (Global)GetNode("/root/Global");
    global.StopMainThemeMusic();
  }


}
