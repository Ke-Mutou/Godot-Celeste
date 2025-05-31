using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerHair : CanvasGroup
{
    public Sprite2D Bangs { get; set; }
    [Export] public Vector2 Offset { get; set; } = new(0, 2.3f); // 偏移量

    [Export] public float Speed { get; set; } = 3f; // 基础速度系数
    
    [Export] public Color Col { get; set; } = new("#ac3232"); // 颜色
    [Export] public float Distance { get; set; } = 4f;

    private Vector2[] _points; // 存储每个点的位置
    private readonly List<Sprite2D> _hairs = []; // 存储每个 Sprite2D

    public override void _Ready()
    {
        Bangs = GetNode<Sprite2D>("Bangs");
        // 创建并添加 Sprite2D 节点
        foreach (var child in Bangs.GetChildren())
        {
            if (!child.Name.ToString().Contains("Hair"))
                continue;
            if (child is Sprite2D sprite)
            {
                // sprite.Modulate = Col;
                _hairs.Add(sprite);
            }
        }
        
        _points = new Vector2[_hairs.Count];
    }

    public override void _Process(double delta)
    {
        var anchor = Bangs.GlobalPosition;

        _points[0] = anchor;

        // 更新每个点的位置，速度根据与第一个点的距离动态调整
        for (var i = 1; i < _hairs.Count; i++)
        {

            // 更新位置
            _points[i] += (_points[i - 1] + Offset - _points[i]) * Speed * (float)delta;

            // 计算最大距离（基于当前点和前一个点的半径）
            var maxDistance = GetRadius(_hairs[i]) + GetRadius(_hairs[i - 1]) - Distance;

            // 检查与前一个点的距离是否在允许范围内
            var direction = _points[i] - _points[i - 1];
            var currentDistance = direction.Length();

            if (currentDistance > maxDistance)
            {
                // 如果超过最大距离，将当前点拉回到最大距离范围内
                _points[i] = _points[i - 1] + direction.Normalized() * maxDistance;
            }
        }

        // 更新每个 Sprite2D 的位置和大小
        for (var i = 0; i < _hairs.Count; i++)
        {
            var num = 0.35f + (1 - (float)i / _hairs.Count) * 0.65f; // 缩放系数
            _hairs[i].GlobalPosition = _points[i];
            _hairs[i].Scale = new (num, num); // 假设图片是圆形的，直接用半径作为缩放
            // _hairs[i].Modulate = Col;
        }
    }

    // 获取 Sprite2D 的半径
    private float GetRadius(Sprite2D sprite)
    {
        // 假设图片是圆形的，半径为图片宽度的一半乘以缩放
        return sprite.Texture.GetSize().X * sprite.Scale.X / 2.0f;
    }

    public void ChangeColor()
    {
        foreach (var hair in _hairs)
        {
            hair.Modulate = new("#46beff");
        }
    }
}