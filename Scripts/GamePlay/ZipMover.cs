using Godot;
using System;
using System.Collections.Generic;

public partial class ZipMover : Node2D
{
    public Area2D Area;
    public AnimationPlayer AnimationPlayer;
    public Sprite2D Block;

    public bool IsMove;
    public readonly List<Player> Players = [];

    public Vector2 PlayerPos;
    public Vector2 InitPos;
    public Vector2 SubPos;

    public override void _Ready()
    {
        Area = GetNode<Area2D>("Block/Area2D");
        AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        
        Area.BodyEntered += AreaOnBodyEntered;
        Area.BodyExited += AreaOnBodyExited;
        Block = GetNode<Sprite2D>("Block");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Players.Count <= 0)
            return;
        
        var player = Players[0];
        
        if ((player.StateMachine.CurrentState.Name == "ClimbState" || player.IsOnFloor()) && AnimationPlayer.CurrentAnimation == "Idle")
        {
            AnimationPlayer.Play("Move");
            InitPos = Block.GlobalPosition;
            PlayerPos = player.GlobalPosition;
            SubPos = PlayerPos - InitPos;
        }
        if (player.StateMachine.CurrentState.Name == "ClimbState")
        {
            var pos = player.GlobalPosition;
            pos.X = Block.GlobalPosition.X + SubPos.X;
            player.GlobalPosition = pos;
        }
        
        if (AnimationPlayer.CurrentAnimation == "Move" && AnimationPlayer.CurrentAnimationPosition >= 0.1f)
        {
            if (player.CanJump)
            {
                player.Speed.X += MathF.Max(140f * (float)AnimationPlayer.CurrentAnimationPosition - 0.1f, 70);
            }
        }
        
        if (AnimationPlayer.CurrentAnimation == "Back" && AnimationPlayer.CurrentAnimationPosition is >= 0.9f and <= 1.1f)
        {
            if (player.CanJump)
            {
                player.Speed.X -= MathF.Max(60f * (float)AnimationPlayer.CurrentAnimationPosition - 0.9f, 30);
            }
        }
    }

    private void AreaOnBodyEntered(Node2D body)
    {
        if (body is not Player player)
            return;

        Players.Add(player);
    }
    
    

    private void AreaOnBodyExited(Node2D body)
    {
        if (body is not Player player)
            return;
        Players.RemoveAt(0);
        player.LiftSpeed = Vector2.Zero;
    }
}
