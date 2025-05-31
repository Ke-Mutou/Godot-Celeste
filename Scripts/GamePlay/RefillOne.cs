using Godot;
using System;

public partial class RefillOne : Node2D
{
    public Area2D RefillArea;
    public AnimationPlayer AnimationPlayer;
    public AudioStreamPlayer2D Touch;

    public override void _Ready()
    {
        RefillArea = GetNode<Area2D>("Sprite2D/Area2D");
        AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        Touch = GetNode<AudioStreamPlayer2D>("Touch");
        RefillArea.BodyEntered += RefillAreaOnBodyEntered;
    }

    private void RefillAreaOnBodyEntered(Node2D body)
    {
        if (body is not Player player)
            return;
        if (player.Dashes >= 1 && player.Stamina >= 20)
            return;

        Touch.Play();
        player.EffectsManager.ChangeHairColorDash1();
        player.RefillDash();
        player.RefillStamina();
        _ = player.Globals.Freeze(0.1f);
        AnimationPlayer.Play("OutLine");
    }
}
