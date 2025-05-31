using Godot;
using System;
using System.Threading.Tasks;

[GlobalClass]
public partial class DashState : State
{
    public Vector2 DashDirection { get; set; }
    // 冲刺前的速度
    private Vector2 BeforeDashSpeed { get; set; }
    
    private Player _player;
    
    public override void Enter()
    {
        GD.Print(Name);
        _player = GetParent().GetParent<Player>();
        _player.AnimationPlayer.Play("Dash");
        
        _ = _player.Globals.Freeze(0.06f);
        
        _player.DashTimer.Start();
        _player.DashCooldownTimer.Start();
        _player.DashRefillCooldownTimer.Start();
        
        _player.TrailTimer.Start();
        _player.CreateSilhouette();
        DashDirection = _player.LastAim == Vector2.Zero ? Vector2.Right * (int)_player.Facing : _player.LastAim.Normalized();
        if (DashDirection.X > 0)
            _player.SoundManager.Sounds.GetNode<AudioStreamPlayer2D>("char_mad_dash_red_left").Play();
        else if (DashDirection.X < 0)
            _player.SoundManager.Sounds.GetNode<AudioStreamPlayer2D>("char_mad_dash_red_right").Play();
        else if (DashDirection.Y > 0)
            _player.SoundManager.Sounds.GetNode<AudioStreamPlayer2D>("char_mad_dash_red_left").Play();
        else if (DashDirection.Y < 0)
            _player.SoundManager.Sounds.GetNode<AudioStreamPlayer2D>("char_mad_dash_red_right").Play();
            
            
        // 保存冲刺前的速度并重置当前速度
        BeforeDashSpeed = _player.Speed;
        _player.Speed = Vector2.Zero;
        _player.DashStartOnFloor = _player.IsOnFloor();
        
        _ = _player.Camera.Shake(_player.LastAim == Vector2.Zero ? Vector2.Right * (int)_player.Facing : _player.LastAim,
            0.2f);
        
        // 减少冲刺1次
        _player.Dashes = Mathf.Max(0, _player.Dashes - 1);
        
        
        _ = _player.EffectsManager.PlayDashEffect(_player.LastAim == Vector2.Zero ? Vector2.Right * (int)_player.Facing : _player.LastAim.Normalized());
    }

    public override void Exit()
    {
        // 判断能否刷新恢复冲刺数量
        if (_player.DashRefillCooldownTimer.IsStopped() && !_player.DashTimer.IsStopped() && DashDirection.Y == 0)
            _player.Dashes = Mathf.Min(_player.Dashes + 1, Constants.MaxDashes);
        _player.TrailTimer.Stop();
    }

    public override void Update(double delta)
    {
    }

    public override void PhysicsUpdate(double delta)
    {
        if (!_player.CanMove)
            return;
        // 控制凌波微步，放在SuperDash会导致WaveDash变成SuperDash
        if (_player.IsOnFloor() && DashDirection.X != 0 && DashDirection.Y > 0 && _player.Speed.Y > 0)
        {
            DashDirection = new(Math.Sign(DashDirection.X), 0);
            _player.Speed.Y = 0;
            _player.Speed.X *= Constants.DodgeSlideSpeedMult;
            _player.Ducking = true;
        }
        
        if (DashDirection.Y == 0)
        {
            if (_player.CanJump)
            {
                _player.SuperDash();
                
                TransitionTo("NormalState");
                return; // 反向需要提前退出避免进入后续帧
            }
        }
        DashHandle();
        
        // 处理垂直向上冲刺时的超级墙跳
        if (DashDirection is { X: 0, Y: -1 })
        {
            if (Input.IsActionJustPressed("Jump"))
            {
                // 检查两侧墙壁并执行超级墙跳
                if (_player.CanWallJump(1))
                {
                    _player.SuperWallJump(-1);
                    TransitionTo("NormalState");
                    return;
                }

                if (_player.CanWallJump(-1))
                {
                    _player.SuperWallJump(1);
                    TransitionTo("NormalState");
                    return;
                }
            }
        }
    }

    public void DashHandle()
    {
        // 获取冲刺方向并计算新速度
        var dir = _player.LastAim == Vector2.Zero ? Vector2.Right * (int)_player.Facing : _player.LastAim.Normalized();
        var newSpeed =  dir * Constants.DashSpeed;
        
        // 处理惯性，保持较大的水平速度
        if (Math.Sign(BeforeDashSpeed.X) == Math.Sign(newSpeed.X) && 
            Math.Abs(BeforeDashSpeed.X) > Math.Abs(newSpeed.X))
        {
            newSpeed.X = BeforeDashSpeed.X;
        }
        
        _player.Speed = newSpeed;
        
        // 设置冲刺方向和面向
        DashDirection = dir;
        
        if (DashDirection.X != 0)
            _player.Facing = (Facings)Math.Sign(DashDirection.X);

        if (!_player.DashTimer.IsStopped())
            return;
        if (DashDirection.Y <= 0)
        {
            _player.Speed = DashDirection * Constants.EndDashSpeed;
        }

        if (_player.Speed.Y < 0)
            _player.Speed.Y *= Constants.EndDashUpMult;

        TransitionTo("NormalState");
    }
}
