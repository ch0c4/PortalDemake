using Godot;
using System;

public class Main : Node2D
{

  private Global _Global;

  private int _PortalUpTileId;

  private int _PortalDownTileId;

  private History _historyLMB = new History(new Vector2[] { Vector2.Zero, Vector2.Zero }, "up");
  private History _historyRMB = new History(new Vector2[] { Vector2.Zero, Vector2.Zero }, "up");

  private TileMap _StageMap;

  private Position2D _SpawnPosition;

  private KinematicBody2D _Player;

  private RigidBody2D _DroppedCube;

  private WinZone _WinZone;

  private AudioStreamPlayer2D _HitSound;

  private Stat _Stat;

  private bool _firstHitLMB = true;
  private bool _firstHitRMB = true;

  private Resource _Cursor = GD.Load("res://Assets/cursor.png");

  private bool AlreadyPlay = false;

  public override void _Ready()
  {
	Input.SetCustomMouseCursor(_Cursor);

	_StageMap = GetNode<TileMap>("StageMap");
	_PortalUpTileId = _StageMap.TileSet.FindTileByName("portal_up");
	_PortalDownTileId = _StageMap.TileSet.FindTileByName("portal_down");
	_SpawnPosition = GetNode<Position2D>("Spawn");
	_Player = GetNode<KinematicBody2D>("Player");
	_Player.Position = _SpawnPosition.Position;
	_WinZone = GetNode<WinZone>("WinZone");
	_HitSound = GetNode<AudioStreamPlayer2D>("HitSound");
	_Global = (Global)GetNode("/root/Global");
	_Stat = (Stat)GetNode("/root/Stat");
  }

  private void _on_StageMap_Collided(KinematicCollision2D collision, String mouse)
  {

	var tile = _StageMap.WorldToMap(collision.Position);
	Vector2[] goodTilePosition = _SetupTilePosition(tile);

	int[] badTiles = { -1, _PortalUpTileId, _PortalDownTileId };
	if (Global.IsIntInArray(badTiles, _StageMap.GetCellv(goodTilePosition[0])) ||
	Global.IsIntInArray(badTiles, _StageMap.GetCellv(goodTilePosition[1])))
	{
	  return;
	}

	if (!_firstHitLMB && mouse == "LMB")
	{
	  _Reset(_historyLMB);
	}
	else if (mouse == "LMB")
	{
	  _firstHitLMB = false;
	}

	if (!_firstHitRMB && mouse == "RMB")
	{
	  _Reset(_historyRMB);
	}
	else if (mouse == "RMB")
	{
	  _firstHitRMB = false;
	}

	String direction = "up";

	if (tile.y == 2) // up
	{
	  _StageMap.SetCellv(goodTilePosition[0], _PortalUpTileId, false, true, true);
	  _StageMap.SetCellv(goodTilePosition[1], _PortalDownTileId, false, true, true);
	  direction = "up";
	}
	else if (tile.x == 1) // left
	{
	  _StageMap.SetCellv(goodTilePosition[0], _PortalUpTileId);
	  _StageMap.SetCellv(goodTilePosition[1], _PortalDownTileId);
	  direction = "left";
	}
	else if (tile.y == 14) // down
	{
	  _StageMap.SetCellv(goodTilePosition[0], _PortalUpTileId, false, true, true);
	  _StageMap.SetCellv(goodTilePosition[1], _PortalDownTileId, false, true, true);
	  direction = "down";
	}
	else if (tile.x == 19) // right
	{
	  _StageMap.SetCellv(goodTilePosition[0], _PortalUpTileId);
	  _StageMap.SetCellv(goodTilePosition[1], _PortalDownTileId);
	  direction = "right";
	}
	else if (_StageMap.TileSet.TileGetName(_StageMap.GetCellv(tile)) == "floor")
	{
	  _StageMap.SetCellv(goodTilePosition[0], _PortalUpTileId);
	  _StageMap.SetCellv(goodTilePosition[1], _PortalDownTileId);
	  direction = "left";
	}
	else if (_StageMap.TileSet.TileGetName(_StageMap.GetCellv(tile)) == "left")
	{
	  _StageMap.SetCellv(goodTilePosition[0], _PortalUpTileId);
	  _StageMap.SetCellv(goodTilePosition[1], _PortalDownTileId);
	  direction = "right";
	}

	if (mouse == "LMB")
	{
	  _historyLMB.setAll(goodTilePosition, direction);
	}
	else
	{
	  _historyRMB.setAll(goodTilePosition, direction);
	}
  }

  private void _Reset(History history)
  {
	switch (history.direction)
	{
	  case "up":
		_StageMap.SetCellv(history.tilePosition[0], _StageMap.TileSet.FindTileByName("up"));
		_StageMap.SetCellv(history.tilePosition[1], _StageMap.TileSet.FindTileByName("up"));
		break;
	  case "down":
		_StageMap.SetCellv(history.tilePosition[0], _StageMap.TileSet.FindTileByName("down"));
		_StageMap.SetCellv(history.tilePosition[1], _StageMap.TileSet.FindTileByName("down"));
		break;
	  case "left":
		_StageMap.SetCellv(history.tilePosition[0], _StageMap.TileSet.FindTileByName("right"));
		_StageMap.SetCellv(history.tilePosition[1], _StageMap.TileSet.FindTileByName("right"));
		break;
	  case "right":
		_StageMap.SetCellv(history.tilePosition[0], _StageMap.TileSet.FindTileByName("left"));
		_StageMap.SetCellv(history.tilePosition[1], _StageMap.TileSet.FindTileByName("left"));
		break;
	}
  }

  private void _on_DeadZone_body_entered(object body)
  {
	if (body.GetType().Name == "Player")
	{
	  _Stat.RemoveLife();
	  if (_Stat.Life == 0)
	  {
		_Global.GoToCredits();
	  }
	  _HitSound.Play();
	  _Player.Position = _SpawnPosition.Position;
	}
  }


  private void _on_WinZone_body_entered(object body)
  {
	if (body.GetType().Name == "Player" && !_WinZone.IsClose)
	{
	  _Global.GotoScene(_Global.NextScenePath);
	}
  }


  private void _on_StageMap_CollidedPlayer(KinematicCollision2D collision)
  {

	var tile = _StageMap.WorldToMap(collision.Position);
	Vector2[] playerTilePosition = _SetupTilePosition(tile);

	int tileId = _StageMap.GetCellv(playerTilePosition[0]);

	if (_WhichPortal(playerTilePosition) == null) return;

	if (!_historyRMB.active || !_historyLMB.active) return;

	Vector2 newPlayerPosition = Vector2.Zero;
	if (_WhichPortal(playerTilePosition) == "portal1")
	{
	  // Go to Portal2
	  var worldPosition = _StageMap.MapToWorld(_historyRMB.tilePosition[0]);
	  switch (_historyRMB.direction)
	  {
		case "up":
		  newPlayerPosition = new Vector2(worldPosition.x + 15, worldPosition.y + 30);
		  break;
		case "down":
		  newPlayerPosition = new Vector2(worldPosition.x + 10, worldPosition.y - 20);
		  break;
		case "left":
		  newPlayerPosition = new Vector2(worldPosition.x + 30, worldPosition.y + 10);
		  break;
		case "right":
		  newPlayerPosition = new Vector2(worldPosition.x - 20, worldPosition.y + 10);
		  break;
	  }
	}

	if (_WhichPortal(playerTilePosition) == "portal2")
	{
	  // Go to Portal 2
	  var worldPosition = _StageMap.MapToWorld(_historyLMB.tilePosition[0]);
	  switch (_historyLMB.direction)
	  {
		case "up":
		  newPlayerPosition = new Vector2(worldPosition.x + 15, worldPosition.y + 30);
		  break;
		case "down":
		  newPlayerPosition = new Vector2(worldPosition.x + 10, worldPosition.y - 20);
		  break;
		case "left":
		  newPlayerPosition = new Vector2(worldPosition.x + 30, worldPosition.y + 10);
		  break;
		case "right":
		  newPlayerPosition = new Vector2(worldPosition.x - 20, worldPosition.y + 10);
		  break;
	  }
	}
	_Player.Position = newPlayerPosition;

  }

  private Vector2[] _SetupTilePosition(Vector2 tile)
  {
	Vector2[] goodTilePosition = { Vector2.Zero, Vector2.Zero };

	if (tile.y == 2) // up
	{

	  if (tile.x > 17)
	  {
		goodTilePosition[0] = new Vector2(17, 1);
		goodTilePosition[1] = new Vector2(18, 1);
	  }
	  else
	  {

		goodTilePosition[0] = new Vector2(tile.x, 1);
		goodTilePosition[1] = new Vector2(tile.x + 1, 1);
	  }
	}
	else if (tile.x == 1) // left
	{
	  if (tile.y > 12)
	  {
		goodTilePosition[0] = new Vector2(0, 12);
		goodTilePosition[1] = new Vector2(0, 13);
	  }
	  else
	  {
		goodTilePosition[0] = new Vector2(0, tile.y);
		goodTilePosition[1] = new Vector2(0, tile.y + 1);
	  }
	}
	else if (tile.y == 14) // down
	{
	  if (tile.x > 17)
	  {
		goodTilePosition[0] = new Vector2(17, tile.y);
		goodTilePosition[1] = new Vector2(18, tile.y);
	  }
	  else
	  {
		goodTilePosition[0] = new Vector2(tile.x, tile.y);
		goodTilePosition[1] = new Vector2(tile.x + 1, tile.y);
	  }
	}
	else if (tile.x == 19) // right
	{
	  if (tile.y > 12)
	  {
		goodTilePosition[0] = new Vector2(tile.x, 12);
		goodTilePosition[1] = new Vector2(tile.x, 13);
	  }
	  else
	  {
		goodTilePosition[0] = new Vector2(tile.x, tile.y);
		goodTilePosition[1] = new Vector2(tile.x, tile.y + 1);
	  }
	}
	else if (_StageMap.TileSet.TileGetName(_StageMap.GetCellv(tile)) == "floor")
	{
	  goodTilePosition[0] = new Vector2(tile.x - 1, tile.y - 1);
	  goodTilePosition[1] = new Vector2(tile.x - 1, tile.y);

	}
	else if (_StageMap.TileSet.TileGetName(_StageMap.GetCellv(tile)) == "left")
	{
	  goodTilePosition[0] = new Vector2(tile.x, tile.y - 1);
	  goodTilePosition[1] = new Vector2(tile.x, tile.y);
	}

	return goodTilePosition;
  }

  private string _WhichPortal(Vector2[] playerTilePosition)
  {
	if ((_historyLMB.tilePosition[0] == playerTilePosition[0] || _historyLMB.tilePosition[0] == playerTilePosition[1]) ||
	  (_historyLMB.tilePosition[1] == playerTilePosition[0] || _historyLMB.tilePosition[1] == playerTilePosition[1]))
	{
	  return "portal1";
	}
	if ((_historyRMB.tilePosition[0] == playerTilePosition[0] || _historyRMB.tilePosition[0] == playerTilePosition[1]) ||
	(_historyRMB.tilePosition[1] == playerTilePosition[0] || _historyRMB.tilePosition[1] == playerTilePosition[1]))
	{
	  return "portal2";
	}
	return null;
  }


  private void _on_Player_LiftCube()
  {
	if (_DroppedCube == null)
	{
	  var cube = GetNodeOrNull<RigidBody2D>("Cube");
	  if (cube != null) cube.QueueFree();
	}
	else
	{
	  _DroppedCube.QueueFree();
	}
  }

  private void _on_Player_DropCube(Vector2 position)
  {
	PackedScene Cube = (PackedScene)GD.Load("res://Prefabs/Cube.tscn");
	_DroppedCube = (RigidBody2D)Cube.Instance();
	_DroppedCube.Position = position;
	AddChild(_DroppedCube);
  }


  private void _on_Switch_Activate(object body)
  {
	if (body.GetType().Name != "Bullet")
	{
	  _WinZone.OpenDoor();
	}
  }

  private void _on_Switch_Deactivate(object body)
  {
	if (body.GetType().Name != "Bullet")
	{
	  _WinZone.CloseTheDoor();
	}
  }

  private void _on_ComeBack_body_entered(object body)
  {
	if (body.GetType().Name == "Player")
	{
	  _Global.BacktoScene(_Global.PrevScenePath);
	}
  }
}
