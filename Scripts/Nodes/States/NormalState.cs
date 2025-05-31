using Godot;
using System;

[GlobalClass]
public partial class NormalState : State
{
    private Player _player;

    public override void Enter()
    {
        _player = GetParent().GetParent<Player>();
        GD.Print(Name);
    }

    public override void Exit()
    {
    }

    public override void Update(double delta)
    {
    }

    public override void PhysicsUpdate(double delta)
    {
        if (!_player.CanMove)
            return;
        // fast fall
        if (_player.MoveY == 1 && _player.Speed.Y >= Constants.MaxFall)
        {
            _player.MaxFall = Mathf.MoveToward(_player.MaxFall, Constants.FastMaxFall, Constants.FastMaxAccel *
                (float)delta);
            // 快速下落缩放
            // var half = Constants.MaxFall + (Constants.FastMaxFall - Constants.MaxFall) * 0.5f;
            // if (_player.Speed.Y >= half)
            // {
            //     var spriteLerp = Math.Min(1f, (_player.Speed.Y - half) / (Constants.FastMaxFall - half));
            //     GD.Print(new Vector2(Mathf.Lerp(1f, 0.5f, spriteLerp), Mathf.Lerp(1f, 1.5f, spriteLerp)));
            //     _player.Graphics.Scale = new(Mathf.Lerp(1f, 0.5f, spriteLerp), Mathf.Lerp(1f, 1.5f, spriteLerp));
            // }
        }
        else
        {
            _player.MaxFall = Mathf.MoveToward(_player.MaxFall, Constants.MaxFall, Constants.FastMaxAccel *
                (float)delta);
        }

        if (!_player.IsOnFloor())
        {
            //Wall Slide
            if (_player.IsOnWall() && (int)_player.GetWallNormal().X == -_player.MoveX)
            {
                if (_player.WallSlideTimer.IsStopped())
                    _player.WallSlideTimer.Start();

                //判断是否向下做Wall滑行
                if (!_player.WallSlideTimer.IsStopped())
                {
                    _player.MaxFall = Mathf.Lerp(Constants.MaxFall, Constants.WallSlideStartMax, (float)_player
                        .WallSlideTimer.TimeLeft / Constants.WallSlideTime);
                }
            }
            else
            {
                _player.WallSlideTimer.Stop();
            }

            var mult = Math.Abs(_player.Speed.Y) < Constants.HalfGravThreshold && Input.IsActionPressed("Jump")
                ? .5f
                : 1f;
            _player.Speed.Y = Mathf.MoveToward(_player.Speed.Y, _player.MaxFall, _player.GetGravity().Y * mult * (float)
                delta);
        }
        else
        {
            _player.Speed.Y = 0;
            _player.JumpFlag = true;
        }

        //Climb
        if (_player.CanGrab && !_player.Ducking && _player.Speed.Y >= 0)
        {
            TransitionTo("ClimbState");
            return;
        }

        // Dash
        if (_player.CanDash)
        {
            _player.Dash();
            TransitionTo("DashState");
            return;
        }

        _player.Move(delta);


        if (Input.IsActionJustPressed("Jump"))
        {
            if (_player.CanJump && _player.JumpFlag)
            {
                _player.Jump();
                _player.PlayJumpEffect();
                _player.JumpBufferTimer.Stop();
            }
            else
            {
                if (_player.CanWallJump(-1))
                {
                    _player.WallJump(1);
                }
                else if (_player.CanWallJump(1))
                {
                    _player.WallJump(-1);
                }
            }
        }


        if (_player.VarJumpTimer.IsStopped())
            return;
        if (Input.IsActionPressed("Jump") || _player.AutoJump)
        {
            _player.Speed.Y = Mathf.Min(_player.Speed.Y, _player.VarJumpSpeed);
        }
        else
        {
            _player.VarJumpTimer.Stop();
        }
    }
}