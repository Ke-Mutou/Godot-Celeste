using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

public partial class Problems : Node2D
{
    public AnimationPlayer HeartAnimationPlayer;
    public AnimationPlayer AnimationPlayer;
    public Player Player;
    public string InRoomName;
    [Export] public Array<Vector2> ProblemList = [];
    public int Index;

    public override void _Ready()
    {
        HeartAnimationPlayer = GetParent().GetNode<AnimationPlayer>("GamePlay/Heart/AnimationPlayer");
        AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        Player = GetTree().GetFirstNodeInGroup("Player") as Player;
        InRoomName = GetParent().Name;
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("Get"))
        {
            HeartAnimationPlayer.Play("Show");
        }
        if (Player.InRoomName == InRoomName && !AnimationPlayer.IsPlaying())
        {
            AnimationPlayer.Play("Idle");
        }
        else if (Player.InRoomName != InRoomName && AnimationPlayer.IsPlaying())
        {
            AnimationPlayer.Stop();
        }
    }
}
