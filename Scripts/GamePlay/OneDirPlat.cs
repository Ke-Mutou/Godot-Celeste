using Godot;
using System;
using System.Collections.Generic;
using Array = Godot.Collections.Array;

public partial class OneDirPlat : Node2D
{
    public Sprite2D Sprite;
    
    public override void _Ready()
    {
        Sprite = GetNode<Sprite2D>("Sprite2D");
    }

    public override void _Process(double delta)
    {
    }
}
