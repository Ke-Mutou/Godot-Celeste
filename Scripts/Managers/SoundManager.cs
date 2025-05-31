using Godot;
using System;

public partial class SoundManager : Node
{
    public Node2D Music;
    public Node2D Sounds;
    public Node2D UI;

    public override void _Ready()
    {
        Music = GetNode<Node2D>("Music");
        Sounds = GetNode<Node2D>("Sounds");
        UI = GetNode<Node2D>("UI");
    }

    public int GetVolume(int busIndex)
    {
        var db = AudioServer.GetBusVolumeDb(busIndex);
        return Mathf.RoundToInt(Mathf.DbToLinear(db) * 10);
    }

    public void SetVolume(int busIndex, int volume)
    {
        var db =  Mathf.LinearToDb(volume / 10f);
        AudioServer.SetBusVolumeDb(busIndex, db);
    }
}
