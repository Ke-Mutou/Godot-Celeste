using Godot;
using System;

public partial class StopScreen : Control
{
    public Control Retry { get; set; }
    public Control Settings { get; set; }
    public Control Remap { get; set; }
    public Control Back { get; set; }
    
    public override void _Ready()
    {
        Retry = GetNode<Control>("Retry");
        Retry.GrabFocus();
        Settings = GetNode<Control>("Settings");
        Remap = GetNode<Control>("Remap");
        Back = GetNode<Control>("Back");
    }
}
