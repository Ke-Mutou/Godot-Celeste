using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class WingsStrawberry : Area2D
{
    public bool IsFollow;
    public readonly List<Player> Players = [];
    public Vector2 Offset { get; set; } = new(-13, -11); // 偏移量
    public float Speed { get; set; } = 3f; // 基础速度系数
    public bool isFlying;

    public Timer Timer;
    public AnimationPlayer AnimationPlayer;

    public string InRoomName;
    
    public Player Player;
    public Vector2 InitPos;
    
    public AudioStreamPlayer2D TouchSound;
    public AudioStreamPlayer2D LaughSound;
    public AudioStreamPlayer2D FlyAwaySound;
    public AudioStreamPlayer2D PulseSound;
    public AudioStreamPlayer2D WingFlapSound;

    public override void _Ready()
    {
        Player = GetTree().GetFirstNodeInGroup("Player") as Player;
        BodyEntered += OnBodyEntered;
        AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        Timer = GetNode<Timer>("Timer");
        Timer.Timeout += TimerOnTimeout;
        InRoomName = GetParent().GetParent().Name;
        InitPos = GlobalPosition;
        TouchSound = GetNode<AudioStreamPlayer2D>("Touch");
        LaughSound = GetNode<AudioStreamPlayer2D>("Laugh");
        FlyAwaySound = GetNode<AudioStreamPlayer2D>("FlyAway");
        PulseSound = GetNode<AudioStreamPlayer2D>("Pulse");
        WingFlapSound = GetNode<AudioStreamPlayer2D>("WingFlap");
    }

    public async Task Fly()
    {
        LaughSound.Play();
        FlyAwaySound.Play();
        isFlying = true;
        var tween = GetTree().CreateTween();
        tween.TweenProperty(this, "position", Position + 320 * Vector2.Up, 2f);
        await ToSignal(tween, Tween.SignalName.Finished);
        Visible = false;
        LaughSound.Stop();
        FlyAwaySound.Stop();
    }

    public override void _Process(double delta)
    {
        if (Player.InRoomName == InRoomName && !isFlying)
        {
            switch (AnimationPlayer.CurrentAnimation)
            {
                case "Idle":
                {
                    if (Math.Abs(AnimationPlayer.CurrentAnimationPosition - 0.4f) < 0.01f)
                        PulseSound.Play();
                    break;
                }
                case "Wings":
                {
                    if (Math.Abs(AnimationPlayer.CurrentAnimationPosition - 0.4f) < 0.01f)
                        WingFlapSound.Play();
                    break;
                }
            }
        }
        if (Players.Count <= 0)
        {
            if (Player.InRoomName == InRoomName)
            {
                if (Player.StateMachine.CurrentState.Name == "DashState" && !isFlying)
                {
                    _ = Fly();
                }
            }
            else
            {
                GlobalPosition = InitPos;
                Visible = true;
                isFlying = false;
            }
        }
        else
        {
            var player = Players[0];

            if (player.IsDie)
            {
                GlobalPosition = InitPos;
                AnimationPlayer.Play("Wings");
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
    }
    
    private void TimerOnTimeout()
    {
        AnimationPlayer.Play("Display");
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is not Player player)
            return;

        IsFollow = true;
        Players.Add(player);
        AnimationPlayer.Play("Transition");
        TouchSound.Play();
        BodyEntered -= OnBodyEntered;
    }
}
