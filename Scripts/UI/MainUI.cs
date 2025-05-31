using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class MainUI : Control
{
    public SceneManager SceneManager { get; set; }
    public SoundManager SoundManager { get; set; }
    public MainInterface Mountain { get; set; }
    
    public Control ClimbControl { get; set; }
    public Control SettingControl { get; set; }
    public Control SaluteControl { get; set; }
    public Control ExitControl { get; set; }
    public Control ConfirmControl { get; set; }
    public Control BackControl { get; set; }
    
    public AnimationPlayer AnimationPlayer { get; set; }

    public override void _Ready()
    {
        SceneManager = GetTree().Root.GetNode<SceneManager>("SceneManager");
        SceneManager.CurrentScene = "MainUI";
        SoundManager = GetTree().Root.GetNode<SoundManager>("SoundManager");
        Mountain = GetParent().GetParent().GetNode<MainInterface>("MainInterface");
        
        AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        AnimationPlayer.AnimationFinished += animName =>
        {
            if (animName == "Exit")
                GetTree().ChangeSceneToPacked(SceneManager.TitleScreenScene);
        };
        
        ClimbControl = GetNode<Control>("Control/Climb");
        ClimbControl.FocusEntered += () =>
        {
            ClimbControl.GetNode<Label>("Label").LabelSettings.FontColor = new(Constants.FocusColor);
            var tween = GetTree().CreateTween().SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Cubic);
            tween.TweenProperty(ClimbControl, "position", ClimbControl.Position + Vector2.Up, 0.07f);
            tween.TweenProperty(ClimbControl, "position", ClimbControl.Position - Vector2.Up * 0.5f, 0.07f);
            tween.TweenProperty(ClimbControl, "position", ClimbControl.Position + Vector2.Up * 0.5f, 0.07f);
            tween.TweenProperty(ClimbControl, "position", ClimbControl.Position, 0.07f);
        };
        ClimbControl.FocusExited += () =>
        {
            ClimbControl.GetNode<Label>("Label").LabelSettings.FontColor = new(Constants.UnFocusColor);
        };
        ClimbControl.GuiInput += @event =>
        {
            if (SceneManager.CurrentScene == "MainUI")
            {
                if (@event.IsActionPressed("ui_accept"))
                {
                    AnimationPlayer.Play("Climb");
                    SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_button_climb");
                }
            }
        };
        
        SettingControl = GetNode<Control>("Control/Setting");
        SettingControl.FocusEntered += () =>
        {
            SettingControl.GetNode<Label>("Label").LabelSettings.FontColor = new(Constants.FocusColor);
            var tween = GetTree().CreateTween().SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Cubic);
            tween.TweenProperty(SettingControl, "position", SettingControl.Position + Vector2.Right * 5, 0.1f);
        };
        SettingControl.FocusExited += () =>
        {
            SettingControl.GetNode<Label>("Label").LabelSettings.FontColor = new(Constants.UnFocusColor);
            var tween = GetTree().CreateTween().SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Cubic);
            tween.TweenProperty(SettingControl, "position", SettingControl.Position - Vector2.Right * 5, 0.1f);
        };
        SettingControl.GuiInput += @event =>
        {
            if (SceneManager.CurrentScene == "MainUI")
            {
                if (@event.IsActionPressed("ui_accept"))
                {
                    AnimationPlayer.Play("ExitT");
                    SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_button_select").Play();
                }
            }
        };
        
        SaluteControl = GetNode<Control>("Control/Salute");
        SaluteControl.FocusEntered += () =>
        {
            SaluteControl.GetNode<Label>("Label").LabelSettings.FontColor = new(Constants.FocusColor);
            var tween = GetTree().CreateTween().SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Cubic);
            tween.TweenProperty(SaluteControl, "position", SaluteControl.Position + Vector2.Right * 5, 0.1f);
        };
        SaluteControl.FocusExited += () =>
        {
            SaluteControl.GetNode<Label>("Label").LabelSettings.FontColor = new(Constants.UnFocusColor);
            var tween = GetTree().CreateTween().SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Cubic);
            tween.TweenProperty(SaluteControl, "position", SaluteControl.Position - Vector2.Right * 5, 0.1f);
        };
        SaluteControl.GuiInput += @event =>
        {
            if (SceneManager.CurrentScene == "MainUI")
            {
                if (@event.IsActionPressed("ui_accept"))
                {
                    SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_button_select").Play();
                    return;
                }
            }
        };
        
        ExitControl = GetNode<Control>("Control/Exit");
        ExitControl.FocusEntered += () =>
        {
            ExitControl.GetNode<Label>("Label").LabelSettings.FontColor = new(Constants.FocusColor);
            var tween = GetTree().CreateTween().SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Cubic);
            tween.TweenProperty(ExitControl, "position", ExitControl.Position + Vector2.Right * 5, 0.1f);
        };
        ExitControl.FocusExited += () =>
        {
            ExitControl.GetNode<Label>("Label").LabelSettings.FontColor = new(Constants.UnFocusColor);
            var tween = GetTree().CreateTween().SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Cubic);
            tween.TweenProperty(ExitControl, "position", ExitControl.Position - Vector2.Right * 5, 0.1f);
        };
        ExitControl.GuiInput += @event =>
        {
            if (SceneManager.CurrentScene == "MainUI")
            {
                if (@event.IsActionPressed("ui_accept"))
                {
                    SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_button_select").Play();
                    Exit();
                }
            }
        };
        
        ClimbControl.GrabFocus();
        
        ConfirmControl = GetNode<Control>("Control2/Confirm");
        BackControl = GetNode<Control>("Control2/Back");
    }

    public void TweenShake(Control control)
    {
        var tween = GetTree().CreateTween();
        tween.TweenProperty(control, "scale", new Vector2(1.01f, 1.01f), 0.07f);
        tween.TweenProperty(control, "scale", new Vector2(1, 1), 0.07f);
        tween.TweenProperty(control, "scale", new Vector2(1.01f, 1.01f), 0.07f);
        tween.TweenProperty(control, "scale", new Vector2(1, 1), 0.07f);
    }

    public override void _Process(double delta)
    {
        if (SceneManager.CurrentScene == "MainUI")
        {
            if (Input.IsActionJustPressed("ui_cancel"))
            {
                TweenShake(BackControl);
                SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_button_back").Play();
                TitleScreen();
            }
            else if (Input.IsActionJustPressed("ui_accept"))
            {
                TweenShake(ConfirmControl);
            }
        }
    }

    public void Climb()
    {
        var select = GetParent().GetNode<SelectUI>("SelectUI");
        SceneManager.CurrentScene = "SelectUI";
        select.AnimationPlayer.Play("Enter");
        Mountain.SetCurrentCamera(1);
    }

    public void SettingUI()
    {
        var setting = GetParent().GetNode<SettingUI>("SettingUI");
        SceneManager.CurrentScene = "SettingUI";
        setting.AnimationPlayer.Play("Enter");
    }

    public void Exit()
    {
        GetTree().Quit();
    }

    public void TitleScreen()
    {
        SceneManager.CurrentScene = "TitleScreen";
        AnimationPlayer.Play("Exit");
    }
}
