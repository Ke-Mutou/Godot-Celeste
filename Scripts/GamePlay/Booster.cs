using Godot;
using System;
using System.Collections.Generic;

public partial class Booster : Area2D
{
    public Sprite2D Sprite;
    public AnimationPlayer AnimationPlayer;
    public Timer Timer;
    public Timer ReTimer;
    public AudioStreamPlayer2D SoundPlayer;
    
    public readonly List<Player> Players = [];
    public bool IsInSide;
    public bool IsDash;
    public Vector2 InitPosition;
    public int InitDashes;

    public override void _Ready()
    {
        InitPosition = GlobalPosition;
        Sprite = GetNode<Sprite2D>("Sprite2D");
        AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        SoundPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
        Timer = GetNode<Timer>("Timer");
        Timer.Timeout += TimerOnTimeout;
        
        
        ReTimer = GetNode<Timer>("ReTimer");
        ReTimer.Timeout += () =>
        {
            GlobalPosition = InitPosition;
            AnimationPlayer.Play("Appear");
        };
        AnimationPlayer.AnimationFinished += name =>
        {
            if (name == "Display")
            {
                ReTimer.Start();
            }
        }; 
        
        BodyEntered += OnBodyEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!IsInSide)
            return;
        
        var player = Players[0];
        if (IsDash)
        {
            GlobalPosition = player.GlobalPosition;
            if (player.StateMachine.CurrentState.Name != "DashState")
            {
                player.Visible = true;
                IsDash = false;
                IsInSide = false;
                player.Dashes = InitDashes;
                _ = player.Globals.Freeze(0.05f);
                AnimationPlayer.Play("Display");
            }
        }
        else
        {
            if (Input.IsActionJustPressed("Dash"))
            {
                Timer.Stop();
                BoosterDash();
            }
            player.GlobalPosition = GlobalPosition;
        }
    }

    private void TimerOnTimeout()
    {
        BoosterDash();
    }

    private void BoosterDash()
    {
        var player = Players[0];
        AnimationPlayer.Play("Move");
        player.Dash();
        IsDash = true;
        player.StateMachine.CurrentState.TransitionTo("DashState");
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is not Player player)
            return;
        _ = player.Globals.Freeze(0.05f);
        player.GlobalPosition = GlobalPosition;
        IsInSide = true;
        InitDashes = player.Dashes;
        Players.Add(player);
        Timer.Start();
        player.Visible = false;
        SoundPlayer.Play();
    }
}
