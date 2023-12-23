using Godot;
using System;

public static class FindHelper
{
    private const string USER_DATA_HOLDER = "Temple/UserDataHolder";
    private const string BOWLING_BALL_SETUP = "Temple/BowlingBallSetup";

    public static UserDataHolder FindUserDataHolder(Node caller)
    {
        return caller.GetTree().Root.GetNode<UserDataHolder>(USER_DATA_HOLDER);
    }

    public static BowlingBallSetup FindBowlingBallSetup(Node caller)
    {
        return caller.GetTree().Root.GetNode<BowlingBallSetup>(BOWLING_BALL_SETUP);
    }
}
