public struct SBuildingData
{
    public EFurniture Parent { get; private set;}
    public EFurniture Furniture { get; private set; }
    public string Label { get; private set; }
    public int Cost { get; private set; }
    public string Description { get; private set; }
    public string IconPath { get; private set; }

    public SBuildingData(EFurniture parent, EFurniture furniture, string label, int cost, string description = "", string icon = "")
    {
        Parent = parent;
        Furniture = furniture;
        Label = label;
        Cost = cost;
        Description = description;
        IconPath = icon;
    }
}
