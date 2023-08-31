// using Godot;
// using Godot.Collections;
// using System;

// public partial class Builder : Node
// {
//     [Export]
//     private UserDataHolder _userDataHolder;

//     [Export]
//     [ExportGroup("Building Places")]
//     private Array<NodePath> _buildingPlaceSocketPathes;

//     // Called when the node enters the scene tree for the first time.
// 	public override void _Ready()
// 	{
//         #if DEBUG
//         CheckHelper.CheckNode(_userDataHolder, this);
//         #endif
// 	}

//     public void OnBuildingDataLoaded(Dictionary<int, EFurniture> builtBuildings)
//     {
//         foreach (var builtBuilding in builtBuildings)
//         {
//             Build(builtBuilding.Key, builtBuilding.Value);
//         }
//     }

//     public void BuildAndPushUserData(int placeIndex, EFurniture building)
//     {
//         Build(building);

//         _userDataHolder.AddBuiltBuilding(building);
//     }

//     private void Build(EFurniture building)
//     {
//         string scenePath = BuildingDataHelper.GetBuildingData(building).ScenePath;

//         PackedScene buildingScenePacked = GD.Load<PackedScene>(scenePath);
//         Node3D buildingScene = buildingScenePacked.Instantiate<Node3D>();

//         Node3D buildSocket = GetNode<Node3D>(_buildingPlaceSocketPathes[placeIndex]);
//         ClearBuildingPlaceSocket(buildSocket);
//         buildSocket.AddChild(buildingScene);
//     }

//     public void Unlock(EFurniture building, int cost)
//     {
//         #if DEBUG
//         if (_userDataHolder.IsFurnitureUnlocked(building))
//         {
//             GD.PrintErr($"{Name}: Building is already unlocked!!!!!! It shouldn't be like that!!!!");
//             return;
//         }
//         #endif

//         if (_userDataHolder.RemoveScore(cost))
//         {
//             _userDataHolder.AddUnlockedFurniture(building);
//         }
//     }

//     private void ClearBuildingPlaceSocket(Node3D socket)
//     {
//         if (socket.GetChildren().Count == 0)
//         {
//             return;
//         }

//         foreach (Node child in socket.GetChildren())
//         {
//             child.QueueFree();
//         }
//     }
// }
