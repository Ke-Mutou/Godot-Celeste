using Godot;
using System;

public partial class RefillTwo : Node2D
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
        if (player.Dashes >= 2 && player.Stamina >= 20)
            return;

        player.RefillDash(2);
        Touch.Play();
        player.EffectsManager.ChangeHairColorDash2();
        player.RefillStamina();
        _ = player.Globals.Freeze(0.1f);
        AnimationPlayer.Play("OutLine");
    }
}
