using Godot;
using System;

public partial class DebugPanel : Control
{
    private Label _debugLabel;

    public override void _Ready()
    {
        _debugLabel = GetNode<Label>("./PanelContainer/MarginContainer/VBoxContainer/ScrollContainer/Debug");
    }
    
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("Show"))
        {
            Visible = !Visible;
        }
        var fps = Engine.GetFramesPerSecond();
        _debugLabel.Text = $"Fps: {fps}\n" +
            $"RoomName: {Info.RoomName}, RoomSize: {Info.RoomSize}\n" +
            $"Stamina: {Info.Stamina}\n" +
            $"Facing: {(int)Info.Facing}（{Info.Facing}）\n" +
            $"(MoveX: {Info.MoveX}  MoveY: {Info.MoveY})\n" +
            $"(SpeedX: {Info.Speed.X}  SpeedY: {Info.Speed.Y})\n" +
            $"CurrentState: {Info.CurrentState}\n" +
            $"CanJump: {Info.CanJump}\n" +
            $"CanDash: {Info.CanDash}\n" +
            $"Dashes: {Info.Dashes}\n" +
            $"Ducking: {Info.Ducking}\n" +
            $"JumpBufferTime: {Info.JumpBufferTime}\n";
    }
}
