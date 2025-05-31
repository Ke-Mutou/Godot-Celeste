using Godot;
using System;

public partial class SceneManager : Node
{
    public string CurrentScene { get; set; }
    public PackedScene TitleScreenScene { get; set; }
    public PackedScene MainScene { get; set; }
    public PackedScene TestGameScene { get; set; }
    public PackedScene Level1GameScene { get; set; }
    public PackedScene Level2GameScene { get; set; }
    public PackedScene EndScreen1Scene { get; set; }
    public PackedScene EndScreen2Scene { get; set; }

    public override void _Ready()
    {
        TitleScreenScene = GD.Load<PackedScene>("res://Tscns/TitleScreen.tscn");
        MainScene = GD.Load<PackedScene>("res://Tscns/Main.tscn");
        Level1GameScene = GD.Load<PackedScene>("res://Tscns/Games/万家灯火Game.tscn");
        EndScreen1Scene = GD.Load<PackedScene>("res://Tscns/UI/万家灯火EndScreen.tscn");
        Level2GameScene = GD.Load<PackedScene>("res://Tscns/Games/GlyphGame.tscn");
        EndScreen2Scene = GD.Load<PackedScene>("res://Tscns/UI/GlyphEndScreen.tscn");
        TestGameScene = GD.Load<PackedScene>("res://Tscns/Games/TestGame.tscn");
    }
}
