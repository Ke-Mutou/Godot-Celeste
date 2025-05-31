using Godot;
using System;

public partial class MainInterface : Node3D
{

    public override void _Ready()
    {
    }

    public void SetCurrentCamera(int index)
    {
        switch (index)
        {
            case 0:
                GetNode<Camera3D>("Mountain/CameraPivot/Camera3D").Current = true;
                GetNode<Camera3D>("Cameras/Camera1").Current = false;
                GetNode<Camera3D>("Cameras/Camera2").Current = false;
                GetNode<Camera3D>("Cameras/Camera3").Current = false;
                break;
            case 1:
                GetNode<Camera3D>("Mountain/CameraPivot/Camera3D").Current = false;
                GetNode<Camera3D>("Cameras/Camera1").Current = true;
                GetNode<Camera3D>("Cameras/Camera2").Current = false;
                GetNode<Camera3D>("Cameras/Camera3").Current = false;
                break;
            case 2:
                GetNode<Camera3D>("Mountain/CameraPivot/Camera3D").Current = false;
                GetNode<Camera3D>("Cameras/Camera1").Current = false;
                GetNode<Camera3D>("Cameras/Camera2").Current = true;
                GetNode<Camera3D>("Cameras/Camera3").Current = false;
                break;
            case 3:
                GetNode<Camera3D>("Mountain/CameraPivot/Camera3D").Current = false;
                GetNode<Camera3D>("Cameras/Camera1").Current = false;
                GetNode<Camera3D>("Cameras/Camera2").Current = false;
                GetNode<Camera3D>("Cameras/Camera3").Current = true;
                break;
        }
    }
}
