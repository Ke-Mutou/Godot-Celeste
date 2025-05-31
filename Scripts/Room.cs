using Godot;
using System;
using System.Threading.Tasks;


public partial class Room : Area2D
{
    public Vector2 RoomSize { get; set; }
    public Vector2 RoomCenter { get; set; }
    public bool IsFixed { get; set; } = true;
    [Export] public AudioStreamMP3 Bgm { get; set; }

    public Vector2 DefaultRoomSize { get; } = new(320, 184);

    private FollowCamera _followCamera;
    private CollisionShape2D _area;

    public override void _Ready()
    {
        _area = GetNode<CollisionShape2D>("CollisionShape2D");
        RoomSize = _area.Shape.GetRect().Size;
        RoomCenter = _area.GlobalPosition;
        
        BodyEntered += OnBodyEntered;

        _followCamera = GetTree().GetFirstNodeInGroup("FollowCamera") as FollowCamera;
    }
    
    public void SetCollision(Player player, int layer, bool value)
    {
        player.SetCollisionLayerValue(layer, value);
        player.SetCollisionMaskValue(layer, value);
        player.HandCast.SetCollisionMaskValue(layer, value);
        player.FootCast.SetCollisionMaskValue(layer, value);
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is not Player player)
            return;
        player.RefillDash();
        
        SetCollision(player, 7, true);
        Info.RoomSize = RoomSize;
        Info.RoomName = Name;
        Info.RoomCenter = RoomCenter;
        player.InRoomName = Name;

        _followCamera.SetLimits(Info.RoomCenter.X - RoomSize.X / 2, Info.RoomCenter.Y - RoomSize.Y / 2,
            Info.RoomCenter.X + RoomSize.X / 2, Info.RoomCenter.Y + RoomSize.Y / 2);

        
        if (player.IsFirstIn)
        {
            _ = _followCamera.TransitionedRoom(0);
            player.IsFirstIn = false;
            _followCamera.PositionSmoothingEnabled = true;
            _followCamera.PositionSmoothingSpeed = 5;
        }
        else
        {
            _ = _followCamera.TransitionedRoom(0.5f);
            player.MoveAndCollide(player.Speed.Sign() * 10);
        }


    }

    
}