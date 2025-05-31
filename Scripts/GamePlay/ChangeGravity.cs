using Godot;
using System;

public partial class ChangeGravity : Area2D
{
    public float Gra;
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        Gra = GetGravity();
        BodyExited += OnBodyExited;
    }

    private void OnBodyExited(Node2D body)
    {
        if (body is not Player player)
            return;
        SetGravity(Gra);
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is not Player player)
            return;
        SetGravity(-Gra);
    }  
}
