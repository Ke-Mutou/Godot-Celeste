using Godot;
using System;

public partial class CrumblePlatform : StaticBody2D
{
    public Sprite2D Sprite;
    public AnimationPlayer AnimationPlayer;
    public Area2D TopArea;
    public Area2D BesideArea;
    
    public override void _Ready()
    {
        Sprite = GetNode<Sprite2D>("Sprite2D");
        AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        TopArea = GetNode<Area2D>("TopArea");
        TopArea.BodyEntered += TopAreaOnBodyEntered;
        BesideArea = GetNode<Area2D>("BesideArea");
        BesideArea.BodyEntered += BesideAreaOnBodyEntered;
    }

    private void TopAreaOnBodyEntered(Node body)
    {
        if (body is not Player player)
            return;
        if (AnimationPlayer.CurrentAnimation != "Idle")
            return;
        
        AnimationPlayer.Play("Display");
    }
    
    private void BesideAreaOnBodyEntered(Node body)
    {
        if (body is not Player player)
            return;
        if (AnimationPlayer.CurrentAnimation != "Idle")
            return;
        if (player.StateMachine.CurrentState.Name == "ClimbState")
            AnimationPlayer.Play("DisplayClimb");
    }
}
