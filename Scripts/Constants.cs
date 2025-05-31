using Godot;
using System;

public partial class Constants : Node
{
    public const bool EnableWallSlide = true;
    public const bool EnableJumpGrace = true;
    public const bool EnableWallBoost = true;

    public const float Gravity = 900f; //重力

    public const float HalfGravThreshold = 40f; //滞空时间阈值
    public const float MaxFall = 160; //普通最大下落速度
    public const float FastMaxFall = 240f; //快速最大下落速度

    public const float FastMaxAccel = 300f; //快速下落加速度

    //最大移动速度
    public const float MaxRun = 90f;

    //Hold情况下的最大移动速度
    public const float HoldingMaxRun = 7f;

    //空气阻力
    public const float AirMult = 0.65f;

    //移动加速度
    public const float RunAccel = 1000f;

    //移动减速度
    public const float RunReduce = 400f;

    
    public const float JumpSpeed = -105f; //最大跳跃速度
    public const float VarJumpTime = 0.2f; //跳跃持续时间(跳起时,会持续响应跳跃按键[VarJumpTime]秒,影响跳跃的最高高度);
    public const float JumpHBoost = 40f; //退离墙壁的力
    public const float JumpGraceTime = 0.1f; //土狼时间

    #region WallJump

    public const int WallJumpCheckDist = 3;
    public const float WallJumpForceTime = .16f; //墙上跳跃强制时间
    public const float WallJumpHSpeed = MaxRun + JumpHBoost;

    #endregion

    #region SuperWallJump

    public const float SuperJumpSpeed = JumpSpeed;
    public const float SuperJumpH = 260f;
    public const float SuperWallJumpSpeed = -160f;
    public const float SuperWallJumpVarTime = .25f;
    public const float SuperWallJumpForceTime = .2f;
    public const float SuperWallJumpH = MaxRun + JumpHBoost * 2;

    #endregion

    #region WallSlide

    public const float WallSpeedRetentionTime = .06f; //撞墙以后可以x允许的保持速度的时间
    public const float WallSlideTime = 1.2f; //墙壁滑行时间
    public const float WallSlideStartMax = 20f;

    #endregion

    #region Dash相关参数

    public const float DashSpeed = 240f; //冲刺速度
    public const float EndDashSpeed = 160f; //结束冲刺速度
    public const float EndDashUpMult = .75f; //如果向上冲刺，阻力。
    public const float DashTime = .15f; //冲刺时间
    public const float DashCooldown = .2f; //冲刺冷却时间，
    public const float DashRefillCooldown = .1f; //冲刺重新装填时间
    public const int DashHJumpThruNudge = 6; //
    public const int DashCornerCorrection = 4; //水平Dash时，遇到阻挡物的可纠正像素值
    public const int DashVFloorSnapDist = 3; //DashAttacking下的地面吸附像素值
    public const float DashAttackTime = .3f; //
    public const int MaxDashes = 1;
    public const int MaxDashes2 = 2;
    public const float DodgeSlideSpeedMult = 1.2f; // 下蹲滑行速度倍率

    #endregion

    #region Climb参数

    public const float ClimbMaxStamina = 110; //最大耐力
    public const float ClimbUpCost = 100 / 2.2f; //向上爬得耐力消耗
    public const float ClimbStillCost = 100 / 10f; //爬着不动耐力消耗
    public const float ClimbJumpCost = 110 / 4f; //爬着跳跃耐力消耗
    public const int ClimbCheckDist = 2; //攀爬检查像素值
    public const int ClimbUpCheckDist = 2; //向上攀爬检查像素值
    public const float ClimbNoMoveTime = .1f;
    public const float ClimbTiredThreshold = 20f; //表现疲惫的阈值
    public const float ClimbUpSpeed = -45f; //上爬速度
    public const float ClimbDownSpeed = 80f; //下爬速度
    public const float ClimbSlipSpeed = 30f; //下滑速度
    public const float ClimbAccel = 900f; //下滑加速度
    public const float ClimbGrabYMult = .2f; //攀爬时抓取导致的Y轴速度衰减
    public const float ClimbHopY = -120f; //Hop的Y轴速度
    public const float ClimbHopX = 100f; //Hop的X轴速度
    public const float ClimbHopForceTime = .2f; //Hop时间
    public const float ClimbJumpBoostTime = .2f; //WallBoost时间
    public const float ClimbHopNoWindTime = .3f; //Wind情况下,Hop会无风0.3秒

    #endregion

    #region Duck参数

    public const float DuckFriction = 500f;
    public const float DuckSuperJumpXMult = 1.25f;
    public const float DuckSuperJumpYMult = .5f;

    #endregion

    #region Corner Correct

    public const int UpwardCornerCorrection = 4; //向上移动，X轴上边缘校正的最大距离

    #endregion

    public const float LaunchedMinSpeedSq = 196;
    
    public const float BounceVarJumpTime = .2f;
    public const float BounceSpeed = -140f;
    public const float SuperBounceVarJumpTime = .2f;
    public const float SuperBounceSpeed = -185f;
    public const float SideBounceSpeed = 240;
    public const float SideBounceForceMoveXTime = .3f;


    public const string FocusColor = "#90ff85";
    public const string UnFocusColor = "#ffffff";
    public const string DisplayColor = "#415e82";
    
    
    public const string HairColorDash0 = "#46beff";
    public const string HairColorDash1 = "#ac3232";
    public const string HairColorDash2 = "#ff9cff";
}