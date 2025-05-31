using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot.Collections;

public partial class EffectsManager : Node
{
    public Player Player;
    public Node2D Effects;
    
    public GpuParticles2D DashDust;
    public PackedScene SlashEffect;
    public PackedScene ShockWaveEffect;
    public PackedScene JumpDust;
    public PackedScene LandDust;
    public PackedScene LeftWallJumpDust;
    public PackedScene RightWallJumpDust;
    
    public PlayerHair PlayerHair;

    public override void _Ready()
    {
        Player = GetTree().GetFirstNodeInGroup("Player") as Player;
        Effects = GetTree().GetFirstNodeInGroup("Effects") as Node2D;    

        DashDust = Effects!.GetNode<GpuParticles2D>("DashDust");
        SlashEffect = GD.Load<PackedScene>("res://Tscns/Effects/Slash.tscn");
        ShockWaveEffect = GD.Load<PackedScene>("res://Tscns/Effects/ShockWave.tscn");
        
        JumpDust = GD.Load<PackedScene>("res://Tscns/Effects/JumpDust.tscn");
        LandDust = GD.Load<PackedScene>("res://Tscns/Effects/LandDust.tscn");
        LeftWallJumpDust = GD.Load<PackedScene>("res://Tscns/Effects/LeftWallJumpDust.tscn");
        RightWallJumpDust = GD.Load<PackedScene>("res://Tscns/Effects/RightWallJumpDust.tscn");
        
        PlayerHair = GetTree().GetFirstNodeInGroup("PlayerHair") as PlayerHair;
    }

    public override void _Process(double delta)
    {
        DashDust.GlobalPosition = Player.GlobalPosition;
    }

    public async Task PlayDust(string dustName)
    {
        switch (dustName)
        {
            case "JumpDust":
            {
                var jumpDust = JumpDust.Instantiate<GpuParticles2D>();
                jumpDust.GlobalPosition = Player.GlobalPosition;
                jumpDust.ZIndex = 1;
                Effects.AddChild(jumpDust);
                jumpDust.Emitting = true;
                await ToSignal(jumpDust, GpuParticles2D.SignalName.Finished);
                jumpDust.QueueFree();
                break;
            }
            case "LandDust":
            {
                var landDust = LandDust.Instantiate<GpuParticles2D>();
                landDust.GlobalPosition = Player.GlobalPosition;
                landDust.ZIndex = 1;
                Effects.AddChild(landDust);
                landDust.Emitting = true;
                await ToSignal(landDust, GpuParticles2D.SignalName.Finished);
                landDust.QueueFree();
                break;
            }
            
            case "LeftWallJumpDust":
                var leftWallJumpDust = LeftWallJumpDust.Instantiate<GpuParticles2D>();
                leftWallJumpDust.GlobalPosition = Player.GlobalPosition + Vector2.Left * 3;
                leftWallJumpDust.ZIndex = 1;
                Effects.AddChild(leftWallJumpDust);
                leftWallJumpDust.Emitting = true;
                await ToSignal(leftWallJumpDust, GpuParticles2D.SignalName.Finished);
                leftWallJumpDust.QueueFree();
                break;
            
            case "RightWallJumpDust":
                var rightWallJumpDust = RightWallJumpDust.Instantiate<GpuParticles2D>();
                rightWallJumpDust.GlobalPosition = Player.GlobalPosition + Vector2.Right * 3;
                rightWallJumpDust.ZIndex = 1;
                Effects.AddChild(rightWallJumpDust);
                rightWallJumpDust.Emitting = true;
                await ToSignal(rightWallJumpDust, GpuParticles2D.SignalName.Finished);
                rightWallJumpDust.QueueFree();
                break;
        }
    }

    public async Task PlayDashEffect(Vector2 direction)
    {
        DashDust.Emitting = true;
        _ = PlayShockWaveEffect();
        _ = PlaySlashEffect(direction);
        await ToSignal(GetTree().CreateTimer(0.25f), Timer.SignalName.Timeout);
        DashDust.Emitting = false;  
    }
    
    public async Task PlaySlashEffect(Vector2 direction)
    {
        var slashEffect = SlashEffect.Instantiate<Sprite2D>();
        slashEffect.RotationDegrees = direction switch
        {
            { X: 0} => 90,
            { Y: 0 } => 0,
            { X: > 0, Y: > 0 } => 45,
            { X: < 0, Y: < 0} => 45,
            { X: > 0, Y: < 0 } => -45,
            { X: < 0, Y: >0 } => -45,
            _ => slashEffect.RotationDegrees
        };
        slashEffect.GlobalPosition = Player.GlobalPosition + Vector2.Up * 6;
        Effects.AddChild(slashEffect);
        var animation = slashEffect.GetNode<AnimationPlayer>("AnimationPlayer");
        animation.Play("Slash");
        await ToSignal(animation, AnimationMixer.SignalName.AnimationFinished);
        Effects.RemoveChild(slashEffect);
    }

    public async Task PlayShockWaveEffect()
    {
        // GD.Print("ShockWaveEffects Count" + ShockWaveEffects.Count);
        var shockWaveEffect = ShockWaveEffect.Instantiate<ColorRect>();
        shockWaveEffect.GlobalPosition = Player.GlobalPosition + new Vector2(-20, -20);
        shockWaveEffect.ZIndex = 1;
        Effects.AddChild(shockWaveEffect);
        var animation = shockWaveEffect.GetNode<AnimationPlayer>("AnimationPlayer");
        animation.Play("ShockWave");
        await ToSignal(animation, AnimationMixer.SignalName.AnimationFinished);
        Effects.RemoveChild(shockWaveEffect);
    }

    public void ChangeHairColorDash0()
    {
        PlayerHair.Modulate = new(Constants.HairColorDash0);
    }
    
    public void ChangeHairColorDash1()
    {
        PlayerHair.Modulate = new(Constants.HairColorDash1);
    }
    
    public void ChangeHairColorDash2()
    {
        PlayerHair.Modulate = new(Constants.HairColorDash2);
    }

    public void ClearEffects()
    {
        foreach (var child in Effects.GetChildren())
        {
            if (child.Name == "DashDust")
                continue;
            Effects.RemoveChild(child);
        }
    }
}