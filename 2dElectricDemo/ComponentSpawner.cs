using Godot;
using System;

public partial class ComponentSpawner : Button
{
    [Export] PackedScene Component;

    public override void _Ready()
    {
        this.Pressed += SpawnComponent;
    }

    void SpawnComponent() {
        // TODO: Move spawned thing to more appropriate place, like center of screen
        GetTree().CurrentScene.AddChild(Component.Instantiate());
        
    }
}
