using Godot;
using System;
using System.Threading.Tasks;

public partial class FollowCamera : Camera2D
{
    public Vector2 CameraCenter { get; set; }
    public float ShakeStrength { get; set; } = 1;

    public override void _Ready()
    {
        
    }

    public override void _PhysicsProcess(double delta)
    {
        Position = Position.Round();
        ForceUpdateScroll();
    }

    public void SetLimits(float left, float top, float right, float bottom)
    {
        LimitLeft = (int)left;
        LimitTop = (int)top;
        LimitRight = (int)right;
        LimitBottom = (int)bottom - 4;
    }
    
    public async Task TransitionedRoom(float time)
    {
        GetTree().Paused = true;

        await ToSignal(GetTree().CreateTimer(time), "timeout");
        
        GetTree().Paused = false;
    }

    public async Task Shake(Vector2 direction, float duration)
    {
        var tween = GetTree().CreateTween().SetProcessMode(Tween.TweenProcessMode.Physics).SetEase(Tween.EaseType.In)
            .SetTrans(Tween.TransitionType.Back);
        tween.TweenProperty(this, "offset", -direction, duration / 2);
        tween.TweenProperty(this, "offset", direction, duration / 2);
        tween.TweenProperty(this, "offset", Vector2.Zero, duration / 2);
        
        await ToSignal(tween, Tween.SignalName.Finished);
    }
}
