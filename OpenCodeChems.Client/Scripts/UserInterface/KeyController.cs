using Godot;
using System;
using System.Collections.Generic;
using OpenCodeChems.Client.Server;
using static OpenCodeChems.Client.Resources.Objects;
using OpenCodeChems.Client.Resources;


namespace OpenCodeChems.Client.UserInterface
{
  public class KeyController : Control
  {

    Network serverClient;


    public override void _Ready()
    {   
      serverClient = GetNode<Network>("/root/Network") as Network;
      serverClient.Connect("UpdateBoardSignal", this, nameof(ChangeScreen));
      serverClient.Connect("CleanRoom", this, nameof(ChangeToMainMenu));
    }


    public void ChangeToMainMenu()
    {
      GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
    }

    public void ChangeScreen(string rool, int number)
    {
      GD.Print("Llegué aqui");
      if(rool == Constants.BLUE_SPY_MASTER || rool == Constants.RED_SPY_MASTER)
      {
        LoadKey(number);
      }
      else
      {
        GetTree().ChangeScene("res://Scenes/SpyPlayer.tscn");
      }
    }



    public void LoadKey(int randomNumber)
    {   
      PackedScene packedScene;
      if(randomNumber == 0)
      {
        packedScene = (PackedScene)GD.Load("res://Scenes/MasterPlayer.tscn");
        Control key = (Control)packedScene.Instance();
        this.AddChild(key);
      }
      else if(randomNumber == 1)
      {
        packedScene = (PackedScene)GD.Load("res://Scenes/KeyRedOne.tscn");
        Control key = (Control)packedScene.Instance();
        this.AddChild(key);
      }
      else if(randomNumber == 2)
      {
        packedScene = (PackedScene)GD.Load("res://Scenes/KeyRedTwo.tscn");
        Control key = (Control)packedScene.Instance();
        this.AddChild(key);
      }
      else if(randomNumber == 3)
      {
        packedScene = (PackedScene)GD.Load("res://Scenes/KeyBlueTwo.tscn");
        Control key = (Control)packedScene.Instance();
        this.AddChild(key);
      }
    }


  }
}  