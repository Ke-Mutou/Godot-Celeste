using Godot;
using System;
using System.Collections.Generic;

public partial class Resurrection : Node2D
{
    public Player Player { get; set; }
    
    private readonly List<Sprite2D> _images = []; // 图片节点列表
    private Vector2 _rotationCenter; // 旋转中心
    private const float TargetRadius = 17; // 目标半径
    private const int NumberOfImages = 8; // 图片数量
    private const float AngleStep = (float)(Math.PI * 2 / NumberOfImages); // 每个图片之间的夹角（弧度）
    private const float RotationSpeed = 0.1f; // 每帧旋转速度（弧度）
    private const float CenterRotationSpeed = 0.1f; // 中心点集体旋转速度（弧度）
    private const float GatherSpeed = 1f; // 向中心聚集的速度

    // 每个图片的独立旋转角度
    private readonly List<float> _individualAngles = [];
    // 中心点的集体旋转角度
    private float _centerRotationAngle;

    // 是否开始向中心聚集
    private bool _isGathering;

    // 旋转计时器
    private Timer _rotateTimer;

    public override void _Ready()
    {
        Player = GetTree().GetFirstNodeInGroup("Player") as Player;
        // 获取所有图片节点
        for (var i = 0; i < NumberOfImages; i++)
        {
            _images.Add(GetNode<Sprite2D>($"Hair0{i}"));
            _individualAngles.Add(0); // 初始化独立旋转角度
            _images[i].Visible = false;
        }

        // 创建旋转计时器
        _rotateTimer = GetNode<Timer>("RotateTimer");
        _rotateTimer.Timeout += () =>
        {
            _isGathering = true; // 开始向中心聚集
        };
    }

    public override void _Process(double delta)
    {
        // 每帧更新图片位置和旋转角度
        if (Player.IsResurrection)
        {
            _rotationCenter = Player.GetGlobalTransformWithCanvas()[2];
            
            _isGathering = false;

            // 初始化时将所有图片放在目标位置
            for (var i = 0; i < NumberOfImages; i++)
            {
                var currentAngle = i * AngleStep;
                var offset = new Vector2(TargetRadius, 0).Rotated(currentAngle);
                _images[i].Position = _rotationCenter + offset;
                _images[i].Rotation = 0;
                _images[i].Scale = new(0.7f, 0.7f); // 重置图片的缩放比例
                _images[i].Visible = true;
            }

            _rotateTimer.Start(); // 开始旋转计时器
        }

        // 更新中心点的集体旋转角度
        _centerRotationAngle += CenterRotationSpeed;

        // 更新每个图片的位置和旋转角度
        for (var i = 0; i < NumberOfImages; i++)
        {
            // 计算每个图片的当前角度
            var currentAngle = i * AngleStep + _centerRotationAngle;

            // 如果开始向中心聚集
            if (_isGathering)
            {
                // 计算每个图片向中心移动的新位置
                var currentPosition = _images[i].Position;
                var direction = (_rotationCenter - currentPosition).Normalized();
                var newPosition = currentPosition + direction * GatherSpeed;
                _images[i].Position = newPosition;

                // 如果图片接近中心点，则停止移动
                if (newPosition.DistanceTo(_rotationCenter) < 1)
                {
                    _images[i].Position = _rotationCenter;
                    _images[i].Visible = false; // 可选：将图片隐藏
                }
            }
            else
            {
                // 计算每个图片的新位置
                var offset = new Vector2(TargetRadius, 0).Rotated(currentAngle);
                _images[i].Position = _rotationCenter + offset;
            }

            // 更新每个图片的独立旋转角度
            _individualAngles[i] += RotationSpeed;

            // 设置每个图片的旋转
            _images[i].Rotation = _individualAngles[i];
        }
    }
}