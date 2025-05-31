using Godot;
using System;

public partial class EndScreen : CanvasLayer
{
    public SceneManager SceneManager;

    public override void _Ready()
    {
        SceneManager = GetTree().Root.GetNode<SceneManager>("SceneManager");
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionPressed("Jump"))
        {
            GetTree().ChangeSceneToPacked(SceneManager.MainScene);
        }
    }
}
