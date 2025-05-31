using Godot;
using System;
using System.Threading.Tasks;

public partial class Heart : Area2D
{
    public AnimatedSprite2D AnimatedSprite;
    public SceneManager SceneManager { get; set; }
    public Timer Timer;
    
    public override void _Ready()
    {
        SceneManager = GetTree().Root.GetNode<SceneManager>("SceneManager");
        BodyEntered += OnBodyEntered;
        AnimatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        Timer = GetNode<Timer>("Timer");
        Timer.Timeout += () =>
        {
            if (SceneManager.CurrentScene == "万家灯火")
                GetTree().ChangeSceneToPacked(SceneManager.EndScreen1Scene);
            else if (SceneManager.CurrentScene == "Glyph")
                GetTree().ChangeSceneToPacked(SceneManager.EndScreen2Scene);
            Engine.TimeScale = 1;
        };
    }

    public override void _Process(double delta)
    {
        if (Timer.IsStopped())
            return;
        if (Math.Abs(Timer.TimeLeft - 0.2) < 0.01f)
        {
            Engine.TimeScale = 0.2f;
        }
        else if (Math.Abs(Timer.TimeLeft - 0.1) < 0.01f)
        {
            Engine.TimeScale = 0.1f;
        }
        else if (Timer.TimeLeft < 0.1f)
        {
            Engine.TimeScale = 0;
        }
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is not Player player)
            return;
        Engine.TimeScale = 0.2;
        Timer.Start();
        AnimatedSprite.Visible = false;
        SetDeferred("monitorable", false);
        SetDeferred("monitoring", false);
    }
}
