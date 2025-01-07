using Godot;
using Godot.Collections;

/* Used to store nodes and keep them between levels. Spawned by Godot's Autoload */
public partial class NodeStorage : Node
{
	private Dictionary<string, Node> _nodeStorage = new Dictionary<string, Node>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void StoreNode(string key, Node nodeToStore)
	{
#if DEBUG
		if (_nodeStorage.ContainsKey(key))
		{
			GD.PrintErr($"NodeStorage contains {key} key already!");
			return;
		}
#endif

		_nodeStorage.Add(key, nodeToStore);
		nodeToStore.Reparent(this);
	}

	public Node FetchNode(string key, Node newParent)
	{
#if DEBUG
		if (!_nodeStorage.ContainsKey(key))
		{
			GD.PrintErr($"NodeStorage doesn't have a key: {key}");
			return null;
		}
#endif

		Node storedNode;
		_nodeStorage.TryGetValue(key, out storedNode);

		storedNode.Reparent(newParent);

		_nodeStorage.Remove(key);
		
		return storedNode;
	}
}