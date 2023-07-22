using Godot;

public interface IGrabbable
{
    public bool IsCanBeGrabbed();
    public void Grab();
    public void Release();
    public void ThrowToDirection(Vector3 direction, float force);
}
