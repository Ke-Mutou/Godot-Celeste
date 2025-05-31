using Godot;
using System;
using System.Collections.Generic;

public partial class Trail : Node2D
{
    public override void _Ready()
    {
        var player = GetTree().GetFirstNodeInGroup("Player") as Player;
        var playerSprite = player!.Graphics.GetNode<Sprite2D>("Sprite2D");

        var bangsSprite = playerSprite.GetNode<Sprite2D>("PlayerHair/Bangs");
        
        var spritePlayer = new Sprite2D()
        {
            Texture = playerSprite.Texture,
            RegionEnabled = playerSprite.RegionEnabled,
            RegionRect = playerSprite.RegionRect,
            GlobalPosition = playerSprite.GlobalPosition,
            GlobalScale = playerSprite.GlobalScale,
            Frame = playerSprite.Frame,
            Hframes = playerSprite.Hframes,
            // UseParentMaterial = true
        };
        AddChild(spritePlayer);
        //
        // var spriteBangs = new Sprite2D()
        // {
        //     Texture = bangsSprite.Texture,
        //     GlobalPosition = bangsSprite.GlobalPosition,
        //     GlobalScale = bangsSprite.GlobalScale,
        //     UseParentMaterial = true
        // };
        // AddChild(spriteBangs);
        //
        // foreach (var node in bangsSprite.GetChildren())
        // {
        //     if (node is Sprite2D hair)
        //     {
        //         var spriteHair = new Sprite2D()
        //         {
        //             Texture = hair.Texture,
        //             GlobalPosition = hair.GlobalPosition,
        //             GlobalScale = hair.GlobalScale,
        //             UseParentMaterial = true
        //         };
        //         AddChild(spriteHair);
        //     }
        // }
    }
}
