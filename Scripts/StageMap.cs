using Godot;
using System;

public class StageMap : TileMap
{
  [Signal]
  public delegate void Collided(KinematicCollision2D collision2D, String mouse);

  [Signal]
  public delegate void CollidedPlayer(KinematicCollision2D collision2D);

  public void Hit(KinematicCollision2D collision, String mouse)
  {
    EmitSignal(nameof(Collided), collision, mouse);
  }

  public void HitPlayer(KinematicCollision2D collision)
  {
    EmitSignal(nameof(CollidedPlayer), collision);
  }
}
