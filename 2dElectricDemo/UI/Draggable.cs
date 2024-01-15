using Godot;
using System;

public partial class Draggable : Button
{
    bool held = false;
    Vector2 mouseOffset;

    public override void _Ready()
    {
        ButtonDown += press;
        ButtonUp += release;
    }

    void press()
    {
        held = true;
        mouseOffset = GetViewport().GetMousePosition() - Position;
    }

    void release()
    {
        held=false;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (!held) return;

        SetPosition(GetViewport().GetMousePosition() - mouseOffset);
    }
}
