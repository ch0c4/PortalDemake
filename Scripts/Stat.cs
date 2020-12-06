using Godot;
using System;

public class Stat : Node
{
  public int Life = 3;

  public void RemoveLife()
  {
    Life--;
  }
}
