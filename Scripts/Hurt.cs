using Godot;
using System;

public partial class Hurt : Area2D
{
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    public void OnBodyEntered(Node body)
    {
        if (body is Spring spring)
        {
            spring.AnimationPlayer.Play("Idle");
            switch ((int)spring.RotationDegrees)
            {
                case 0 or 360 or -360:
                    (GetParent() as Player)?.SuperBounce(spring.GlobalPosition.Y + 8);
                    break;
                case 90 or -270:
                    (GetParent() as Player)?.SideBounce(1, spring.GlobalPosition.Y - 8, 0);
                    break;
                case -90 or 270:
                    (GetParent() as Player)?.SideBounce(-1, spring.GlobalPosition.X + 8, 0);
                    break;
                case -180 or 180:
                    (GetParent() as Player)?.SideBounce(0, 0,spring.GlobalPosition.Y - 8);
                    break;
            }
            spring.AnimationPlayer.Play("Bounce");
        }

        if (body is Danger)
        {
            var player = GetParent() as Player;
            player?.Die();
        }
    }
}
