using Godot;
using System;

[GlobalClass]
public partial class ClimbState : State
{
    private Player _player;
    private int _lastClimbMove;

    public override void Enter()
    {
        GD.Print(Name);
        _player = GetParent().GetParent<Player>();
        _player.Position += Vector2.Right * 3 * (int)_player.Facing; // 两像素的抓取宽容
        // 停止水平移动
        _player.Speed *= Vector2.Zero;
        // 重置墙壁滑行时间
        _player.WallSlideTimer.Stop();
        // // 重置墙壁推力计时器
        // _player.WallBoost?.ResetTime();
        // 设置攀爬无法移动的冷却时间
        _player.ClimbNoMoveTimer.Start();
    }

    public override void Exit()
    {
    }

    public override void Update(double delta)
    {
        if (!_player.CanMove)
            return;

        switch (_lastClimbMove)
        {
            case -1:
                _player.SweatAnimationPlayer.Play(_player.IsTired ? "Danger" : "Climb");
                _player.Stamina -= Constants.ClimbUpCost * (float)delta;
                break;
            case 0:
                _player.SweatAnimationPlayer.Play(_player.IsTired ? "Danger" : "Still");
                _player.Stamina -= Constants.ClimbStillCost * (float)delta;
                break;
        }
    }

    public override void PhysicsUpdate(double delta)
    {
        if (!_player.CanMove)
            return;
        
        if (!Input.IsActionPressed("Grab") || (!_player.IsCollidingAndNotIce && !_player.FootCast.IsColliding()))
        {
            TransitionTo("NormalState");
            return;
        }
        
        if (_player.CanDash)
        {
            _player.Dash();
            TransitionTo("DashState");
            return;
        }

        if (_player.IsTired && _player.Stamina < 0)
        {
            if (_player.WallSlideTimer.IsStopped())
                _player.WallSlideTimer.Start();

            //判断是否向下做Wall滑行
            if (!_player.WallSlideTimer.IsStopped())
            {
                _player.MaxFall = Mathf.Lerp(Constants.MaxFall, Constants.WallSlideStartMax, (float)_player
                    .WallSlideTimer.TimeLeft / Constants.WallSlideTime);
            }
            else
            {
                _player.WallSlideTimer.Stop();
            }

            _player.Speed.Y = Mathf.MoveToward(_player.Speed.Y, _player.MaxFall, _player.GetGravity().Y * (float)
                delta);
            return;
        }

        if (Input.IsActionJustPressed("Jump"))
        {
            if (_player.MoveX == -(int)_player.Facing)
                _player.WallJump(-(int)_player.Facing);
            else
            {
                _player.Stamina -= Constants.ClimbJumpCost;
                _player.ClimbJump();
            }

            TransitionTo("NormalState");
            return;
        }

        if (!_player.IsCollidingAndNotIce)
        {
            if (_player.Speed.Y < 0)
            {
                // 执行墙壁翻越
                ClimbHop();
            }

            TransitionTo("NormalState");
            return;
        }

        float target = 0;
        if (_player.ClimbNoMoveTimer.IsStopped())
        {
            // 处理向上攀爬
            if (_player.MoveY == -1)
            {
                target = Constants.ClimbUpSpeed;
                if (_player.IsOnCeiling())
                {
                    _player.Speed.Y = Mathf.Min(_player.Speed.Y, 0);
                    target = 0;
                }
            }
            // 处理向下攀爬
            else if (_player.MoveY == 1)
            {
                target = Constants.ClimbDownSpeed;

                if (_player.IsOnFloor())
                {
                    _player.Speed.Y = Mathf.Max(_player.Speed.Y, 0);
                    target = 0;
                }
            }
        }

        _lastClimbMove = Mathf.Sign(target);
        
        _player.Speed.Y = Mathf.MoveToward(_player.Speed.Y, target, Constants.ClimbAccel * (float)delta);
    }

    /// <summary>
    /// 执行墙壁翻越动作
    /// </summary>
    private void ClimbHop()
    {
        _player.Speed = new((int)_player.Facing * Constants.ClimbHopX * 1.4f,
            Math.Min(_player.Speed.Y, Constants.ClimbHopY));
    }
}