public struct SBuildingData
{
    public string Label { get; private set; }
    public int Cost { get; private set; }
    public string ScenePath { get; private set; }

    public SBuildingData(string label, int cost, string scenePath)
    {
        Label = label;
        Cost = cost;
        ScenePath = scenePath;
    }
}
