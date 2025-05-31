using Godot;
using System;

public enum Bus
{
    Master,
    Music,
    Sounds
}

public partial class SettingUI : Control
{
    public SceneManager SceneManager { get; set; }
    public SoundManager SoundManager { get; set; }
    public AnimationPlayer AnimationPlayer;
    
    public Control KeyboardControl { get; set; }
    public Control FullScreenControl { get; set; }
    public Control WindowScaleControl { get; set; }
    public Control VerticalSyncControl { get; set; }
    public Control FPSControl { get; set; }
    public Control MusicControl { get; set; }
    public Control SoundsControl { get; set; }

    public int MusicVolume { get; set; }
    public int SoundsVolume { get; set; }

    public override void _Ready()
    {
        SceneManager = GetTree().Root.GetNode<SceneManager>("SceneManager");
        SoundManager = GetTree().Root.GetNode<SoundManager>("SoundManager");
        
        AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        
        KeyboardControl = GetNode<Control>("Keyboard");
        KeyboardControl.FocusEntered += () =>
        {
            KeyboardControl.GetNode<Label>("Label").LabelSettings.FontColor = new(Constants.FocusColor);
            SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_button_lowkey").Play();
        };
        KeyboardControl.FocusExited += () =>
        {
            KeyboardControl.GetNode<Label>("Label").LabelSettings.FontColor = new(Constants.UnFocusColor);
        };
        KeyboardControl.GuiInput += @event =>
        {
            if (SceneManager.CurrentScene == "SettingUI")
            {
                if (@event.IsActionPressed("ui_accept"))
                {
                    GD.Print("VAR1");
                    SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_button_select").Play();
                    GetNode<TextureRect>("TextureRect").Visible = true;
                    SceneManager.CurrentScene = "Keyboard";
                }
            }
        };
        
        FullScreenControl = GetNode<Control>("FullScreen");
        FullScreenControl.FocusEntered += () =>
        {
            FullScreenControl.GetNode<Label>("Label").LabelSettings.FontColor = new(Constants.FocusColor);
            FullScreenControl.GetNode<Label>("Info").LabelSettings.FontColor = new(Constants.FocusColor);
            AnimationPlayer.Play("FullScreen");
            SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_button_lowkey").Play();
        };
        FullScreenControl.FocusExited += () =>
        {
            FullScreenControl.GetNode<Label>("Label").LabelSettings.FontColor = new(Constants.UnFocusColor);
            FullScreenControl.GetNode<Label>("Info").LabelSettings.FontColor = new(Constants.UnFocusColor);
        };
        FullScreenControl.GuiInput += @event =>
        {
            if (SceneManager.CurrentScene == "SettingUI")
            {
                if (@event.IsActionPressed("ui_left"))
                {
                    if (DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Windowed)
                    {
                        SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_button_invalid").Play();
                        return;
                    }
                    SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_button_toggle_off").Play();
                    DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
                    FullScreenControl.GetNode<Label>("Info").Text = "关";
                }
                else if (@event.IsActionPressed("ui_right"))
                {
                    if (DisplayServer.WindowGetMode() == DisplayServer.WindowMode.Fullscreen)
                    {
                        SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_button_invalid").Play();
                        return;
                    }
                    SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_button_toggle_on").Play();
                    DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
                    FullScreenControl.GetNode<Label>("Info").Text = "开";
                }
            }
        };
        
        WindowScaleControl = GetNode<Control>("WindowScale");
        WindowScaleControl.FocusEntered += () =>
        {
            WindowScaleControl.GetNode<Label>("Label").LabelSettings.FontColor = new(Constants.FocusColor);
            WindowScaleControl.GetNode<Label>("Info").LabelSettings.FontColor = new(Constants.FocusColor);
            AnimationPlayer.Play("WindowScale");
            SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_button_lowkey").Play();
        };
        WindowScaleControl.FocusExited += () =>
        {
            WindowScaleControl.GetNode<Label>("Label").LabelSettings.FontColor = new(Constants.UnFocusColor);
            WindowScaleControl.GetNode<Label>("Info").LabelSettings.FontColor = new(Constants.UnFocusColor);
        };
        WindowScaleControl.GuiInput += @event =>
        {
            if (SceneManager.CurrentScene == "SettingUI")
            {
                if (@event.IsActionPressed("ui_left"))
                {
                    if (DisplayServer.WindowGetSize() == new Vector2I(1920, 1080))
                    {
                        SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_button_invalid").Play();
                        return;
                    }
                    SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_button_toggle_off").Play();
                    DisplayServer.WindowSetSize(new(1920, 1080));
                    WindowScaleControl.GetNode<Label>("Info").Text = "6x";
                }
                else if (@event.IsActionPressed("ui_right"))
                {
                    if (DisplayServer.WindowGetSize() == new Vector2I(2240, 1440))
                    {
                        SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_button_invalid").Play();
                        return;
                    }
                    SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_button_toggle_on").Play();
                    DisplayServer.WindowSetSize(new(2160, 1240));
                    WindowScaleControl.GetNode<Label>("Info").Text = "7x";
                }
            }
        };
        
        VerticalSyncControl = GetNode<Control>("VerticalSync");
        VerticalSyncControl.FocusEntered += () =>
        {
            VerticalSyncControl.GetNode<Label>("Label").LabelSettings.FontColor = new(Constants.FocusColor);
            VerticalSyncControl.GetNode<Label>("Info").LabelSettings.FontColor = new(Constants.FocusColor);
            AnimationPlayer.Play("VerticalSync");
            SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_button_lowkey").Play();
        };
        VerticalSyncControl.FocusExited += () =>
        {
            VerticalSyncControl.GetNode<Label>("Label").LabelSettings.FontColor = new(Constants.UnFocusColor);
            VerticalSyncControl.GetNode<Label>("Info").LabelSettings.FontColor = new(Constants.UnFocusColor);
        };
        VerticalSyncControl.GuiInput += @event =>
        {
            if (SceneManager.CurrentScene == "SettingUI")
            {
                if (@event.IsActionPressed("ui_left"))
                {
                    if (DisplayServer.WindowGetVsyncMode() == DisplayServer.VSyncMode.Disabled)
                    {
                        SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_button_invalid").Play();
                        return;
                    }
                    SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_button_toggle_off").Play();
                    DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Disabled);
                    VerticalSyncControl.GetNode<Label>("Info").Text = "关";
                }
                else if (@event.IsActionPressed("ui_right"))
                {
                    if (DisplayServer.WindowGetVsyncMode() == DisplayServer.VSyncMode.Enabled)
                    {
                        SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_button_invalid").Play();
                        return;
                    }
                    SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_button_toggle_on").Play();
                    DisplayServer.WindowSetVsyncMode(DisplayServer.VSyncMode.Enabled);
                    VerticalSyncControl.GetNode<Label>("Info").Text = "开";
                }
            }
        };
        
        FPSControl = GetNode<Control>("FPS");
        FPSControl.FocusEntered += () =>
        {
            FPSControl.GetNode<Label>("Label").LabelSettings.FontColor = new(Constants.FocusColor);
            FPSControl.GetNode<Label>("Info").LabelSettings.FontColor = new(Constants.FocusColor);
            AnimationPlayer.Play("FPS");
            SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_button_lowkey").Play();
        };
        FPSControl.FocusExited += () =>
        {
            FPSControl.GetNode<Label>("Label").LabelSettings.FontColor = new(Constants.UnFocusColor);
            FPSControl.GetNode<Label>("Info").LabelSettings.FontColor = new(Constants.UnFocusColor);
        };
        FPSControl.GuiInput += @event =>
        {
            if (SceneManager.CurrentScene == "SettingUI")
            {
                if (@event.IsActionPressed("ui_left"))
                {
                    if (Engine.MaxFps == 0)
                    {
                        SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_button_invalid").Play();
                        return;
                    }
                    SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_button_toggle_off").Play();
                    Engine.MaxFps = 0;
                    FPSControl.GetNode<Label>("Info").Text = "\u221e";
                }
                else if (@event.IsActionPressed("ui_right"))
                {
                    if (Engine.MaxFps == 60)
                    {
                        SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_button_invalid").Play();
                        return;
                    }
                    SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_button_toggle_on").Play();
                    Engine.MaxFps = 60;
                    FPSControl.GetNode<Label>("Info").Text = "60";
                }
            }
        };
        
        MusicControl = GetNode<Control>("Music");
        MusicControl.FocusEntered += () =>
        {
            MusicControl.GetNode<Label>("Label").LabelSettings.FontColor = new(Constants.FocusColor);
            MusicControl.GetNode<Label>("Info").LabelSettings.FontColor = new(Constants.FocusColor);
            AnimationPlayer.Play("Music");
            SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_button_lowkey").Play();
        };
        MusicControl.FocusExited += () =>
        {
            MusicControl.GetNode<Label>("Label").LabelSettings.FontColor = new(Constants.UnFocusColor);
            MusicControl.GetNode<Label>("Info").LabelSettings.FontColor = new(Constants.UnFocusColor);
        };
        MusicControl.GuiInput += @event =>
        {
            if (SceneManager.CurrentScene == "SettingUI")
            {
                if (@event.IsActionPressed("ui_left"))
                {
                    var db = SoundManager.GetVolume((int)Bus.Music);
                    if (db == 0)
                    {
                        SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_button_invalid").Play();
                        return;
                    }
                    SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_button_toggle_off").Play();
                    SoundManager.SetVolume((int)Bus.Music, --db);
                    MusicControl.GetNode<Label>("Info").Text = db.ToString();
                }
                else if (@event.IsActionPressed("ui_right"))
                {
                    var db = SoundManager.GetVolume((int)Bus.Music);
                    if (db == 10)
                    {
                        SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_button_invalid").Play();
                        return;
                    }
                    SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_button_toggle_on").Play();
                    SoundManager.SetVolume((int)Bus.Music, ++db);
                    MusicControl.GetNode<Label>("Info").Text = db.ToString();
                }
            }
        };
        
        SoundsControl = GetNode<Control>("Sounds");
        SoundsControl.FocusEntered += () =>
        {
            SoundsControl.GetNode<Label>("Label").LabelSettings.FontColor = new(Constants.FocusColor);
            SoundsControl.GetNode<Label>("Info").LabelSettings.FontColor = new(Constants.FocusColor);
            AnimationPlayer.Play("Sounds");
            SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_button_lowkey").Play();
        };
        SoundsControl.FocusExited += () =>
        {
            SoundsControl.GetNode<Label>("Label").LabelSettings.FontColor = new(Constants.UnFocusColor);
            SoundsControl.GetNode<Label>("Info").LabelSettings.FontColor = new(Constants.UnFocusColor);
        };
        SoundsControl.GuiInput += @event =>
        {
            if (SceneManager.CurrentScene == "SettingUI")
            {
                if (@event.IsActionPressed("ui_left"))
                {
                    var db = SoundManager.GetVolume((int)Bus.Sounds);
                    if (db == 0)
                    {
                        SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_button_invalid").Play();
                        return;
                    }
                    SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_button_toggle_off").Play();
                    SoundManager.SetVolume((int)Bus.Sounds, --db);
                    SoundsControl.GetNode<Label>("Info").Text = db.ToString();
                }
                else if (@event.IsActionPressed("ui_right"))
                {
                    var db = SoundManager.GetVolume((int)Bus.Sounds);
                    if (db == 10)
                    {
                        SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_button_invalid").Play();
                        return;
                    }
                    SoundManager.UI.GetNode<AudioStreamPlayer2D>("ui_main_button_toggle_on").Play();
                    SoundManager.SetVolume((int)Bus.Sounds, ++db);
                    SoundsControl.GetNode<Label>("Info").Text = db.ToString();
                }
            }
        };
    }
    
    public override void _Process(double delta)
    {
        if (SceneManager.CurrentScene == "SettingUI")
        {
            if (Input.IsActionJustPressed("ui_cancel"))
            {
                GetParent().GetNode<MainUI>("MainUI").TweenShake(GetParent().GetNode<MainUI>("MainUI").GetNode<Control>("Control2/Back"));
                AnimationPlayer.Play("Exit");
            }
            else if (Input.IsActionJustPressed("ui_accept"))
            {
                GetParent().GetNode<MainUI>("MainUI").TweenShake(GetParent().GetNode<MainUI>("MainUI").GetNode<Control>("Control2/Confirm"));
            }
        }

        if (SceneManager.CurrentScene == "Keyboard")
        {
            if (Input.IsActionJustPressed("ui_cancel"))
            {
                GetNode<TextureRect>("TextureRect").Visible = false;
                SceneManager.CurrentScene = "SettingUI";
            }
        }

        SetSetting();
    }

    public void MainUI()
    {
        var mainUI = GetParent().GetNode<MainUI>("MainUI");
        SceneManager.CurrentScene = "MainUI";
        mainUI.SettingControl.GrabFocus();
        mainUI.AnimationPlayer.Play("EnterT");
    }

    public void SetSetting()
    {
        var verticalSync = DisplayServer.WindowGetVsyncMode();
        if (verticalSync == DisplayServer.VSyncMode.Disabled)
        {
            VerticalSyncControl.GetNode<Label>("Left").LabelSettings.FontColor = new(Constants.DisplayColor);
            VerticalSyncControl.GetNode<Label>("Right").LabelSettings.FontColor = new(Constants.UnFocusColor);
        }
        else if (verticalSync == DisplayServer.VSyncMode.Enabled)
        {
            VerticalSyncControl.GetNode<Label>("Left").LabelSettings.FontColor = new(Constants.UnFocusColor);
            VerticalSyncControl.GetNode<Label>("Right").LabelSettings.FontColor = new(Constants.DisplayColor);
        }

        var fullScreen = DisplayServer.WindowGetMode();
        if (fullScreen == DisplayServer.WindowMode.Windowed)
        {
            FullScreenControl.GetNode<Label>("Left").LabelSettings.FontColor = new(Constants.DisplayColor);
            FullScreenControl.GetNode<Label>("Right").LabelSettings.FontColor = new(Constants.UnFocusColor);
        }
        else if (fullScreen == DisplayServer.WindowMode.Fullscreen)
        {
            FullScreenControl.GetNode<Label>("Left").LabelSettings.FontColor = new(Constants.UnFocusColor);
            FullScreenControl.GetNode<Label>("Right").LabelSettings.FontColor = new(Constants.DisplayColor);
        }
        
        if (DisplayServer.WindowGetSize() == new Vector2I(1920, 1080))
        {
            WindowScaleControl.GetNode<Label>("Left").LabelSettings.FontColor = new(Constants.DisplayColor);
            WindowScaleControl.GetNode<Label>("Right").LabelSettings.FontColor = new(Constants.UnFocusColor);
        }
        else if (DisplayServer.WindowGetSize() == new Vector2I(2160, 1260))
        {
            WindowScaleControl.GetNode<Label>("Left").LabelSettings.FontColor = new(Constants.UnFocusColor);
            WindowScaleControl.GetNode<Label>("Right").LabelSettings.FontColor = new(Constants.DisplayColor);
        }
        
        if (Engine.MaxFps == 0)
        {
            FPSControl.GetNode<Label>("Left").LabelSettings.FontColor = new(Constants.DisplayColor);
            FPSControl.GetNode<Label>("Right").LabelSettings.FontColor = new(Constants.UnFocusColor);
        }
        else if (Engine.MaxFps == 60)
        {
            FPSControl.GetNode<Label>("Left").LabelSettings.FontColor = new(Constants.UnFocusColor);
            FPSControl.GetNode<Label>("Right").LabelSettings.FontColor = new(Constants.DisplayColor);
        }
        
        if (SoundManager.GetVolume((int)Bus.Music) == 0)
        {
            MusicControl.GetNode<Label>("Left").LabelSettings.FontColor = new(Constants.DisplayColor);
            MusicControl.GetNode<Label>("Right").LabelSettings.FontColor = new(Constants.UnFocusColor);
        }
        else if (SoundManager.GetVolume((int)Bus.Music) == 10)
        {
            MusicControl.GetNode<Label>("Left").LabelSettings.FontColor = new(Constants.UnFocusColor);
            MusicControl.GetNode<Label>("Right").LabelSettings.FontColor = new(Constants.DisplayColor);
        }
        
        if (SoundManager.GetVolume((int)Bus.Sounds) == 0)
        {
            SoundsControl.GetNode<Label>("Left").LabelSettings.FontColor = new(Constants.DisplayColor);
            SoundsControl.GetNode<Label>("Right").LabelSettings.FontColor = new(Constants.UnFocusColor);
        }
        else if (SoundManager.GetVolume((int)Bus.Sounds) == 10)
        {
            SoundsControl.GetNode<Label>("Left").LabelSettings.FontColor = new(Constants.UnFocusColor);
            SoundsControl.GetNode<Label>("Right").LabelSettings.FontColor = new(Constants.DisplayColor);
        }
    }
}
