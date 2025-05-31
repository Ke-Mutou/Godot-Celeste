using Godot;
using System;

public enum Facings
{
    Left = -1,
    Right = 1
}

public partial class Info : Node
{
    public static Facings Facing { get; set; }
    public static int MoveX { get; set; }
    public static int MoveY { get; set; }
    public static Vector2 Speed { get; set; }
    public static string CurrentState { get; set; }
    public static bool CanJump { get; set; }
    public static bool CanDash { get; set; }
    public static int Dashes { get; set; }
    public static bool Ducking { get; set; }
    public static float Stamina { get; set; }


    public static string RoomName { get; set; }
    public static Vector2 RoomSize { get; set; }
    public static Vector2 RoomCenter { get; set; }
    
    public static float JumpBufferTime { get; set; }
}