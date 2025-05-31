using Godot;
using System;
using System.Collections.Generic;

public partial class NormalStrawberry : Area2D
{
    public bool IsFollow;
    public readonly List<Player> Players = [];
    public Vector2 Offset { get; set; } = new(-13, -11); // 偏移量
    public float Speed { get; set; } = 3f; // 基础速度系数

    public string InRoomName;
    public Player Player;
    public Timer Timer;
    public AnimationPlayer AnimationPlayer;
    
    public AudioStreamPlayer2D TouchSound;
    public AudioStreamPlayer2D PulseSound;
    public Vector2 InitPos;

    public override void _Ready()
    {
        Player = GetTree().GetFirstNodeInGroup("Player") as Player;
        InRoomName = GetParent().GetParent().Name;
        BodyEntered += OnBodyEntered;
        AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        Timer = GetNode<Timer>("Timer");
        Timer.Timeout += TimerOnTimeout;
        TouchSound = GetNode<AudioStreamPlayer2D>("Touch");
        PulseSound = GetNode<AudioStreamPlayer2D>("Pulse");
        InitPos = GlobalPosition;
    }

    private void TimerOnTimeout()
    {
        AnimationPlayer.Play("Display");
    }

    public override void _Process(double delta)
    {
        if (Player.InRoomName == InRoomName)
        {
            if (AnimationPlayer.CurrentAnimation == "Idle")
                if (Math.Abs(AnimationPlayer.CurrentAnimationPosition - 0.4f) < 0.01f)
                    PulseSound.Play();
        }
        if (Players.Count <= 0)
            return;
        var player = Players[0];

        if (player.IsDie)
        {
            GlobalPosition = InitPos;
            Players.Clear();
            Timer.Stop();
            BodyEntered += OnBodyEntered;
        }
        Offset = Offset with{ X = Offset.X * (int)player.Facing };
        // 更新位置
        GlobalPosition += (player.GlobalPosition + Offset - GlobalPosition) * Speed * (float)delta;
        if (player.IsOnFloor() && Timer.IsStopped())
        {
            Timer.Start();
        }
        if (!player.IsOnFloor())
            Timer.Stop();
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is not Player player)
            return;

        IsFollow = true;
        Players.Add(player);
        TouchSound.Play();
        BodyEntered -= OnBodyEntered;
    }
}