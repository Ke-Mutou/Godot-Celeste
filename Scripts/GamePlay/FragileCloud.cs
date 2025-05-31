using Godot;
using System;
using System.Collections.Generic;

public partial class FragileCloud : StaticBody2D
{
    public Area2D Area;
    public AnimationPlayer AnimationPlayer;

    public readonly List<Player> AreaPlayer = [];

    public bool IsPlay;
    public Player Player;


    public override void _Ready()
    {
        Player = GetTree().GetFirstNodeInGroup("Player") as Player;
        Area = GetNode<Area2D>("Area2D");
        Area.BodyEntered += OnBodyEntered;
        Area.BodyExited += AreaOnBodyExited;
        
        AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        AnimationPlayer.AnimationFinished += name =>
        {
            if (name == "MoveUp" && AreaPlayer.Count > 0)
            {
                
                Player.LiftSpeed = new(0, -200f);
                IsPlay = false;
            }
        };
    }
    public override void _PhysicsProcess(double delta)
    {
        if (AreaPlayer.Count <= 0)
            return;
        var player = AreaPlayer[0];
        if (player.IsOnFloor())
        {
            IsPlay = true;
        }

        if (IsPlay)
        {
            if (player.Speed.Y == 0)
            {
                var pos = player.GlobalPosition;
                pos.Y = Area.GlobalPosition.Y - 2;
                player.GlobalPosition = pos;
            }
            if (player.IsOnFloor())
            {
                if (AnimationPlayer.CurrentAnimation == "Idle")
                {
                    AnimationPlayer.Play("MoveDown");
                }
            }

            if (player.CanJump)
            {
                if (AnimationPlayer.CurrentAnimation == "MoveUp" && AnimationPlayer.CurrentAnimationPosition > 0.03f)
                {
                    player.LiftSpeed = new(0, -200f);
                    IsPlay = false;
                }
            }
        }
        
    }

    private void OnBodyEntered(Node body)
    {
        if (body is not Player player)
            return;
        AreaPlayer.Add(player);
    }

    private void AreaOnBodyExited(Node2D body)
    {
        if (body is not Player player)
            return;
        
        if (player.IsOnFloor())
            return;
        if (AreaPlayer.Count > 0)
        {
            AnimationPlayer.Play("Display");
            AreaPlayer.Clear();
            IsPlay = false;
            player.LiftSpeed = Vector2.Zero;
        }
    }
}
