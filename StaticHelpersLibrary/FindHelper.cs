using Godot;
using System;

public static class FindHelper
{
    public static UserDataHolder FindUserDataHolder(Node caller)
    {
        return caller.GetTree().Root.GetNode<UserDataHolder>("Temple/UserDataHolder");
    }
}
