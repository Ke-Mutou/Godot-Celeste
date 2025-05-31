using Godot;
using System;
using System.Threading.Tasks;

public partial class SelectUI : Node2D
{
    public SceneManager SceneManager { get; set; }
    public AnimationPlayer AnimationPlayer { get; set; }
    public Node2D Icon { get; set; }
    
    public MainInterface Mountain { get; set; }
    
    public int Index = 1;
    public bool CanMove = true;

    public override void _Ready()
    {
        SceneManager = GetTree().Root.GetNode<SceneManager>("SceneManager");
        AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        Icon = GetNode<Node2D>("Node2D");
        Mountain = GetParent().GetParent().GetNode<MainInterface>("MainInterface");
    }

    public override void _Process(double delta)
    {
        
        if (SceneManager.CurrentScene == "SelectUI")
        {
            _ = MoveUI();

            if (Input.IsActionJustPressed("ui_cancel"))
            {
                AnimationPlayer.Play("Exit");
                Mountain.SetCurrentCamera(0);
                Index = 1;
            }
            else if (Input.IsActionJustPressed("ui_accept"))
            {
                GetParent().GetNode<MainUI>("MainUI")
                    .TweenShake(GetParent().GetNode<MainUI>("MainUI").GetNode<Control>("Control2/Confirm"));
                Game(Index);
            }
        }
    }

    public void Game(int level)
    {
        switch (level)
        {
            case 1:
                GetTree().ChangeSceneToPacked(SceneManager.Level1GameScene);
                SceneManager.CurrentScene = "万家灯火";
                break;
            case 2:
                GetTree().ChangeSceneToPacked(SceneManager.Level2GameScene);
                SceneManager.CurrentScene = "Glyph";
                break;
            case 3:
                GetTree().ChangeSceneToPacked(SceneManager.TestGameScene);
                SceneManager.CurrentScene = "Test";
                break;
        }
    }

    public async Task MoveUI()
    {
        if (!CanMove)
            return;
        if (Input.IsActionJustPressed("Left"))
        {
            if (Index == 1)
                return;
            --Index;
            var tween = GetTree().CreateTween();
            tween.TweenProperty(Icon, "position", Icon.Position + Vector2.Right * 38, 0.1f);
            CanMove = false;
            await ToSignal(tween, Tween.SignalName.Finished);
            Mountain.SetCurrentCamera(Index);
            CanMove = true;
        }
        else if (Input.IsActionJustPressed("Right"))
        {
            if (Index == 3)
                return;
            ++Index;
            var tween = GetTree().CreateTween();
            tween.TweenProperty(Icon, "position", Icon.Position + Vector2.Left * 38, 0.1f);
            CanMove = false;
            await ToSignal(tween, Tween.SignalName.Finished);
            Mountain.SetCurrentCamera(Index);
            CanMove = true;
        }
    }

    public void MainUI()
    {
        var mainUI = GetParent().GetNode<MainUI>("MainUI");
        SceneManager.CurrentScene = "MainUI";
        mainUI.ClimbControl.GrabFocus();
        mainUI.AnimationPlayer.Play("RESET");
        mainUI.AnimationPlayer.Play("EnterT");
    }
}