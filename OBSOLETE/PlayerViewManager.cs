using Godot;
using System;

public partial class PlayerViewManager : Node
{
    [Export]
    Camera3D DudeCamera;

    [Export]
    Camera3D BowlingCamera;

    // public void ChangeCameraForGameMode(TempleState.GameMode gameMode)
    // {
    //     switch (gameMode)
    //     {
    //         case TempleState.GameMode.Walking:
    //             DudeCamera.MakeCurrent();
    //             break;
    //         case TempleState.GameMode.RollingBalls:
    //             BowlingCamera.MakeCurrent();
    //             break;
    //     }
    // }
}
