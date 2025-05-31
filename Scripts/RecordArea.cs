using Godot;
using System;

public partial class RecordArea : Area2D
{
    public Sprite2D Sprite;
        
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        Sprite = GetNode<Sprite2D>("Sprite2D");
        Sprite.Visible = false;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is not Player player)
            return;

        player.RespawnPoint = Sprite.GlobalPosition;
    }
}
