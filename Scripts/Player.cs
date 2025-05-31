using Godot;
using System;
using System.Threading.Tasks;

public partial class Player : CharacterBody2D
{
    public Globals Globals { get; set; }
    public Vector2 LiftSpeed { get; set; }
    public EffectsManager EffectsManager { get; set; }
    public SoundManager SoundManager { get; set; }
    
    public Vector2 RespawnPoint { get; set; }
    public Facings Facing { get; set; } = Facings.Right;
    public bool Ducking { get; set; }
    public string InRoomName { get; set; }
    public int Dashes { get; set; } = 1;
    public bool DashStartOnFloor { get; set; }
    public Vector2 LastAim { get; set; }
    public float VarJumpSpeed { get; set; }
    public int MoveX { get; set; }
    public int ForceMoveX { get; set; }
    public int MoveY { get; set; }
    public int SuperWallJumpDir = 1;
    public Vector2 InputMove { get; set; }
    public float Stamina { get; set; } = Constants.ClimbMaxStamina; // 体力值
    public float MaxFall { get; set; } = Constants.MaxFall;
    public bool JumpFlag { get; set; } = true; // 控制SuperDash以及变体之后会不会触发Normal的普通Jump
    public bool AutoJump { get; set; }

    public Vector2 Speed;
    public bool IsFirstIn { get; set; } = true;
    public bool IsDie { get; set; }
    public bool IsResurrection { get; set; }
    public bool CanMove { get; set; } = true;

    public bool WasOnFloor { get; set; }
    public bool CanRefillDash { get; set; }

    public Timer VarJumpTimer;
    public Timer CoyoteTimer;
    public Timer JumpBufferTimer;

    public Timer DashTimer;
    public Timer DashCooldownTimer;
    public Timer DashRefillCooldownTimer;
    public Timer DashBufferTimer;

    public Timer WallSlideTimer;
    public Timer ClimbNoMoveTimer;
    public Timer ForceMoveXTimer;

    public Timer TrailTimer;
    public Timer RippleTimer;
    public Timer RippleTimer2;

    public FollowCamera Camera;

    public AnimationPlayer AnimationPlayer;
    public AnimationPlayer SweatAnimationPlayer;
    public StateMachine StateMachine;
    public Node2D Graphics;
    public Sprite2D Sprite;

    public RayCast2D HandCast;
    public RayCast2D FootCast;

    public GpuParticles2D TiredEffect;
    public Hurt Hurt;

    // 能否解除蹲姿
    public bool CanUnDuck => !TestMove(Transform, Vector2.Up * 5) && IsOnFloor();

    // 能否跳跃
    public bool CanJump
    {
        get
        {
            if (Input.IsActionJustPressed("Jump"))
                JumpBufferTimer.Start();
            var canJump = !JumpBufferTimer.IsStopped() && (IsOnFloor() || !CoyoteTimer.IsStopped());
            return canJump;
        }
    }

    // 能否冲刺
    public bool CanDash
    {
        get
        {
            if (Input.IsActionJustPressed("Dash"))
                DashBufferTimer.Start();
            var canDash = !DashBufferTimer.IsStopped() && DashCooldownTimer.IsStopped() && Dashes > 0;
            return canDash;
        }
    }

    // 能否抓墙
    public bool CanGrab => Input.IsActionPressed("Grab") && IsCollidingAndNotIce;

    public bool IsCollidingAndNotIce => HandCast.IsColliding() && HandCast.GetCollider() is not IceWall;

    // 是否疲劳
    public bool IsTired => Stamina < Constants.ClimbTiredThreshold;

    public PackedScene Silhouette;
    public int RippleCounter;

    public override void _Ready()
    {
        Hurt = GetNode<Hurt>("Hurt");
        Globals = GetTree().GetFirstNodeInGroup("Globals") as Globals;
        EffectsManager = GetTree().GetFirstNodeInGroup("EffectsManager") as EffectsManager;
        SoundManager = GetTree().Root.GetNode<SoundManager>("SoundManager");

        VarJumpTimer = GetNode<Timer>("Timers/VarJumpTimer");
        CoyoteTimer = GetNode<Timer>("Timers/CoyoteTimer");
        JumpBufferTimer = GetNode<Timer>("Timers/JumpBufferTimer");

        DashTimer = GetNode<Timer>("Timers/DashTimer");
        DashCooldownTimer = GetNode<Timer>("Timers/DashCooldownTimer");
        DashRefillCooldownTimer = GetNode<Timer>("Timers/DashRefillCooldownTimer");
        DashBufferTimer = GetNode<Timer>("Timers/DashBufferTimer");

        WallSlideTimer = GetNode<Timer>("Timers/WallSlideTimer");
        ClimbNoMoveTimer = GetNode<Timer>("Timers/ClimbNoMoveTimer");
        ForceMoveXTimer = GetNode<Timer>("Timers/ForceMoveXTimer");
        
        TrailTimer = GetNode<Timer>("Timers/TrailTimer");
        TrailTimer.Timeout += TrailTimerOnTimeout;
        RippleTimer = GetNode<Timer>("Timers/RippleTimer");
        RippleTimer.Timeout += RippleTimerOnTimeout;
        RippleTimer2 = GetNode<Timer>("Timers/RippleTimer2");
        RippleTimer2.Timeout += RippleTimer2OnTimeout;

        Camera = GetNode<FollowCamera>("FollowCamera");

        StateMachine = GetNode<StateMachine>("StateMachine");
        AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        SweatAnimationPlayer = GetNode<AnimationPlayer>("Graphics/Sweat/AnimationPlayer");
        
        Graphics = GetNode<Node2D>("Graphics");
        Sprite = Graphics.GetNode<Sprite2D>("Sprite2D");

        HandCast = GetNode<RayCast2D>("Graphics/HandCast");
        FootCast = GetNode<RayCast2D>("Graphics/FootCast");

        Silhouette = GD.Load<PackedScene>("res://Tscns/Effects/Silhouette.tscn");
    }

    private void RippleTimerOnTimeout()
    {
        if (RippleCounter++ >= 3)
        {
            RippleCounter = 0;
            RippleTimer.Stop();
            return;
        }
        if (Input.IsActionPressed("Jump"))
            Globals.CreateRipple(RippleCounter);
        else
        {
            RippleTimer.Stop();
        }
    }
    
    private void RippleTimer2OnTimeout()
    {
        Globals.CreateRipple(RippleCounter, true, SuperWallJumpDir);
    }

    private void TrailTimerOnTimeout()
    {
        CreateSilhouette();
    }

    public void CreateSilhouette()
    {
        var bodySprite = Graphics.GetNode<Sprite2D>("Sprite2D");
        var bangsSprite = bodySprite.GetNode<Sprite2D>("PlayerHair/Bangs");
        var silhouette = Silhouette.Instantiate<CanvasGroup>();
        GetParent().AddChild(silhouette);
        GetParent().MoveChild(silhouette, GetIndex());

        foreach (var node in bangsSprite.GetChildren())
        {
            if (node is Sprite2D hairSprite)
            {
                var hair = new Sprite2D
                {
                    GlobalPosition = hairSprite.GlobalPosition,
                    Scale = Graphics.Scale,
                    Texture = hairSprite.Texture,
                    ZIndex = hairSprite.ZIndex
                };
                silhouette.AddChild(hair);
            }
        }
        
        var bangs = new Sprite2D
        {
            GlobalPosition = bangsSprite.GlobalPosition,
            Scale = Graphics.Scale,
            Texture = bangsSprite.Texture,
            ZIndex = bangsSprite.ZIndex,
        };
        silhouette.AddChild(bangs);
        
        var body = new Sprite2D
        {
            Hframes = bodySprite.Hframes,
            RegionEnabled = bodySprite.RegionEnabled,
            RegionRect = bodySprite.RegionRect,
            GlobalPosition = bodySprite.GlobalPosition,
            Scale = Graphics.Scale,
            Texture = bodySprite.Texture,
            ZIndex = bodySprite.ZIndex
        };
        silhouette.AddChild(body);
    }

    #region 角色基础操作

    public override void _Process(double delta)
    {
        if (!CanMove)
            return;
        AddInfos();
        Animation();
        
        var scale = Graphics.Scale;
        scale.X = Mathf.MoveToward(scale.X, 1f * (int)Facing, 1.75f * (float)delta);
        scale.Y = Mathf.MoveToward(scale.Y, 1f, 1.75f * (float)delta);
        Graphics.Scale = scale;

        if (IsTired)
        {
            (Sprite.Material as ShaderMaterial)?.SetShaderParameter("hit_effect", 0.3f);
        }
        else
        {
            (Sprite.Material as ShaderMaterial)?.SetShaderParameter("hit_effect", 0);
        }
        
        if (Dashes == 0)
            EffectsManager.ChangeHairColorDash0();
        else if (Dashes == 1)
            EffectsManager.ChangeHairColorDash1();
        else if (Dashes == 2)
            EffectsManager.ChangeHairColorDash2();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!CanMove)
            return;
        // if (Input.IsActionJustPressed("ui_down"))
        //     Engine.TimeScale = 0.1;
        
        InputMove = Input.GetVector("Left", "Right", "Up", "Down").Sign();
        MoveX = !ForceMoveXTimer.IsStopped() ? ForceMoveX : (int)InputMove.X;
        MoveY = (int)InputMove.Y;
        Facing = MoveX != 0 && StateMachine.CurrentState.Name != "ClimbState" ? (Facings)MoveX : Facing;


        if (DashRefillCooldownTimer.IsStopped() && IsOnFloor())
        {
            RefillDash();
        }
        else if (CanRefillDash && DashTimer.IsStopped())
        {
            RefillDash();
        }

        if (MoveY == 1 && IsOnFloor() && Speed.Y >= 0)
        {
            Ducking = true;
        }

        if (IsOnFloor() && MoveY != 1 && CanUnDuck)
        {
            Ducking = false;
        }

        if (IsOnCeiling())
        {
            if (!CorrectX() && Speed.Y < 0)
            {
                VarJumpTimer.Stop();
                Speed.Y = 0;
            }
        }

        if (IsOnFloor())
        {
            RefillStamina();
            if (!WasOnFloor)
                CorrectX();
        }

        if (IsOnWall())
        {
            CorrectY();
        }

        WasOnFloor = IsOnFloor();

        Velocity = Speed;
        MoveAndSlide();

        switch (WasOnFloor)
        {
            case true when !IsOnFloor():
                CoyoteTimer.Start();
                break;
            case false when IsOnFloor():
                Land();
                break;
        }
    }

    public void AddInfos()
    {
        Info.Facing = Facing;
        Info.MoveX = MoveX;
        Info.MoveY = MoveY;
        Info.Speed = Speed;
        Info.CurrentState = StateMachine.CurrentState.Name;
        Info.CanJump = CanJump;
        Info.CanDash = CanDash;
        Info.Dashes = Dashes;
        Info.Ducking = Ducking;
        Info.Stamina = Stamina;
        Info.JumpBufferTime = (float)JumpBufferTimer.TimeLeft;
    }

    public void Move(double delta)
    {
        if (Ducking && IsOnFloor())
        {
            Speed.X = Mathf.MoveToward(Speed.X, 0, Constants.DuckFriction * (float)delta);
        }
        else
        {
            var moveX = MoveX;

            var mult = IsOnFloor() ? 1 : Constants.AirMult;
            var max = Constants.MaxRun;

            if (Mathf.Abs(Speed.X) > max && Mathf.Sign(Speed.X) == moveX)
                Speed.X = Mathf.MoveToward(Speed.X, max * moveX, Constants.RunReduce * mult * (float)delta);
            else
                Speed.X = Mathf.MoveToward(Speed.X, max * moveX, Constants.RunAccel * mult * (float)delta);
        }
    }

    public void PlayJumpEffect()
    {
        Graphics.Scale = new(0.6f * (int)Facing, 1.4f);
        _ = EffectsManager.PlayDust("JumpDust");
    }

    public void PlayLandEffect()
    {
        // var squish = Mathf.Min(ySpeed / Mathf.Abs(Constants.FastMaxFall), 1);
        // var scaleX = Mathf.Lerp(1, 1.6f, squish);
        // var scaleY = Mathf.Lerp(1, 0.4f, squish);
        // Scale *= new Vector2(scaleX, scaleY);
        // Scale = Vector2.One;
        _ = EffectsManager.PlayDust("LandDust");
    }

    public void Jump()
    {
        Speed.X += Constants.JumpHBoost * MoveX;
        Speed.Y = Constants.JumpSpeed;
        VarJumpTimer.WaitTime = Constants.VarJumpTime;
        VarJumpTimer.Start();
        VarJumpSpeed = Speed.Y;
        Ducking = false;
        AutoJump = false;
        Speed += LiftSpeed;
        PlayJumpEffect();
        SoundManager.Sounds.GetNode<AudioStreamPlayer2D>("char_mad_jump").Play();
    }

    public void Land()
    {
        SoundManager.Sounds.GetNode<AudioStreamPlayer2D>("char_mad_land").Play();
        PlayLandEffect();
    }

    public void Dash()
    {
        if (!ForceMoveXTimer.IsStopped())
            LastAim = InputMove;
        else
            LastAim = new(MoveX, MoveY);
        // StateMachine.CurrentState.TransitionTo("DashState");
    }

    public void SuperDash()
    {
        Speed.X = Constants.SuperJumpH * (int)Facing;
        Speed.Y = Constants.JumpSpeed;
        JumpFlag = false;
        PlayJumpEffect();
        
        VarJumpTimer.WaitTime = Constants.VarJumpTime;
        VarJumpTimer.Start();
        JumpBufferTimer.Start();
        RippleTimer.Start();

        if (Speed.Y > 0 && IsOnFloor())
            Ducking = true;

        // 特殊HyperDash
        if (Ducking && CanUnDuck)
        {
            Ducking = false;
            Speed.X *= Constants.DuckSuperJumpXMult;
            Speed.Y *= Constants.DuckSuperJumpYMult;
        }

        VarJumpSpeed = Speed.Y;
    }

    public bool RefillDash(int refillDashCount = 1)
    {
        CanRefillDash = false;
        var refillDashes = 0;
        if (refillDashCount == 1)
            refillDashes = Constants.MaxDashes;
        else if (refillDashCount == 2)
            refillDashes = Constants.MaxDashes2;
        if (Dashes >= refillDashes)
            return false;
        Dashes = refillDashCount;
        return true;
    }

    public void RefillStamina()
    {
        Stamina = Constants.ClimbMaxStamina;
    }

    public void ClimbJump()
    {
        var wallNormalX = (int)GetWallNormal().X;
        Jump();
        if (wallNormalX == 1)
        {
            _ = EffectsManager.PlayDust("LeftWallJumpDust");
            SoundManager.Sounds.GetNode<AudioStreamPlayer2D>("char_mad_jump_climb_left").Play();
        }
        else if (wallNormalX == -1)
        {
            _ = EffectsManager.PlayDust("RightWallJumpDust");
            SoundManager.Sounds.GetNode<AudioStreamPlayer2D>("char_mad_jump_climb_right").Play();
        }
        SweatAnimationPlayer.Play("Jump");
    }

    public bool CanWallJump(int dir)
    {
        return TestMove(Transform, Vector2.Right * dir * Constants.WallJumpCheckDist);
    }

    public void WallJump(int dir)
    {
        Ducking = false;
        VarJumpTimer.WaitTime = Constants.VarJumpTime;
        VarJumpTimer.Start();
        var wallNormalX = (int)GetWallNormal().X;
        if (wallNormalX == 1)
        {
            _ = EffectsManager.PlayDust("LeftWallJumpDust");
            SoundManager.Sounds.GetNode<AudioStreamPlayer2D>("char_mad_jump_wall_left").Play();
        }
        else if (wallNormalX == -1)
        {
            _ = EffectsManager.PlayDust("RightWallJumpDust");
            SoundManager.Sounds.GetNode<AudioStreamPlayer2D>("char_mad_jump_wall_right").Play();
        }

        if (MoveX != 0)
        {
            ForceMoveX = dir;
            ForceMoveXTimer.WaitTime = Constants.SuperWallJumpForceTime;
            ForceMoveXTimer.Start();
        }

        Speed.X = Constants.WallJumpHSpeed * dir;
        Speed.Y = Constants.JumpSpeed;
        //TODO 考虑电梯对速度的加成
        //Speed += LiftBoost;
        VarJumpSpeed = Speed.Y;
    }

    //在墙边Dash时，当前按住上，不按左右时，执行SuperWallJump
    public void SuperWallJump(int dir)
    {
        SuperWallJumpDir = dir;
        Ducking = false;
        VarJumpTimer.WaitTime = Constants.SuperWallJumpVarTime;
        VarJumpTimer.Start();
        var wallNormalX = (int)GetWallNormal().X;
        
        if (wallNormalX == 1)
        {
            _ = EffectsManager.PlayDust("LeftWallJumpDust");
            SoundManager.Sounds.GetNode<AudioStreamPlayer2D>("char_mad_jump_wall_left").Play();
        }
        else if (wallNormalX == -1)
        {
            _ = EffectsManager.PlayDust("RightWallJumpDust");
            SoundManager.Sounds.GetNode<AudioStreamPlayer2D>("char_mad_jump_wall_right").Play();
        }
        SoundManager.Sounds.GetNode<AudioStreamPlayer2D>($"char_mad_jump_superwall").Play();

        RippleTimer2.Start();
        Speed.X = Constants.SuperWallJumpH * dir;
        Speed.Y = Constants.SuperWallJumpSpeed;
        //Speed += LiftBoost;
        VarJumpSpeed = Speed.Y;
    }

    public bool CorrectX()
    {
        var successCorrect = false;
        if (Speed.Y > 0)
        {
            if (StateMachine.CurrentState.Name == "DashState" && !DashStartOnFloor)
            {
                for (var i = 1; i <= Constants.DashCornerCorrection; i++)
                {
                    if (!TestMove(new(0, Position with { X = Position.X + i }), new(0, 1)))
                    {
                        MoveAndCollide(Vector2.Right * i);
                        successCorrect = true;
                        break;
                    }
                }

                for (var i = 1; i <= Constants.DashCornerCorrection; i++)
                {
                    if (!TestMove(new(0, Position with { X = Position.X - i }), new(0, 1)))
                    {
                        MoveAndCollide(Vector2.Right * -i);
                        successCorrect = true;
                        break;
                    }
                }
            }
        }
        else
        {
            for (var i = 1; i <= Constants.UpwardCornerCorrection; i++)
            {
                if (!TestMove(new(0, Position with { X = Position.X + i }), new(0, -1)))
                {
                    MoveAndCollide(Vector2.Right * i);
                    successCorrect = true;
                    break;
                }
            }

            for (var i = 1; i <= Constants.UpwardCornerCorrection; i++)
            {
                if (!TestMove(new(0, Position with { X = Position.X - i }), new(0, -1)))
                {
                    MoveAndCollide(Vector2.Right * -i);
                    successCorrect = true;
                    break;
                }
            }
        }

        return successCorrect;
    }

    public bool CorrectY()
    {
        var successCorrect = false;
        if (StateMachine.CurrentState.Name != "DashState")
            return false;

        if (Speed.Y == 0 && Speed.X != 0)
        {
            for (var i = 1; i <= Constants.DashCornerCorrection; i++)
            {
                if (!TestMove(new(0, Position with { Y = Position.Y + i }), new(Mathf.Sign(Speed.X), 0)))
                {
                    MoveAndCollide(Vector2.Down * i);
                    successCorrect = true;
                    break;
                }
            }

            for (var i = 1; i <= Constants.DashCornerCorrection; i++)
            {
                if (!TestMove(new(0, Position with { Y = Position.Y - i }), new(Mathf.Sign(Speed.X), 0)))
                {
                    MoveAndCollide(Vector2.Down * -i);
                    successCorrect = true;
                    break;
                }
            }
        }

        return successCorrect;
    }

    public async Task Die()
    {
        CanMove = false;
        Visible = false;
        Hurt.BodyEntered -= Hurt.OnBodyEntered;
        IsDie = true;
        var direction = new Vector2((float)GD.RandRange(-3.0, 3.0), (float)GD.RandRange(-3.0, 3.0));
        _ = Camera.Shake(direction, 0.2f);
        IsDie = true;
        await Globals.AwaitFrame(3);
        IsDie = false;
        SoundManager.Sounds.GetNode<AudioStreamPlayer2D>("char_mad_death").Play();
        await ToSignal(GetTree().CreateTimer(1.2f), Timer.SignalName.Timeout);
        GlobalPosition = RespawnPoint;
        await Globals.DeathTransitioned();
        IsResurrection = true;
        await Globals.AwaitFrame(3);
        IsResurrection = false;
        SoundManager.Sounds.GetNode<AudioStreamPlayer2D>("char_mad_revive").Play();
        Reset();
        await ToSignal(GetTree().CreateTimer(0.45f), Timer.SignalName.Timeout);
        Visible = true;
        CanMove = true;
        Dashes = 1;
        Speed = Vector2.Zero;
        Velocity = Vector2.Zero;
        GetNode<CollisionShape2D>("Hurt/HurtBox").Disabled = false;
        Hurt.BodyEntered += Hurt.OnBodyEntered;
    }

    public void Reset()
    {
        VarJumpTimer.Stop();
        CoyoteTimer.Stop();
        JumpBufferTimer.Stop();
        DashTimer.Stop();
        DashCooldownTimer.Stop();
        TrailTimer.Stop();
        DashBufferTimer.Stop();
        RefillDash();
        Speed = Vector2.Zero;
        Stamina = Constants.ClimbMaxStamina;
        Facing = Facings.Right;
        EffectsManager.ClearEffects();
        SweatAnimationPlayer.Play("Idle");
        AnimationPlayer.Play("Idle");
        SetCollision(this, 7, true);
        SetCollision(this, 8, false);
    }

    private void Animation()
    {
        Graphics.Scale = Graphics.Scale with { X = (int)Facing * Mathf.Abs(Graphics.Scale.X) };

        if (StateMachine.CurrentState.Name != "ClimbState")
        {
            if (SweatAnimationPlayer.CurrentAnimation != "Jump")
                SweatAnimationPlayer.Play("Idle");
        }

        if (Ducking)
        {
            AnimationPlayer.Play("Duck");
            return;
        }

        if (StateMachine.CurrentState.Name == "ClimbState")
        {
            AnimationPlayer.Play(MoveY is 0 or 1 ? "WallSlide" : "ClimbUp");
        }

        if (StateMachine.CurrentState.Name != "NormalState")
            return;


        if (Velocity == Vector2.Zero)
        {
            AnimationPlayer.Play("Idle");
        }
        else
        {
            AnimationPlayer.Play(Mathf.Abs(Speed.X) < Constants.MaxRun * 0.5f ? "RunSlow" : "RunFast");
        }


        switch (Velocity.Y)
        {
            case < 0:
                AnimationPlayer.Play(Speed.X > Constants.MaxRun ? "JumpFast" : "JumpSlow");
                break;
            case > 0:
                if (Speed.X > Constants.MaxRun || Speed.Y > Constants.MaxFall)
                    AnimationPlayer.Play("FallFast");
                else
                    AnimationPlayer.Play("FallSlow");
                break;
        }

        if (!IsOnFloor())
        {
            //Wall Slide
            if (IsOnWallOnly() && (int)GetWallNormal().X == -MoveX && Speed.Y > 0)
                AnimationPlayer.Play("WallSlide");
        }
    }
    
    

    public void SetCollision(Player player, int layer, bool value)
    {
        player.SetCollisionLayerValue(layer, value);
        player.SetCollisionMaskValue(layer, value);
        player.HandCast.SetCollisionMaskValue(layer, value);
        player.FootCast.SetCollisionMaskValue(layer, value);
    }

    #endregion

    #region 实体交互操作
    // 踩冰球 雪球 oshiro时使用，并非spring
    // public void Bounce(float fromY)
    // {
    //     MoveAndCollide(new(0, Mathf.Clamp(fromY - (GlobalPosition.Y + 6), -4, 4)));
    //     GD.Print(Mathf.Clamp(fromY - (GlobalPosition.Y + 6), -4, 4));
    //     CanRefillDash = true;
    //     // RefillDash();
    //     RefillStamina();
    //     CoyoteTimer.Stop();
    //     VarJumpTimer.Start();
    //     AutoJump = true;
    //     Speed.X = 0f;
    //     Speed.Y = Constants.BounceSpeed;
    //     VarJumpSpeed = Speed.Y;
    //     StateMachine.CurrentState.TransitionTo("NormalState");
    //     // this.Sprite.Scale = new Vector2(0.5f, 1.5f);
    // }
    
    public void SuperBounce(float fromY)
    {
        MoveAndCollide(new(0, Mathf.Clamp(fromY - (GlobalPosition.Y + 6), -4, 4)));
        _ = Camera.Shake(Vector2.Up, 0.1f);
        RefillDash();
        RefillStamina();
        CoyoteTimer.Stop();
        VarJumpTimer.WaitTime = Constants.SuperBounceVarJumpTime;
        VarJumpTimer.Start();
        AutoJump = true;
        Speed.X = 0f;
        Speed.Y = Constants.SuperBounceSpeed;
        VarJumpSpeed = Speed.Y;
        StateMachine.CurrentState.TransitionTo("NormalState");
        // this.Sprite.Scale = new Vector2(0.5f, 1.5f);
    }
    
    public void SideBounce(int dir, float fromX, float fromY)
    {
        MoveAndCollide(new(0, Mathf.Clamp(fromY - (GlobalPosition.Y + 6), -4, 4)));
        if (dir > 0)
            MoveAndCollide(new(Mathf.Clamp(fromX - (GlobalPosition.X + 4), -4, 4), 0));
        else if (dir < 0)
            MoveAndCollide(new(Mathf.Clamp(fromX - (GlobalPosition.X + 4), -4, 4), 0));
        
        _ = Camera.Shake(Vector2.Right * dir, 0.1f);
        RefillDash();
        RefillStamina();
        CoyoteTimer.Stop();
        VarJumpTimer.Start();
        ForceMoveX = dir;
        ForceMoveXTimer.WaitTime = Constants.SideBounceForceMoveXTime;
        ForceMoveXTimer.Start();
        AutoJump = true;
        Speed.X = Constants.SideBounceSpeed * dir;
        Speed.Y = Constants.BounceSpeed;
        VarJumpSpeed = Speed.Y;
        StateMachine.CurrentState.TransitionTo("NormalState");
        // this.Sprite.Scale = new Vector2(0.5f, 1.5f);
    }
    

#endregion
}