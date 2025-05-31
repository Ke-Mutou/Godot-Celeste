using Godot;
using System;

public partial class Spring : RigidBody2D
{
    public AnimationPlayer AnimationPlayer;

    public override void _Ready()
    {
        AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }
}
