using Godot;
using System;
using System.Threading.Tasks;

public partial class Globals : Node2D
{
    public Player Player { get; set; }
    public AnimationPlayer DeathTransitionedAnimationPlayer { get; set; }
    
    public ColorRect EntranceShader { get; set; }
    public AnimationPlayer EntranceAnimationPlayer { get; set; }
    public PackedScene Ripple;

    public override void _Ready()
    {
        Player = GetTree().GetFirstNodeInGroup("Player") as Player;
        DeathTransitionedAnimationPlayer = GetNode<AnimationPlayer>("CanvasLayer/DeathTransitioned/AnimationPlayer");
        EntranceShader =  GetNode<ColorRect>("CanvasLayer/Entrance");
        EntranceShader.Visible = true;
        EntranceAnimationPlayer = EntranceShader.GetNode<AnimationPlayer>("AnimationPlayer");
        Ripple = GD.Load<PackedScene>("res://Tscns/Effects/Ripple.tscn");
    }

    public async Task Freeze(float freezeTime)
    {
        Engine.TimeScale = 0.01f;
        await ToSignal(GetTree().CreateTimer(freezeTime, true, false, true), Timer.SignalName.Timeout);
        Engine.TimeScale = 1;
    }

    public async Task DeathTransitioned()
    {
        DeathTransitionedAnimationPlayer.Play("DeathTransitioned");
        await ToSignal(DeathTransitionedAnimationPlayer, AnimationMixer.SignalName.AnimationFinished);
    }

    public async Task Entrance(Vector2 pos)
    {
        Player.CanMove = false;
        var shader = EntranceShader.Material as ShaderMaterial;
        shader?.SetShaderParameter("center_pos", pos);
        EntranceAnimationPlayer.Play("Entrance");
        await ToSignal(EntranceAnimationPlayer, AnimationMixer.SignalName.AnimationFinished);
        Player.CanMove = true;
    }

    public void CreateRipple(int count, bool isWallJump = false, int dir = 1)
    {
        var ripple = Ripple.Instantiate<ColorRect>();
        ripple.ZIndex = 0;
        
        if (isWallJump)
        {
            ripple.RotationDegrees = dir * 45;
            var  offsetX = dir switch
            {
                -1 => 15,
                1 => 5,
                _ => 0
            };
            var offsetY = dir switch
            {
                -1 => -5,
                1 => 5,
                _ => 0
            };
            ripple.GlobalPosition = Player.GlobalPosition - new Vector2(offsetX, offsetY);
            Player.GetParent().AddChild(ripple);
            Player.GetParent().MoveChild(ripple, Player.GetIndex());
        }
        else
        {
            ripple.RotationDegrees = count switch
            {
                1 => -20 * (int)Player.Facing,
                2 => 0,
                3 => 20 * (int)Player.Facing,
                _ => ripple.RotationDegrees
            };
            var  offsetX = 0;
            var offsetY = 0;
            if (Player.Facing == Facings.Right)
            {
                offsetY = count switch
                {
                    1 => 2,
                    2 => 7, 
                    3 => 10,
                    _ => 0
                };
                offsetX = count switch
                {
                    1 => 0,
                    2 => -2, 
                    3 => -7,
                    _ => 0
                };
            }
            else if (Player.Facing == Facings.Left)
            {
                offsetY = count switch
                {
                    1 => 12,
                    2 => 7, 
                    3 => 0,
                    _ => 0
                };
                offsetX = count switch
                {
                    1 => -3,
                    2 => 2, 
                    3 => 5,
                    _ => 0
                };
            }
            ripple.GlobalPosition = Player.GlobalPosition - new Vector2(16 + offsetX, 16 + offsetY);
            Player.GetParent().AddChild(ripple);
            Player.GetParent().MoveChild(ripple, Player.GetIndex());
        }
    }

    public async Task AwaitFrame(int count)
    {
        for (var i = 0; i < count; i++)
        {
            await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        }
    }
}
