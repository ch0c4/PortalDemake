using Godot;
using System;

public class History
{
  public Vector2[] tilePosition;

  public String direction;

  public bool active = false;

  public History(Vector2[] tilePosition, String direction)
  {
    this.tilePosition = tilePosition;
    this.direction = direction;
  }

  public void setAll(Vector2[] tilePosition, String direction)
  {
    this.tilePosition = tilePosition;
    this.direction = direction;
    this.active = true;
  }
}
