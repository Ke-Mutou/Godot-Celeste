using Godot;
using System;

public partial class FallBlockFall : RigidBody2D
{
    public Area2D TopArea;
    public Area2D BesideArea;
    public AudioStreamPlayer2D AudioStreamPlayer;
    public bool IsPlay;
    public Vector2 InitPos;
    public Player Player;
    public bool CanPlay = true;
    
    public override void _Ready()
    {
        Player = GetTree().GetFirstNodeInGroup("Player") as Player;
        TopArea = GetNode<Area2D>("TopArea");
        TopArea.BodyEntered += TopAreaOnBodyEntered;
        BesideArea = GetNode<Area2D>("BesideArea");
        BesideArea.BodyEntered += BesideAreaOnBodyEntered;
        
        AudioStreamPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
        InitPos = GlobalPosition;
    }

    public override void _Process(double delta)
    {
        if (IsPlay && CanPlay)
        {
            IsPlay = false;
            AudioStreamPlayer.Play();
            CanPlay = false;
        }
        if (Player.IsDie)
        {
            GlobalPosition = InitPos;
            Freeze = true;
            CanPlay = true;
        }
    }

    private void TopAreaOnBodyEntered(Node body)
    {
        if (body is not Player player)
            return;
        IsPlay = true;
        var tween = GetTree().CreateTween();
        tween.TweenProperty(this, "freeze", false, 0.7f);
    }
    
    private void BesideAreaOnBodyEntered(Node body)
    {
        if (body is not Player player)
            return;
        if (player.StateMachine.CurrentState.Name == "ClimbState")
        {
            IsPlay = true;
            var tween = GetTree().CreateTween();
            tween.TweenProperty(this, "freeze", false, 1f);
        }
    }
}
