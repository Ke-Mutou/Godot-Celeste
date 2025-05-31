using Godot;
using System;
using System.Collections.Generic;

public partial class Mountain : Node3D
{
    public Camera3D Camera { get; set; }
    public Node3D CameraPivot { get; set; }

    public override void _Ready()
    {
        Camera = GetNode<Camera3D>("CameraPivot/Camera3D");
        CameraPivot = GetNode<Node3D>("CameraPivot");
    }

    public override void _PhysicsProcess(double delta)
    {
        CameraPivot.RotateY((float)delta * 0.05f);
    }
}
