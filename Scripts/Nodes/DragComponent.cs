using Godot;

namespace CelesteInHeart.Nodes;

[GlobalClass]
public partial class DragComponent : Control
{
    private bool _isDragging;
    private Vector2 _offset;

    private Control _parent;

    public override void _Ready()
    {
        _parent = GetParent<Control>();
        Size = _parent.Size;
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mb)
        {
            switch (mb.ButtonIndex)
            {
                case MouseButton.Left when mb.Pressed:
                    _isDragging = true;
                    _offset = GlobalPosition - GetGlobalMousePosition();
                    break;
                case MouseButton.Left when !mb.Pressed:
                    _isDragging = false;
                    break;
            }
        }
        else if (_isDragging && @event is InputEventMouseMotion)
        {
            // CreateTween().TweenProperty(_parent, "global_position", GetGlobalMousePosition() + _offset, 0.02);
            _parent.GlobalPosition = GetGlobalMousePosition() + _offset;
        }
    }
}