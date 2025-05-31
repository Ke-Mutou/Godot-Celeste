using Godot;
using System;

public partial class TitleScreen : Node2D
{
    public SceneManager SceneManager { get; set; }
    public SoundManager SoundManager { get; set; }
    public AnimationPlayer AnimationPlayer { get; set; }

    public override void _Ready()
    {
        SceneManager = GetTree().Root.GetNode<SceneManager>("SceneManager");
        SceneManager.CurrentScene = "TitleScreen";
        SoundManager = GetTree().Root.GetNode<SoundManager>("SoundManager");
        
        AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        AnimationPlayer.AnimationFinished += animName =>
        {
            if (animName == "Exit")
                GetTree().ChangeSceneToPacked(SceneManager.MainScene);
        };
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("ui_accept"))
        {
            AnimationPlayer.Play("Exit");
            SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_title_firstinput").Play();
        }
    }
}
