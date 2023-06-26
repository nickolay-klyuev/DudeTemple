using Godot;
using System;

public partial class Builder : Node
{
    public void Build(int placeIndex, string scenePath)
    {
        GD.Print(scenePath);
    }
}
