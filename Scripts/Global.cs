using Godot;
using System;

public class Global : Node
{
  public Node CurrentScene { get; set; }

  public string CurrentScenePath = "res://Scenes/Main.tscn";

  public string NextScenePath = "res://Scenes/Level2.tscn";

  public string PrevScenePath = "";

  public bool isPlayingMusic = false;

  public string[] ScenePathList = {
    "res://Scenes/Main.tscn",
    "res://Scenes/Level2.tscn",
    "res://Scenes/Level3.tscn",
    "res://Scenes/Level4.tscn",
    "res://Scenes/Level5.tscn",
    "res://Scenes/Credits.tscn"
  };

  private int indexScene = 0;

  public override void _Ready()
  {
    indexScene = 0;
    Viewport root = GetTree().Root;
    CurrentScene = root.GetChild(root.GetChildCount() - 1);
  }

  public void GoToMenu()
  {
    string path = "res://Scenes/Menu.tscn";
    CallDeferred(nameof(DefferedGotoScene), path);
  }

  public void GoToTutorial()
  {
    var music = (MainThemeMusic)GetNode("/root/MainThemeMusic");
    music.Play();

    string path = "res://Scenes/Tutorial.tscn";
    CallDeferred(nameof(DefferedGotoScene), path);
  }

  public void GoToCredits()
  {
    string path = "res://Scenes/Credits.tscn";
    CallDeferred(nameof(DefferedGotoScene), path);
  }

  public void BacktoScene(string path)
  {
    NextScenePath = CurrentScenePath;
    CurrentScenePath = path;

    indexScene--;
    if (indexScene > -1) NextScenePath = ScenePathList[indexScene];

    CallDeferred(nameof(DefferedGotoScene), path);
  }


  public void GotoScene(string path)
  {
    PrevScenePath = CurrentScenePath;
    CurrentScenePath = path;

    indexScene++;
    if (indexScene < ScenePathList.Length) NextScenePath = ScenePathList[indexScene];


    CallDeferred(nameof(DefferedGotoScene), path);
  }

  public void DefferedGotoScene(string path)
  {
    CurrentScene.Free();
    var nextScene = (PackedScene)GD.Load(path);

    CurrentScene = nextScene.Instance();

    GetTree().Root.AddChild(CurrentScene);

    GetTree().CurrentScene = CurrentScene;
  }

  public static bool IsIntInArray(int[] intArray, int key)
  {
    bool retval = false;
    foreach (int element in intArray)
    {
      if (element == key)
      {
        retval = true;
      }
    }
    return retval;
  }

  public void StopMainThemeMusic()
  {
    var mainThemeMusic = (MainThemeMusic)GetNode("/root/MainThemeMusic");
    mainThemeMusic.Stop();
  }
}
