using Godot;
using System;

public partial class TaiJi : Area2D
{
    public AudioStreamPlayer2D AudioStreamPlayer;
    public Timer Timer;

    public override void _Ready()
    {
        AudioStreamPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
        Timer = GetNode<Timer>("Timer");
        Timer.Timeout += () =>
        {
            Visible = true;
            SetCollisionMaskValue(1, true);
        };
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is not Player player)
            return;

        AudioStreamPlayer.Play();
        
        SetCollisionMaskValue(1, false);
        Visible = false;
        Timer.Start();
        if (player.GetCollisionLayerValue(7))
        {
            SetCollision(player, 7, false);
            SetCollision(player, 8, true);
        }
        else if (player.GetCollisionLayerValue(8))
        {
            SetCollision(player, 7, true);
            SetCollision(player, 8, false);
        }
    }

    public void SetCollision(Player player, int layer, bool value)
    {
        player.SetCollisionLayerValue(layer, value);
        player.SetCollisionMaskValue(layer, value);
        player.HandCast.SetCollisionMaskValue(layer, value);
        player.FootCast.SetCollisionMaskValue(layer, value);
    }
}
