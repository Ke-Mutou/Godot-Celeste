using Godot;
using System;

public partial class Level : Node2D
{
    public Player Player;
    public bool IsEntrance = true;

    public Node2D Map;
    public AudioStreamMP3 LastBgm;
    public AudioStreamMP3 CurrentBgm;
    public AudioStreamPlayer2D AudioStreamPlayer;
    public SceneManager SceneManager;

    public override void _Ready()
    {
        SceneManager = GetTree().Root.GetNode<SceneManager>("SceneManager");
        Player = GetNode<Player>("Player");
        Map = GetNode<Node2D>("Map");
        AudioStreamPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("Stop"))
        {
            GetTree().ChangeSceneToPacked(SceneManager.MainScene);
        }
        if (IsEntrance)
        {
            _ = Player.Globals.Entrance(Player.GetGlobalTransformWithCanvas()[2] + Vector2.Up * 7);
            IsEntrance = false;
            CurrentBgm = Map.GetNode<Room>(Player.InRoomName).Bgm;
        }
        
        if (IsNeedChangeBgm())
        {
            ChangeAndPlayBgm(CurrentBgm);
        }
    }

    public bool IsNeedChangeBgm()
    {
        if (LastBgm is null)
            return true;
        LastBgm = CurrentBgm;
        CurrentBgm = Map.GetNode<Room>(Player.InRoomName).Bgm;
        return LastBgm != CurrentBgm;
    }
    public void ChangeAndPlayBgm(AudioStreamMP3 bgm)
    {
        AudioStreamPlayer.Stream = bgm;
        LastBgm = bgm;
        AudioStreamPlayer.Play();
    }
}
