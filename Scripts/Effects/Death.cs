using Godot;
using System;
using System.Collections.Generic;

public partial class Death : Node2D
{
    public Player Player { get; set; }
    
    private readonly List<Sprite2D> _images = []; // 图片节点列表
    private Vector2 _rotationCenter; // 旋转中心
    private float _radius; // 当前半径，初始为0
    private const float TargetRadius = 17; // 目标半径
    private const float RadiusIncrement = 0.5f; // 每帧增加的半径
    private const int NumberOfImages = 8; // 图片数量
    private const float AngleStep = (float)(Math.PI * 2 / NumberOfImages); // 每个图片之间的夹角（弧度）
    private const float RotationSpeed = 0.2f; // 每帧旋转速度（弧度）
    private const float CenterRotationSpeed = 0.1f; // 中心点集体旋转速度（弧度）

    // 每个图片的独立旋转角度
    private readonly List<float> _individualAngles = [];
    // 中心点的集体旋转角度
    private float _centerRotationAngle;

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
            foreach (var image in _images)
            {
                var tween = GetTree().CreateTween();
                tween.TweenProperty(image, "scale", Vector2.Zero, 0.7f);
            }
        };
    }

    public override void _Process(double delta)
    {
        // 每帧更新图片位置和旋转角度
        if (Player.IsDie) // 检测鼠标点击
        {
            // 获取鼠标点击位置
            _rotationCenter = Player.GetGlobalTransformWithCanvas()[2];
            var shader = Material as ShaderMaterial;
            shader?.SetShaderParameter("flash_color", new Color(Player.Dashes == 1 ? "#ac3232" : "#46beff"));

            // 重置半径和每个图片的独立旋转角度
            _radius = 0;
            _centerRotationAngle = 0; // 重置中心点的集体旋转角度
            for (var i = 0; i < NumberOfImages; i++)
            {
                _individualAngles[i] = 0;
            }

            // 初始化时将所有图片放在中心点
            foreach (var image in _images)
            {
                image.Visible = true;
                image.Position = _rotationCenter;
                image.Rotation = 0;
                image.Scale = new(0.7f, 0.7f); // 重置图片的缩放比例
            }
            _rotateTimer.Start(); // 开始旋转计时器
        }

        // 逐步增加半径，直到达到目标半径
        if (_radius < TargetRadius)
        {
            _radius += RadiusIncrement;
        }

        // 更新中心点的集体旋转角度
        _centerRotationAngle += CenterRotationSpeed;

        // 更新每个图片的位置和旋转角度
        for (var i = 0; i < NumberOfImages; i++)
        {
            // 计算每个图片的当前角度
            var currentAngle = i * AngleStep + _centerRotationAngle;

            // 计算每个图片的新位置
            var offset = new Vector2(_radius, 0).Rotated(currentAngle);
            _images[i].Position = _rotationCenter + offset;

            // 更新每个图片的独立旋转角度
            _individualAngles[i] += RotationSpeed;

            // 设置每个图片的旋转
            _images[i].Rotation = _individualAngles[i];
        }
    }
}