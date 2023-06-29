public struct SBuildingData
{
    public EBuilding Building { get; private set; }
    public string Label { get; private set; }
    public int Cost { get; private set; }
    public string ScenePath { get; private set; }

    public SBuildingData(EBuilding building, string label, int cost, string scenePath)
    {
        Building = building;
        Label = label;
        Cost = cost;
        ScenePath = scenePath;
    }
}
